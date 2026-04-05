namespace Projet_IHM
{
    public partial class MainScreen : Form
    {
        public MainScreen()
        {
            InitializeComponent();
            Modele modele = new Modele();
            ZoneDessin zoneDessin = new ZoneDessin(modele);
            Controls.Add(zoneDessin);
        }
    }
}
