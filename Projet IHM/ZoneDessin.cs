using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Projet_IHM
{
    internal class ZoneDessin : Control
    {
        private Modele modele;
        private Brush redBrush = new SolidBrush(System.Drawing.Color.Red);
        private Pen redPen = new Pen(System.Drawing.Color.Red, 2);
        private Brush selectedBrush = new SolidBrush(System.Drawing.Color.DimGray);
        private Pen selectedPen = new Pen(System.Drawing.Color.DimGray, 3);
        private bool ctrlPressed = false;
        private bool selectTool = false;
        private bool justSelected = false;

        public ZoneDessin(Modele md) : base()
        {
            this.Location = new System.Drawing.Point(50, 50);
            this.Size = new System.Drawing.Size(1000, 1000);
            this.modele = md;
            this.DoubleBuffered = true;
        }

        //Repeint le widget
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.Clear(System.Drawing.Color.FloralWhite);
            foreach (Movable forme in modele.GetMovables())
            {
                if (forme is Simple)
                {
                    Simple simpleForme = (Simple)forme;
                    if (simpleForme is Rectangle)
                    {
                        if (simpleForme.isFull) e.Graphics.FillRectangle(redBrush, simpleForme.getRect());
                        else                    e.Graphics.DrawRectangle(redPen, simpleForme.getRect());
                    }
                    else if (simpleForme is Ellipse)
                    {
                        if (simpleForme.isFull) e.Graphics.FillEllipse(redBrush, simpleForme.getRect());
                        else                    e.Graphics.DrawEllipse(redPen, simpleForme.getRect());
                    }
                }
            }

            foreach (Movable forme in modele.GetSelected())
            {
                if (forme is Simple)
                {
                    Simple simpleForme = (Simple)forme;
                    RectangleF rect = simpleForme.getRect();
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

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.ControlKey) this.ctrlPressed = true;
            if (e.KeyCode == Keys.D1)
            {
                this.selectTool = true;
                // change mouse cursor to select
                Cursor = Cursors.SizeAll;
            }
            if (e.KeyCode == Keys.D2)
            {
                this.selectTool = false;
                Cursor = Cursors.Default;
            }
            if (e.KeyCode == Keys.Escape)
            {
                if (this.selectTool) modele.removeSelected();

                this.Invalidate();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.ControlKey) this.ctrlPressed = false;
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                if (this.selectTool)
                {
                    if (!this.ctrlPressed)
                    {
                        if (modele.collide(e.Location) != -1) this.justSelected = true;
                        modele.setDeltaMouse(e.Location);
                    } else
                    {
                        modele.removeCollide(e.Location);
                    }
                }
                
            }
            if (e.Button == MouseButtons.Right)
            {
                
            }
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this.justSelected = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left)
            {
                if (this.selectTool)
                    if (!this.ctrlPressed)
                        if (!this.justSelected)
                            modele.MoveSelected(e.Location);
                this.Invalidate();
            }
        }
    }
}
