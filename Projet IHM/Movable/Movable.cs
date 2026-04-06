using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM.Movable
{
    internal abstract class Movable
    {
        protected Point position;
        protected Size deltaMouse = Size.Empty;
        protected bool currentlySelected = false;

        public Movable(Point pos) { this.position = pos; }

        public abstract void Accept(IVisitor visitor);

        protected void setSelected(bool selected) { this.currentlySelected = selected; }
        public bool isSelected() => this.currentlySelected;

        public void setDeltaMouse(Point mousePos)
        {
            this.deltaMouse = new Size(mousePos.X - position.X, mousePos.Y - position.Y);
            this.currentlySelected = true;
        }

        public void releaseShape() { this.currentlySelected = false; this.deltaMouse = Size.Empty; }
        public abstract bool isInside(Point p);
        public void updatePosition(Point newPos) { this.position = newPos - this.deltaMouse; }

        public abstract RectangleF getRect();

    }
}
