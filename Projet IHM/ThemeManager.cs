using System.Drawing;

namespace Projet_IHM
{
    public static class Theme
    {
        // On définit des rôles sémantiques, pas juste "Rouge" ou "Bleu"
        // Exemple avec un thème sombre (Dark Mode) moderne

        public static Color FondPrincipal = ColorTranslator.FromHtml("#1E1E1E"); // Gris très foncé
        public static Color FondSecondaire = ColorTranslator.FromHtml("#2D2D30"); // Gris un peu plus clair (pour les panels)
        public static Color Accentuation = ColorTranslator.FromHtml("#007ACC"); // Bleu (pour les boutons actifs ou la sélection)

        public static Color TextePrincipal = Color.White;
        public static Color TexteSecondaire = Color.Gray;

        public static Color Bordure = ColorTranslator.FromHtml("#3F3F46");


        // Méthode pour appliquer le thème à toute une fenêtre
        public static void Appliquer(Control conteneurParent)
        {
            // 1. On applique la couleur de base au conteneur actuel
            conteneurParent.BackColor = FondPrincipal;
            conteneurParent.ForeColor = TextePrincipal;

            // 2. On parcourt tous les enfants directs
            foreach (Control controle in conteneurParent.Controls)
            {
                // Application spécifique selon le type de contrôle
                if (controle is Button btn)
                {
                    btn.BackColor = FondSecondaire;
                    btn.ForeColor = TextePrincipal;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = Accentuation; // Le bouton devient bleu au survol
                }
                else if (controle is Panel || controle is TableLayoutPanel || controle is SplitContainer)
                {
                    controle.BackColor = FondPrincipal;
                }
                else if (controle is MenuStrip menu)
                {
                    menu.BackColor = FondPrincipal;
                    menu.ForeColor = TextePrincipal;
                }

                // 3. LA RÉCURSIVITÉ : Si le contrôle contient lui-même d'autres contrôles (ex: un Panel qui contient des boutons)
                if (controle.HasChildren)
                {
                    Appliquer(controle); // On s'appelle soi-même
                }
            }
        }
    }
}