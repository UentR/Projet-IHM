using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM
{
    public abstract class Movable
    {
        protected Point position;
        protected Size deltaMouse = Size.Empty;
        protected bool currentlySelected = false;

        public Movable(Point pos) { this.position = pos; }

        protected void setSelected(bool selected) { this.currentlySelected = selected; }
        public bool isSelected() => this.currentlySelected;

        public void setDeltaMouse(Point mousePos)
        {
            this.deltaMouse = new Size(mousePos.X - position.X, mousePos.Y - position.Y);
            this.currentlySelected = true;
        }

        public void releaseShape() { this.currentlySelected = false; this.deltaMouse = Size.Empty; }
        public abstract bool isInside(Point p);
        public void updatePosition(Point newPos) { this.position = newPos - this.deltaMouse; }

    }


    abstract class Shape : Movable
    {
        

        public Shape(Point pos) : base(pos) {}

        public abstract double Area();
        public Point getPosition() => this.position;

    }

    abstract class Simple : Shape
    {
        protected Size size;
        public bool isFull { get; }
        public Simple(Point position, Size s) : base(position)
        {
            this.size = s;
            this.isFull = true;
        }

        public Simple(Point position, Size s, bool isFull) : base(position)
        {
            this.size = s;
            this.isFull = isFull;
        }

        public RectangleF getRect() => new RectangleF(this.position, this.size);
    }

    class Rectangle : Simple
    {

        public Rectangle(Point position, Size s) : base(position, s) {}

        public Rectangle(Point position, Size s, bool isFull) : base(position, s, isFull) {}

        public override double Area() => this.size.Height * this.size.Width;
        
        public override bool isInside(Point p)
        {
            return (p.X >= position.X && p.X <= position.X + this.size.Width) &&
                   (p.Y >= position.Y && p.Y <= position.Y + this.size.Height);
        }
    }

    class Square : Rectangle
    {
        public Square(Point position, int side) : base(position, new Size(side, side)) {}
        public Square(Point position, int side, bool isFull) : base(position, new Size(side, side), isFull) {}
    }

    class Ellipse : Simple
    {
        public Ellipse(Point position, Size s) : base(position, s) {}

        public Ellipse(Point position, Size s, bool isFull) : base(position, s, isFull) {}

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
    }

    class Circle : Ellipse
    {
        public Circle(Point position, int radius) : base(position, new Size(radius * 2, radius * 2)) {}
        public Circle(Point position, int radius, bool isFull) : base(position, new Size(radius * 2, radius * 2), isFull) {}
    }

    internal class Modele
    {
        private List<Movable> formes = new List<Movable> { new Rectangle(new Point(0, 0), new Size(410, 210)), new Ellipse(new Point(500, 500), new Size(350, 505)), new Square(new Point(100, 600), 300, false) };

        private Dictionary<int, Movable> selectedFormes = new Dictionary<int, Movable>();

        public int FormesCount() { return formes.Count; }
        public Movable GetForme(int index) { return formes[index]; }

        public void addSelected(int idx) { selectedFormes[idx] = formes[idx]; }
        public void removeSelected() { selectedFormes.Clear(); }

        public void AddForme(Movable forme) { formes.Add(forme); }

        public List<Movable> GetMovables() { return formes; }
        public List<Movable> GetSelected() { return new List<Movable>(selectedFormes.Values); }

        public void MoveSelected(Point newPos)
        {
            foreach (var forme in selectedFormes.Values)
            {
                forme.updatePosition(newPos);
            }
        }

        public void setDeltaMouse(Point mousePos)
        {
            foreach (var forme in selectedFormes.Values)
            {
                forme.setDeltaMouse(mousePos);
            }
        }

        public int collide(Point p)
        {
            for (int i = 0; i < FormesCount(); i++)
            {
                if (formes[i].isInside(p))
                {
                    selectedFormes[i] = formes[i];
                    return i;
                }
            }
            return -1;
        }

        public void removeCollide(Point p)
        {
            foreach (var kvp in selectedFormes)
            {
                if (kvp.Value.isInside(p))
                {
                    selectedFormes.Remove(kvp.Key);
                    return;
                }
            }
        }
    }
}
