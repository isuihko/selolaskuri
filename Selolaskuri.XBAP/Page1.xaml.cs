
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

            vastustajanSelo_comboBox.Width = 725;

            Versio.Text = "C#/.NET/WPF/XBAP 6.9.2018 https://isuihko.github.io/";
        }
    }
}
