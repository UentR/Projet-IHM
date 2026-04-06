using Projet_IHM.Movable;
using Projet_IHM.Movable.Shape;
using Projet_IHM.Movable.Shape.Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Projet_IHM
{

    internal class DrawVisitor : Projet_IHM.Movable.IVisitor
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
            List<Point> relPoints = fh.GetPoints();
            Point basePos = fh.getPosition();

            
            if (relPoints == null || relPoints.Count < 2)
                return;

            
            Point[] absolutePoints = new Point[relPoints.Count];
            for (int i = 0; i < relPoints.Count; i++)
            {
                absolutePoints[i] = new Point(
                    basePos.X + relPoints[i].X,
                    basePos.Y + relPoints[i].Y
                );
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
        private bool ctrlPressed = false;
        private bool justSelected = false;
        private Tool currentTool = Tool.Default;

        public ZoneDessin(Modele md) : base()
        {
            this.Location = new Point(50, 50);
            this.Size = new Size(1000, 1000);
            this.modele = md;
            this.modele.OnModelChanged += () => this.Invalidate();
            this.DoubleBuffered = true;
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
                case Tool.Rect:
                    break;
                case Tool.Ellipse:
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
                    this.ctrlPressed = true;
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
                    this.currentTool = Tool.FreeHand;
                    Cursor = Cursors.Hand;
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.ControlKey) this.ctrlPressed = false;
        }


        private void handleLeftSelect(MouseEventArgs e)
        {
            if (!this.ctrlPressed)
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
                    modele.addFHPoint(e.Location);
                    break;
            }            
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this.justSelected = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            switch (e.Button, this.currentTool)
            {
                case (MouseButtons.Left, Tool.Select):
                    if (!this.ctrlPressed)
                        if (!this.justSelected)
                            modele.MoveSelected(e.Location);
                    break;
                case (MouseButtons.None, Tool.FreeHand):
                    modele.addMouseFH(e.Location);
                    break;
            }
        }
    }
}
