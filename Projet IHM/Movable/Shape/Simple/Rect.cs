using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM.Movable.Shape.Simple
{
    class Rect : Simple
    {

        public Rect(PointF position, SizeF s) : base(position, s) { }

        public Rect(PointF position, SizeF s, bool isFull) : base(position, s, isFull) { }

        public override double Area() => this.size.Height * this.size.Width;

        public override bool isInside(PointF p)
        {
            return (p.X >= position.X && p.X <= position.X + this.size.Width) &&
                   (p.Y >= position.Y && p.Y <= position.Y + this.size.Height);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    class Square : Rect
    {
        public Square(PointF position, float side) : base(position, new SizeF(side, side)) { }
        public Square(PointF position, float side, bool isFull) : base(position, new SizeF(side, side), isFull) { }
    }
}
