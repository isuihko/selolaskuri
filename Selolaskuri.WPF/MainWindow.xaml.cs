using System.Deployment.Application;
using System.Windows;

namespace Selolaskuri.WPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow()
        {
            InitializeComponent();

            // Set default here. If set in MainWindow.xaml, would call Checked routine too early, before the window is ready.
            miettimisaika_vah90_btn.IsChecked = true;

            // Version text to be shown in window bottom right corner
            Versio.Text = "C#/.NET/WPF 19.2.2020 github.com/isuihko/selolaskuri";
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
