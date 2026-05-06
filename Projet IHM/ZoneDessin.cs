using Projet_IHM.Movable;
using Projet_IHM.Movable.Shape;
using Projet_IHM.Movable.Shape.Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Projet_IHM
{
    #region Classes et Enums externes (Visiteur, Outils)
    internal class DrawVisitor : IVisitor
    {
        private Graphics g;
        public Font mainFont = new Font("Arial", 12);

        public DrawVisitor(Graphics graphics)
        {
            this.g = graphics;
        }

        public void Visit(Rect rect)
        {
            if (rect.isFull)
                g.FillRectangle(new SolidBrush(rect.GetColor()), rect.getRect());
            else
                g.DrawRectangle(new Pen(rect.GetColor()), rect.getRect());
        }

        public void Visit(Ellipse ellipse)
        {
            if (ellipse.isFull)
                g.FillEllipse(new SolidBrush(ellipse.GetColor()), ellipse.getRect());
            else
                g.DrawEllipse(new Pen(ellipse.GetColor()), ellipse.getRect());
        }

        public void Visit(FreeHand fh)
        {
            List<PointF> relPoints = fh.GetPoints();
            PointF basePos = fh.getPosition();

            if (relPoints == null || relPoints.Count < 2)
                return;

            PointF[] absolutePoints = new PointF[relPoints.Count];
            for (int i = 0; i < relPoints.Count; i++)
            {
                absolutePoints[i] = new PointF(
                    basePos.X + relPoints[i].X,
                    basePos.Y + relPoints[i].Y
                );
            }

            if (absolutePoints.Length == 2)
            {
                g.DrawLine(new Pen(fh.GetColor()), absolutePoints[0], absolutePoints[1]);
                return;
            }

            if (fh.isFull)
            {
                g.FillPolygon(new SolidBrush(fh.GetColor()), absolutePoints);
            }
            else
            {
                g.DrawPolygon(new Pen(fh.GetColor()), absolutePoints);
            }
        }

        public void Visit(TextLabel label)
        {
            if (string.IsNullOrEmpty(label.Text)) return;
            SizeF size = g.MeasureString(label.Text, mainFont);
            label.UpdateSize(size);
            g.DrawString(label.Text, mainFont, new SolidBrush(label.GetColor()), label.getPosition());
        }

        public void Visit(Line line)
        {
            g.DrawLine(new Pen(line.GetColor(), 2), line.getPosition(), line.GetEndPoint());
        }
    }

    internal enum Tool
    {
        Default,
        Zoom,
        Select,
        Rect,
        Ellipse,
        FreeHand,
        Star,
        Label,
        Line
    }

    internal enum HandleType
    {
        None,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Top,
        Bottom,
        Left,
        Right
    }
    #endregion

    internal class ZoneDessin : Control
    {
        #region 1. Attributs et Variables d'état
        private Modele modele;
        private Brush selectedBrush = new SolidBrush(System.Drawing.Color.DimGray);
        private Pen selectedPen = new Pen(System.Drawing.Color.DimGray, 3);
        private bool justSelected = false;
        private Tool currentTool = Tool.Default;
        private HandleType currentHandle = HandleType.None;
        private Dictionary<string, bool> states = new Dictionary<string, bool> { ["ctrl"] = false, ["shift"] = false, ["staring"] = false };

        
        private float xWindowZoom = 1.0f;
        private float yWindowZoom = 1.0f;
        private float xZoom = 1.0f;
        private float yZoom = 1.0f;
        private PointF dPos = PointF.Empty;
        #endregion

        
        #region 2. Constructeur et Propriétés
        public ZoneDessin(Modele md, SizeF s) : base()
        {
            this.Location = new Point(10, 10);
            this.Size = s.ToSize();
            this.modele = md;
            this.modele.OnModelChanged += () => this.Invalidate();
            this.DoubleBuffered = true;
        }

        public Size GetSize() => this.Size;

        public void setTool(Tool tool)
        {
            this.currentTool = tool;
        }

        public void setCursor(Cursor cursor)
        {
            this.Cursor = cursor;
        }

        public void setState(String s, bool state)
        {
            states[s] = state;
        }
        #endregion

        
        #region 3. Mathématiques du Zoom et Coordonnées (Vue)
        public void ResizeZoom(Point p, Size s, float zoom)
        {
            if (p != Point.Empty) this.Location = p;
            this.Size = s;
            this.xWindowZoom *= zoom;
            this.yWindowZoom *= zoom;
            this.Invalidate();
        }

        public void UpdateZoom(RectangleF zoomRect)
        {
            if (zoomRect.Width < 1 || zoomRect.Height < 1) return;

            float newXZoom = (this.Size.Width / zoomRect.Width) / this.xWindowZoom;
            float newYZoom = (this.Size.Height / zoomRect.Height) / this.yWindowZoom;

            float finalZoom = Math.Min(newXZoom, newYZoom);

            this.xZoom = finalZoom;
            this.yZoom = finalZoom;

            dPos = new PointF(
                -zoomRect.Left * this.xZoom * this.xWindowZoom,
                -zoomRect.Top * this.yZoom * this.yWindowZoom
            );
        }

        public void ResetZoom()
        {
            xZoom = 1.0f;
            yZoom = 1.0f;
            dPos = PointF.Empty;
            this.Invalidate();
        }

        private PointF ScreenToWorld(Point screenPoint)
        {
            float totalZoomX = this.xZoom * this.xWindowZoom;
            float totalZoomY = this.yZoom * this.yWindowZoom;

            return new PointF(
                (screenPoint.X - dPos.X) / totalZoomX,
                (screenPoint.Y - dPos.Y) / totalZoomY
            );
        }

        private void ClampTranslation()
        {
            float totalZoomX = this.xZoom * this.xWindowZoom;
            float totalZoomY = this.yZoom * this.yWindowZoom;

            float limiteX = this.Width * (1 - totalZoomX);
            float minX = Math.Min(0, limiteX);
            float maxX = Math.Max(0, limiteX);

            float limiteY = this.Height * (1 - totalZoomY);
            float minY = Math.Min(0, limiteY);
            float maxY = Math.Max(0, limiteY);

            dPos.X = Math.Max(minX, Math.Min(maxX, dPos.X));
            dPos.Y = Math.Max(minY, Math.Min(maxY, dPos.Y));
        }
        #endregion

        
        #region 4. Logique de Rendu (Paint)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.FloralWhite);
            e.Graphics.TranslateTransform(dPos.X, dPos.Y);
            e.Graphics.ScaleTransform(xWindowZoom, yWindowZoom);
            e.Graphics.ScaleTransform(xZoom, yZoom);

            DrawVisitor drawVisitor = new DrawVisitor(e.Graphics);

            foreach (int idx in modele.getCalquesOrder())
            {
                if (!modele.isCalqueVisible(idx)) continue;
                foreach (Movable.Movable forme in modele.getCalques(idx))
                {
                    forme.Accept(drawVisitor);
                }
            }

            switch (this.currentTool)
            {
                case Tool.Select:
                    DrawSelect(e);
                    break;
                case Tool.Rect or Tool.Ellipse:
                    modele.getSimpleDraw()?.Accept(drawVisitor);
                    break;
                case Tool.Star:
                case Tool.FreeHand:
                    modele.getFH()?.Accept(drawVisitor);
                    break;
                case Tool.Zoom:
                    modele.getZoomBorder().Accept(drawVisitor);
                    break;
            }
        }

        private void DrawSelect(PaintEventArgs e)
        {
            int handleSize = 6;
            Brush handleBrush = new SolidBrush(Color.White);
            Pen handlePen = new Pen(Color.DimGray, 1);

            foreach (Movable.Movable forme in modele.GetSelected())
            {
                RectangleF rect = forme.getRect();
                rect.Inflate(4, 4);
                e.Graphics.DrawRectangle(selectedPen, rect);

                PointF[] handles = {
                    new PointF(rect.Left, rect.Top),
                    new PointF(rect.Right, rect.Top),
                    new PointF(rect.Left, rect.Bottom),
                    new PointF(rect.Right, rect.Bottom),
                    new PointF(rect.Left + rect.Width / 2, rect.Top),
                    new PointF(rect.Left + rect.Width / 2, rect.Bottom),
                    new PointF(rect.Left, rect.Top + rect.Height / 2),
                    new PointF(rect.Right, rect.Top + rect.Height / 2)
                };

                foreach (var pt in handles)
                {
                    RectangleF hRect = new RectangleF(pt.X - handleSize / 2f, pt.Y - handleSize / 2f, handleSize, handleSize);
                    e.Graphics.FillRectangle(handleBrush, hRect);
                    e.Graphics.DrawRectangle(handlePen, Rectangle.Round(hRect));
                }
            }
        }
        #endregion

        
        #region 5. Gestion des Entrées (Souris et Clavier)
        private HandleType GetHandleAtPoint(PointF worldPoint)
        {
            int hitArea = 8;
            foreach (var forme in modele.GetSelected())
            {
                RectangleF rect = forme.getRect();
                rect.Inflate(4, 4);

                bool Hit(float x, float y) => Math.Abs(worldPoint.X - x) < hitArea && Math.Abs(worldPoint.Y - y) < hitArea;

                if (Hit(rect.Left, rect.Top)) return HandleType.TopLeft;
                if (Hit(rect.Right, rect.Top)) return HandleType.TopRight;
                if (Hit(rect.Left, rect.Bottom)) return HandleType.BottomLeft;
                if (Hit(rect.Right, rect.Bottom)) return HandleType.BottomRight;
                if (Hit(rect.Left + rect.Width / 2, rect.Top)) return HandleType.Top;
                if (Hit(rect.Left + rect.Width / 2, rect.Bottom)) return HandleType.Bottom;
                if (Hit(rect.Left, rect.Top + rect.Height / 2)) return HandleType.Left;
                if (Hit(rect.Right, rect.Top + rect.Height / 2)) return HandleType.Right;
            }
            return HandleType.None;
        }

        private void handleLeftSelect(MouseEventArgs e)
        {
            PointF world = ScreenToWorld(e.Location);

            if (!this.states["ctrl"])
            {
                this.justSelected = modele.collide(world);
                modele.setDeltaMouse(world);
            }
            else
            {
                modele.removeCollide(world);
            }
        }

        public void handleKey(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (this.currentTool == Tool.Select) modele.removeSelected();
                    if (this.currentTool == Tool.Zoom) ResetZoom();
                    break;
                case Keys.Enter:
                    if (this.currentTool == Tool.FreeHand) modele.addFH();
                    break;
            }
            this.Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (states["staring"])
            {
                modele.updateNbPicStar(Math.Sign(e.Delta));
            }
            else
            {
                float scrollSpeed = 0.5f;
                float deplacement = e.Delta * scrollSpeed;

                if (states["shift"])
                {
                    dPos.X += deplacement;
                }
                else if (states["ctrl"])
                {
                    PointF worldPointUnderMouse = ScreenToWorld(e.Location);
                    float zoomFactor = (e.Delta > 0) ? 1.5f : (1.0f / 1.5f);
                    xZoom *= zoomFactor;
                    yZoom *= zoomFactor;

                    float totalZoomX = this.xZoom * this.xWindowZoom;
                    float totalZoomY = this.yZoom * this.yWindowZoom;

                    dPos.X = e.Location.X - (worldPointUnderMouse.X * totalZoomX);
                    dPos.Y = e.Location.Y - (worldPointUnderMouse.Y * totalZoomY);
                }
                else
                {
                    dPos.Y += deplacement;
                }

                this.Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            PointF world = ScreenToWorld(e.Location);

            switch (e.Button, this.currentTool)
            {
                case (MouseButtons.Left, Tool.Select):
                    HandleType clickedHandle = GetHandleAtPoint(world);
                    if (clickedHandle != HandleType.None)
                        this.currentHandle = clickedHandle;
                    else
                        handleLeftSelect(e);
                    break;
                case (MouseButtons.Left, Tool.FreeHand):
                    modele.addFHPointF(world);
                    break;
                case (MouseButtons.Left, Tool.Rect):
                    if (this.states["shift"])
                        modele.setPosNewShape(world, Types.Square);
                    else
                        modele.setPosNewShape(world, Types.Rect);
                    break;
                case (MouseButtons.Left, Tool.Ellipse):
                    if (this.states["shift"])
                        modele.setPosNewShape(world, Types.Circle);
                    else
                        modele.setPosNewShape(world, Types.Ellipse);
                    break;
                case (MouseButtons.Left, Tool.Zoom):
                    modele.setPosZoom(world);
                    break;
                case (MouseButtons.Left, Tool.Star):
                    modele.addStar(world);
                    states["staring"] = true;
                    break;
                case (MouseButtons.Left, Tool.Label):
                    string newText = Microsoft.VisualBasic.Interaction.InputBox("Entrez le texte :", "Nouveau Label", "Texte");
                    modele.AddLabel(world, newText);
                    break;
                case (MouseButtons.Left, Tool.Line):
                    modele.setPosNewShape(world, Types.Line);
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this.justSelected = false;
            this.currentHandle = HandleType.None;

            switch (e.Button, this.currentTool)
            {
                case (MouseButtons.Left, Tool.Rect):
                    modele.addSimple();
                    break;
                case (MouseButtons.Left, Tool.Ellipse):
                    modele.addSimple();
                    break;
                case (MouseButtons.Left, Tool.Zoom):
                    UpdateZoom(modele.getZoomBorder().getRect());
                    modele.clearZoom();
                    break;
                case (MouseButtons.Left, Tool.Star):
                    modele.addFH();
                    states["staring"] = false;
                    break;
                case (MouseButtons.Left, Tool.Line):
                    modele.addSimple();
                    break;
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            PointF world = ScreenToWorld(e.Location);

            if (this.currentTool == Tool.Select)
            {
                foreach (var forme in modele.GetSelected())
                {
                    if (forme is TextLabel label && label.isInside(world))
                    {
                        string editText = Microsoft.VisualBasic.Interaction.InputBox("Modifier le texte :", "Édition Label", label.Text);
                        if (!string.IsNullOrEmpty(editText))
                        {
                            label.Text = editText;
                            this.Invalidate();
                        }
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            PointF world = ScreenToWorld(e.Location);

            if (this.currentTool == Tool.Select && e.Button == MouseButtons.None)
            {
                switch (GetHandleAtPoint(world))
                {
                    case HandleType.TopLeft:
                    case HandleType.BottomRight:
                        this.Cursor = Cursors.SizeNWSE; break;
                    case HandleType.TopRight:
                    case HandleType.BottomLeft:
                        this.Cursor = Cursors.SizeNESW; break;
                    case HandleType.Top:
                    case HandleType.Bottom:
                        this.Cursor = Cursors.SizeNS; break;
                    case HandleType.Left:
                    case HandleType.Right:
                        this.Cursor = Cursors.SizeWE; break;
                    default:
                        this.Cursor = Cursors.SizeAll; break;
                }
            }

            switch (e.Button, this.currentTool)
            {
                case (MouseButtons.Left, Tool.Select):
                    if (this.currentHandle != HandleType.None)
                        modele.ResizeSelected(world, currentHandle);
                    else if (!this.states["ctrl"])
                        if (!this.justSelected)
                            modele.MoveSelected(world);
                    break;
                case (MouseButtons.None, Tool.FreeHand):
                    modele.addMouseFH(world);
                    break;
                case (MouseButtons.Left, Tool.Rect or Tool.Ellipse or Tool.Line):
                    modele.setSizeSimple(world);
                    break;
                case (MouseButtons.Left, Tool.Zoom):
                    modele.setSizeZoom(world);
                    break;
                case (MouseButtons.Left, Tool.Star):
                    modele.resizeStar(world);
                    break;
            }
        }
        #endregion
    }
}