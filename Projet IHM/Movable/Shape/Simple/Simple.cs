using System;
using System.Collections.Generic;
using System.Text;


namespace Projet_IHM.Movable.Shape.Simple
{
    abstract class Simple : Shape
    {
        protected Size size;
        public Simple(Point position, Size s) : base(position)
        {
            this.size = s;
        }

        public Simple(Point position, Size s, bool isFull) : base(position, isFull)
        {
            this.size = s;
        }

        public override RectangleF getRect() => new RectangleF(this.position, this.size);
    }
}
