//
// Selolaskuri   https://github.com/isuihko/selolaskuri
//
// 1.4.2018 Ismo Suihko 1.0.0.12
//
// Aiempi versio 7.1.2018 1.0.0.12, johon verrattuna tässä ei ole ulkoisia muutoksia muuta kuin päivämäärä.
//
// C#/.NET, Visual Studio Community 2015, Windows 7.
//
// Ensimmäinen C#/.NET -ohjelmani.
// Sisältää käyttöliittymän, syötteen tarkistuksen, laskentaa ja tuloksien näyttämisen.
//
// HUOM!  Käännetty binääri/asennusohjelma setup.exe vaatii nyt .NET Framework 4.6:n.
//        Voiko tässä Community-versiossa muuttaa asetuksia, niin että toimisi
//        kaikilla versioilla esim. .NET Framework 4.0:sta alkaen?
//
// KOODIA JÄRJESTELLÄÄN JA OPTIMOIDAAN VIELÄ, MUTTA TÄMÄN PITÄISI NYT TOIMIA AIKA HYVIN.
// Tein ohjelmasta vastaavan version myös Javalla, mutta siitä puuttuu osa uusista ominaisuuksista (lista syötteistä ja menut).
//
// Kuva version 1.0.0.12 näytöstä on linkissä
//   https://goo.gl/pSVZcU ( https://drive.google.com/open?id=1e4z34Rh2YOz5xC8G2fDOK4x9__r-qB5n )
// Jossa esimerkki: Miettimisika enintään 10 minuuttia, selo 1996, pelimäärä tyhjä
//                  Vastustajan SELO: 10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684
//                  Tulos: 2033 +37 (jos lasketaan miettimisajalla väh. 90 min, saadaan eri tulos 2048)
//  Toinen esimerkkikuva, Uuden pelaajan selo, miettimisaika väh. 90 min.
//                  Oma SELO 1525, pelimäärä 0, vastustajat: +1525 +1441 -1973 +1718 -1784 -1660 -1966
//                  Tulos 1695 +170
//
//
// Laskee pelaajalle vahvuusluvun miettimisaikojen vähintään 90, 60-89 ja 11-59 minuutin peleistä,
// sekä pikapelistä, kun miettimisaika on enintään 10 minuuttia.
//
// Jos pelimäärä 0-10, käytetään uuden pelaajan selon kaavaa. Jos pelataan turnaus, niin kaikissa turnauksen otteluissa.
// Jos pelimäärä tyhjä tai > 10, käytetään normaalia laskukaavaa.
//
// Lomakkeella kentät:
//        Valintanapit:  Miettimisaika: väh. 90 min, 60-89 min, 15-59 min ja alle 15 min.
//
//        Numerot: Nykyinen oma SELO   (1000-2999)
//                 Oma pelimäärä       (tyhjä tai numero 0-9999)
//                 vastustajan SELO    (1000-2999) tai jos turnaus, niin
//                                     useampi SELO tuloksineen, esim. +1622 -1880 =1633
//                                     tai 1.5 1622 1880 1683
//        Valintanapit: ottelun tulos, jossa vaihtoehdot:  0, 1/2 ja 1.
//
//        Vastustajan SELO-kenttä muistaa laskennassa käytetyt syötteet (comboBox-kenttä).
//        Syöte tallennetaan listaan kun on painettu kentässä Enter tai valittu Laske uusi SELO/PELO.
//
// Painikkeet:
//    - Laske uusi SELO     Uusi SELO lasketaan myös aina kun valitaan ottelun tulos.
//    - Käytä uutta SELOa jatkolaskennassa -> kopioi tuloksen & pelimäärän uutta laskentaa varten
//                                            Jos ei vielä laskettu, asettaa arvot 1525 ja 0.
//
// Kun valittu miettimisajaksi enintään 10 minuuttia, niin lomakkeen SELO-tekstit vaihdetaan -> PELO.
//
// Laskentakaavat:  http://skore.users.paivola.fi/selo.html  (odotustulos, kerroin, SELO)
// Käytössä on kaavat miettimisajoille väh. 90 min, 60-89 min ja 11-59 min
// sekä PELO:n laskenta enintään 10 min.
//
// Tulostiedot:
//                  Uusi SELO             laskettu uusi vahvuusluku
//                  pelimäärä             jos annettu, niin yhdellä lisättynä
//                  piste-ero             annettujen selojen ero (jos yksi vastustaja)
//                  odotustulos           voiton todennäköisyys/turnauksessa kaikkien odotustuloksien summa
//                  kerroin               laskennassa käytetty kerroin
//                  vastustajien keskivahvuus
//                  turnauksen tulos
//                  SELOn vaihteluväli, jos annettu useampi SELO tuloksineen +1622 -1880 =1633
//
// TODO:
//      -Onko muuttujien ja aliohjelmien nimeäminen ym. C#-käytännön mukaista?
//       Tämä on nyt tehty ilman erityisiä ohjeita. Tullaan tarkistamaan!
//
//      -Tarkista luokkien ja objektiläheisen ohjelmoinnin käyttö, onko parantamista?
//         ... voi ollakin, koska tämä on ensimmäinen oikea C#-ohjelmani
//
//      -Syötekenttien merkkimäärän rajoittaminen niin ettei voi laittaa satoja merkkejä
//      -Tarkista laskentakaavat vielä, esim. tarvitaanko pyöristää (+0.5) ennen kokonaisluvuksi muuttamista.
//
//
//
// Versiohistoria:
//
// 17.11.2017  Aloitettu. Kentät, virhetarkastukset ja laskennat OK eli toimii!
//  Publish --> Versio 1.0.0.0
//
// 21.11.2017  Ulkoasu: Laskettu SELO näytetään isommalla fontilla, koko: 8.25 -> 12.25
//             Pelimäärä-kentän voi jättää tyhjäksi, jolloin normaali laskenta eikä
//             silloin uuttakaan pelimäärää lasketa ja näytetä.
//             Estä ikkunan koon muuttaminen: leveys 290 ja korkeus 500 pikseliä.
//  Publish --> Versio 1.0.0.1
//
// 21.11.2017  Kommentteja säädetty. Ei muutosta toimintaan.
//
// 22.11.2017  Kommentteja, muuttujia ja virheilmoituksia säädetty. Optimointia.
//             Laskentapainike isommalla fontilla.
//             Ei muutosta toimintaan muuta kuin virheilmoituksiin.
//  Publish --> Versio 1.0.0.2
//             Lisätty virheellisen datan tarkastuksia:
//             Syötekenttien arvojen nollaukset, jos niissä on virheellistä dataa
//             ja merkkejä on yli MAX_LENGTH määrä. Esim. jos on copy+pastattu satoja merkkejä.
//             Jos kaikissa kentissä on virheellistä dataa, niin ne käsitellään ja niistä annetaan
//             virheilmoitukset vain yksi kerrallaan.
//             Myös näytä kentän virheellinen arvo punaisena virheilmoituksen ajan.
//  Publish --> Versio 1.0.0.3
//
// 24.11.2017  Muutettu lomaketta, jotta voitaisiin jatkossa syöttää useamman ottelun tulokset
//             -> Vastustajien SELOt (1000-2999 ja tulokset esim. +1725 =1622 -1810).
//             Lisätty tuloskentät: Vastustajien keskivahvuus ja Tulos.
//             Muutettu varsinainen laskenta omaksi aliohjelmakseen, jotta sitä voidaan kutsua
//             jokaiselle tulokselle erikseen.
//             TODO: Tulosrivin kaikkien otteluiden läpikäynti
//
// 25.11.2017  Aloitettu koodin refaktorointi: luokka pelaaja, jolla ominaisuuksina selo ja pelimaara jne.
//
// 26.11.2017  Ensiksi laitettu laskenta toimimaan niin kuin aiemmallakin versiolla eli yhden ottelun tuloksella.
//             Ja nyt osaa lukea useamman ottelun tuloksen ja laskea niistä vahvuusluvun, vastustajien keskiselon
//             ja myös turnauksen pistemäärän. Ottelut annetaan formaatissa +1716 -1822 =1681 +1444
//             Tasapeli voidaan antaa myös ilman '='-merkkiä eli myös OK: +1716 -1822 1681 +1444
//             Kun lasketaan useampaa ottelua, niin tuloksen valintanapeilla ei ole merkitystä eikä
//             näytetä matsikohtaisia seloeroja tai kertoimia. Koko turnauksen odotustulos näytetään.
//             Lisätty mahdollisuus valita eri miettimisajat: Vähintään 90 min, 60-89 min, 15-59 min.
//             -> Vanhan pelaajan laskennassa käytetään eri kertoimia.
//             Ulkoasun muokkausta: tekstit, fontin koko, ikkunan koko.
//  Publish --> Versio 1.0.0.4
//
// 27.11.2017 -Koodin siistimistä.
//            -Tulostuskenttien TabStop = False, koska niihin ei pidä päästä TAB-napilla.
//            -Enter painettu vastustajan SELO-kentässä -> suoritetaan laskenta ja jäädään kenttään.
//            -Kenttien TabIndex-numerot niin, että TAB:lla mennään syötekentät halutussa järjestyksessä
//            -Ulkoasun muokkausta: tekstit, fontin koko, ikkunan koko.
//  Publish --> Versio 1.0.0.5
//            -Tulostuksien säätöä.
//            -Korjattu laskentaa: Kun annettu monta tulosta, niin käytetään alkuperäisestä selosta laskettu odotustulosta.
//            -Lisätty mahdollisuus laskea tulos myös muodosta:
//                  ottelun_tulos selo selo selo, esim.  1.5 1722 1581 1608
//             jolloin laskennassa käytetään annettua tulosta, vastustajien selojen keskiarvoa ja odotustuloksien summaa.
//            -Syötekentistä poistetaan kaikki alussa ja lopussa olevat välilyönnit.
//            -VastustajanSelo-kentässä saa olla lukujen välissä vain yksi välilyönti, joten
//             ylimääräiset poistetaan, jotta kentässä olevat merkkijonot saadaan erotettua toisistaan oikein!
//            -Kommentoitu pois käyttämättömät using-määrittelyt.
//            Hm... laskenta ilmeisesti toimii! Ehkä jotain pyöristysongelmaa vielä. Ja alle 15 min puuttuu.
//  Publish --> Versio 1.0.0.6
//
// 28.11.2017 -Lisätty pikashakin vahvuusluvun laskenta (PELO) ja pidennetty ottelujen syöttökenttää
//             sillä pikashakissa pelataan useita otteluita, joskus parikymmentäkin.
//            .Kun valitaan miettimisajaksi alle 15 min, niin tekstit vaihdetaan SELO->PELO.
//            -Kun miettimisaika >= 15 minuuttia, niin palautetaan SELO-tekstit.
//            -Tekstien ja ulkoasun muokkaamista.
//  Publish --> Versio 1.0.0.7
//            -Vaihdettu ohjelman nimi ikkunasta "SELO-laskuri" -> "Selolaskuri".
//            -Myös namespace SELO_laskuri -> Selolaskuri, tiedostot: Form1.cs ja Program.cs
//            -Nyt käännetty ohjelma tulee nimelle "Selolaskuri.exe".
//  Publish --> Versio 1.0.0.8 -> github/isuihko/selolaskuri (ensimmäinen versio siellä)
//            -Pyöristyksiä: get_turnauksen_keskivahvuus() jakolaskuun +0.5F ennen kokonaisluvuksi muuttamista.
//            -Shakki.net:n selolaskentasivujen tuloksiin vertaamalla huomattu, että pikashakin laskentakaavaan 
//             tarvittiin korjauksia. Nyt laskee oikein!
//  Publish --> Versio 1.0.0.9, myös github
// 30.11.2017 -Oma luokka vakioille, jotta niitä ei tarvitse määritellä kaikissa luokissa.
//             Kaikki vakiot nyt ovat luokassa constants.
// 12.12.2017 -VastustajanSelo-kentästä tehty ComboBox, josta voidaan nähdä ja valita aiempiakin syötteitä
//               Korvattu kenttä vastustajanSelo_input kentällä vastustajanSelo_combobox.
//               Uusi teksti lisätään listaan silloin kun se on kelvollinen, eikä ole siellä ennestään.
//            -pelaaja_class muutettu nimelle seloPelaaja
//            -vakioiden luokan nimi muutetut -> vakiot (koska ohjelma on muutenkin suomenkielinen)
//            -muutettu käyttämään uudempaa .NET Framework 4.6:tta, oli 4.5.2.
//  Publish --> Versio 1.0.0.10, myös github
// 7.1.2018   -Käytä C# properties: selo, pelimaara, miettimisaika ja selo_alkuperainen.
//            -Turhat apumuuttujat pois, kun tulos voidaan sijoittaa suoraankin
//                Esim. nykyinenSelo_input.Text = nykyinenSelo_input.Text.Trim();
//            -Math.Min, Max ja Round käyttöön.
//            -Luokat omiin tiedostoihinsa: -> seloPelaaja.cs ja vakiot.cs
//            -Lisätty valikot:
//                   Menu -> Ohjeita
//                   Menu -> Tietoja ohjelmasta       
//                   Menu -> Laskentakaavat
//                   Menu -> Sulje ohjelma
//            -Varmistetaan ohjelmasta poistuminen, kun valitaan Menu->Sulje ohjelma tai suljetaan ikkuna.
//
//  Publish --> Versio 1.0.0.11, myös github
// 7.1.2018    -Päivitetty versionumerot -> 1.0.0.12
//  Publish --> Versio 1.0.0.12, myös github
//
// 1.4.2018   -Propertyjä lisätty -> int Muuttuja { get; set; } yms.
//            -koodin formatointia niin, että alkava aaltosulku tulee esim. if-lauseessa samalla rivillä
//            -Lisää vakioita, mm. ottelun tuloksen ja miettimisajan vaihtoehdot
//            -Jotain pientä järjestelyä
//            Ei muutoksia ohjelman toimintaan, joten ei julkaista uutta versiota.
//            Vain versionro ja päivämäärä muuttuisivat.
//
// TODO:
//            -Tarkista_vastustajanSelo() ym. rutiinit: parametreista pois "out".
//             kuitenkin int.TryParse() käyttää out.
//            -automaattinen testaus (Unit Test Project?), joka vaatinee hieman enemmän koodin muokkausta
//            -syötteen tarkastukset omaan luokkaansa?
//
//

