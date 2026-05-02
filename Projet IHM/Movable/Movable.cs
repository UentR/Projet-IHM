using Projet_IHM.Movable.Shape;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Projet_IHM.Movable
{
    [DataContract]
    [KnownType(typeof(Shape.Shape))]
    [KnownType(typeof(FreeHand))]
    internal abstract class Movable
    {
        [DataMember]
        protected PointF position;
        protected SizeF deltaMouse = SizeF.Empty;

        public Movable(PointF pos) { this.position = pos; }

        public abstract void Accept(IVisitor visitor);


        public void setDeltaMouse(PointF mousePos)
        {
            this.deltaMouse = new SizeF(mousePos.X - position.X, mousePos.Y - position.Y);
        }

        public void releaseShape() { this.deltaMouse = SizeF.Empty; }
        public abstract bool isInside(PointF p);
        public void updatePosition(PointF newPos) { this.position = newPos - this.deltaMouse; }

        public abstract RectangleF getRect();

    }
}
