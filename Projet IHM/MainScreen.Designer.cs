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
            editionToolStripMenuItem = new ToolStripMenuItem();
            affichageToolStripMenuItem = new ToolStripMenuItem();
            optionToolStripMenuItem = new ToolStripMenuItem();
            ModifyWindowPanel = new FlowLayoutPanel();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            separation = new Panel();
            MainScreenSpliter = new SplitContainer();
            MainTopBar.SuspendLayout();
            panelArrondi1.SuspendLayout();
            menuStrip2.SuspendLayout();
            TitleBar.SuspendLayout();
            menuStrip1.SuspendLayout();
            ModifyWindowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainScreenSpliter).BeginInit();
            MainScreenSpliter.SuspendLayout();
            SuspendLayout();
            // 
            // MainTopBar
            // 
            MainTopBar.ColumnCount = 1;
            MainTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            MainTopBar.Controls.Add(panelArrondi1, 0, 1);
            MainTopBar.Controls.Add(TitleBar, 0, 0);
            MainTopBar.Dock = DockStyle.Top;
            MainTopBar.Location = new Point(0, 0);
            MainTopBar.Name = "MainTopBar";
            MainTopBar.RowCount = 2;
            MainTopBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            MainTopBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            MainTopBar.Size = new Size(1109, 125);
            MainTopBar.TabIndex = 0;
            MainTopBar.Margin = new Padding(0);
            // 
            // panelArrondi1
            // 
            panelArrondi1.BackColor = Color.Gray;
            panelArrondi1.Controls.Add(menuStrip2);
            panelArrondi1.Dock = DockStyle.Fill;
            panelArrondi1.Location = new Point(3, 53);
            panelArrondi1.Name = "panelArrondi1";
            panelArrondi1.Padding = new Padding(15, 5, 15, 5);
            panelArrondi1.Size = new Size(1103, 69);
            panelArrondi1.TabIndex = 0;
            // 
            // menuStrip2
            // 
            menuStrip2.Dock = DockStyle.Fill;
            menuStrip2.ImageScalingSize = new Size(24, 24);
            menuStrip2.Items.AddRange(new ToolStripItem[] { selectToolStripMenuItem, zoomToolStripMenuItem, shapeToolStripMenuItem });
            menuStrip2.Location = new Point(15, 5);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(1073, 59);
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
            shapeToolStripMenuItem.Size = new Size(85, 54);
            shapeToolStripMenuItem.Text = "Shapes";
            // 
            // simpleShapesToolStripMenuItem
            // 
            simpleShapesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { rectangleToolStripMenuItem, ellipseToolStripMenuItem });
            simpleShapesToolStripMenuItem.Name = "simpleShapesToolStripMenuItem";
            simpleShapesToolStripMenuItem.Size = new Size(270, 34);
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
            freehandShapeToolStripMenuItem.Size = new Size(270, 34);
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
            TitleBar.Padding = new Padding(0, 0, 0, 5);
            TitleBar.Name = "TitleBar";
            TitleBar.RowCount = 1;
            TitleBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TitleBar.Size = new Size(1109, 50);
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
            menuStrip1.Size = new Size(974, 50);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.MouseDown += panelTitre_MouseDown;
            // 
            // fichierToolStripMenuItem
            // 
            fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            fichierToolStripMenuItem.Size = new Size(78, 46);
            fichierToolStripMenuItem.Text = "Fichier";
            // 
            // editionToolStripMenuItem
            // 
            editionToolStripMenuItem.Name = "editionToolStripMenuItem";
            editionToolStripMenuItem.Size = new Size(83, 46);
            editionToolStripMenuItem.Text = "Edition";
            // 
            // affichageToolStripMenuItem
            // 
            affichageToolStripMenuItem.Name = "affichageToolStripMenuItem";
            affichageToolStripMenuItem.Size = new Size(103, 46);
            affichageToolStripMenuItem.Text = "Affichage";
            // 
            // optionToolStripMenuItem
            // 
            optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            optionToolStripMenuItem.Size = new Size(84, 46);
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
            ModifyWindowPanel.Location = new Point(974, 0);
            ModifyWindowPanel.Margin = new Padding(0);
            ModifyWindowPanel.Name = "ModifyWindowPanel";
            ModifyWindowPanel.Size = new Size(150, 50);
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
            separation.Height = 5;
            separation.Dock = DockStyle.Top;
            // 
            // MainScreenSpliter
            // 
            MainScreenSpliter.Dock = DockStyle.Fill;
            MainScreenSpliter.Location = new Point(0, 125);
            MainScreenSpliter.Name = "MainScreenSpliter";
            MainScreenSpliter.Margin = new Padding(0);
            MainScreenSpliter.Padding = new Padding(0);
            MainScreenSpliter.Size = new Size(1109, 949);
            MainScreenSpliter.SplitterDistance = 300;
            MainScreenSpliter.TabIndex = 0;
            MainScreenSpliter.SplitterWidth = 5;
            MainScreenSpliter.TabStop = false;
            MainScreenSpliter.IsSplitterFixed = true;
            // 
            // MainScreenSpliter.Panel2
            // 
            MainScreenSpliter.Panel2.BackColor = Theme.Accentuation;
            // 
            // MainScreen
            // 
            Padding = new Padding(5);
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1109, 1074);
            Controls.Add(MainScreenSpliter);
            Controls.Add(separation);
            Controls.Add(MainTopBar);
            FormBorderStyle = FormBorderStyle.None;
            MainMenuStrip = menuStrip1;
            Name = "MainScreen";
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
            ((System.ComponentModel.ISupportInitialize)MainScreenSpliter).EndInit();
            MainScreenSpliter.ResumeLayout(false);
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
    }
}
