
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
        }
    }
}