using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;  // Regex rx, rx.Replace


namespace Selolaskuri
{
    // lomakkeen käsittely, mm. syötteen tarkastus ja laskennan kutsuminen

    public partial class Form1 : Form
    {

        // Luodaan shakinpelaaja, jolla tietoina mm. SELO ja pelimäärä.
        // Uudella pelaajalla SELO 1525, pelimäärä 0
        // Nämä arvot kopioidaan käyttöön, jos valitaan "Käytä uutta SELOa jatkolaskennassa"
        // ilman, että on suoritettu laskentaa sitä ennen.
        seloPelaaja shakinpelaaja = new seloPelaaja(1525, 0);

        public Form1()
        {
            InitializeComponent();
        }


        //  KENTTIEN TARKASTUKSET

        // Nämä valintapainikkeet ovat omana ryhmänään paneelissa
        // Käynnistyksessä valitaan oletukseksi vähintään 90 min.
        // Magic numbers:   90, 60, 15, 5
        private void tarkista_miettimisaika()
        {
            if (miettimisaika_vah90_Button.Checked)
                shakinpelaaja.miettimisaika = vakiot.Miettimisaika_vah90min;  // oletuksena
            else if (miettimisaika_60_89_Button.Checked)
                shakinpelaaja.miettimisaika = vakiot.Miettimisaika_60_89min;
            else if (miettimisaika_11_59_Button.Checked)
                shakinpelaaja.miettimisaika = vakiot.Miettimisaika_11_59min;
            else
                shakinpelaaja.miettimisaika = vakiot.Miettimisaika_enint10min;
        }

