//
// Selolaskuri-projektin Main()
//
// Luotu 17.11.2017 Ismo Suihko
//
// Muutettu
//  10.6.2018 Lomakkeella nyt uusi nimi: Form1 -> SelolaskuriForm (SelolaskuriForm.cs)
//  9.9.2018  GetPublishVersion
//

using System;
using System.Deployment.Application;
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
