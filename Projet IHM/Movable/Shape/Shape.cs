using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Projet_IHM.Movable.Shape
{
    [DataContract]
    [KnownType(typeof(Simple.Simple))]
    abstract class Shape : Movable
    {
        [DataMember]
        public bool isFull { get; set; }
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
