using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM.Movable.Shape.Simple
{
    class Ellipse : Simple
    {
        public Ellipse(Point position, Size s) : base(position, s) { }

        public Ellipse(Point position, Size s, bool isFull) : base(position, s, isFull) { }

        public override double Area()
        {
            return Math.PI * (this.size.Width / 2.0) * (this.size.Height / 2.0);
        }
        public override bool isInside(Point p)
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

    class Circle : Ellipse
    {
        public Circle(Point position, int radius) : base(position, new Size(radius * 2, radius * 2)) { }
        public Circle(Point position, int radius, bool isFull) : base(position, new Size(radius * 2, radius * 2), isFull) { }
    }
}
