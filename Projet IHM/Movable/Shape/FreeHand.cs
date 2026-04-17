using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM.Movable.Shape
{
    class FreeHand : Shape
    {
        protected List<PointF> relativePoints;
        protected PointF lastPoint = PointF.Empty;

        public FreeHand(PointF pos, List<PointF> points) : base(pos) { relativePoints = points; }
        public FreeHand(PointF pos, List<PointF> points, bool isFull) : base(pos, isFull) { relativePoints = points; }
        public FreeHand(PointF pos) : base(pos) { relativePoints = new List<PointF> { PointF.Empty }; }
        public FreeHand(PointF pos, bool isFull) : base(pos, isFull) { relativePoints = new List<PointF> { PointF.Empty }; }

        public override double Area() => throw new NotImplementedException();

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

        public override void Accept(IVisitor visitor, SizeF ratio)
        {
            visitor.Visit(this, ratio);
        }
    }
}