        // Tarkista Oma SELO -kenttä, oltava numero ja rajojen sisällä
        private bool tarkista_nykyinenselo(out int nykyinenSelo)
        {
            nykyinenSelo_input.Text = nykyinenSelo_input.Text.Trim();  // remove leading and trailing white spaces

            // onko numero?
            if (int.TryParse(nykyinenSelo_input.Text, out nykyinenSelo) == false) {
                nykyinenSelo = vakiot.MIN_SELO - 1;  // -> virheilmoitus
            }

            // onko kelvollinen numero?
            if (nykyinenSelo < vakiot.MIN_SELO || nykyinenSelo > vakiot.MAX_SELO) {
                string message = String.Format("VIRHE: Nykyisen SELOn oltava numero {0}-{1}.", vakiot.MIN_SELO, vakiot.MAX_SELO);
                nykyinenSelo_input.ForeColor = Color.Red;
                MessageBox.Show(message);
                nykyinenSelo_input.ForeColor = Color.Black;

                if (nykyinenSelo_input.Text.Length > vakiot.MAX_PITUUS)
                    nykyinenSelo_input.Text = "";  // tyhjennetään liian täysi kenttä
                nykyinenSelo_input.Select();       // takaisin syöttämään
                return false;
            }
            return true; // true = kenttä OK
        }

