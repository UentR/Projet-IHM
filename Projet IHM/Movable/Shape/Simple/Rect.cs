using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM.Movable.Shape.Simple
{
    class Rect : Simple
    {

        public Rect(Point position, Size s) : base(position, s) { }

        public Rect(Point position, Size s, bool isFull) : base(position, s, isFull) { }

        public override double Area() => this.size.Height * this.size.Width;

        public override bool isInside(Point p)
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
        public Square(Point position, int side) : base(position, new Size(side, side)) { }
        public Square(Point position, int side, bool isFull) : base(position, new Size(side, side), isFull) { }
    }
}
