using System;
using System.Collections.Generic;
using System.Text;


namespace Projet_IHM.Movable.Shape.Simple
{
    abstract class Simple : Shape
    {
        protected SizeF size;
        public Simple(PointF position, SizeF s) : base(position)
        {
            this.size = s;
        }

        public Simple(PointF position, SizeF s, bool isFull) : base(position, isFull)
        {
            this.size = s;
        }

        public override RectangleF getRect() => new RectangleF(this.position, this.size);

        public virtual void resize(SizeF size)
        {
            this.size = size;
        }
    }
}