        //
        // tarkista pelimäärä
        // Saa olla tyhjä, mutta jos annettu, oltava numero, joka on 0-9999.
        // Käytetään uuden pelaajan laskentakaavaa, jos pelimäärä on 0-10.
        //
        bool tarkista_pelimaara(out int pelimaara)
        {
            pelimaara_input.Text = pelimaara_input.Text.Trim();  // remove leading and trailing white spaces

            if (string.IsNullOrWhiteSpace(pelimaara_input.Text)) {
                pelimaara = vakiot.MIN_PELIMAARA - 1; // OK
                return true;   // ei tarkisteta enempää
            }

            // onko numero?
            if (int.TryParse(pelimaara_input.Text, out pelimaara) == false) {
                pelimaara = vakiot.MIN_PELIMAARA - 1;   // -> virheilmoitus
            }

            // onko kelvollinen numero?
            if (pelimaara < vakiot.MIN_PELIMAARA || pelimaara > vakiot.MAX_PELIMAARA) {
                string message = String.Format("VIRHE: pelimäärän oltava numero 0-{0} tai tyhjä.", vakiot.MAX_PELIMAARA);
                pelimaara_input.ForeColor = Color.Red;
                MessageBox.Show(message);
                pelimaara_input.ForeColor = Color.Black;

                if (pelimaara_input.Text.Length > vakiot.MAX_PITUUS)
                    pelimaara_input.Text = "";
                pelimaara_input.Select();
                return false;
            }

            return true; // true = kenttä OK
        }

        // Tarkista Vastustajan SELO -kenttä, oltava numero ja rajojen sisällä
        //
        // Syöte voi olla annettu kolmella eri formaatilla:
        //  1)  1720   -> ja sitten tulos valintanapeilla
        //  2)  +1624 -1700 =1685 +1400    jossa  '+' voitto, '=' tasapeli ja '-' tappio.
        //                                 Tasapeli voidaan myös antaa ilman '='-merkkiä.
        //  3)  2,5 1624 1700 1685 1400    Eli aloitetaan kokonaispistemäärällä.
        //                                 SELOt ilman erillisiä tuloksia.
        //
        // Yhden ottelun tulos voidaan antaa kolmella tavalla:
        //   1)  1720      ja tulos erikseen valintanapeilla, esim. 1/2 tasapeli
        //   2)  =1720     tasapeli myös näin
        //   3)  0.5 1720  tai näin
        //

