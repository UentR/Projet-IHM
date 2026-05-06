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
        #region 1. Constructeurs
        public Ellipse(PointF position, SizeF s, Color c) : base(position, s, c) { }
        public Ellipse(PointF position, Color c) : base(position, SizeF.Empty, c) { }
        public Ellipse(PointF position, SizeF s, bool isFull, Color c) : base(position, s, isFull, c) { }
        #endregion

        #region 2. Géométrie et Collisions
        public override bool isInside(PointF p)
        {
            double centerX = position.X + this.size.Width / 2.0;
            double centerY = position.Y + this.size.Height / 2.0;
            double normalizedX = (p.X - centerX) / (this.size.Width / 2.0);
            double normalizedY = (p.Y - centerY) / (this.size.Height / 2.0);
            return normalizedX * normalizedX + normalizedY * normalizedY <= 1;
        }
        #endregion

        #region 3. Pattern Visiteur
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        #endregion
    }

    [DataContract]
    class Circle : Ellipse
    {
        #region 1. Constructeurs
        public Circle(PointF position, Color c) : base(position, c) { }
        public Circle(PointF position, float radius, Color c) : base(position, new SizeF(radius * 2, radius * 2), c) { }
        public Circle(PointF position, float radius, bool isFull, Color c) : base(position, new SizeF(radius * 2, radius * 2), isFull, c) { }
        #endregion

        #region 2. Modifications
        public override void resize(SizeF size)
        {
            float side = Math.Max(size.Width, size.Height);
            this.size = new SizeF(side, side);
        }
        #endregion
    }
}