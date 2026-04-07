using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM.Movable.Shape
{
    abstract class Shape : Movable
    {
        public bool isFull { get; }
        public Shape(PointF pos) : base(pos)
        { 
            this.isFull = true;
        }

        public Shape(PointF pos, bool isFull) : base(pos)
        {
            this.isFull = isFull;
        }

        public abstract double Area();
        public PointF getPosition() => this.position;

        

    }
}