        // XXX:   tarkista_vastustajanSelo() ON LIIAN PITKÄ. JAA OSIIN!
        //
        bool tarkista_vastustajanSelo(out int vastustajanSelo)
        {
            float syotetty_tulos = 0F;

            vastustajanSelo_comboBox.Text = vastustajanSelo_comboBox.Text.Trim();  // remove leading and trailing white spaces

            if (string.IsNullOrWhiteSpace(vastustajanSelo_comboBox.Text)) {
                vastustajanSelo = vakiot.MIN_SELO - 1;     // ei saa olla tyhjä -> virheilmoitus
            }
            else if (vastustajanSelo_comboBox.Text.Length == vakiot.SELO_PITUUS)  // numero 1000-2999
            {
                if (int.TryParse(vastustajanSelo_comboBox.Text, out vastustajanSelo) == false) {
                    vastustajanSelo = vakiot.MIN_SELO - 1;  // ei ollut numero -> virheilmoitus
                }
            }
            else {
                // kentässä voidaan antaa alussa turnauksen tulos, esim. 0.5, 2.0, 2.5, 7.5 eli saadut pisteet
                shakinpelaaja.set_syotetty_turnauksen_tulos(-1.0F);  // oletus: ei annettu

                // poista sanojen väleistä ylimääräiset välilyönnit!
                string pattern = "\\s+";    // \s = any whitespace, + one or more repetitions
                string replacement = " ";   // tilalle vain yksi välilyönti
                Regex rx = new Regex(pattern);
                vastustajanSelo_comboBox.Text = rx.Replace(vastustajanSelo_comboBox.Text, replacement);

                // Nyt voidaan jakaa syöte merkkijonoihin!
                List<string> selo_lista = vastustajanSelo_comboBox.Text.Split(' ').ToList();
                int selo1 = vakiot.MIN_SELO;
                int tulos1 = 0;
                bool ensimmainen = true;
                bool turnauksen_tulos = false;

                // Tutki vastustajanSelo_comboBox-kenttä
                // Tallenna listaan selo_lista vastustajien SELO:t ja tulokset merkkijonona
                foreach (string tulos in selo_lista) {
                    if (ensimmainen) {
                        // Tarkista, onko alussa annettu turnauksen lopputulos eli kokonaispistemäärä?
                        ensimmainen = false;
                        // Auttavatkohan nuo NumberStyles ja CultureInfo... testaa
                        if (float.TryParse(tulos, System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out syotetty_tulos) == true) {
                            if (syotetty_tulos >= 0.0F && syotetty_tulos <= 99.5F) {
                                turnauksen_tulos = true;
                                shakinpelaaja.set_syotetty_turnauksen_tulos(syotetty_tulos);

                                // alussa oli annettu turnauksen lopputulos, jatka SELOjen tarkistamista
                                // Nyt selojen on oltava ilman tulosmerkintää!
                                continue;
                            }
                        }
                    }

                    // merkkijono voi alkaa merkillä '+', '=' tai '-'
                    // Mutta tasapeli voidaan antaa myös ilman '='-merkkiä
                    // Jos oli annettu turnauksen tulos, niin selot on syötettävä näin ilman tulosta
                    if (tulos.Length == vakiot.SELO_PITUUS)  // numero(4 merkkiä)
                    {
                        if (int.TryParse(tulos, out selo1) == false) {
                            selo1 = vakiot.MIN_SELO - 1;  // -> virheilmoitus, ei ollut numero
                            break;
                        }
                        tulos1 = 1;  // 1=tasapeli  HUOM! Jos tulos oli jo annettu, niin tätä ei huomioida laskuissa
                        shakinpelaaja.lista_lisaa_ottelun_tulos(selo1, tulos1);
                    }
                    else if (tulos.Length == vakiot.MAX_PITUUS && turnauksen_tulos == false)  // tulos(1 merkki)+numero(4 merkkiä)
                    {
                        // Erillisten tulosten antaminen hyväksytään vain, jos turnauksen
                        // lopputulosta ei oltu jo annettu
                        switch (tulos[0])  // ensimmäinen merkki antaa tuloksen
                        {
                            case '+':   // voitto 1 piste, tallentetaan 2
                                tulos1 = vakiot.VOITTOx2;
                                break;
                            case '=':   // tasapeli 1/2 pistettä, tallentaan 1
                                tulos1 = vakiot.TASAPELIx2;
                                break;
                            case '-':   // tappio, tallennetaan 0
                                tulos1 = vakiot.TAPPIOx2;
                                break;
                            default:
                                selo1 = vakiot.MIN_SELO - 1;  // ei ollut oikea tulos!
                                break;
                        }
                        if (selo1 >= vakiot.MIN_SELO)  // Vielä OK?  Selvitä sitten numero
                        {
                            // parse: ohita +=- eli aloita numerosta, oli esim. =1612
                            if (int.TryParse(tulos.Substring(1), out selo1) == false) {
                                selo1 = vakiot.MIN_SELO - 1;  // -> virheilmoitus, ei ollut numero
                                break;
                            }

                            shakinpelaaja.lista_lisaa_ottelun_tulos(selo1, tulos1);
                        }
                    }
                    else {
                        // pituus ei ollut SELO_PITUUS (4 esim. 1234) eikä MAX_PITUUS (5 esim. +1234)
                        selo1 = vakiot.MIN_SELO - 1; // -> virheellistä dataa
                        break;
                    }

                    // DEBUG:           MessageBox.Show(tulos + " : " + selo1.ToString() + " " + tulos1.ToString());

                    // Oliko asetettu virhe, mutta ei vielä poistuttu foreach-loopista?
                    if (selo1 < vakiot.MIN_SELO)
                        break;

                } // foreach

                if (turnauksen_tulos) {
                    // Syötteen annettu turnauksen tulos ei saa olla suurempi kuin pelaajien lukumäärä
                    // Vertailu kokonaislukuina, syötetty tulos 3.5 vs 4, vertailu 7 vs 8.
                    if ((int)(2 * syotetty_tulos + 0.01F) > 2 * shakinpelaaja.get_vastustajien_lkm_listassa()) {
                        selo1 = vakiot.MIN_SELO - 2;  // tästä oma virheilmoitus
                    }
                }

                // vain virhetarkastusta varten
                vastustajanSelo = selo1;
            }

            // VIRHEILMOITUKSET
            if (vastustajanSelo < vakiot.MIN_SELO || vastustajanSelo > vakiot.MAX_SELO) {

                string message;
                if (vastustajanSelo == vakiot.MIN_SELO - 2)
                    message =
                        String.Format("VIRHE: Turnauksen pistemäärä (annettu {0}) voi olla enintään sama kuin vastustajien lukumäärä ({1}).",
                        syotetty_tulos, shakinpelaaja.get_vastustajien_lkm_listassa());
                else
                    message =
                        String.Format("VIRHE: Vastustajan SELOn oltava numero {0}-{1}.", vakiot.MIN_SELO, vakiot.MAX_SELO);

                vastustajanSelo_comboBox.ForeColor = Color.Red;
                MessageBox.Show(message);
                vastustajanSelo_comboBox.ForeColor = Color.Black;

                // Ei tyhjennetä kenttää, jotta sitä on helpompi korjata
                //              if (vastustajanSelo_comboBox.Text.Length > MAX_PITUUS)
                //                  vastustajanSelo_comboBox.Text = "";
                // Kentästä on kuitenkin jo poistettu ylimääräiset välilyönnit
                vastustajanSelo_comboBox.Select();
                return false;
            }

            return true;
        }

