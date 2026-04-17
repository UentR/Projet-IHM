using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Projet_IHM // ATTENTION : À remplacer par le nom exact de ton projet !
{
    public class PanelArrondi : Panel
    {
        public PanelArrondi()
        {
            // Évite les clignotements lors du redimensionnement (Double Buffering)
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            // La couleur "extérieure" de la pilule, pour se fondre dans la MainTopBar
            this.BackColor = Theme.FondSecondaire;

            // On force une marge interne pour que le ToolStrip ne déborde pas sur les courbes
            this.Padding = new Padding(15, 5, 15, 5);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Active l'anticrénelage pour que les courbes soient douces et non pixellisées
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // On définit le rectangle de dessin avec une légère marge pour ne pas couper le trait
            int marge = 2;
            Rectangle rect = new Rectangle(marge, marge, this.Width - (marge * 2), this.Height - (marge * 2));

            int rayon = 20; // La force de l'arrondi (tu pourras modifier cette valeur)

            // On dessine le fond blanc et on le remplit
            using (GraphicsPath path = ObtenirCheminArrondi(rect, rayon))
            using (SolidBrush pinceauFond = new SolidBrush(Theme.FondSecondaire))
            {
                e.Graphics.FillPath(pinceauFond, path);

                // Optionnel : on dessine une bordure grise autour de la forme blanche
                using (Pen styloBordure = new Pen(Color.DarkGray, 1))
                {
                    e.Graphics.DrawPath(styloBordure, path);
                }
            }
        }

        // Géométrie : calcul des 4 arcs de cercle pour les coins
        private GraphicsPath ObtenirCheminArrondi(Rectangle rect, int rayon)
        {
            GraphicsPath path = new GraphicsPath();
            int diametre = rayon * 2;

            path.AddArc(rect.X, rect.Y, diametre, diametre, 180, 90); // Coin Haut Gauche
            path.AddArc(rect.Right - diametre, rect.Y, diametre, diametre, 270, 90); // Coin Haut Droite
            path.AddArc(rect.Right - diametre, rect.Bottom - diametre, diametre, diametre, 0, 90); // Coin Bas Droite
            path.AddArc(rect.X, rect.Bottom - diametre, diametre, diametre, 90, 90); // Coin Bas Gauche
            path.CloseFigure();

            return path;
        }
    }
}