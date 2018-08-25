//
// Selolaskuri-projektin Main()
//
// Luotu 17.11.2017 Ismo Suihko
//
// Muutettu
//  10.6.2018 Lomakkeella nyt uusi nimi: Form1 -> SelolaskuriForm (SelolaskuriForm.cs)
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Selolaskuri
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SelolaskuriForm());
        }
    }
}
