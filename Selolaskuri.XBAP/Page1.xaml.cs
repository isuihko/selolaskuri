using System.Deployment.Application;
using System.Windows.Controls;

namespace Selolaskuri.XBAP {
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page {
        public Page1()
        {
            InitializeComponent();

            // Set default here. If set in MainWindow.xaml, would call Checked routine too early, before the window is ready.
            miettimisaika_vah90_btn.IsChecked = true;

            // Clipboard can't be used in XBAP/web version or can it?
            cutVastustajatToolStripMenuItem.IsEnabled = false;
            copyVastustajatToolStripMenuItem.IsEnabled = false;
            pasteVastustajatToolStripMenuItem.IsEnabled = false;

            // Can't exit the web version, but you can close the window
            suljeToolStripMenuItem.IsEnabled = false;

            // You can't resize the form field here in XBAP version like in normal WPF/XAML version,
            // so set it wider already in startup
            vastustajanSelo_comboBox.Width = 725;

            // Version text to be shown in window bottom right corner
            Versio.Text = "C#/.NET/WPF/XBAP 9.9.2018 https://isuihko.github.io/";
        }

        // Publish version needs to be checked in main program class. Shown in Menu->Tietoa ohjelmasta
        public static string GetPublishVersion()
        {
            string version = null;
            try {
                // get deployment version, e.g. 1.0.0.12
                version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            catch (InvalidDeploymentException) {
                // you cannot read publish version when app isn't installed (e.g. when testing in Visual Studio)
            }
            return version;
        }
    }
}