        // Tarkista ottelun tulos -painikkeet ja tallenna niiden vaikutus
        // pisteet: tappiosta 0, tasapelistä puoli ja voitosta yksi
        //          tallentetaan kokonaisulukuna 0, 1 ja 2
        bool tarkista_ottelun_tulos(out int pisteet)
        {
            if (tulosTappio_Button.Checked)
                pisteet = vakiot.TAPPIOx2;
            else if (tulosTasapeli_Button.Checked)
                pisteet = vakiot.TASAPELIx2;
            else if (tulosVoitto_Button.Checked)
                pisteet = vakiot.VOITTOx2;
            else {
                MessageBox.Show("Ottelun tulosta ei annettu!");
                tulosTappio_Button.Select();   // siirry ensimmäiseen valintanapeista

                // Aseta joku arvo, jotta ei tule virheilmoitusta
                //   "The out parameter must be assigned before control leaves the current method"
                pisteet = 0;
                return false;
            }
            return true;
        }

        // FUNKTIO: tarkistaInput
        //
        // Tarkistaa
        //      -miettimisaika-valintanapit
        //      -nykyinen SELO eli oma alkuselo
        //      -nykyinen oma pelimäärä
        //      -vastustajan SELO tai vastustajien SELOT
        //      -yhtä ottelua syötettäessä tuloksen valintanapit
        //
        // Tuloksena
        //    palautetaan parametreissa oma selo ja pelimäärä
        //    1) Jos syötetty yksi ottelu, niin palautetaan vastustajan selo ja ottelun pisteet
        //    2) Jos syötetty monen ottelun tulokset, niin selo_lista sisältää selot ja tulokset
        //
        // VIrhetilanteet:
        //    Jos jokin syötekenttä on virheellinen, annetaan virheilmoitus, siirrytään ko kenttään ja keskeytetään.
        //    Kenttiä tarkistetaan yo. järjestyksessä ja vain ensimmäisestä virheestä annetaan virheilmoitus.
        //
        private bool tarkistaInput(out int nykyinenSelo, out int pelimaara, out int vastustajanSelo, out int pisteet)
        {

            // ************ TARKISTA SYÖTE ************


            // ENSIN TARKISTA MIETTIMISAIKA. Tässä ei voi olla virheellista tietoa.
            tarkista_miettimisaika();

            nykyinenSelo = 0; pelimaara = 0; vastustajanSelo = 0; pisteet = 0;  // alkuarvot

            if (tarkista_nykyinenselo(out nykyinenSelo) == false)
                return false;

            if (tarkista_pelimaara(out pelimaara) == false)
                return false;

            if (tarkista_vastustajanSelo(out vastustajanSelo) == false)
                return false;

            // DEBUG:   MessageBox.Show("Syöte: " + nykyinenSelo + " " + pelimaara + " " + vastustajanSelo);

            // jos ottelut ovat listassa, niin ottelutuloksen valintanapeilla ei ole merkitystä
            if (shakinpelaaja.get_vastustajien_lkm_listassa() > 0)
                return true;

            if (tarkista_ottelun_tulos(out pisteet) == false)
                return false;

            return true;
        }


        // FUNKTIO: suorita_laskenta()
        //
        // Laske tulokset
        // Näytä tulokset
        private bool suorita_laskenta()
        {
            int nykyinenSelo;
            int pelimaara;
            int vastustajanSelo;
            int pisteet;

            // tyhjennä ottelulista!
            shakinpelaaja.lista_tyhjenna();

            // jos annettu yksi ottelu, niin vastustajan tiedot:
            //     vastustajanSelo ja pisteet
            // jos annettu useampi ottelu, niin ottelut ovat listassa ja get_vastustajien_lkm_listassa > 0
            if (tarkistaInput(out nykyinenSelo, out pelimaara, out vastustajanSelo, out pisteet) == false)
                return false;

            // asettaa omat tiedot, nollaa tilastotiedot ym.
            shakinpelaaja.aloita_laskenta(nykyinenSelo, pelimaara);

            //  *** LASKETAAN ***

            // Lasketaanko yhtä ottelua vai turnausta?
            if (shakinpelaaja.get_vastustajien_lkm_listassa() == 0) // tyhjä lista
                shakinpelaaja.pelaa_ottelu(vastustajanSelo, pisteet); // pelaa yksi tietty ottelu
            else
                shakinpelaaja.pelaa_ottelu();       // pelaa kaikki (turnauksen) ottelut

            return true;
        }

