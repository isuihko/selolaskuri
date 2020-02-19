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
        private readonly Vakiot.Selolaskuri_enum selolaskurisovellus; // käytetty tietoikkunassa NaytaTietoaOhjelmasta()

        public FormOperations(Vakiot.Selolaskuri_enum sovellus)
        {
            this.selolaskurisovellus = sovellus;  // WinForms, WPF_XAML, XBAP_WEB
        }

        // Anna Vastustajat-kenttään komento test 
        public readonly string[] TestaustaVartenVastustajia = {
            // Add some data (uncomplete and complete) to help running couple of test cases for window captures
            "5,1996,,10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684",
            "5,1996,,10½ 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684",

            // Also Miettimisaika enint. 10 min, nykyinen SELO 1996, pelimäärä tyhjä
            "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684",

            "90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966",
            "90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966 +1321",

            // 11 peliä, uuden pelaajan laskenta, tästä tulos 1660 eli oikein!
            "90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966 +1321 -1678 -1864 -1944",
            // lisäksi neljä peliä, joissa normaali laskenta, aloitetaan vahvuusluvusta 1660, tulos 1682, virallinen 1683
            "90,1660,11,-1995 +1695 -1930 1901",

            // UUSI formaatti, jossa ensin uuden pelaajan selon laskenta ja sitten merkin '/' jälkeen normaali laskenta
            // Ottelujen tulokset on annettava vastustajan selon kanssa, tulos 1683 on oikein
            "90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966 +1321 -1678 -1864 -1944 / -1995 +1695 -1930 1901",

            // Also Miettimisaika väh. 90 min, nykyinen SELO 1525, pelimäärä 0
            "+1525 +1441 -1973 +1718 -1784 -1660 -1966",
            "+1525 +1441 -1973 +1718 -1784 -1660 -1966 +1321",

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

        //
        // Instruction and information window texts
        //
        private static readonly string[] ohjeita = {
            "Shakin vahvuusluvun laskenta SELO/PELO",
            "myös uuden pelaajan vahvuusluku",
            "",
            "Annettavat tiedot:",
            "1) Miettimisaika",
            "2) Oma vahvuusluku",
            "3) Oma pelimäärä (0-10 jos uusi pelaaja tai tyhjä)", 
            "4) Vastustajan/vastustajien vahvuusluvut (tuloksineen)",
            "5) Ottelun tulos, jos vain yksi vastustaja",
            "",
            "Tarkemmin:",
            "1) Miettimisaika määrää käytettävän laskentakaavan. Jos 10 minuuttia, niin PELO-laskenta.",
            "2) Oma SELO, paitsi jos ei ole yhtään peliä pelattuna, on laitettava 1525",
            "3) Oma pelimäärä vaikuttaa vain, jos on enintään 10, jolloin käytetään uuden pelaajan laskentakaavaa.",

            "",
            "Laskennan vaihtoehdot:",
            "- Yksi vastustaja ja yksi tulos:",
            "  Annetaan vastustajan vahvuusluku (4) ja valitaan tulos 1, ½ tai 0 radiobuttoneista (5)",
            "",
            "- Monta vastustajaa (esim. turnaus) ja tulokset eri tavalla:",
            "  Vastustajat-kenttä:",
            "   1) Vahvuusluvut tuloksineen, esim. +1505 =1600 -1611 +1558, jossa + voitto, = tasan ja - tappio (myös 1600 tasapeli)",
            "   2) Turnauksen pistemäärä ja vastustajien vahvuusluvut, esim. 1,5 1505 1600 1611 1558",
            "",
            "Tämän lisäksi voidaan käyttää CSV-formaattia (pilkulla erotettu arvot)",
            "   Arvojen lkm 2, 3, 4 tai 5. Tuloksissa käytetään desimaalipistettä esim. 2.5",
            "     2: oma selo,ottelut",
            "     3: oma selo,pelimaara,ottelut",
            "     4: minuutit,oma selo,pelimaara,ottelut",
            "     5: minuutit,oma selo,pelimaara,ottelu,tulos",
            " Näistä esimerkit:",
            "     2: 1712,2.5 1505 1600 1611 1558 tai 1712,+1505 1600 -1611 +1558",
            "     3: 1525,0,+1505 +1441",
            "     4: 90,1525,0,+1525 +1441 tai 90,1525,,2.5 1505 1600 1611 1558",
            "     5: 90,1683,2,1973,0 (tasapeli voidaan antaa 1/2, ½ tai 0.5)",
            "",
            "   Jos miettimisaika on antamatta, käytetään ikkunasta valittua",
            "   Jos pelimäärä on antamatta, käytetään tyhjää",
            "",
            "HUOM! CSV-formaatissa annetut tiedot ovat etusijalla, jolloin lomakkeen tiedoilla ei merkitystä.",
            "",
            "Laskenta suoritetaan klikkaamalla laskenta-painiketta tai painamalla Enter vastustajan SELO-kentässä tai (jos yksi vastustaja) tuloksen valinta -radiobuttoneilla.",
            "",
            "Käytä tulosta jatkolaskennassa -painike:",
            "  Jos haluat jatkaa laskentaa lasketulla vahvuusluvulla, klikkaa Käytä tulosta jatkolaskennassa.",
            "  Jos ei ole vielä ollut laskentaa, saadaan tällä uuden pelaajan oletusarvot SELO 1525 ja pelimäärä 0.",
            "",
            "Laskenta voidaan aloittaa uuden pelaajan laskennalla ja jatkaa normaalilla laskennalla.",
            "Erota Vastustajat-kentässä vahvuusluvut oikeassa kohdassa '/' -merkillä.",
            "Alkuperäisen pelimäärän on oltava enintään 10 ja pelimäärän pitää laskennan vaihdossa olla vähintään 11.",
            "Kunkin ottelun tulos on annettava vahvuusluvun yhteydessä,",
            "Esim. jos alkup. pelimäärä on enintään 10, niin tässä 3 peliä lisää turnauksesta, ja sitten vaihtuu laskentakaava",
            "     1512 +1505 1600 / -1611 +1558",
            "",
            "Suoritusluvun laskenta",
            "Lasketaan kolme suorituslukua: vahvuusluku, jolla odotustulos vastaa saatua pistemäärää, FIDE ja lineaarinen.",
            "FIDE:n suorituslukulaskennasta saadaan uuden pelaajan ELO, kun pelejä on vähintään 5, joista on saatu vähintään 1/2 pistettä",
            "",
            "Vastustajat-kentän extrat: komento test lisää valintalistaan laskentaa. Komento clear nollaa valintalistan.",

        };

        private static readonly string[] laskentakaavat = {           
            "Shakin vahvuusluvun laskentakaavat",
            "            http://skore.users.paivola.fi/selo.html", // XXX: TARKISTA
            
            "Lisätietoa: http://www.shakkiliitto.fi/",
            "            http://www.shakki.net/cgi-bin/selo",
            "            http://www.shakki.net/pelaaminen/vahvuus",
            "Selo- ja suorituslukulaskuri:",
            "            http://shakki.kivij.info/performance_calculator.shtml",
            "Tietoa suoritusluvuista:",
            "            http://shakki.kivij.info/performance_formulas.shtml",
            "Vahvuusluvun laskenta (ei uusi pelaaja):",
            "            http://www.shakki.net/kerhot/salsk/ohjelmat/selo.html",
            "ELO-luku",
            "            https://fi.wikipedia.org/wiki/Elo-luku"
        };

        private static readonly string[] tietoaOhjelmasta = {
            "Shakin vahvuusluvun laskenta",
            "",
            "C#/.NET-lähdekoodit ja asennusohjelma",
            "            https://github.com/isuihko/selolaskuri",
            "Sisältää WinForms-, WPF/XAML- että XBAP/web-versioiden lähdekoodit.",
            "",
            "IE-selaimella Windows 7/8/10:ssä toimiva XBAP/web-versio:",
            "            https://isuihko.github.io/index.html",
            "",
            "Lisäksi on Java-versio",
            "            https://github.com/isuihko/jSelolaskuri"
        };

        public void NaytaOhjeita()
        {
            string tekstit = "";
            for (int i = 0; i < ohjeita.Length; i++)
                tekstit += ohjeita[i] + Environment.NewLine;
            MessageBox.Show(tekstit, "Ohjeita");
        }

       public void NaytaLaskentakaavat()
        {
            string tekstit = "";
            for (int i = 0; i < laskentakaavat.Length; i++)
                tekstit += laskentakaavat[i] + Environment.NewLine;
            MessageBox.Show(tekstit, "Laskentakaavat");
        }

        // Parameter PublishVersion: information needs to be fetched in main program level
        public void NaytaTietoaOhjelmasta(string PublishVersion)
        {
            string kayttoliittyma = "";

            switch (this.selolaskurisovellus) {  // set by constructor
                case Vakiot.Selolaskuri_enum.WINFORMS:
                    kayttoliittyma = " WinForms";
                    break;
                case Vakiot.Selolaskuri_enum.WPF_XAML:
                    kayttoliittyma = " WPF/XAML";
                    break;
                case Vakiot.Selolaskuri_enum.XBAP_WEB:
                    kayttoliittyma = " WPF/XAML XBAP/web";
                    break;
            }

            string tekstit = "";
            for (int i = 0; i < tietoaOhjelmasta.Length; i++)
                tekstit += tietoaOhjelmasta[i] + Environment.NewLine;
            MessageBox.Show(tekstit, "Selolaskuri C#/.NET" + kayttoliittyma + (PublishVersion != null ? " v. " + PublishVersion : " (not published/installed)"));
        }
    }
}
