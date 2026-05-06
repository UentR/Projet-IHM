using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Projet_IHM
{
    public class PanelArrondi : Panel
    {
        #region 1. Constructeur
        public PanelArrondi()
        {
            
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            
            this.BackColor = ColorTranslator.FromHtml("#2D2D30");

            
            this.Padding = new Padding(15, 5, 15, 5);
        }
        #endregion

        #region 2. Surcharge du Rendu Graphique (OnPaint)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            
            int marge = 2;
            Rectangle rect = new Rectangle(marge, marge, this.Width - (marge * 2), this.Height - (marge * 2));

            int rayon = 20; 

            
            using (GraphicsPath path = ObtenirCheminArrondi(rect, rayon))
            using (SolidBrush pinceauFond = new SolidBrush(ColorTranslator.FromHtml("#2D2D30")))
            {
                e.Graphics.FillPath(pinceauFond, path);

                
                using (Pen styloBordure = new Pen(Color.DarkGray, 1))
                {
                    e.Graphics.DrawPath(styloBordure, path);
                }
            }
        }
        #endregion

        #region 3. Mathématiques et Géométrie
        
        private GraphicsPath ObtenirCheminArrondi(Rectangle rect, int rayon)
        {
            GraphicsPath path = new GraphicsPath();
            int diametre = rayon * 2;

            path.AddArc(rect.X, rect.Y, diametre, diametre, 180, 90); 
            path.AddArc(rect.Right - diametre, rect.Y, diametre, diametre, 270, 90); 
            path.AddArc(rect.Right - diametre, rect.Bottom - diametre, diametre, diametre, 0, 90); 
            path.AddArc(rect.X, rect.Bottom - diametre, diametre, diametre, 90, 90); 
            path.CloseFigure();

            return path;
        }
        #endregion
    }
}