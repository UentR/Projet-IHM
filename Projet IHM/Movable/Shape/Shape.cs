using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM.Movable.Shape
{
    abstract class Shape : Movable
    {
        public bool isFull { get; }
        public Shape(Point pos) : base(pos)
        { 
            this.isFull = true;
        }

        public Shape(Point pos, bool isFull) : base(pos)
        {
            this.isFull = isFull;
        }

        public abstract double Area();
        public Point getPosition() => this.position;

        

    }
}
