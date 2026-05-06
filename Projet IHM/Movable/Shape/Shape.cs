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
        #region 1. Attributs et Propriétés
        [DataMember]
        public bool isFull { get; set; }
        [DataMember]
        protected Color color = Color.Red;
        #endregion

        #region 2. Constructeurs
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
        #endregion

        #region 3. Accesseurs
        public Color GetColor() => this.color;
        public void SetColor(Color color) => this.color = color;
        public PointF getPosition() => this.position;
        #endregion
    }
}