using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Projet_IHM.Movable
{
    [DataContract]
    internal class TextLabel : Movable
    {
        #region 1. Attributs et Propriétés
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        private Color color;

        private SizeF measuredSize = new SizeF(50, 20); // Taille par défaut avant le premier dessin
        #endregion

        #region 2. Constructeur
        public TextLabel(PointF pos, string text, Color color) : base(pos)
        {
            this.Text = text;
            this.color = color;
        }
        #endregion

        #region 3. Accesseurs et Mises à jour
        public Color GetColor() => color;
        public void SetColor(Color c) => color = c;
        public PointF getPosition() => this.position;

        // Permet au DrawVisitor de mettre à jour la taille réelle de la boîte de collision
        public void UpdateSize(SizeF size)
        {
            this.measuredSize = size;
        }
        #endregion

        #region 4. Géométrie et Collisions
        public override bool isInside(PointF p)
        {
            return getRect().Contains(p);
        }

        public override RectangleF getRect()
        {
            return new RectangleF(position.X, position.Y, measuredSize.Width, measuredSize.Height);
        }
        #endregion

        #region 5. Pattern Visiteur
        public override void Accept(IVisitor visitor)
        {
            // Vérifie si le visiteur gère les textes
            if (visitor is DrawVisitor dv) dv.Visit(this);
        }
        #endregion
    }
}