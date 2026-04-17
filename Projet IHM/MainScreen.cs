using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Projet_IHM
{
    public partial class MainScreen : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        Modele md;
        ZoneDessin zD = null;


        public MainScreen()
        {
            InitializeComponent();

            // 1. Initialisation de ton modèle et de ta zone de dessin
            this.md = new Modele();
            this.zD = new ZoneDessin(this.md, new SizeF(this.MainScreenSpliter.Panel2.Width, this.MainScreenSpliter.Panel2.Height));
            this.MainScreenSpliter.Panel2.Controls.Add(this.zD);

            Theme.Appliquer(this);
            TitleBar.BackColor = Theme.FondSecondaire;
            this.BackColor = Theme.FondSecondaire;
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MenuCouleursPlates());
            menuStrip2.Renderer = new ToolStripProfessionalRenderer(new MenuPiluleCouleurs());
            menuStrip2.BackColor = Theme.FondSecondaire; // Doit être la même couleur que l'intérieur de la pilule
            MainScreenSpliter.BackColor = Theme.FondSecondaire;
            separation.BackColor = Theme.FondSecondaire;

            this.ResizeRedraw = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }


       // Bouton Fermer
        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close(); // ou Application.Exit();
        }

        // Bouton Réduire (Minimize)
        private void btnReduire_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Bouton Plein Écran (Maximize / Restore)
        private void btnAgrandir_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void panelTitre_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((ToolStripMenuItem)sender).Name);
            switch (((ToolStripMenuItem)sender).Name)
            {
                case "selectToolStripMenuItem":
                    this.zD.setTool(Tool.Select);
                    break;
                case "ellipseToolStripMenuItem":
                    this.zD.setTool(Tool.Ellipse);
                    break;
                case "rectangleToolStripMenuItem":
                    this.zD.setTool(Tool.Rect);
                    break;
                case "freehandShapeToolStripMenuItem":
                    this.zD.setTool(Tool.FreeHand);
                    break;
            }
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.zD != null)
                this.zD.Resize(this.MainScreenSpliter.Panel2.Width, this.MainScreenSpliter.Panel2.Height);
        }
    }

    // Une classe pour redéfinir les couleurs par défaut
    public class MenuCouleursPlates : ProfessionalColorTable
    {
        public override Color MenuStripGradientBegin => Theme.FondPrincipal;
        public override Color MenuStripGradientEnd => Theme.FondPrincipal;

        public override Color MenuItemSelected => Theme.Accentuation; // Bleu au survol
        public override Color MenuItemSelectedGradientBegin => Theme.Accentuation;
        public override Color MenuItemSelectedGradientEnd => Theme.Accentuation;

        public override Color MenuItemBorder => Color.Transparent;

        public override Color ToolStripGradientBegin => Theme.FondSecondaire;
        public override Color ToolStripGradientEnd => Theme.FondSecondaire;
        public override Color ToolStripBorder => Color.Transparent;
        public override Color MenuBorder => Color.Transparent;
        public override Color ToolStripDropDownBackground => Theme.FondPrincipal;

        // AJOUTE CECI : Supprime la bande blanche/grise à gauche (marge des icônes)
        public override Color ImageMarginGradientBegin => Theme.FondPrincipal;
        public override Color ImageMarginGradientMiddle => Theme.FondPrincipal;
        public override Color ImageMarginGradientEnd => Theme.FondPrincipal;
    }

    public class MenuPiluleCouleurs : ProfessionalColorTable
    {
        private Color fondPilule = ColorTranslator.FromHtml("#2D2D30");
        private Color survolPilule = ColorTranslator.FromHtml("#3E3E42"); // Un peu plus clair au survol

        // On force le fond à utiliser cette couleur
        public override Color MenuStripGradientBegin => fondPilule;
        public override Color MenuStripGradientEnd => fondPilule;
        public override Color ToolStripGradientBegin => fondPilule;
        public override Color ToolStripGradientEnd => fondPilule;

        // On garde les bordures transparentes
        public override Color MenuBorder => Color.Transparent;
        public override Color ToolStripBorder => Color.Transparent;

        // Couleurs quand on passe la souris sur les boutons (Select, Zoom, Shapes)
        public override Color MenuItemSelected => survolPilule;
        public override Color MenuItemSelectedGradientBegin => survolPilule;
        public override Color MenuItemSelectedGradientEnd => survolPilule;
        public override Color MenuItemBorder => Color.Transparent;
        public override Color MenuItemPressedGradientBegin => survolPilule;
        public override Color MenuItemPressedGradientEnd => survolPilule;
        public override Color ToolStripDropDownBackground => fondPilule;

        // AJOUTE CECI : Supprime la bande blanche/grise à gauche (marge des icônes)
        public override Color ImageMarginGradientBegin => fondPilule;
        public override Color ImageMarginGradientMiddle => fondPilule;
        public override Color ImageMarginGradientEnd => fondPilule;
    }
}
