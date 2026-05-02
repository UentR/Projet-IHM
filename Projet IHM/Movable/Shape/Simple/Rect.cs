using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Projet_IHM.Movable.Shape.Simple
{
    [DataContract]
    [KnownType(typeof(Square))]
    class Rect : Simple
    {

        public Rect(PointF position, SizeF s) : base(position, s) { }
        public Rect(PointF position) : base(position, SizeF.Empty) { }

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

    [DataContract]
    class Square : Rect
    {
        public Square(PointF position, float side) : base(position, new SizeF(side, side)) { }
        public Square(PointF position) : base(position) { }
        public Square(PointF position, float side, bool isFull) : base(position, new SizeF(side, side), isFull) { }

        public override void resize(SizeF size)
        {
            float side = Math.Max(size.Width, size.Height);
            this.size = new SizeF(side, side);
        }
    }
}
