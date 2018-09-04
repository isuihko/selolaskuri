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

        public string[] TestaustaVartenVastustajia = {
            // Add some data (uncomplete and complete) to help running couple of test cases for window captures
            "5,1996,,10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684",
            "5,1996,,10½ 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684",

            // Also Miettimisaika enint. 10 min, nykyinen SELO 1996, pelimäärä tyhjä
            "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684",

            "90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966",
            // Also Miettimisaika väh. 90 min, nykyinen SELO 1525, pelimäärä 0
            "+1525 +1441 -1973 +1718 -1784 -1660 -1966",

            "90,1683,2,1973,0",
            // Also Miettimisaika väh. 90 min, nykyinen SELO 1683, pelimäärä 2, ottelun tulos 0=tappio
            "1973",

            "90,1713,3,1718,1",

            "90,1713,3,1718,½",

            // Lisää Joukkuepikashakin SM-kisoista otteluita
            // Esimerkki Joukkuepikashakin SM 2018 alkukilpailut, alkukilpailuryhmä C  4.8.2018, LauttSSK 1 pöytä 1
            // Kilpailuryhmä C: http://www.shakki.net/cgi-bin/selo?do=turnaus&turnaus_id=5068
            "5,2180,2054,14.5 1914 2020 1869 2003 2019 1979 2131 2161 2179 2392 1590 1656 1732 1944 1767 1903 1984 2038 2083 2594 2324 1466 1758",
            // Esimerkki Joukkuepikashakin SM 2018 alkukilpailut, alkukilpailuryhmä C  4.8.2018, LauttSSK 1 pöytä 4
            // Kilpailuryhmä C: http://www.shakki.net/cgi-bin/selo?do=turnaus&turnaus_id=5068
            "5,2045,1225,19.5 1548 1560 1699 1737 1735 1880 1856 2019 2102 2177 1539 1531 1672 1592 1775 1842 1847 1905 1970 2308 1988 1454 1481",
            // Esimerkki Joukkuepikashakin SM 2018 sijoituskilpailut 5.8.2018, sijoitusryhmä C, LauttSSK 4 pöytä 4
            // Sijoitusryhmä 5: http://www.shakki.net/cgi-bin/selo?do=turnaus&turnaus_id=5068
            "5,1262,,11 1623 1591 1318 1560 1493 1417 1343 1493 1524 1227 1716 1490 1454 1479 1329 1429 1444 1289 1576 1445 1280"
        };

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
