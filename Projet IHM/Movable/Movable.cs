using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM.Movable
{
    internal abstract class Movable
    {
        protected PointF position;
        protected SizeF deltaMouse = SizeF.Empty;
        protected bool currentlySelected = false;

        public Movable(PointF pos) { this.position = pos; }

        public abstract void Accept(IVisitor visitor, SizeF ratio);

        protected void setSelected(bool selected) { this.currentlySelected = selected; }
        public bool isSelected() => this.currentlySelected;

        public void setDeltaMouse(PointF mousePos)
        {
            this.deltaMouse = new SizeF(mousePos.X - position.X, mousePos.Y - position.Y);
            this.currentlySelected = true;
        }

        public void releaseShape() { this.currentlySelected = false; this.deltaMouse = SizeF.Empty; }
        public abstract bool isInside(PointF p);
        public void updatePosition(PointF newPos) { this.position = newPos - this.deltaMouse; }

        public abstract RectangleF getRect();

    }
}
