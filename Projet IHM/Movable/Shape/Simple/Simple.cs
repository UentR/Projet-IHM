using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;


namespace Projet_IHM.Movable.Shape.Simple
{
    [DataContract]
    [KnownType(typeof(Rect))]
    [KnownType(typeof(Ellipse))]
    abstract class Simple : Shape
    {
        [DataMember]
        protected SizeF size;
        public Simple(PointF position, SizeF s, Color c) : base(position, c)
        {
            this.size = s;
        }

        public Simple(PointF position, SizeF s, bool isFull, Color c) : base(position, isFull, c)
        {
            this.size = s;
        }

        public override RectangleF getRect() => new RectangleF(this.position, this.size);

        public virtual void resize(SizeF size)
        {
            this.size = size;
        }

        public void setEndBoundingBox(PointF end)
        {
            this.size = new SizeF(end.X - position.X, end.Y - position.Y);
        }
    }
}
