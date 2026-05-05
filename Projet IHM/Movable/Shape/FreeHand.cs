using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Projet_IHM.Movable.Shape
{
    [DataContract]
    class FreeHand : Shape
    {
        [DataMember]
        protected List<PointF> relativePoints;
        protected PointF lastPoint = PointF.Empty;

        public FreeHand(PointF pos, List<PointF> points, Color c) : base(pos, c) { relativePoints = points; }
        public FreeHand(PointF pos, List<PointF> points, bool isFull, Color c) : base(pos, isFull, c) { relativePoints = points; }
        public FreeHand(PointF pos, Color c) : base(pos, c) { relativePoints = new List<PointF> { PointF.Empty }; }
        public FreeHand(PointF pos, bool isFull, Color c) : base(pos, isFull, c) { relativePoints = new List<PointF> { PointF.Empty }; }

        public void Add(PointF p)
        {
            relativePoints.Add(new PointF(p.X - position.X, p.Y - position.Y));
        }

        public void SetLastPointF(PointF p)
        {
            lastPoint = new PointF(p.X - position.X, p.Y - position.Y);
        }

        public void ClearLastPointF()
        {
            lastPoint = PointF.Empty;
        }

        public override RectangleF getRect()
        {
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;

            foreach (PointF p in relativePoints)
            {
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            PointF upLeft = new PointF(minX, minY) + (SizeF)position.ToVector2();
            SizeF size = new SizeF(maxX - minX, maxY - minY);
            return new RectangleF(upLeft, size);
        }

        public List<PointF> GetPoints()
        {
            if (lastPoint == PointF.Empty)  return relativePoints; 
            else
            {
                List<PointF> points = new List<PointF>(relativePoints);
                points.Add(lastPoint);
                return points;
            }
            
        }

        public override bool isInside(PointF p)
        {
            // Algorithme de ray-casting pour déterminer si le PointF est à l'intérieur du polygone
            if (relativePoints == null || relativePoints.Count < 3)
                return false;

            bool inside = false;

            int j = relativePoints.Count - 1;

            for (int i = 0; i < relativePoints.Count; i++)
            {
                
                double v1X = position.X + relativePoints[i].X;
                double v1Y = position.Y + relativePoints[i].Y;

                double v2X = position.X + relativePoints[j].X;
                double v2Y = position.Y + relativePoints[j].Y;

                
                if (((v1Y > p.Y) != (v2Y > p.Y)) &&
                    (p.X < (v2X - v1X) * (p.Y - v1Y) / (v2Y - v1Y) + v1X))
                {
                    inside = !inside;
                }

                j = i;
            }

            return inside;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
