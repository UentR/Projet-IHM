namespace Projet_IHM
{
    partial class MainScreen
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            MainTopBar = new TableLayoutPanel();
            panelArrondi1 = new PanelArrondi();
            menuStrip2 = new MenuStrip();
            selectToolStripMenuItem = new ToolStripMenuItem();
            zoomToolStripMenuItem = new ToolStripMenuItem();
            shapeToolStripMenuItem = new ToolStripMenuItem();
            simpleShapesToolStripMenuItem = new ToolStripMenuItem();
            rectangleToolStripMenuItem = new ToolStripMenuItem();
            ellipseToolStripMenuItem = new ToolStripMenuItem();
            freehandShapeToolStripMenuItem = new ToolStripMenuItem();
            TitleBar = new TableLayoutPanel();
            menuStrip1 = new MenuStrip();
            fichierToolStripMenuItem = new ToolStripMenuItem();
            nouveauToolStripMenuItem = new ToolStripMenuItem();
            ouvrirToolStripMenuItem = new ToolStripMenuItem();
            sauvegarderToolStripMenuItem = new ToolStripMenuItem();
            editionToolStripMenuItem = new ToolStripMenuItem();
            affichageToolStripMenuItem = new ToolStripMenuItem();
            optionToolStripMenuItem = new ToolStripMenuItem();
            ModifyWindowPanel = new FlowLayoutPanel();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            separation = new Panel();
            MainScreenSpliter = new SplitContainer();
            flowLayoutPanel1 = new FlowLayoutPanel();
            controleCalque1 = new ControleCalque();
            MainTopBar.SuspendLayout();
            panelArrondi1.SuspendLayout();
            menuStrip2.SuspendLayout();
            TitleBar.SuspendLayout();
            menuStrip1.SuspendLayout();
            ModifyWindowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainScreenSpliter).BeginInit();
            MainScreenSpliter.Panel1.SuspendLayout();
            MainScreenSpliter.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // MainTopBar
            // 
            MainTopBar.ColumnCount = 1;
            MainTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            MainTopBar.Controls.Add(panelArrondi1, 0, 1);
            MainTopBar.Controls.Add(TitleBar, 0, 0);
            MainTopBar.Dock = DockStyle.Top;
            MainTopBar.Location = new Point(5, 5);
            MainTopBar.Margin = new Padding(0);
            MainTopBar.Name = "MainTopBar";
            MainTopBar.RowCount = 2;
            MainTopBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            MainTopBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            MainTopBar.Size = new Size(1099, 125);
            MainTopBar.TabIndex = 0;
            // 
            // panelArrondi1
            // 
            panelArrondi1.BackColor = Color.Gray;
            panelArrondi1.Controls.Add(menuStrip2);
            panelArrondi1.Dock = DockStyle.Fill;
            panelArrondi1.Location = new Point(3, 53);
            panelArrondi1.Name = "panelArrondi1";
            panelArrondi1.Padding = new Padding(15, 5, 15, 5);
            panelArrondi1.Size = new Size(1093, 69);
            panelArrondi1.TabIndex = 0;
            // 
            // menuStrip2
            // 
            menuStrip2.Dock = DockStyle.Fill;
            menuStrip2.ImageScalingSize = new Size(24, 24);
            menuStrip2.Items.AddRange(new ToolStripItem[] { selectToolStripMenuItem, zoomToolStripMenuItem, shapeToolStripMenuItem });
            menuStrip2.Location = new Point(15, 5);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(1063, 59);
            menuStrip2.TabIndex = 1;
            menuStrip2.Text = "menuStrip2";
            // 
            // selectToolStripMenuItem
            // 
            selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            selectToolStripMenuItem.Size = new Size(74, 55);
            selectToolStripMenuItem.Text = "Select";
            selectToolStripMenuItem.Click += ToolStripMenuItem_Click;
            // 
            // zoomToolStripMenuItem
            // 
            zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            zoomToolStripMenuItem.Size = new Size(76, 55);
            zoomToolStripMenuItem.Text = "Zoom";
            // 
            // shapeToolStripMenuItem
            // 
            shapeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { simpleShapesToolStripMenuItem, freehandShapeToolStripMenuItem });
            shapeToolStripMenuItem.Name = "shapeToolStripMenuItem";
            shapeToolStripMenuItem.Size = new Size(85, 55);
            shapeToolStripMenuItem.Text = "Shapes";
            // 
            // simpleShapesToolStripMenuItem
            // 
            simpleShapesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { rectangleToolStripMenuItem, ellipseToolStripMenuItem });
            simpleShapesToolStripMenuItem.Name = "simpleShapesToolStripMenuItem";
            simpleShapesToolStripMenuItem.Size = new Size(241, 34);
            simpleShapesToolStripMenuItem.Text = "Simple Shapes";
            // 
            // rectangleToolStripMenuItem
            // 
            rectangleToolStripMenuItem.Name = "rectangleToolStripMenuItem";
            rectangleToolStripMenuItem.Size = new Size(190, 34);
            rectangleToolStripMenuItem.Text = "Rectangle";
            rectangleToolStripMenuItem.Click += ToolStripMenuItem_Click;
            // 
            // ellipseToolStripMenuItem
            // 
            ellipseToolStripMenuItem.Name = "ellipseToolStripMenuItem";
            ellipseToolStripMenuItem.Size = new Size(190, 34);
            ellipseToolStripMenuItem.Text = "Ellipse";
            ellipseToolStripMenuItem.Click += ToolStripMenuItem_Click;
            // 
            // freehandShapeToolStripMenuItem
            // 
            freehandShapeToolStripMenuItem.Name = "freehandShapeToolStripMenuItem";
            freehandShapeToolStripMenuItem.Size = new Size(241, 34);
            freehandShapeToolStripMenuItem.Text = "Freehand Shape";
            freehandShapeToolStripMenuItem.Click += ToolStripMenuItem_Click;
            // 
            // TitleBar
            // 
            TitleBar.ColumnCount = 2;
            TitleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TitleBar.ColumnStyles.Add(new ColumnStyle());
            TitleBar.Controls.Add(menuStrip1, 0, 0);
            TitleBar.Controls.Add(ModifyWindowPanel, 1, 0);
            TitleBar.Dock = DockStyle.Fill;
            TitleBar.Location = new Point(0, 0);
            TitleBar.Margin = new Padding(0);
            TitleBar.Name = "TitleBar";
            TitleBar.Padding = new Padding(0, 0, 0, 5);
            TitleBar.RowCount = 1;
            TitleBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TitleBar.Size = new Size(1099, 50);
            TitleBar.TabIndex = 0;
            TitleBar.MouseDown += panelTitre_MouseDown;
            // 
            // menuStrip1
            // 
            menuStrip1.Dock = DockStyle.Fill;
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fichierToolStripMenuItem, editionToolStripMenuItem, affichageToolStripMenuItem, optionToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(949, 45);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.MouseDown += panelTitre_MouseDown;
            // 
            // fichierToolStripMenuItem
            // 
            fichierToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nouveauToolStripMenuItem, ouvrirToolStripMenuItem, sauvegarderToolStripMenuItem });
            fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            fichierToolStripMenuItem.Size = new Size(78, 41);
            fichierToolStripMenuItem.Text = "Fichier";
            // 
            // nouveauToolStripMenuItem
            // 
            nouveauToolStripMenuItem.Name = "nouveauToolStripMenuItem";
            nouveauToolStripMenuItem.Size = new Size(270, 34);
            nouveauToolStripMenuItem.Text = "Nouveau";
            // 
            // ouvrirToolStripMenuItem
            // 
            ouvrirToolStripMenuItem.Name = "ouvrirToolStripMenuItem";
            ouvrirToolStripMenuItem.Size = new Size(270, 34);
            ouvrirToolStripMenuItem.Text = "Ouvrir";
            ouvrirToolStripMenuItem.Click += ouvrirToolStripMenuItem_Click;
            // 
            // sauvegarderToolStripMenuItem
            // 
            sauvegarderToolStripMenuItem.Name = "sauvegarderToolStripMenuItem";
            sauvegarderToolStripMenuItem.Size = new Size(270, 34);
            sauvegarderToolStripMenuItem.Text = "Sauvegarder";
            sauvegarderToolStripMenuItem.Click += sauvegarderToolStripMenuItem_Click;
            // 
            // editionToolStripMenuItem
            // 
            editionToolStripMenuItem.Name = "editionToolStripMenuItem";
            editionToolStripMenuItem.Size = new Size(83, 41);
            editionToolStripMenuItem.Text = "Edition";
            // 
            // affichageToolStripMenuItem
            // 
            affichageToolStripMenuItem.Name = "affichageToolStripMenuItem";
            affichageToolStripMenuItem.Size = new Size(103, 41);
            affichageToolStripMenuItem.Text = "Affichage";
            // 
            // optionToolStripMenuItem
            // 
            optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            optionToolStripMenuItem.Size = new Size(84, 41);
            optionToolStripMenuItem.Text = "Option";
            // 
            // ModifyWindowPanel
            // 
            ModifyWindowPanel.AutoSize = true;
            ModifyWindowPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ModifyWindowPanel.Controls.Add(button1);
            ModifyWindowPanel.Controls.Add(button2);
            ModifyWindowPanel.Controls.Add(button3);
            ModifyWindowPanel.Dock = DockStyle.Fill;
            ModifyWindowPanel.Location = new Point(949, 0);
            ModifyWindowPanel.Margin = new Padding(0);
            ModifyWindowPanel.Name = "ModifyWindowPanel";
            ModifyWindowPanel.Size = new Size(150, 45);
            ModifyWindowPanel.TabIndex = 2;
            ModifyWindowPanel.WrapContents = false;
            // 
            // button1
            // 
            button1.BackColor = Color.LightGray;
            button1.Location = new Point(0, 0);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Size = new Size(50, 50);
            button1.TabIndex = 0;
            button1.Text = "🗕";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnReduire_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.LightGray;
            button2.Location = new Point(50, 0);
            button2.Margin = new Padding(0);
            button2.Name = "button2";
            button2.Size = new Size(50, 50);
            button2.TabIndex = 1;
            button2.Text = "🗖";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnAgrandir_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.LightGray;
            button3.Location = new Point(100, 0);
            button3.Margin = new Padding(0);
            button3.Name = "button3";
            button3.Size = new Size(50, 50);
            button3.TabIndex = 0;
            button3.Text = "✖";
            button3.UseVisualStyleBackColor = true;
            button3.Click += btnFermer_Click;
            // 
            // separation
            // 
            separation.Dock = DockStyle.Top;
            separation.Location = new Point(5, 130);
            separation.Name = "separation";
            separation.Size = new Size(1099, 5);
            separation.TabIndex = 1;
            // 
            // MainScreenSpliter
            // 
            MainScreenSpliter.Dock = DockStyle.Fill;
            MainScreenSpliter.FixedPanel = FixedPanel.Panel1;
            MainScreenSpliter.IsSplitterFixed = true;
            MainScreenSpliter.Location = new Point(5, 135);
            MainScreenSpliter.Margin = new Padding(0);
            MainScreenSpliter.Name = "MainScreenSpliter";
            // 
            // MainScreenSpliter.Panel1
            // 
            MainScreenSpliter.Panel1.Controls.Add(flowLayoutPanel1);
            // 
            // MainScreenSpliter.Panel2
            // 
            MainScreenSpliter.Panel2.BackColor = Color.FromArgb(0, 122, 204);
            MainScreenSpliter.Size = new Size(1099, 934);
            MainScreenSpliter.SplitterDistance = 297;
            MainScreenSpliter.SplitterWidth = 5;
            MainScreenSpliter.TabIndex = 0;
            MainScreenSpliter.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(controleCalque1);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(297, 934);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.WrapContents = false;
            // 
            // controleCalque1
            // 
            controleCalque1.BackColor = Color.White;
            controleCalque1.BorderStyle = BorderStyle.FixedSingle;
            controleCalque1.Location = new Point(3, 3);
            controleCalque1.MaximumSize = new Size(290, 215);
            controleCalque1.MinimumSize = new Size(290, 215);
            controleCalque1.Name = "controleCalque1";
            controleCalque1.Padding = new Padding(5);
            controleCalque1.Size = new Size(290, 215);
            controleCalque1.TabIndex = 0;
            // 
            // MainScreen
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1109, 1074);
            Controls.Add(MainScreenSpliter);
            Controls.Add(separation);
            Controls.Add(MainTopBar);
            FormBorderStyle = FormBorderStyle.None;
            MainMenuStrip = menuStrip1;
            Name = "MainScreen";
            Padding = new Padding(5);
            Text = "MainScreen";
            MainTopBar.ResumeLayout(false);
            panelArrondi1.ResumeLayout(false);
            panelArrondi1.PerformLayout();
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            TitleBar.ResumeLayout(false);
            TitleBar.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ModifyWindowPanel.ResumeLayout(false);
            MainScreenSpliter.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainScreenSpliter).EndInit();
            MainScreenSpliter.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel separation;
        private TableLayoutPanel MainTopBar;
        private SplitContainer MainScreenSpliter;
        private TableLayoutPanel TitleBar;
        private Button button1;
        private Button button2;
        private Button button3;
        private FlowLayoutPanel ModifyWindowPanel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fichierToolStripMenuItem;
        private ToolStripMenuItem editionToolStripMenuItem;
        private ToolStripMenuItem affichageToolStripMenuItem;
        private ToolStripMenuItem optionToolStripMenuItem;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem selectToolStripMenuItem;
        private ToolStripMenuItem zoomToolStripMenuItem;
        private ToolStripMenuItem shapeToolStripMenuItem;
        private ToolStripMenuItem simpleShapesToolStripMenuItem;
        private ToolStripMenuItem rectangleToolStripMenuItem;
        private ToolStripMenuItem ellipseToolStripMenuItem;
        private ToolStripMenuItem freehandShapeToolStripMenuItem;
        private PanelArrondi panelArrondi1;
        private FlowLayoutPanel flowLayoutPanel1;
        private ControleCalque controleCalque1;
        private ToolStripMenuItem nouveauToolStripMenuItem;
        private ToolStripMenuItem ouvrirToolStripMenuItem;
        private ToolStripMenuItem sauvegarderToolStripMenuItem;
    }
}
