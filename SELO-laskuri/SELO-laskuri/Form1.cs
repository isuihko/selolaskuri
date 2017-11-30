//
// Selolaskuri   https://github.com/isuihko/selolaskuri
//
// 28.11.2017 Ismo Suihko 1.0.0.9
//
// C#/.NET, Visual Studio Community 2015, Windows 7.
//
// Ensimmäinen C#/.NET -ohjelmani.
// Sisältää käyttöliittymän, syötteen tarkistuksen, laskentaa ja tuloksien näyttämisen.
//
// HUOM!  Käännetty binääri/asennusohjelma setup.exe vaatii nyt .NET Framework 4.5.2:n.
//        Voiko tässä Community-versiossa muuttaa asetuksia, niin että toimisi
//        kaikilla versioilla esim. .NET Framework 4.0:sta alkaen?
//
// KOODIA JÄRJESTELLÄÄN JA OPTIMOIDAAN VIELÄ, MUTTA TÄMÄN PITÄISI NYT TOIMIA AIKA HYVIN.
// Teen ohjelmasta vielä vastaavan version Javalla.
//
// Kuva version 1.0.0.9 näytöstä on linkissä
//   https://goo.gl/pSVZcU ( https://drive.google.com/open?id=1e4z34Rh2YOz5xC8G2fDOK4x9__r-qB5n )
//
//
// Laskee pelaajalle vahvuusluvun miettimisaikojen vähintään 90, 60-89 ja 15-59 minuutin peleistä.
//
// Jos pelimäärä 0-10, käytetään uuden pelaajan selon kaavaa.
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
// Painikkeet:
//    - Laske uusi SELO     Uusi SELO lasketaan myös aina kun valitaan ottelun tulos.
//    - Käytä uutta SELOa jatkolaskennassa -> kopioi tuloksen & pelimäärän uutta laskentaa varten
//                                            Jos ei vielä laskettu, asettaa arvot 1525 ja 0.
//
// Kun valittu miettimisajaksi alle 15 minuuttia, niin lomakkeen SELO-tekstit vaihdetaan -> PELO.
//
// Laskentakaavat:  http://skore.users.paivola.fi/selo.html  (odotustulos, kerroin, SELO)
// Käytössä on kaavat miettimisajoille väh. 90 min, 60-89 min ja 15-59 min.
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
        pelaaja_class shakinpelaaja = new pelaaja_class(1525, 0);


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
                shakinpelaaja.set_miettimisaika(90);  // vähintään 90 minuuttia (oletus)
            else if (miettimisaika_60_89_Button.Checked)
                shakinpelaaja.set_miettimisaika(60);  // 60-89 minuuttia
            else if (miettimisaika_15_59_Button.Checked)
                shakinpelaaja.set_miettimisaika(15);  // 15-59 minuuttia
            else
                shakinpelaaja.set_miettimisaika(5);   // alle 15 minuuttia
        }

        // Tarkista Oma SELO -kenttä, oltava numero ja rajojen sisällä
        private bool tarkista_nykyinenselo(out int nykyinenSelo)
        {
            string str = nykyinenSelo_input.Text.Trim();  // remove leading and trailing white spaces
            nykyinenSelo_input.Text = str;

            // onko numero?
            if (int.TryParse(nykyinenSelo_input.Text, out nykyinenSelo) == false)
            {
                nykyinenSelo = constants.MIN_SELO - 1;  // -> virheilmoitus
            }

            // onko kelvollinen numero?
            if (nykyinenSelo < constants.MIN_SELO || nykyinenSelo > constants.MAX_SELO)
            {
                string message = String.Format("VIRHE: Nykyisen SELOn oltava numero {0}-{1}.", constants.MIN_SELO, constants.MAX_SELO);
                nykyinenSelo_input.ForeColor = Color.Red;
                MessageBox.Show(message);
                nykyinenSelo_input.ForeColor = Color.Black;

                if (nykyinenSelo_input.Text.Length > constants.MAX_PITUUS)
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
            string str = pelimaara_input.Text.Trim();  // remove leading and trailing white spaces
            pelimaara_input.Text = str;

            if (string.IsNullOrWhiteSpace(pelimaara_input.Text))
            {
                pelimaara = constants.MIN_PELIMAARA - 1; // OK
                return true;   // ei tarkisteta enempää
            }

            // onko numero?
            if (int.TryParse(pelimaara_input.Text, out pelimaara) == false)
            {
                pelimaara = constants.MIN_PELIMAARA - 1;   // -> virheilmoitus
            }

            // onko kelvollinen numero?
            if (pelimaara < constants.MIN_PELIMAARA || pelimaara > constants.MAX_PELIMAARA)
            {
                string message = String.Format("VIRHE: pelimäärän oltava numero 0-{0} tai tyhjä.", constants.MAX_PELIMAARA);
                pelimaara_input.ForeColor = Color.Red;
                MessageBox.Show(message);
                pelimaara_input.ForeColor = Color.Black;

                if (pelimaara_input.Text.Length > constants.MAX_PITUUS)
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

            string str = vastustajanSelo_input.Text.Trim();  // remove leading and trailing white spaces
            vastustajanSelo_input.Text = str;

            if (string.IsNullOrWhiteSpace(vastustajanSelo_input.Text))
            {
                vastustajanSelo = constants.MIN_SELO - 1;     // ei saa olla tyhjä -> virheilmoitus
            }
            else if (vastustajanSelo_input.Text.Length == constants.SELO_PITUUS)  // numero 1000-2999
            {
                if (int.TryParse(vastustajanSelo_input.Text, out vastustajanSelo) == false)
                {
                    vastustajanSelo = constants.MIN_SELO - 1;  // ei ollut numero -> virheilmoitus
                }
            }
            else
            {
                // kentässä voidaan antaa alussa turnauksen tulos, esim. 0.5, 2.0, 2.5, 7.5 eli saadut pisteet
                shakinpelaaja.set_syotetty_turnauksen_tulos(-1.0F);  // oletus: ei annettu

                // poista sanojen väleistä ylimääräiset välilyönnit!
                string pattern = "\\s+";    // \s = any whitespace, + one or more repetitions
                string replacement = " ";   // tilalle vain yksi välilyönti
                Regex rx = new Regex(pattern);
                string result = rx.Replace(vastustajanSelo_input.Text, replacement);
                vastustajanSelo_input.Text = result;

                // Nyt voidaan jakaa syöte merkkijonoihin!
                List<string> selo_lista = vastustajanSelo_input.Text.Split(' ').ToList();
                int selo1 = constants.MIN_SELO;
                int tulos1 = 0;
                bool ensimmainen = true;
                bool turnauksen_tulos = false;

                // Tutki vastustajanSelo_input-kenttä
                // Tallenna listaan selo_lista vastustajien SELO:t ja tulokset merkkijonona
                foreach (string tulos in selo_lista)
                {
                    if (ensimmainen)
                    {
                        // Tarkista, onko alussa annettu turnauksen lopputulos eli kokonaispistemäärä?
                        ensimmainen = false;
                        // Auttavatkohan nuo NumberStyles ja CultureInfo... testaa
                        if (float.TryParse(tulos, System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out syotetty_tulos) == true)
                        {
                            if (syotetty_tulos >= 0.0F && syotetty_tulos <= 99.5F)
                            {
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
                    if (tulos.Length == constants.SELO_PITUUS)  // numero(4 merkkiä)
                    {
                        if (int.TryParse(tulos, out selo1) == false)
                        {
                            selo1 = constants.MIN_SELO - 1;  // -> virheilmoitus, ei ollut numero
                            break;
                        }
                        tulos1 = 1;  // 1=tasapeli  HUOM! Jos tulos oli jo annettu, niin tätä ei huomioida laskuissa
                        shakinpelaaja.lista_lisaa_ottelun_tulos(selo1, tulos1);
                    }
                    else if (tulos.Length == constants.MAX_PITUUS && turnauksen_tulos == false)  // tulos(1 merkki)+numero(4 merkkiä)
                    {
                        // Erillisten tulosten antaminen hyväksytään vain, jos turnauksen
                        // lopputulosta ei oltu jo annettu
                        switch (tulos[0])  // ensimmäinen merkki antaa tuloksen
                        {
                            case '+':   // voitto 1 piste, tallentetaan 2
                                tulos1 = 2;
                                break;
                            case '=':   // tasapeli 1/2 pistettä, tallentaan 1
                                tulos1 = 1;
                                break;
                            case '-':   // tappio, tallennetaan 0
                                tulos1 = 0;
                                break;
                            default:
                                selo1 = constants.MIN_SELO - 1;  // ei ollut oikea tulos!
                                break;
                        }
                        if (selo1 >= constants.MIN_SELO)  // Vielä OK?  Selvitä sitten numero
                        {
                            // parse: ohita +=- eli aloita numerosta, oli esim. =1612
                            if (int.TryParse(tulos.Substring(1), out selo1) == false)
                            {
                                selo1 = constants.MIN_SELO - 1;  // -> virheilmoitus, ei ollut numero
                                break;
                            }

                            shakinpelaaja.lista_lisaa_ottelun_tulos(selo1, tulos1);
                        }
                    }
                    else
                    {
                        // pituus ei ollut SELO_PITUUS (4 esim. 1234) eikä MAX_PITUUS (5 esim. +1234)
                        selo1 = constants.MIN_SELO - 1; // -> virheellistä dataa
                        break;
                    }

// DEBUG:           MessageBox.Show(tulos + " : " + selo1.ToString() + " " + tulos1.ToString());

                    // Oliko asetettu virhe, mutta ei vielä poistuttu foreach-loopista?
                    if (selo1 < constants.MIN_SELO)
                        break;

                } // foreach

                if (turnauksen_tulos)
                {
                    // Syötteen annettu turnauksen tulos ei saa olla suurempi kuin pelaajien lukumäärä
                    // Vertailu kokonaislukuina, syötetty tulos 3.5 vs 4, vertailu 7 vs 8.
                    if ((int)(2*syotetty_tulos + 0.01F) > 2*shakinpelaaja.get_vastustajien_lkm_listassa())
                    {
                        selo1 = constants.MIN_SELO - 2;  // tästä oma virheilmoitus
                    }
                }

                // vain virhetarkastusta varten
                vastustajanSelo = selo1;
            }

            // VIRHEILMOITUKSET
            if (vastustajanSelo < constants.MIN_SELO || vastustajanSelo > constants.MAX_SELO)
            {

                string message;
                if (vastustajanSelo == constants.MIN_SELO - 2)
                    message =
                        String.Format("VIRHE: Turnauksen pistemäärä ({0}) voi olla enintään sama kuin vastustajien lukumäärä ({1}).",
                        syotetty_tulos, shakinpelaaja.get_vastustajien_lkm_listassa());
                else
                    message =
                        String.Format("VIRHE: Vastustajan SELOn oltava numero {0}-{1}.", constants.MIN_SELO, constants.MAX_SELO);

                vastustajanSelo_input.ForeColor = Color.Red;
                MessageBox.Show(message);
                vastustajanSelo_input.ForeColor = Color.Black;

                // Ei tyhjennetä kenttää, jotta sitä on helpompi korjata
                //              if (vastustajanSelo_input.Text.Length > MAX_PITUUS)
                //                  vastustajanSelo_input.Text = "";
                // Kentästä on kuitenkin jo poistettu ylimääräiset välilyönnit
                vastustajanSelo_input.Select();
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
                pisteet = 0;
            else if (tulosTasapeli_Button.Checked)
                pisteet = 1;
            else if (tulosVoitto_Button.Checked)
                pisteet = 2;
            else
            {
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
            if (shakinpelaaja.get_vastustajien_lkm_listassa() == 0)
            {
                // tyhjä lista, joten yksi ottelu -> näytä uusi vahvuusluku, pelimäärä ym. tiedot
                seloEro_output.Text =
                    Math.Abs(shakinpelaaja.get_selo_alkuperainen() - shakinpelaaja.get_viimeisin_vastustaja()).ToString();
                odotustulos_output.Text = (shakinpelaaja.get_odotustulos() / 100F).ToString("0.00");
                kerroin_output.Text = shakinpelaaja.get_kerroin().ToString();
                vaihteluvali_output.Text = "";  // ei vaihteluväliä, koska vain yksi luku laskettu
            }
            else
            {
                // tyhjennä yksittäisen ottelun tuloskentät
                seloEro_output.Text = "";
                // odotustulos näytetään, jos ei ollut uuden pelaajan laskenta
                if (shakinpelaaja.get_pelimaara() < 0 || shakinpelaaja.get_pelimaara() > 10)
                    odotustulos_output.Text = (shakinpelaaja.get_odotustuloksien_summa() / 100F).ToString("0.00");
                else
                    odotustulos_output.Text = "";

                // kerroin on laskettu alkuperäisestä omasta selosta
                kerroin_output.Text = shakinpelaaja.get_kerroin().ToString();

                // Valintanapeilla ei merkitystä, kun käsitellään turnausta eli valinnat pois
                tulosTappio_Button.Checked = false;
                tulosTasapeli_Button.Checked = false;
                tulosVoitto_Button.Checked = false;

                // Näytä laskennan aikainen vahvuusluvun vaihteluväli
                // Jos oli annettu turnauksen tulos, niin laskenta tehdään yhdellä lauseella eikä vaihteluväliä ole
                // Vaihteluväliä ei ole myöskään, jos oli laskettu yhden ottelun tulosta
                // On vain, jos tulokset formaatissa "+1622 -1880 =1633"
                if (shakinpelaaja.get_syotetty_turnauksen_tulos() < 0 && shakinpelaaja.get_turnauksen_ottelumaara() > 1)
                {
                    vaihteluvali_output.Text =
                        shakinpelaaja.get_min_selo().ToString() + " - " + shakinpelaaja.get_max_selo().ToString();
                }
                else
                    vaihteluvali_output.Text = "";  // muutoin siis tyhjä
            }

            // Näytä uusi vahvuusluku ja pelimäärä. Näytä myös vahvuusluvun muutos +/-NN pistettä,
            // sekä vastustajien keskivahvuus ja omat pisteet.
            uusiSelo_output.Text = shakinpelaaja.get_uusiselo().ToString();
            uusiSelo_diff_output.Text =
                (shakinpelaaja.get_uusiselo() - shakinpelaaja.get_selo_alkuperainen()).ToString("+#;-#;0");
            if (shakinpelaaja.get_uusipelimaara() >= 0)
                uusi_pelimaara_output.Text = shakinpelaaja.get_uusipelimaara().ToString();
            else
                uusi_pelimaara_output.Text = "";
            keskivahvuus_output.Text = shakinpelaaja.get_turnauksen_keskivahvuus().ToString();
            // Turnauksen loppupisteet / ottelujen lkm, esim.  2.5 / 6
            tulos_output.Text =
                (shakinpelaaja.get_turnauksen_tulos() / 2F).ToString("0.0") + " / " + shakinpelaaja.get_turnauksen_ottelumaara();
        }

        // FUNKTIO: Laske_button_Click
        // 
        // Suoritetaan laskenta -button
        private void Laske_button_Click(object sender, EventArgs e)
        {
            if (suorita_laskenta())
                nayta_tulokset();
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
            int selo = shakinpelaaja.get_uusiselo();
            int pelimaara = shakinpelaaja.get_uusipelimaara();

            if (selo == 0)  // ei ollut vielä laskentaa
            {
                selo = shakinpelaaja.get_selo();
                pelimaara = shakinpelaaja.get_pelimaara();
            }

            nykyinenSelo_input.Text = selo.ToString();
            if (pelimaara > constants.MIN_PELIMAARA-1)
            {
                // vain, jos pelimaara oli annettu (muutoin on jo valmiiksi tyhjä)
                pelimaara_input.Text = pelimaara.ToString();
            }
            vastustajanSelo_input.Select();
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

        // FUNKTIO: vastustajanSelo_input_KeyDown
        //
        // Kun painettu Enter vastustajan SELO-kentässä, suoritetaan laskenta
        //
        private void vastustajanSelo_input_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter painettu vastustajan selojen tai useammankin syöttämisen jälkeen?
            if (e.KeyCode == Keys.Enter)
            {
                // Kun painettu Enter, niin lasketaan
                // siirrytäänkö myös kentän loppuun? Nyt jäädään samaan paikkaan, mikä myös OK.
                if (suorita_laskenta())
                    nayta_tulokset();
            }
        }

        private enum vaihda_miettimisaika_enum { VAIHDA_SELOKSI, VAIHDA_PELOKSI};

        private void miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum suunta)
        {
            string tmpstr;
            string alkup, uusi;

            if (suunta == vaihda_miettimisaika_enum.VAIHDA_SELOKSI)
            {
                alkup = "PELO";
                uusi = "SELO";
                TuloksetPistemaaranKanssa_teksti.Font = new Font(TuloksetPistemaaranKanssa_teksti.Font, FontStyle.Regular);
            }
            else
            {
                alkup = "SELO";
                uusi = "PELO";
                // korosta PELO-ohje
                TuloksetPistemaaranKanssa_teksti.Font = new Font(TuloksetPistemaaranKanssa_teksti.Font, FontStyle.Bold);
            }

            tmpstr = OmaVahvuusluku_teksti.Text.Replace(alkup, uusi);
            OmaVahvuusluku_teksti.Text = tmpstr;

            tmpstr = VastustajanVahvuusluku_teksti.Text.Replace(alkup, uusi);
            VastustajanVahvuusluku_teksti.Text = tmpstr;

            tmpstr = TuloksetPistemaaranKanssa_teksti.Text.Replace(alkup, uusi);
            TuloksetPistemaaranKanssa_teksti.Text = tmpstr;

            tmpstr = UusiSELO_teksti.Text.Replace(alkup, uusi);
            UusiSELO_teksti.Text = tmpstr;

            tmpstr = Laske_button.Text.Replace(alkup, uusi);
            Laske_button.Text = tmpstr;

            tmpstr = Kayta_uutta_button.Text.Replace(alkup, uusi);
            Kayta_uutta_button.Text = tmpstr;
        }
        
        private void miettimisaika_vah90_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_60_89_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_15_59_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_alle15_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum.VAIHDA_PELOKSI);
        }
    }



    // *****  OMAT LUOKAT *****

    static class constants
    {
        // YLEISET VAKIOT, joilla määrätään syötteen rajat
        // Käytetään pääasiassa tarkista_input() -rutiinissa
        public const int MIN_SELO = 1000;
        public const int MAX_SELO = 2999;
        public const int MIN_PELIMAARA = 0;
        public const int MAX_PELIMAARA = 9999;

        // input-kentän syötteen maksimipituus. Tarkistetaan virhetilanteissa
        // ja jos merkkejä yli tuon, niin tyhjennetään kenttä
        public const int MAX_PITUUS = 5;
        public const int SELO_PITUUS = 4;
    }


    // pelaaja_class
    //   - selo
    //   - pelimäärä (merkitystä, jos 0-10)
    //     ym.
    //
    // pelaa shakkiotteluita, joissa on vastustaja (selo) ja tulos (tappio, tasapeli tai voitto)
    //
    public class pelaaja_class
    {
        // pelaajan omat tiedot
        private int miettimisaika;  // miettimisajan mukaan laskukaavat, 90 / 60 / 15 / 5
        private int selo;           // shakinpelaajan vahvuusluku, 1000-2999
        private int pelimaara;      // pelimaara tai -1, jolloin sitä ei huomioida
        private int selo_alkuperainen; // SELO, jolla laskenta aloitettiin

        // nämä lasketaan
        private int uusi_selo;      
        private int uusi_pelimaara;

        // vaihteluväli, jos vastustajien selot ja tulokset formaatissa: +1622 -1880 =1633
        private int min_selo;
        private int max_selo;

        // laskennan aputiedot
        private int viimeisin_vastustaja;
        private int odotustulos;
        private int kerroin;

        // kun lasketaan kerralla useamman ottelun (turnauksen matsit) vaikutus
        private int turnauksen_vastustajien_lkm;       // päivitetään laskennan edetessä
        private int turnauksen_vastustajien_selosumma; // keskiarvoa varten
        private int odotustuloksien_summa;             // laskettu summa
        private int turnauksen_tulos;                  // laskettu tulos

        // Jos tulos ja vastustajien selo on annettu formaatissa: 1.5 1622 1880 1633
        private int syotetty_turnauksen_tulos;


        // FUNKTIO: pelaaja_class (constructor)
        public pelaaja_class(int selo, int pelimaara)
        {
            this.selo = selo;
            this.pelimaara = pelimaara;
        }


        // Ottelujen tietojen tallennus, vastustajan vahvuusluku ja ottelun tulos
        private struct ottelu_table
        {
            private int vastustajan_selo;
            private int ottelun_tulos;

            public ottelu_table(int selo, int tulos)
            {
                vastustajan_selo = selo;
                ottelun_tulos = tulos;
            }
            public int get_vastustajan_selo()
            {
                return vastustajan_selo;
            }
            public int get_ottelun_tulos()
            {
                return ottelun_tulos;
            }
        }
        // Lista vastustajien tiedoille ja ottelutuloksille
        private IList<ottelu_table> ottelut_list = null;


        // FUNKTIO: lista_lisaa_ottelun_tulos()
        public void lista_lisaa_ottelun_tulos(int vastustajan_selo, int tulos)
        {
            ottelu_table ottelu = new ottelu_table(vastustajan_selo, tulos);

            ottelut_list.Add(ottelu);
        }
        
        // FUNKTIO: lista_tyhjenna()
        //
        // Listan tyhjennys ennen vastustajan SELO -kentän tarkistusta
        // jotta listaan voidaan tallentaa uudet ottelut
        public void lista_tyhjenna()
        {
            // Luo lista tuloksia varten, jos puuttui.
            // Jos oli jo olemassa, niin tyhjennä!
            if (ottelut_list == null)  // XXX: voidaanko tutkia näin?
                ottelut_list = new List<ottelu_table>();
            else
                ottelut_list.Clear();
        }

        // FUNKTIO: get_vastustajien_lkm_listassa()
        //
        // Kun pelaajat on syötetty listaan, niin tämä on sama kuin turnauksen_vastustajien_lkm
        // Mutta yhden vastustajan tapauksessa tämä on nolla, koska ei ole käytetty listaa!
        public int get_vastustajien_lkm_listassa()
        {
            return ottelut_list.Count;   // Myös: onko lista olemassa
        }


        // FUNKTIO: aloita_laskenta(int selo, int pelimaara)
        //
        // Ennen laskennan aloittamista asetetaan omat tiedot ja nollataan apumuuttujat.
        public void aloita_laskenta(int selo, int pelimaara)
        {
            this.selo = selo;
            this.pelimaara = pelimaara;
            selo_alkuperainen = selo;  // tästä aloitettiin!  XXX: jos selo päivittyy, niin?
            // vaihteluvälin alustus
            min_selo = constants.MAX_SELO;
            max_selo = constants.MIN_SELO;

            uusi_pelimaara = -1;
            turnauksen_vastustajien_lkm = 0;
            turnauksen_vastustajien_selosumma = 0;
            turnauksen_tulos = 0;
            odotustuloksien_summa = 0;
        }


        //   SETTERS & GETTERS

        public void set_selo(int selo)
        {
            this.selo = selo;
        }

        public int get_selo()
        {
            return selo;
        }

        public void set_pelimaara(int pelimaara)
        {
            this.pelimaara = pelimaara;
        }

        public int get_pelimaara()
        {
            return pelimaara;
        }

        public void set_syotetty_turnauksen_tulos(float f)
        {
            // tallennetaan kokonaislukuna
            //  0 = 0, tasapeli on 0.5:n sijaan 1, voitto on 1:n sijaan 2
            syotetty_turnauksen_tulos = (int)(2 * f + 0.01F); // pyöristys kuntoon
        }

        public int get_syotetty_turnauksen_tulos()
        {
            return syotetty_turnauksen_tulos;
        }


        //   SETTERS ONLY

        public void set_miettimisaika(int aika)
        {
            miettimisaika = aika;
        }


        //   GETTERS ONLY

        public int get_viimeisin_vastustaja()
        {
            return viimeisin_vastustaja;
        }

        public int get_selo_alkuperainen()
        {
            return selo_alkuperainen;
        }

        public int get_odotustulos()
        {
            return odotustulos;
        }

        public int get_odotustuloksien_summa()
        {
            return odotustuloksien_summa;
        }

        public int get_kerroin()
        {
            return kerroin;
        }

        public int get_uusiselo()
        {
            return uusi_selo;
        }

        public int get_uusipelimaara()
        {
            return uusi_pelimaara;
        }

        public int get_min_selo()
        {
            return min_selo;
        }

        public int get_max_selo()
        {
            return max_selo;
        }

        public int get_turnauksen_ottelumaara()
        {
            return turnauksen_vastustajien_lkm;  // päivitetty laskennan edetessä
        }

        public int get_turnauksen_keskivahvuus()
        {
            return (int)((float)turnauksen_vastustajien_selosumma / turnauksen_vastustajien_lkm + 0.5F);
        }

        public int get_turnauksen_tulos()
        {
            return turnauksen_tulos;
        }


        // Eri miettimisajoilla voi olla omia kertoimia
        private float maarita_lisakerroin()
        {
            float f = 1.0F;

            // Tämä ei vaikuta uuden pelaajan SELOn laskentaan
            if (miettimisaika == 60) // 60-89 minuuttia
                f = 0.5F;
            else if (miettimisaika == 15)  // 15-59 minuuttia
                f = (selo < 2300) ? 0.3F : 0.15F;
            return f;
        }


        // FUNKTIO: pelaa_ottelu(int vastustajan_selo, int tulos)
        //
        // pelaaja pelaa shakkiotteluita
        //
        // IN: vastustajan_selo 1000-2999
        // IN: ottelun tulos: 0 = 0, 1 = tasapeli, 2 = voitto
        //
        // -> selo muuttuu
        // -> pelimaara kasvaa yhdellä (jos ei ollut -1)
        public int pelaa_ottelu(int vastustajan_selo, int tulos)
        {
            // Uuden pelaajan laskennassa käytetään vastustajan seloa tuloksen mukaan -200 / +0 / +200
            int[] selomuutos = { -200, 0, 200 };  // indeksinä pisteet
            float lisakerroin;

            viimeisin_vastustaja = vastustajan_selo;

            lisakerroin = maarita_lisakerroin();

            // Vanhan pelaajan SELOn laskennassa käytetään odotustulosta ja kerrointa.
            // Lasketaan ja näytetään ne myös uuden pelaajan laskennassa.
            odotustulos = maarita_odotustulos(vastustajan_selo);
            kerroin     = maarita_kerroin();

// DEBUG:   MessageBox.Show("Odotus " + odotustulos + " kerroin " + kerroin + " selo " + selo + " pelim " + pelimaara + " vastus " + vastustajan_selo);

            if (pelimaara >= 0 && pelimaara <= 10)
            {
                // Uuden pelaajan SELO, kun pelimäärä 0-10
                // Jos pelimäärä on 0, niin nykyinenSelo-kentän arvolla ei ole merkitystä
                // XXX: tarvitseeko pyöristää jakolaskun jälkeen? (+0,5F)
                uusi_selo = (selo * pelimaara + (vastustajan_selo + selomuutos[tulos])) / (pelimaara + 1);
            }
            else
            {
                // vanhan pelaajan SELO, kun pelimäärä jätetty tyhjäksi tai on yli 10.
                // XXX: SELOn pyöristys? lisätään 0.5F, kaavassa lisäksi +0.1F
                uusi_selo = (int)(selo + kerroin * lisakerroin * ((tulos / 2F) - (odotustulos / 100F)) + 0.5F + 0.1F);
            }

            if (pelimaara >= 0)
                uusi_pelimaara = pelimaara + 1;

            // laskenta etenee!
            turnauksen_vastustajien_selosumma += vastustajan_selo;
            turnauksen_vastustajien_lkm++;   // tässä tieto myös kun vastustajia on vain yksi
            odotustuloksien_summa += odotustulos;  // = vastustajien_lkm * odotustulos
            turnauksen_tulos += tulos;

            // tallenna vaihteluväli
            if (uusi_selo < min_selo)
                min_selo = uusi_selo;
            if (uusi_selo > max_selo)
                max_selo = uusi_selo;

            return uusi_selo;
        }

        // FUNKTIO: pelaa_ottelu
        //
        // pelaa kaikki listalta ottelut_list löytyvät ottelut!
        public int pelaa_ottelu()
        {
            // ottelu_table ottelu1 = new ottelu_table();

            foreach (ottelu_table ottelu1 in ottelut_list)
            {
                // tarkista, että tiedot ovat alkuperäisessä järjestyksessään  -> OK!
                // MessageBox.Show(" vastustaja: " + ottelu1.get_vastustajan_selo() + "tulos: " + ottelu1.get_ottelun_tulos());

                // päivitä seloa jokaisella ottelulla, jotta käytetään laskennassa aina viimesintä
                selo = pelaa_ottelu(ottelu1.get_vastustajan_selo(), ottelu1.get_ottelun_tulos());

                // päivitä pelimäärää, jos oli annettu
                if (pelimaara != constants.MIN_PELIMAARA - 1)
                    pelimaara++;
            }


            // Pikashakin laskentakaavaan mennään täällä eli sitä käytetään vain
            // jos ottelun pisteet on annettu ensimmäisenä
            if (syotetty_turnauksen_tulos >= 0)   
            {
                // DEBUG         MessageBox.Show("laske turnauksen tulos: " + syotetty_turnauksen_tulos + "selo/alkup " + selo + "/" + selo_alkuperainen);

                // unohdetaan aiempi selolaskenta!
                // Mutta sieltä saadaan odotustuloksien_summa ja pelimaara valmiiksi!
                selo = selo_alkuperainen;
                turnauksen_tulos = syotetty_turnauksen_tulos;

                if (miettimisaika < 15)
                {
                    //
                    // pikashakilla on oma laskentakaavansa
                    //
                    // http://skore.users.paivola.fi/selo.html kertoo:
                    // Pikashakin laskennassa odotustulos lasketaan samoin, mutta ilman 0,85 - sääntöä.
                    // Itse laskentakaava onkin sitten hieman vaikeampi:
                    // pelo = vanha pelo + 200 - 200 * e(odotustulos - tulos) / 10 , kun saavutettu tulos on odotustulosta suurempi
                    // pelo = vanha pelo - 200 + 200 * e(tulos - odotustulos) / 10 , kun saavutettu tulos on odotustulosta pienempi
                    //            Loppuosan pitää olla e((tulos - odotustulos) / 10)  eli sulut lisää, jakolasku ensin.
                    // turnauksen tulos on kokonaisulukuna, pitää jakaa 2:lla
                    // odotustuloksien_summa on kokonaislukuja ja pitää jakaa 100:lla
                    if ((syotetty_turnauksen_tulos / 2F) > (odotustuloksien_summa / 100F))
                    {
                        uusi_selo =
                            (int)(selo + 200 - 200 * Math.Pow(Math.E, (odotustuloksien_summa / 100F - syotetty_turnauksen_tulos / 2F) / 10F)); 
                    }
                    else
                    {
                        uusi_selo =
                            (int)(selo - 200 + 200 * Math.Pow(Math.E, (syotetty_turnauksen_tulos / 2F - odotustuloksien_summa / 100F) / 10F));
                    }
                }
                else
                {
                    //
                    // pidemmän miettimisajan pelit eli >= 15 min
                    //
                    float lisakerroin = maarita_lisakerroin();
                    // myös 0.5F pyöristystä varten
                    uusi_selo =
                        (int)((selo + maarita_kerroin() * lisakerroin * (syotetty_turnauksen_tulos/2F - odotustuloksien_summa / 100F)) + (pelimaara * 0.1F) + 0.5F);
                    min_selo = max_selo = uusi_selo;  // tässä ei voida laskea minimi- eikä maksimiseloa
                }
            }

            return 0;
        }


        // FUNKTIO: maarita_odotustulos
        //
        // Selvitä ottelun odotustulos vertaamalla SELO-lukuja
        //    50 (eli 0,50), jos samantasoiset eli SELO-ero 0-3 pistettä
        //    > 50, jos voitto odotetumpi, esim. 51 jos 4-10 pistettä parempi
        //    < 50, jos tappio odotetumpi, esim. 49, jos 4-10 pistettä alempi
        //
        // Odotustulos voi olla enintään 92. Paitsi pikashakissa voi olla jopa 100.
        // ks. ohje http://skore.users.paivola.fi/selo.html
        private int maarita_odotustulos(int vastustajan_selo)
        {
            int odotustulos;
            // odotustulokset lasketaan aina alkuperäisellä selolla
            int SELO_diff = selo_alkuperainen - vastustajan_selo;    // XXX: selo_alkuperainen
            int diff = Math.Abs(SELO_diff);   // itseisarvo
            int sign = Math.Sign(SELO_diff);  // etumerkki
            
            // Käytä löydetyn paikan mukaista indeksiä laskennassa, 0-49
            // Paremmalle pelaajalle: odotusarvo 50+índeksi
            // Huonommalle pelaajalle: odotusarvo 50-indeksi
            // Jos piste-ero 736, niin ylimmillään 100 (1,00) ja alimmillaan 0 (0,00).
            int[] difftable =
            {
                    4, 11, 18, 26, 33,        40, 47, 54, 62, 69,
                    77, 84, 92, 99, 107,      114, 122, 130, 138, 146,
                    154, 163, 171, 180, 189,  198, 207, 216, 226, 236,
                    246, 257, 268, 279, 291,  303, 316, 329, 345, 358,
                    375, 392, 412, 433, 457,  485, 518, 560, 620, 736
                };

            // etsi taulukosta
            // esim. SELOt 1500 ja 1505, diff = 5 pistettä
            //   5 < difftable[0]? Ei, joten jatketaan...
            //   5 < difftable[1] On. Indeksi 1 ja odotustulos siten 49 (50-indeksi)
            int index = 0;
            while (index < difftable.Length)
            {
                if (diff < difftable[index])
                    break;
                index++;
            }

            // laske odotustulos taulukon paikkaa eli indeksiä käyttäen
            // jos ei löytynyt, niin index 50 ja odotustulos 0 (0,00) tai 100 (1,00)
            odotustulos = 50 + sign * index;

            // Pikashakissa ei odotustulosta rajoiteta 92:een
            return (odotustulos > 92 && miettimisaika >= 15) ? 92 : odotustulos;   // enintään 92 (0,92)
        }

        // FUNKTIO: maarita_kerroin
        //
        // Kerroin määritetään alkuperäisen selon mukaan.
        // ks. kerrointaulukko http://skore.users.paivola.fi/selo.html
        private int maarita_kerroin()
        {
            if (selo >= 2050)
                return 20;
            if (selo < 1650)
                return 45;
            return 40 - 5 * ((selo - 1650) / 100);
        }
    }
} // END Form1.cs
