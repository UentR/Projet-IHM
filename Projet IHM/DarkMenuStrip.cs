using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_IHM
{
    public class DarkMenuStrip1 : MenuStrip
    {
        public DarkMenuStrip1()
        {
            Renderer = new DarkMenuRenderer(new MenuCouleursPlates());
        }
    }

    public class DarkMenuStrip2 : MenuStrip
    {
        public DarkMenuStrip2()
        {
            this.Renderer = new DarkMenuRenderer(new MenuPiluleCouleurs());
        }
    }

    internal class DarkMenuRenderer : ToolStripProfessionalRenderer
    {
        public DarkMenuRenderer(ProfessionalColorTable table) : base(table) { }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Color.White;

            base.OnRenderItemText(e);
        }
    }

    internal class MenuPiluleCouleurs : ProfessionalColorTable
    {
        private Color fondPilule = ColorTranslator.FromHtml("#2D2D30");
        private Color survolPilule = ColorTranslator.FromHtml("#3E3E42");

        public override Color MenuStripGradientBegin => fondPilule;
        public override Color MenuStripGradientEnd => fondPilule;
        public override Color ToolStripGradientBegin => fondPilule;
        public override Color ToolStripGradientEnd => fondPilule;

        public override Color MenuBorder => Color.Transparent;
        public override Color ToolStripBorder => Color.Transparent;

        public override Color MenuItemSelected => survolPilule;
        public override Color MenuItemSelectedGradientBegin => survolPilule;
        public override Color MenuItemSelectedGradientEnd => survolPilule;
        public override Color MenuItemBorder => Color.Transparent;
        public override Color MenuItemPressedGradientBegin => survolPilule;
        public override Color MenuItemPressedGradientEnd => survolPilule;
        public override Color ToolStripDropDownBackground => fondPilule;

        public override Color ImageMarginGradientBegin => fondPilule;
        public override Color ImageMarginGradientMiddle => fondPilule;
        public override Color ImageMarginGradientEnd => fondPilule;
    }

    public class MenuCouleursPlates : ProfessionalColorTable
    {
        public override Color MenuStripGradientBegin => ColorTranslator.FromHtml("#1E1E1E");
        public override Color MenuStripGradientEnd => ColorTranslator.FromHtml("#1E1E1E");

        public override Color MenuItemSelectedGradientBegin => ColorTranslator.FromHtml("#007ACC"); 
        public override Color MenuItemSelectedGradientEnd => ColorTranslator.FromHtml("#007ACC");

        public override Color MenuItemPressedGradientBegin => ColorTranslator.FromHtml("#007ACC");
        public override Color MenuItemPressedGradientEnd => ColorTranslator.FromHtml("#007ACC");

        public override Color ToolStripDropDownBackground => ColorTranslator.FromHtml("#1E1E1E");

        public override Color MenuItemSelected => ColorTranslator.FromHtml("#007ACC");

        public override Color MenuBorder => ColorTranslator.FromHtml("#2D2D30");
        public override Color MenuItemBorder => Color.Transparent;
        public override Color ToolStripBorder => Color.Transparent;

        public override Color ImageMarginGradientBegin => ColorTranslator.FromHtml("#1E1E1E");
        public override Color ImageMarginGradientMiddle => ColorTranslator.FromHtml("#1E1E1E");
        public override Color ImageMarginGradientEnd => ColorTranslator.FromHtml("#1E1E1E");
    }

}
