using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM.Movable.Shape
{
    class FreeHand : Shape
    {
        protected List<Point> relativePoints;
        protected Point lastPoint = Point.Empty;

        public FreeHand(Point pos, List<Point> points) : base(pos) { relativePoints = points; }
        public FreeHand(Point pos, List<Point> points, bool isFull) : base(pos, isFull) { relativePoints = points; }
        public FreeHand(Point pos) : base(pos) { relativePoints = new List<Point> { Point.Empty }; }
        public FreeHand(Point pos, bool isFull) : base(pos, isFull) { relativePoints = new List<Point> { Point.Empty }; }

        public override double Area() => throw new NotImplementedException();

        public void Add(Point p)
        {
            relativePoints.Add(new Point(p.X - position.X, p.Y - position.Y));
        }

        public void SetLastPoint(Point p)
        {
            lastPoint = new Point(p.X - position.X, p.Y - position.Y);
        }

        public void ClearLastPoint()
        {
            lastPoint = Point.Empty;
        }

        public override RectangleF getRect()
        {
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;

            foreach (Point p in relativePoints)
            {
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            Point upLeft = new Point(minX, minY) + (Size)position;
            Size size = new Size(maxX - minX, maxY - minY);
            return new RectangleF(upLeft, size);
        }

        public List<Point> GetPoints()
        {
            if (lastPoint == Point.Empty)  return relativePoints; 
            else
            {
                List<Point> points = new List<Point>(relativePoints);
                points.Add(lastPoint);
                return points;
            }
            
        }

        public override bool isInside(Point p)
        {
            // Algorithme de ray-casting pour déterminer si le point est à l'intérieur du polygone
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
