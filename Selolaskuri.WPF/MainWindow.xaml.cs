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

            Versio.Text = "C#/.NET/WPF 5.9.2018 github.com/isuihko/selolaskuri";
        }
    }
}
