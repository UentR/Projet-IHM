using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Projet_IHM.Movable.Shape.Simple
{
    [DataContract]
    [KnownType(typeof(Circle))]
    class Ellipse : Simple
    {
        public Ellipse(PointF position, SizeF s) : base(position, s) { }
        public Ellipse(PointF position) : base(position, SizeF.Empty) { }
        public Ellipse(PointF position, SizeF s, bool isFull) : base(position, s, isFull) { }

        public override double Area()
        {
            return Math.PI * (this.size.Width / 2.0) * (this.size.Height / 2.0);
        }
        public override bool isInside(PointF p)
        {
            double centerX = position.X + this.size.Width / 2.0;
            double centerY = position.Y + this.size.Height / 2.0;
            double normalizedX = (p.X - centerX) / (this.size.Width / 2.0);
            double normalizedY = (p.Y - centerY) / (this.size.Height / 2.0);
            return normalizedX * normalizedX + normalizedY * normalizedY <= 1;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    [DataContract]
    class Circle : Ellipse
    {

        public Circle(PointF position) : base(position) { }

        public Circle(PointF position, float radius) : base(position, new SizeF(radius * 2, radius * 2)) { }
        public Circle(PointF position, float radius, bool isFull) : base(position, new SizeF(radius * 2, radius * 2), isFull) { }

        public override void resize(SizeF size)
        {
            float side = Math.Max(size.Width, size.Height);
            this.size = new SizeF(side, side);
        }
    }
}
