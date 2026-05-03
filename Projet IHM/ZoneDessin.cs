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
                g.FillRectangle(Brushes.Red, rect.getRect());
            else
                g.DrawRectangle(Pens.Red, rect.getRect());
        }

        public void Visit(Ellipse ellipse)
        {
            if (ellipse.isFull)
                g.FillEllipse(Brushes.Red, ellipse.getRect());
            else
                g.DrawEllipse(Pens.Red, ellipse.getRect());
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
                g.DrawLine(Pens.Red, absolutePoints[0], absolutePoints[1]);
                return;
            }

            if (fh.isFull)
            {
                g.FillPolygon(Brushes.Red, absolutePoints);
            }
            else
            {
                
                g.DrawPolygon(Pens.Red, absolutePoints);
            }
        }
    }

    internal enum Tool
    {
        Default,
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
        

        public ZoneDessin(Modele md, SizeF s) : base()
        {
            this.Location = new Point(10, 10);
            this.Size = s.ToSize();
            this.modele = md;
            this.modele.OnModelChanged += () => this.Invalidate();
            this.DoubleBuffered = true;
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

                continue;

                // Resize handles
                int minX = (int)rect.Left;
                int minY = (int)rect.Top;
                int maxX = (int)rect.Right;
                int maxY = (int)rect.Bottom;
                int diffX = maxX - minX;
                int diffY = maxY - minY;
                RectangleF sizeRect = new RectangleF(-4, -4, 8, 8);
                sizeRect.Offset(minX, minY);
                e.Graphics.FillRectangle(selectedBrush, sizeRect);
                sizeRect.Offset(0, diffY);
                e.Graphics.FillRectangle(selectedBrush, sizeRect);
                sizeRect.Offset(diffX, -diffY);
                e.Graphics.FillRectangle(selectedBrush, sizeRect);
                sizeRect.Offset(0, diffY);
                e.Graphics.FillRectangle(selectedBrush, sizeRect);

                
            }
        }
    

        //Repeint le widget
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.FloralWhite);

            DrawVisitor drawVisitor = new DrawVisitor(e.Graphics);

            foreach (Movable.Movable forme in modele.GetMovables())
            {
                forme.Accept(drawVisitor);
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
            }


            if (this.currentTool == Tool.Select)
                DrawSelect(e);

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    this.states["ctrl"] = true;
                    break;
                case Keys.ShiftKey:
                    this.states["shift"] = true;
                    break;
                case Keys.Escape:
                    if (this.currentTool == Tool.Select) modele.removeSelected();
                    break;
                case Keys.Enter:
                    if (this.currentTool == Tool.FreeHand) modele.addFH();
                    break;
                case Keys.D0:
                    this.currentTool = Tool.Default;
                    Cursor = Cursors.Default;
                    break;
                case Keys.D1:
                    this.currentTool = Tool.Select;
                    Cursor = Cursors.SizeAll;
                    break;
                case Keys.D2:
                    this.currentTool = Tool.Rect;
                    Cursor = Cursors.Hand;
                    break;
                case Keys.D3:
                    this.currentTool = Tool.Ellipse;
                    Cursor = Cursors.Hand;
                    break;
                case Keys.D4:
                    this.currentTool = Tool.FreeHand;
                    Cursor = Cursors.Hand;
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    this.states["ctrl"] = false;
                    break;
                case Keys.ShiftKey:
                    this.states["shift"] = false;
                    break;
            }
        }


        private void handleLeftSelect(MouseEventArgs e)
        {
            if (!this.states["ctrl"])
            {
                this.justSelected = modele.collide(e.Location);
                modele.setDeltaMouse(e.Location);
            }
            else
            {
                modele.removeCollide(e.Location);
            }
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            switch (e.Button, this.currentTool)
            {
                case (MouseButtons.Left, Tool.Select):
                    handleLeftSelect(e);
                    break;
                case (MouseButtons.Left, Tool.FreeHand):
                    modele.addFHPointF(e.Location);
                    break;
                case (MouseButtons.Left, Tool.Rect):
                    if (this.states["shift"])
                        modele.setPosNewShape(e.Location, Types.Square);
                    else
                        modele.setPosNewShape(e.Location, Types.Rect);
                    break;
                case (MouseButtons.Left, Tool.Ellipse):
                    if (this.states["shift"])
                        modele.setPosNewShape(e.Location, Types.Circle);
                    else
                        modele.setPosNewShape(e.Location, Types.Ellipse);
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
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            switch (e.Button, this.currentTool)
            {
                case (MouseButtons.Left, Tool.Select):
                    if (!this.states["ctrl"])
                        if (!this.justSelected)
                            modele.MoveSelected(e.Location);
                    break;
                case (MouseButtons.None, Tool.FreeHand):
                    modele.addMouseFH(e.Location);
                    break;
                case (MouseButtons.Left, Tool.Rect or Tool.Ellipse):
                    modele.setSizeSimple(e.Location);
                    break;
            }
        }
    }
}
