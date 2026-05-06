using Projet_IHM.Movable.Shape;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Projet_IHM.Movable
{
    [DataContract]
    [KnownType(typeof(Shape.Shape))]
    [KnownType(typeof(FreeHand))]
    [KnownType(typeof(TextLabel))]
    internal abstract class Movable
    {
        #region 1. Attributs
        [DataMember]
        protected PointF position;
        protected SizeF deltaMouseMove = SizeF.Empty;
        protected PointF deltaMouseResize = PointF.Empty;
        #endregion

        #region 2. Constructeur
        public Movable(PointF pos) { this.position = pos; }
        #endregion

        #region 3. Déplacements et Gestion Souris
        public void setDeltaMouse(PointF mousePos)
        {
            this.deltaMouseMove = new SizeF(mousePos.X - position.X, mousePos.Y - position.Y);
        }

        public void releaseShape() { this.deltaMouseMove = SizeF.Empty; }

        public void updatePosition(PointF newPos) { this.position = newPos - this.deltaMouseMove; }
        #endregion

        #region 4. Géométrie et Redimensionnement (Méthodes à redéfinir)
        public abstract bool isInside(PointF p);

        public abstract RectangleF getRect();

        public virtual void ResizeFromHandle(PointF newMousePos, HandleType handle)
        {
            // Par défaut, ne fait rien. Sera redéfini dans Simple.
        }
        #endregion

        #region 5. Pattern Visiteur
        public abstract void Accept(IVisitor visitor);
        #endregion
    }
}