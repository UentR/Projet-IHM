using Projet_IHM.Movable;
using Projet_IHM.Movable.Shape;
using Projet_IHM.Movable.Shape.Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Projet_IHM
{

    internal class DrawVisitor : IVisitor
    {
        private Graphics g;

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
    }

    internal enum Tool
    {
        Default,
        Zoom,
        Select,
        Rect,
        Ellipse,
        FreeHand
    }


    internal class ZoneDessin : Control
    {
        private Modele modele;
        private Brush selectedBrush = new SolidBrush(System.Drawing.Color.DimGray);
        private Pen selectedPen = new Pen(System.Drawing.Color.DimGray, 3);
        private bool justSelected = false;
        private Tool currentTool = Tool.Default;
        private Dictionary<string, bool> states = new Dictionary<string, bool> { ["ctrl"] = false, ["shift"] = false };

        public Size GetSize() => this.Size;

        public ZoneDessin(Modele md, SizeF s) : base()
        {
            this.Location = new Point(10, 10);
            this.Size = s.ToSize();
            this.modele = md;
            this.modele.OnModelChanged += () => this.Invalidate();
            this.DoubleBuffered = true;
        }

        private float xWindowZoom = 1.0f;
        private float yWindowZoom = 1.0f;
        private float xZoom = 1.0f;
        private float yZoom = 1.0f;
        private PointF dPos = PointF.Empty;

        public void ResizeZoom(Point p, Size s, float zoom)
        {
            if (p != Point.Empty) this.Location = p;
            this.Size = s;
            this.xWindowZoom *= zoom;
            this.yWindowZoom *= zoom;
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


        public void setTool(Tool tool)
        {
            this.currentTool = tool;
        }


        private void DrawSelect(PaintEventArgs e)
        {
            foreach (Movable.Movable forme in modele.GetSelected())
            {
                RectangleF rect = forme.getRect();
                rect.Inflate(4, 4);
                e.Graphics.DrawRectangle(selectedPen, rect);                
            }
        }
    

        //Repeint le widget
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
                case Tool.FreeHand:
                    modele.getFH()?.Accept(drawVisitor);
                    break;
                case Tool.Zoom:
                    modele.getZoomBorder().Accept(drawVisitor);
                    break;
            }

        }

        public void setState(String s, bool state)
        {
            states[s] = state;
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
                case Keys.D0:
                    this.currentTool = Tool.Default;
                    Cursor = Cursors.Default;
                    break;
                case Keys.D1:
                    modele.removeSelected();
                    this.currentTool = Tool.Select;
                    Cursor = Cursors.SizeAll;
                    break;
                case Keys.D2:
                    this.currentTool = Tool.Zoom;
                    Cursor = Cursors.Hand;
                    break;
                case Keys.D3:
                    this.currentTool = Tool.Rect;
                    Cursor = Cursors.Hand;
                    break;
                case Keys.D4:
                    this.currentTool = Tool.FreeHand;
                    Cursor = Cursors.Hand;
                    break;
            }
            this.Invalidate();
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



        private void ClampTranslation()
        {
            float totalZoomX = this.xZoom * this.xWindowZoom;
            float totalZoomY = this.yZoom * this.yWindowZoom;

            // Calcul des limites horizontales
            float limiteX = this.Width * (1 - totalZoomX);
            float minX = Math.Min(0, limiteX);
            float maxX = Math.Max(0, limiteX);

            // Calcul des limites verticales
            float limiteY = this.Height * (1 - totalZoomY);
            float minY = Math.Min(0, limiteY);
            float maxY = Math.Max(0, limiteY);

            // On force dPos à rester entre les valeurs minimum et maximum
            dPos.X = Math.Max(minX, Math.Min(maxX, dPos.X));
            dPos.Y = Math.Max(minY, Math.Min(maxY, dPos.Y));
        }


        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            
            float scrollSpeed = 0.5f;
            float deplacement = e.Delta * scrollSpeed;

            if (this.states["shift"])
            {
                
                dPos.X += deplacement;
            }
            else
            {
                
                dPos.Y += deplacement;
            }

            ClampTranslation();

            this.Invalidate();
        }



        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            PointF world = ScreenToWorld(e.Location);

            switch (e.Button, this.currentTool)
            {
                case (MouseButtons.Left, Tool.Select):
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

            }            
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this.justSelected = false;

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
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            PointF world = ScreenToWorld(e.Location);

            switch (e.Button, this.currentTool)
            {
                case (MouseButtons.Left, Tool.Select):
                    if (!this.states["ctrl"])
                        if (!this.justSelected)
                            modele.MoveSelected(world);
                    break;
                case (MouseButtons.None, Tool.FreeHand):
                    modele.addMouseFH(world);
                    break;
                case (MouseButtons.Left, Tool.Rect or Tool.Ellipse):
                    modele.setSizeSimple(world);
                    break;
                case (MouseButtons.Left, Tool.Zoom):
                    modele.setSizeZoom(world);
                    break;
            }
        }
    }
}