        void nayta_tulokset()
        {
            //  *** NÄYTÄ TULOKSIA ***

            // Laskettiinko yhtä ottelua vai turnausta?
            if (shakinpelaaja.get_vastustajien_lkm_listassa() == 0) {
                // tyhjä lista, joten yksi ottelu -> näytä uusi vahvuusluku, pelimäärä ym. tiedot
                seloEro_output.Text =
                    Math.Abs(shakinpelaaja.selo_alkuperainen - shakinpelaaja.viimeisin_vastustaja).ToString();
                odotustulos_output.Text = (shakinpelaaja.odotustulos / 100F).ToString("0.00");
                kerroin_output.Text = shakinpelaaja.kerroin.ToString();
                vaihteluvali_output.Text = "";  // ei vaihteluväliä, koska vain yksi luku laskettu
            }
            else {
                // tyhjennä yksittäisen ottelun tuloskentät
                seloEro_output.Text = "";
                // odotustulos näytetään, jos ei ollut uuden pelaajan laskenta
                if (shakinpelaaja.pelimaara < 0 || shakinpelaaja.pelimaara > 10)
                    odotustulos_output.Text = (shakinpelaaja.odotustuloksien_summa / 100F).ToString("0.00");
                else
                    odotustulos_output.Text = "";

                // kerroin on laskettu alkuperäisestä omasta selosta
                kerroin_output.Text = shakinpelaaja.kerroin.ToString();

                // Valintanapeilla ei merkitystä, kun käsitellään turnausta eli valinnat pois
                tulosTappio_Button.Checked = false;
                tulosTasapeli_Button.Checked = false;
                tulosVoitto_Button.Checked = false;

                // Näytä laskennan aikainen vahvuusluvun vaihteluväli
                // Jos oli annettu turnauksen tulos, niin laskenta tehdään yhdellä lauseella eikä vaihteluväliä ole
                // Vaihteluväliä ei ole myöskään, jos oli laskettu yhden ottelun tulosta
                // On vain, jos tulokset formaatissa "+1622 -1880 =1633"
                if (shakinpelaaja.syotetty_turnauksen_tulos < 0 && shakinpelaaja.turnauksen_ottelumaara > 1) {
                    vaihteluvali_output.Text =
                        shakinpelaaja.min_selo.ToString() + " - " + shakinpelaaja.max_selo.ToString();
                }
                else
                    vaihteluvali_output.Text = "";  // muutoin siis tyhjä
            }

            // Näytä uusi vahvuusluku ja pelimäärä. Näytä myös vahvuusluvun muutos +/-NN pistettä,
            // sekä vastustajien keskivahvuus ja omat pisteet.
            uusiSelo_output.Text = shakinpelaaja.uusi_selo.ToString();
            uusiSelo_diff_output.Text =
                (shakinpelaaja.uusi_selo - shakinpelaaja.selo_alkuperainen).ToString("+#;-#;0");
            if (shakinpelaaja.uusi_pelimaara >= 0)
                uusi_pelimaara_output.Text = shakinpelaaja.uusi_pelimaara.ToString();
            else
                uusi_pelimaara_output.Text = "";
            keskivahvuus_output.Text = shakinpelaaja.turnauksen_keskivahvuus.ToString();
            // Turnauksen loppupisteet / ottelujen lkm, esim.  2.5 / 6
            tulos_output.Text =
                (shakinpelaaja.turnauksen_tulos / 2F).ToString("0.0") + " / " + shakinpelaaja.turnauksen_ottelumaara;
        }

        // FUNKTIO: Laske_button_Click
        // 
        // Suoritetaan laskenta -button
        private void Laske_button_Click(object sender, EventArgs e)
        {
            if (suorita_laskenta()) {
                nayta_tulokset();

                // 12.12.1027: Annettu teksti talteen -> Drop-down Combo box
                if (!vastustajanSelo_comboBox.Items.Contains(vastustajanSelo_comboBox.Text))
                    vastustajanSelo_comboBox.Items.Add(vastustajanSelo_comboBox.Text);
            }
        }


        // FUNKTIO: Kayta_uutta_button_Click
        //
        // Käytä uutta SELOa jatkolaskennassa -button
        //
        // Kopioi lasketun uuden selon ja mahdollisen pelimäärän käytettäväksi, jotta
        // laskentaa voidaan jatkaa helposti syöttämällä uusi vastustajan selo ja ottelun tulos.
        // Ja sitten siirrytään valmiiksi vastustajan SELO-kenttään.
        //
        // Jos painiketta oli painettu ennen ensimmäistäkään laskentaa, niin
        // otetaan käyttöön pelaajaa luodessa käytetyt alkuarvot 1525 ja 0.
        private void Kayta_uutta_button_Click(object sender, EventArgs e)
        {
            int selo = shakinpelaaja.uusi_selo;
            int pelimaara = shakinpelaaja.uusi_pelimaara;

            if (selo == 0) { // ei ollut vielä laskentaa
                selo = shakinpelaaja.selo;
                pelimaara = shakinpelaaja.pelimaara;
            }

            nykyinenSelo_input.Text = selo.ToString();
            if (pelimaara > vakiot.MIN_PELIMAARA - 1) {
                // vain, jos pelimaara oli annettu (muutoin on jo valmiiksi tyhjä)
                pelimaara_input.Text = pelimaara.ToString();
            }
            vastustajanSelo_comboBox.Select();
        }


        // FUNKTIO: tulosTappio_Button_Enter
        // FUNKTIO: tulosTasapeli_Button_Enter
        // FUNKTIO: tulosVoitto_Button_Enter
        //
        // Suorita laskenta aina kun siirrytään tulos-painikkeeseen.
        // Ennen laskentaa asetetaan nykyinen painike valituksi, koska sitä ei
        // muutoin vielä oltu valittu kenttään siirryttäessä.
        //
        // Jos tässä vaiheessa ei ole vielä annettu SELOja, niin tulee virheilmoitus
        // sekä siirrytään SELO-kenttään.
        // 
        private void tulosTappio_Button_Enter(object sender, EventArgs e)
        {
            tulosTappio_Button.Checked = true;
            if (suorita_laskenta())
                nayta_tulokset();
        }

