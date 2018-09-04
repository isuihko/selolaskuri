//
// FormOperations.cs
//
// General operations used from Selolaskuri, Selolaskuri.WPF (WPF/XAML version) and Selolaskuri.XBAP (WPF/XAML web version)
//
// TODO: Check what could be moved here from SelolaskuriForm.cs, MainWindow.operations.cs and Page1.operations.cs.
//

using System;
using System.Windows.Forms;

namespace SelolaskuriLibrary {
    public class FormOperations {
        public void NaytaOhjeita()
        {
            MessageBox.Show("Shakin vahvuusluvun laskenta SELO ja PELO"
                + Environment.NewLine
                + Environment.NewLine + "Annettavat tiedot:"
                + Environment.NewLine
                + Environment.NewLine + "-Miettimisaika. Pitkä peli (väh. 90 minuuttia) on oletuksena. Jos valitset enint. 10 minuuttia, lasketaan pikashakin vahvuuslukua (PELO)"
                + Environment.NewLine + "-Oma vahvuusluku"
                + Environment.NewLine + "-Oma pelimäärä, joka tarvitaan vain jos olet pelannut enintään 10 peliä. Tällöin käytetään uuden pelaajan laskentakaavaa."
                + Environment.NewLine + "-Vastustajien vahvuusluvut ja tulokset jollakin neljästä tavasta:"
                + Environment.NewLine + "   1) Yhden vastustajan vahvuusluku (esim. 1922) ja lisäksi ottelun tulos 1/½/0 nuolinäppäimillä tai hiirellä. Laskennan tulos päivittyy valinnan mukaan."
                + Environment.NewLine + "   2) Vahvuusluvut tuloksineen, esim. +1505 =1600 -1611 +1558, jossa + voitto, = tasan ja - tappio"
                + Environment.NewLine + "   3) Turnauksen pistemäärä ja vastustajien vahvuusluvut, esim. 1.5 1505 1600 1611 1558, voi käyttää myös desimaalipilkkua 1,5 1505 1600 1611 1558 sekä puolikasta esim. 1½ 1505 1600 1611 1558"
                + Environment.NewLine + "   4) CSV eli pilkulla erotetut arvot, jossa 2, 3, 4 tai 5 kenttää: HUOM! Käytä tuloksissa desimaalipistettä, esim. 0.5 tai 10.5, tai puolikasta eli ½ tai 10½"
                + Environment.NewLine + "           2: oma selo,ottelut   esim. 1712,2.5 1505 1600 1611 1558 tai 1712,+1505  HUOM! Desimaalipiste!"
                + Environment.NewLine + "           3: oma selo,pelimaara,ottelut esim. 1525,0,+1505 +1441"
                + Environment.NewLine + "           4: minuutit,oma selo,pelimaara,ottelut  esim. 90,1525,0,+1525 +1441"
                + Environment.NewLine + "           5: minuutit,oma selo,pelimaara,ottelu,tulos esim. 90,1683,2,1973,0 (jossa tasapeli voidaan antaa 1/2, ½ tai 0.5)"
                + Environment.NewLine + "      Jos miettimisaika on antamatta, käytetään ikkunasta valittua"
                + Environment.NewLine + "      Jos pelimäärä on antamatta, käytetään tyhjää"
                + Environment.NewLine
                + Environment.NewLine + "   HUOM! CSV-formaatissa annettu ottelu on etusijalla ja lomakkeesta käytetään korkeintaan miettimisaikaa (vain jos se puuttui CSV:stä)."
                + Environment.NewLine
                + Environment.NewLine + "Laskenta suoritetaan klikkaamalla laskenta-painiketta tai painamalla Enter vastustajan SELO-kentässä sekä (jos yksi vastustaja) tuloksen valinta -painikkeilla."
                + Environment.NewLine
                + Environment.NewLine + "Jos haluat jatkaa laskentaa uudella vahvuusluvulla, klikkaa Käytä tulosta jatkolaskennassa. Jos ei ole vielä ollut laskentaa, saadaan uuden pelaajan oletusarvot SELO 1525 ja pelimäärä 0.",

                "Ohjeita");
        }

        public void NaytaLaskentakaavat()
        {
            MessageBox.Show("Shakin vahvuusluvun laskentakaavat: http://skore.users.paivola.fi/selo.html"
                + Environment.NewLine + "Lisätietoa: http://www.shakkiliitto.fi/ ja http://www.shakki.net/cgi-bin/selo",
                "Laskentakaavat");
        }

        // is the version number shown correctly?
        public void NaytaTietoaOhjelmasta(Vakiot.Selolaskuri_enum selolaskuri)
        {
            string versionumero = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            string kayttoliittyma = "undefined";

            switch (selolaskuri) {
                case Vakiot.Selolaskuri_enum.WinForms:
                    kayttoliittyma = "WinForms";
                    break;
                case Vakiot.Selolaskuri_enum.WPF_XAML:
                    kayttoliittyma = "WPF/XAML";
                    break;
                case Vakiot.Selolaskuri_enum.XBAP:
                    kayttoliittyma = "WPF/XAML XBAP/web";
                    break;
            }

            MessageBox.Show("Shakin vahvuusluvun laskenta" + " v." + versionumero
                + Environment.NewLine
                + Environment.NewLine + "Ohjelmointikieli C#/.NET, käyttöliittymä " + kayttoliittyma
                + Environment.NewLine + "Lähdekoodit ja asennusohjelma https://github.com/isuihko/selolaskuri"
                + Environment.NewLine + "Sisältää WinForms-, WPF/XAML- että XBAP/web-versioiden lähdekoodit."
                + Environment.NewLine + "IE-selaimella toimiva XBAP/web-versio: https://isuihko.github.io/index.html"
                + Environment.NewLine + "Lisäksi on Java-versio https://github.com/isuihko/jSelolaskuri",
                "Tietoa Selolaskurista");
        }
    }
}
