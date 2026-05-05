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
        [DataMember]
        protected Color color = Color.Red;

        public Color GetColor() => this.color;

        public Shape(PointF pos, Color c) : base(pos)
        {
            this.isFull = true;
            color = c;
        }

        public Shape(PointF pos, bool isFull, Color c) : base(pos)
        {
            this.isFull = isFull;
            color = c;
        }

        public PointF getPosition() => this.position;
        

        

    }
}