        private void tulosTasapeli_Button_Enter(object sender, EventArgs e)
        {
            tulosTasapeli_Button.Checked = true;
            suorita_laskenta();
            if (suorita_laskenta())
                nayta_tulokset();
        }

        private void tulosVoitto_Button_Enter(object sender, EventArgs e)
        {
            tulosVoitto_Button.Checked = true;
            suorita_laskenta();
            if (suorita_laskenta())
                nayta_tulokset();
        }

        // FUNKTIO: vastustajanSelo_combobox_KeyDown_KeyDown
        //
        // Kun painettu Enter vastustajan SELO-kentässä, suoritetaan laskenta
        //
        private void vastustajanSelo_combobox_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter painettu vastustajan selojen tai useammankin syöttämisen jälkeen?
            if (e.KeyCode == Keys.Enter) {
                // Kun painettu Enter, niin lasketaan
                // siirrytäänkö myös kentän loppuun? Nyt jäädään samaan paikkaan, mikä myös OK.
                if (suorita_laskenta()) {
                    nayta_tulokset();

                    // 12.12.1027: Annettu teksti talteen -> Drop-down Combo box
                    if (!vastustajanSelo_comboBox.Items.Contains(vastustajanSelo_comboBox.Text))
                        vastustajanSelo_comboBox.Items.Add(vastustajanSelo_comboBox.Text);
                }
            }
        }

        private enum vaihda_miettimisaika_enum { VAIHDA_SELOKSI, VAIHDA_PELOKSI };

        private void miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum suunta)
        {
            string alkup, uusi;

            if (suunta == vaihda_miettimisaika_enum.VAIHDA_SELOKSI) {
                alkup = "PELO";
                uusi = "SELO";
                TuloksetPistemaaranKanssa_teksti.Font = new Font(TuloksetPistemaaranKanssa_teksti.Font, FontStyle.Regular);
            }
            else {
                alkup = "SELO";
                uusi = "PELO";
                // korosta PELO-ohje
                TuloksetPistemaaranKanssa_teksti.Font = new Font(TuloksetPistemaaranKanssa_teksti.Font, FontStyle.Bold);
            }

            OmaVahvuusluku_teksti.Text = OmaVahvuusluku_teksti.Text.Replace(alkup, uusi);
            VastustajanVahvuusluku_teksti.Text = VastustajanVahvuusluku_teksti.Text.Replace(alkup, uusi);
            TuloksetPistemaaranKanssa_teksti.Text = TuloksetPistemaaranKanssa_teksti.Text.Replace(alkup, uusi);
            UusiSELO_teksti.Text = UusiSELO_teksti.Text.Replace(alkup, uusi);
            Laske_button.Text = Laske_button.Text.Replace(alkup, uusi);
            Kayta_uutta_button.Text = Kayta_uutta_button.Text.Replace(alkup, uusi);
        }

        private void miettimisaika_vah90_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_60_89_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_11_59_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_enint10_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum.VAIHDA_PELOKSI);
        }

        /* MENU */

        private void ohjeitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Oletuksena tulee OK-painike
            MessageBox.Show("(kesken)Ohjeita: "
                + "\r\n"
                + "Syötä oma vahvuusluku. Jos olet uusi pelaaja, niin anna oma pelimäärä 0-10, jolloin käytetään uuden pelaajan laskentakaavaa."
                + "\r\n"
                + "Syötä lisäksi joko "
                + "\r\n"
                + "  1) Vastustajan vahvuusluku ja valitse ottelun tulos 0/0,5/1nuolinäppäimillä tai "
                + "\r\n"
                + "   2) Vahvuusluvut tuloksineen, esim. +1525 =1600 -1611 +1558 tai "
                + "\r\n"
                + "  3) Turnauksen pistemäärä ja vastustajien vahvuusluvut, esim. 2.5 1525 1600 1611 1558"
                + "\r\n"
                + "Lisäksi voidaan valita miettimisaika yläreunan valintapainikkeilla."
                + "\r\n"
                + " Ohjelma sisältää sekä SELO:n (pitkä peli) että PELO:n (pikashakki) laskennat.",
                "Ohjeikkuna");
        }

        private void laskentakaavatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Oletuksena tulee OK-painike
            MessageBox.Show("(kesken)Shakin vahvuusluvun laskentakaavat: http://skore.users.paivola.fi/selo.html"
                + " \r\n"
                + "Lisätietoa: http://www.shakkiliitto.fi/ ja http://www.shakki.net/cgi-bin/selo",
                "Laskentakaavat");
        }

        private void tietojaOhjelmastaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Oletuksena tulee OK-painike
            MessageBox.Show("(kesken)Shakin vahvuusluvun laskenta, ohjelmointikieli: C#/.NET/WinForms, https://github.com/isuihko/selolaskuri", "Tietoja selolaskurista");
        }

        private void suljeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Application.Exit() kutsuu ikkunan FormClosing() -rutiinia, jossa varmistus.
            System.Windows.Forms.Application.Exit();
        }

        // Tähän tullaan myös valikosta Menu->Sulje ohjelma -> Application.Exit()
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult vastaus = MessageBox.Show("Haluatko poistua ohjelmasta?", "Exit/Close window", MessageBoxButtons.YesNo);
            if (vastaus == DialogResult.No) {
                e.Cancel = true; // Ei poistutakaan
            }
        }

    }

} // END Form1.cs
