using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM.Movable.Shape.Complex
{
    internal class Stars : FreeHand
    {
        #region 1. Constantes et Attributs
        private const float RATIO = 0.5f;
        private int nbPoints;
        private float currentSize;
        #endregion

        #region 2. Constructeur
        public Stars(PointF pos, int N, float size, Color c) : base(pos, c)
        {
            nbPoints = Math.Max(N, 3);
            relativePoints = GeneratePoints(nbPoints, size);
            currentSize = size;
            deltaMouseResize = pos;
        }
        #endregion

        #region 3. Génération et Géométrie
        private List<PointF> GeneratePoints(int nbPoints, float size)
        {
            float a;
            bool halfSize = false;
            List<PointF> points = new List<PointF>();
            PointF curr = PointF.Empty;
            float modifier;
            for (int i = 0; i < 2 * nbPoints; i++)
            {
                modifier = halfSize ? RATIO : 1;
                curr.X = (float)Math.Cos(i * Math.PI / nbPoints + Math.PI / 2) * size * modifier;
                curr.Y = (float)Math.Sin(i * Math.PI / nbPoints - Math.PI / 2) * size * modifier;
                halfSize = !halfSize;
                points.Add(curr);
            }
            return points;
        }
        #endregion

        #region 4. Modifications
        public void Resize(PointF newPos)
        {
            float size = Math.Max(Math.Abs(newPos.X - deltaMouseResize.X), Math.Abs(newPos.Y - deltaMouseResize.Y));
            relativePoints = GeneratePoints(nbPoints, size);
            currentSize = size;
        }

        public void modNbPic(int dir)
        {
            this.nbPoints = Math.Max(nbPoints + dir, 3);
            this.relativePoints = GeneratePoints(nbPoints, currentSize);
        }
        #endregion
    }
}