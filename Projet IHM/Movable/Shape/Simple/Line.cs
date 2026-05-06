using Projet_IHM.Movable.Shape;
using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace Projet_IHM.Movable.Shape.Simple
{
    [DataContract]
    internal class Line : Simple 
    {
        #region 1. Attributs
        [DataMember]
        private SizeF span; 
        #endregion

        #region 2. Constructeur
        public Line(PointF start, Color color) : base(start, color)
        {
            this.span = new SizeF(0, 0);
        }
        #endregion

        #region 3. Géométrie et Collisions
        
        public PointF GetEndPoint() => new PointF(position.X + span.Width, position.Y + span.Height);

        public override RectangleF getRect()
        {
            PointF endPoint = GetEndPoint();
            float minX = Math.Min(position.X, endPoint.X);
            float minY = Math.Min(position.Y, endPoint.Y);
            float width = Math.Abs(position.X - endPoint.X);
            float height = Math.Abs(position.Y - endPoint.Y);
            return new RectangleF(minX, minY, width, height);
        }

        public override bool isInside(PointF p)
        {
            PointF endPoint = GetEndPoint();

            
            RectangleF rect = getRect();
            rect.Inflate(5, 5);
            if (!rect.Contains(p)) return false;

            
            float l2 = span.Width * span.Width + span.Height * span.Height;
            if (l2 == 0) return Math.Abs(p.X - position.X) < 5 && Math.Abs(p.Y - position.Y) < 5;

            
            float t = Math.Max(0, Math.Min(1, ((p.X - position.X) * span.Width + (p.Y - position.Y) * span.Height) / l2));

            
            float projectionX = position.X + t * span.Width;
            float projectionY = position.Y + t * span.Height;

            
            float distanceSquared = (p.X - projectionX) * (p.X - projectionX) + (p.Y - projectionY) * (p.Y - projectionY);
            return distanceSquared <= 25;
        }
        #endregion

        #region 4. Modifications
        
        public override void resize(SizeF s)
        {
            this.span = s;
        }
        #endregion

        #region 5. Pattern Visiteur
        public override void Accept(IVisitor visitor)
        {
            if (visitor is DrawVisitor dv) dv.Visit(this);
        }
        #endregion
    }
}