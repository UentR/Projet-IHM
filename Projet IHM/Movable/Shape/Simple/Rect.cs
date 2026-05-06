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
        #region 1. Constructeurs
        public Rect(PointF position, SizeF s, Color c) : base(position, s, c) { }
        public Rect(PointF position, Color c) : base(position, SizeF.Empty, c) { }
        public Rect(PointF position, SizeF s, bool isFull, Color c) : base(position, s, isFull, c) { }
        #endregion

        #region 2. Géométrie et Collisions
        public override bool isInside(PointF p)
        {
            return (p.X >= position.X && p.X <= position.X + this.size.Width) &&
                   (p.Y >= position.Y && p.Y <= position.Y + this.size.Height);
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
    class Square : Rect
    {
        #region 1. Constructeurs
        public Square(PointF position, float side, Color c) : base(position, new SizeF(side, side), c) { }
        public Square(PointF position, Color c) : base(position, c) { }
        public Square(PointF position, float side, bool isFull, Color c) : base(position, new SizeF(side, side), isFull, c) { }
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