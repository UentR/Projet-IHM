using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Projet_IHM
{
    public class ControleCalque : UserControl
    {
        // 1. Déclaration des composants
        private Label lblOeil;
        private Label lblCadenas;
        private Panel pnlConteneur; // Le "petit conteneur" miniature
        private Label lblTitre;     // Le nom du calque
        private TextBox txtEditionTitre;
        private Button btnHaut;
        private Button btnBas;

        // 2. Variables d'état
        private bool estVisible = true;
        private bool estVerrouille = false;

        public ControleCalque()
        {
            InitialiserComposants();
            MettreAJourAffichage();
        }

        private void InitialiserComposants()
        {
            // Nouvelle taille totale
            this.Size = new Size(290, 215);
            this.MinimumSize = new Size(290, 215);
            this.MaximumSize = new Size(290, 215);
            this.BackColor = Color.FromArgb(45, 45, 48); // Thème sombre
            this.BorderStyle = BorderStyle.FixedSingle;

            // --- PARTIE HAUTE (Le conteneur et les flèches) ---

            // 1. Le Conteneur (Il prend maintenant presque toute la hauteur)
            pnlConteneur = new Panel()
            {
                Size = new Size(230, 145), // Plus grand pour remplir le nouvel espace
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(60, 60, 60),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Flèche Haut (En haut à droite du conteneur)
            btnHaut = new Button()
            {
                Text = "▲",
                Size = new Size(35, 30),
                Location = new Point(245, 10),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(55, 55, 55),
                Cursor = Cursors.Hand
            };
            btnHaut.FlatAppearance.BorderSize = 0;

            // Flèche Bas (En bas à droite du conteneur, alignée sur son bord inférieur)
            btnBas = new Button()
            {
                Text = "▼",
                Size = new Size(35, 30),
                Location = new Point(245, 125), // 10 (Y départ) + 145 (hauteur conteneur) - 30 (hauteur bouton)
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(55, 55, 55),
                Cursor = Cursors.Hand
            };
            btnBas.FlatAppearance.BorderSize = 0;

            // --- PARTIE BASSE (Label, oeil et cadenas) ---

            // 2. Le Label (En dessous de la zone du conteneur)
            lblTitre = new Label()
            {
                Text = "Calque 2",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(10, 165),
                Cursor = Cursors.IBeam // Indique à l'utilisateur qu'il peut interagir avec le texte
            };

            // NOUVEAU : Événement pour démarrer l'édition au double-clic
            lblTitre.DoubleClick += (s, e) => DemarrerEditionTitre();

            // NOUVEAU : La zone de texte cachée qui servira à l'édition
            txtEditionTitre = new TextBox()
            {
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(18, 168), // Légèrement décalé pour recouvrir parfaitement le label
                Size = new Size(180, 25), // Assez large mais sans mordre sur l'oeil/cadenas
                Visible = false, // Caché par défaut
                BackColor = Color.FromArgb(60, 60, 60), // Fond un peu plus clair
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Événements de la zone de texte (Touche Entrée, Echap, ou perte de focus)
            txtEditionTitre.KeyDown += TxtEditionTitre_KeyDown;
            txtEditionTitre.Leave += (s, e) => TerminerEditionTitre();

            // N'oublie pas d'ajouter txtEditionTitre à la fin de InitialiserComposants() :
            this.Controls.Add(txtEditionTitre);
            txtEditionTitre.BringToFront(); // S'assure qu'elle passe au-dessus du reste quand elle apparaît

            // 3. L'Oeil et le Cadenas (Sur la même ligne que le titre, décalés sur la droite)
            lblOeil = new Label()
            {
                Font = new Font("Segoe UI Emoji", 12), // Légèrement agrandis pour le nouvel espace
                AutoSize = true,
                Location = new Point(190, 165),
                Cursor = Cursors.Hand
            };
            lblOeil.Click += (s, e) => { estVisible = !estVisible; MettreAJourAffichage(); };

            lblCadenas = new Label()
            {
                Font = new Font("Segoe UI Emoji", 12),
                AutoSize = true,
                Location = new Point(235, 162),
                Cursor = Cursors.Hand
            };
            lblCadenas.Click += (s, e) => { estVerrouille = !estVerrouille; MettreAJourAffichage(); };

            // Ajout des contrôles
            this.Controls.Add(pnlConteneur);
            this.Controls.Add(lblTitre);
            this.Controls.Add(lblOeil);
            this.Controls.Add(lblCadenas);
            this.Controls.Add(btnHaut);
            this.Controls.Add(btnBas);
        }

        private void MettreAJourAffichage()
        {
            // Couleurs adaptées au thème sombre
            lblOeil.Text = estVisible ? "👁️" : "➖";
            lblOeil.ForeColor = estVisible ? Color.White : Color.DimGray;

            lblCadenas.Text = estVerrouille ? "🔒" : "🔓";
            lblCadenas.ForeColor = estVerrouille ? Color.Tomato : Color.White;

            if (!estVisible || estVerrouille)
                lblTitre.ForeColor = Color.DimGray;
            else
                lblTitre.ForeColor = Color.White;
        }

        // --- Propriétés publiques pour manipuler le contrôle depuis l'extérieur ---
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TitreCalque
        {
            get { return lblTitre.Text; }
            set { lblTitre.Text = value; }
        }

        // --- LOGIQUE D'ÉDITION DU TITRE ---

        private void DemarrerEditionTitre()
        {
            // Optionnel : On empêche de renommer si le calque est verrouillé !
            if (estVerrouille) return;

            // On copie le texte actuel dans la TextBox
            txtEditionTitre.Text = lblTitre.Text;

            // On inverse la visibilité
            lblTitre.Visible = false;
            txtEditionTitre.Visible = true;

            // On donne le focus à la TextBox et on sélectionne tout le texte
            txtEditionTitre.Focus();
            txtEditionTitre.SelectAll();
        }

        private void TerminerEditionTitre(bool annuler = false)
        {
            if (!txtEditionTitre.Visible) return; // Évite que l'événement se déclenche deux fois

            // Si on n'annule pas et que le texte n'est pas vide, on sauvegarde
            if (!annuler && !string.IsNullOrWhiteSpace(txtEditionTitre.Text))
            {
                lblTitre.Text = txtEditionTitre.Text;
            }

            // On remet l'affichage normal
            txtEditionTitre.Visible = false;
            lblTitre.Visible = true;
        }

        private void TxtEditionTitre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Évite le son "ding" d'erreur de Windows
                TerminerEditionTitre(false); // Sauvegarde
            }
            else if (e.KeyCode == Keys.Escape)
            {
                TerminerEditionTitre(true); // Annule les modifications
            }
        }
    }
}