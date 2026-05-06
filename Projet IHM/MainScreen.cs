using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Projet_IHM
{
    public partial class MainScreen : Form
    {
        #region 1. Constantes et Imports
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion


        #region 2. Variables et Attributs
        Modele md;
        ZoneDessin zD = null;
        #endregion

        
        #region 3. Constructeur
        public MainScreen()
        {
            InitializeComponent();

            // Arrondi des bouttons
            using (var path4 = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path4.AddEllipse(0, 0, button4.Width, button4.Height);
                button4.Region = new Region(path4);
            }
            using (var path5 = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path5.AddEllipse(0, 0, button5.Width, button5.Height);
                button5.Region = new Region(path5);
            }

            this.md = new Modele();
            this.md.UpdateCalque += (state, obj) =>
            {
                flowLayoutPanel1.SuspendLayout();
                if (state)
                {
                    flowLayoutPanel1.Controls.Add(obj);
                    flowLayoutPanel1.Controls.SetChildIndex(obj, 0);
                }
                else
                {
                    Control? c = flowLayoutPanel1.Controls[obj.Name];
                    if (c != null)
                    {
                        flowLayoutPanel1.Controls.Remove(obj);
                        c.Dispose();
                    }
                }
                flowLayoutPanel1.ResumeLayout();
            };
            this.md.SwapCalque += (first, second) =>
            {
                Control? control1 = flowLayoutPanel1.Controls[first.ToString()];
                Control? control2 = flowLayoutPanel1.Controls[second.ToString()];
                if (control1 != null && control2 != null)
                {
                    flowLayoutPanel1.SuspendLayout();

                    int index1 = flowLayoutPanel1.Controls.GetChildIndex(control1);
                    int index2 = flowLayoutPanel1.Controls.GetChildIndex(control2);

                    flowLayoutPanel1.Controls.SetChildIndex(control1, index2);
                    flowLayoutPanel1.Controls.SetChildIndex(control2, index1);

                    flowLayoutPanel1.ResumeLayout();
                }
            };
            this.md.CreateNewCalque();

            this.zD = new ZoneDessin(this.md, new Size(this.MainScreenSpliter.Panel2.Width - 20, this.MainScreenSpliter.Panel2.Height - 20));
            this.MainScreenSpliter.Panel2.Controls.Add(this.zD);

            this.zD.Focus();
            this.ActiveControl = this.zD;
        }
        #endregion

        
        #region 4. Overrides (Fenêtre et Clavier)
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.zD != null)
            {
                Point p = Point.Empty;
                Size current = this.zD.GetSize();
                Size visee = new Size(this.MainScreenSpliter.Panel2.Width - 20, this.MainScreenSpliter.Panel2.Height - 20);
                float widthRatio = (float)visee.Width / current.Width;
                float heightRatio = (float)visee.Height / current.Height;
                float ratio;
                if (widthRatio < heightRatio)
                {
                    ratio = widthRatio;
                    visee.Height = visee.Width * 2 / 3;
                }
                else
                {
                    ratio = heightRatio;
                    visee.Width = visee.Height * 3 / 2;
                }
                p.X = (MainScreenSpliter.Panel2.Width - visee.Width) / 2;
                p.Y = (MainScreenSpliter.Panel2.Height - visee.Height) / 2;

                this.zD.ResizeZoom(p, visee, ratio);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    zD.setState("ctrl", true);
                    break;
                case Keys.ShiftKey:
                    zD.setState("shift", true);
                    break;
                case Keys.Delete:
                case Keys.Back:
                    md.deleteSelected();
                    break;
                case Keys.K:
                    md.MoveDownShape();
                    break;
                case Keys.I:
                    md.MoveUpShape();
                    break;
                case Keys.S:
                    selectToolStripMenuItem.PerformClick();
                    break;
                case Keys.Z:
                    zoomToolStripMenuItem.PerformClick();
                    break;
                case Keys.C:
                    colorToolStripMenuItem.PerformClick();
                    break;
                default:
                    zD.handleKey(e); break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    zD.setState("ctrl", false);
                    break;
                case Keys.ShiftKey:
                    zD.setState("shift", false);
                    break;
            }
        }
        #endregion

        
        #region 5. Logique de Sauvegarde et Chargement (Fichiers)
        private void saveFile(string chemin)
        {
            var serializer = new DataContractSerializer(typeof(SaveDataWrapper));

            var settings = new XmlWriterSettings { Indent = true };
            var dataToSave = new SaveDataWrapper
            {
                Calques = this.md.getCalques(),
                CalquesOrder = this.md.getCalquesOrder()
            };

            using (var writer = XmlWriter.Create(chemin, settings))
            {
                serializer.WriteObject(writer, dataToSave);
            }
        }

        private void loadFile(string chemin)
        {
            try
            {
                var serializer = new DataContractSerializer(typeof(SaveDataWrapper));

                using (var fs = new FileStream(chemin, FileMode.Open))
                {
                    var data = (SaveDataWrapper)serializer.ReadObject(fs);
                    this.md.setCalques(data.Calques, data.CalquesOrder);
                    this.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement : {ex.Message}",
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        
        #region 6. Événements UI (Boutons, Menus, Fenêtre)
        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReduire_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

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
            switch (((ToolStripMenuItem)sender).Name)
            {
                case "selectToolStripMenuItem":
                    md.removeSelected();
                    zD.Invalidate();
                    zD.setTool(Tool.Select);
                    zD.setCursor(Cursors.SizeAll);
                    break;
                case "zoomToolStripMenuItem":
                    md.removeSelected();
                    zD.setTool(Tool.Zoom);
                    zD.setCursor(Cursors.Cross);
                    break;
                case "ellipseToolStripMenuItem":
                    zD.setTool(Tool.Ellipse);
                    zD.setCursor(Cursors.Hand);
                    break;
                case "rectangleToolStripMenuItem":
                    zD.setTool(Tool.Rect);
                    zD.setCursor(Cursors.Hand);
                    break;
                case "freehandShapeToolStripMenuItem":
                    zD.setTool(Tool.FreeHand);
                    zD.setCursor(Cursors.Hand);
                    break;
                case "starToolStripMenuItem":
                    zD.setTool(Tool.Star);
                    zD.setCursor(Cursors.Hand);
                    break;
                case "lineToolStripMenuItem":
                    zD.setTool(Tool.Line);
                    zD.setCursor(Cursors.Hand);
                    break;
                case "labelToolStripMenuItem":
                    zD.setTool(Tool.Label);
                    zD.setCursor(Cursors.IBeam);
                    break;
            }
        }

        private void sauvegarderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Fichiers XML (*.xml)|*.xml";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string chemin = saveFileDialog.FileName;
                saveFile(chemin);
            }
        }

        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers XML (*.xml)|*.xml";
            openFileDialog.Title = "Ouvrir un projet";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string chemin = openFileDialog.FileName;
                loadFile(chemin);
            }
        }

        private void nouveauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            md.NouveauProjet();
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.AllowFullOpen = true;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Color selectedColor = colorDialog.Color;
                    md.setColor(selectedColor);
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.md.CreateNewCalque();
            zD.Focus();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            this.md.deleteCalque();
            zD.Focus();
        }
        #endregion
    }

    #region Wrappers et Classes externes
    [DataContract(Name = "SaveData")]
    internal class SaveDataWrapper
    {
        [DataMember(Name = "Calques")]
        required public Dictionary<int, List<Movable.Movable>> Calques { get; set; }

        [DataMember(Name = "MyIntList")]
        required public List<int> CalquesOrder { get; set; }
    }
    #endregion
}