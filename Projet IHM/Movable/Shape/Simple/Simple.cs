using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Projet_IHM.Movable.Shape.Simple
{
    [DataContract]
    [KnownType(typeof(Rect))]
    [KnownType(typeof(Ellipse))]
    //[KnownType(typeof(Line))]
    abstract class Simple : Shape
    {
        #region 1. Attributs
        [DataMember]
        protected SizeF size;
        #endregion

        #region 2. Constructeurs
        public Simple(PointF position, SizeF s, Color c) : base(position, c)
        {
            this.size = s;
        }

        public Simple(PointF pos, Color c) : base(pos, c) { }

        public Simple(PointF position, SizeF s, bool isFull, Color c) : base(position, isFull, c)
        {
            this.size = s;
        }
        #endregion

        #region 3. Géométrie
        public override RectangleF getRect() => new RectangleF(this.position, this.size);
        #endregion

        #region 4. Logique de Redimensionnement (Resize et Handles)
        public virtual void resize(SizeF size)
        {
            this.size = size;
        }

        public void setEndBoundingBox(PointF end)
        {
            this.size = new SizeF(end.X - position.X, end.Y - position.Y);
        }

        public override void ResizeFromHandle(PointF newMousePos, HandleType handle)
        {
            RectangleF rect = this.getRect();
            float left = rect.Left;
            float right = rect.Right;
            float top = rect.Top;
            float bottom = rect.Bottom;

            float minSize = 5f; // Pour empêcher la forme de s'inverser ou de disparaître

            switch (handle)
            {
                case HandleType.TopLeft:
                    left = Math.Min(newMousePos.X, right - minSize);
                    top = Math.Min(newMousePos.Y, bottom - minSize);
                    break;
                case HandleType.TopRight:
                    right = Math.Max(newMousePos.X, left + minSize);
                    top = Math.Min(newMousePos.Y, bottom - minSize);
                    break;
                case HandleType.BottomLeft:
                    left = Math.Min(newMousePos.X, right - minSize);
                    bottom = Math.Max(newMousePos.Y, top + minSize);
                    break;
                case HandleType.BottomRight:
                    right = Math.Max(newMousePos.X, left + minSize);
                    bottom = Math.Max(newMousePos.Y, top + minSize);
                    break;
                case HandleType.Top:
                    top = Math.Min(newMousePos.Y, bottom - minSize);
                    break;
                case HandleType.Bottom:
                    bottom = Math.Max(newMousePos.Y, top + minSize);
                    break;
                case HandleType.Left:
                    left = Math.Min(newMousePos.X, right - minSize);
                    break;
                case HandleType.Right:
                    right = Math.Max(newMousePos.X, left + minSize);
                    break;
            }

            this.position = new PointF(left, top);
            this.size = new SizeF(right - left, bottom - top);
        }
        #endregion
    }
}