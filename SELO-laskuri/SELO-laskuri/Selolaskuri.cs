//
// Selolaskuri   https://github.com/isuihko/selolaskuri
//
// 3.4.2018 Ismo Suihko 1.0.0.18
//
// Aiempi versio 7.1.2018 1.0.0.12, johon verrattuna tässä ei ole ulkoisia muutoksia muuta kuin päivämäärä.
//
// C#/.NET, Visual Studio Community 2015, Windows 7.
//
// Ensimmäinen C#/.NET -ohjelmani.
// Sisältää käyttöliittymän, syötteen tarkistuksen, laskentaa ja tuloksien näyttämisen.
//
// HUOM! Käännetty binääri/asennusohjelma setup.exe vaatinee nyt .NET Framework 4.6:n.
//
// KOODIA JÄRJESTELLÄÄN JA OPTIMOIDAAN VIELÄ, MUTTA TÄMÄN PITÄISI NYT TOIMIA AIKA HYVIN.
// Tein ohjelmasta vastaavan version myös Javalla, mutta siitä puuttunee osa ominaisuuksista (lista syötteistä ja menut).
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
// TODO: (TODO.1)
//      -Onko muuttujien ja aliohjelmien nimeäminen ym. C#-käytännön mukaista?
//       Tämä on nyt tehty ilman erityisiä ohjeita. Tullaan tarkistamaan!
//
//      -Tarkista luokkien ja objektiläheisen ohjelmoinnin käyttö, onko parantamista?
//         ... voi ollakin, koska tämä on ensimmäinen oikea C#-ohjelmani
//
//      -Jotain dataa on tuplana eri luokissa. Tutki ja korjaa.
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
// 21.-22.11.2017 -Ulkoasu, ikkuna ja tekstit koko, kommentointia, optimointia,
//             -Syötteen tarkistuksia, virheellisten arvojen nollaukset
//             -Näytetään virheilmoituksen aikana kentän arvo punaisena.
//  Publish --> Versio 1.0.0.3
//
// 24.11.2017  -Muutettu lomaketta, jotta voitaisiin jatkossa syöttää useamman ottelun tulokset
//             -Lisätty tuloskentät: Vastustajien keskivahvuus ja Tulos.
//             -Muutettu varsinainen laskenta omaksi aliohjelmakseen, jotta sitä voidaan kutsua
//              jokaiselle tulokselle erikseen.
//
// 25.-27.11.2017 -Luotu luokka: SeloPelaaja
//             -Nyt osaa lukea useamman ottelun tuloksen ja laskea niistä vahvuusluvun, vastustajien keskiselon
//              ja myös turnauksen pistemäärän. Ottelut annetaan formaatissa +1716 -1822 =1681 +1444.
//             -Tasapeli voidaan antaa myös ilman '='-merkkiä eli myös OK: +1716 -1822 1681 +1444.
//             -Lisätty mahdollisuus valita eri miettimisajat: Vähintään 90 min, 60-89 min, 15-59 min.
//              -> Vanhan pelaajan laskennassa käytetään eri kertoimia.
//              Estetty pääsy tuloskenttiin, valittu TAB-napin siirtymisjärjestys. Myös Enterillä laskenta.
//             -Lisätty mahdollisuus laskea tulos myös muodosta:
//                  ottelun_tulos selo selo selo, esim.  1.5 1722 1581 1608
//             jolloin laskennassa käytetään annettua tulosta, vastustajien selojen keskiarvoa ja odotustuloksien summaa.
//            -Syötekentän formatointia, mm. poistetaan kaikki alussa ja lopussa olevat välilyönnit.
//  Publish --> Versio 1.0.0.6
//
// 28.11.2017 -Lisätty pikashakin vahvuusluvun laskenta (PELO).
//            .Kun valitaan miettimisajaksi alle 15 min, niin vaihdetaan tekstit SELO->PELO.
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
// 12.12.2017 -VastustajanSelo-kentästä tehty ComboBox, josta voidaan nähdä ja valita aiempiakin syötteitä
//               Korvattu kenttä vastustajanSelo_input kentällä vastustajanSelo_combobox.
//               Uusi teksti lisätään listaan silloin kun se on kelvollinen, eikä ole siellä ennestään.
//            -pelaaja_class muutettu nimelle seloPelaaja
//            -vakioiden luokan nimi muutetut -> vakiot (koska ohjelma on muutenkin suomenkielinen)
//            -muutettu käyttämään uudempaa .NET Framework 4.6:tta, oli 4.5.2.
//  Publish --> Versio 1.0.0.10, myös github
//
// 7.1.2018   -Käytä C# properties: selo, pelimaara, miettimisaika ja selo_alkuperainen.
//            -Turhat apumuuttujat pois, kun tulos voidaan sijoittaa suoraankin
//                Esim. nykyinenSelo_input.Text = nykyinenSelo_input.Text.Trim();
//            -Math.Min, Max ja Round käyttöön.
//            -Luokat omiin tiedostoihinsa: -> seloPelaaja.cs ja vakiot.cs
//            -Lisätty valikot: Menu-> Ohjeita/Tietoja ohjelmasta/Laskentakaavat/Sulje ohjelma
//            -Varmistetaan ohjelmasta poistuminen, kun valitaan Menu->Sulje ohjelma tai suljetaan ikkuna.
//  Publish --> Versio 1.0.0.12, myös github
//
// 1.4.-6.4.2018 -Koodin järjestämistä ja siistimistä, mm. moduulijakoa uusittu
//              -"out"-parametrien käyttö pois syötetietojen tarkastuksista. Tuloksien
//               välittämisessä käytetään luokkaa, jossa on int-kentät mm. selo, pelimäärä.
//  Publish --> Versio 1.0.0.18, myös github
//
// TODO: (TODO.2)
//          -automaattinen testaus (Unit Test Project?), joka vaatinee hieman enemmän koodin muokkausta
//          -syötteen tarkastukset omaan luokkaansa?
//          -lisää koodi siistimistä
//          -poista tietojen tallentaminen tuplana (samoja tietoja on luokissa seloPelaaja ja tässä Form1:ssä).
//          -uuden pelaajan laskentakaava käyttöön myös, jos on annettu turnauksen tulos ja
//           sitten vastustajien selot (esim. 1.5 1722 1581 1608)
//
//

using System;
using System.Collections.Generic; // List<string>
//using System.ComponentModel;
//using System.Data;
using System.Drawing; // Color
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms; // MessageBox
using System.Text.RegularExpressions; // Regex rx, rx.Replace
using System.Linq; // tempString.Contains(',')


namespace Selolaskuri
{
    // lomakkeen käsittely, syötteen tarkastuksen ja laskennan kutsuminen

    public partial class Form1 : Form
    {

        // Luodaan shakinpelaaja, jolla tietoina mm. SELO ja pelimäärä.
        // Uudella pelaajalla SELO 1525, pelimäärä 0
        // Nämä arvot kopioidaan käyttöön, jos valitaan "Käytä uutta SELOa jatkolaskennassa"
        // ilman, että on suoritettu laskentaa sitä ennen.

        SeloPelaaja shakinpelaaja = new SeloPelaaja(1525, 0);
        Ottelulista ottelulista = new Ottelulista();

        public Form1()
        {
            InitializeComponent();
        }


        // tarkistaInput
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
        //    Virhetarkastukset ja -käsittelyt (ilmoitusikkuna) ovat kutsuttavissa rutiineissa.
        //
        private bool tarkistaSyote(Syotetiedot syotteet)
        {
            bool status = false;

            // tyhjennä ottelulista, johon tallennetaan vastustajat tuloksineen
            ottelulista.Tyhjenna();

            // ************ TARKISTA SYÖTE ************

            // ENSIN TARKISTA MIETTIMISAIKA. Tässä ei voi olla virheellista tietoa.
            syotteet.miettimisaika = tarkista_miettimisaika();

            do {
                // Hae ensin oma nykyinen vahvuusluku ja pelimäärä
                if ((syotteet.nykyinenSelo = tarkista_nykyinenselo()) == Vakiot.VIRHE_SELO)
                    break;
                if ((syotteet.nykyinenPelimaara = tarkista_pelimaara()) == Vakiot.VIRHE_PELIMAARA)
                    break;

                // TODO: Tässä mietittävää, kun on kaksi eri tallennustapaa
                //    JOS YKSI OTTELU,   saadaan muuttujassa vastustajanSelo vastustajan vahvuusluku,
                //                       ottelun tulosta ei voida tietää vielä
                //    JOS MONTA OTTELUA, ottelut tallennetaan tuloksineen listaan, jossa tuloksetkin ovat mukana
                //
                if ((syotteet.vastustajanSelo = tarkista_vastustajanSelo()) == Vakiot.VIRHE_SELO)
                    break;

                // merkkijonokin talteen sitten kun siitä on poistettu ylimääräiset välilyönnit
                syotteet.vastustajienSelot_str = vastustajanSelo_comboBox.Text;

                // vain jos otteluita ei ole listalla, niin tarkista ottelutuloksen valintanapit
                if (ottelulista.vastustajienLukumaara == 0) {
                    //
                    // Jos oli vain yksi ottelu, niin vastustajan vahvuusluku on vastustajanSelo-kentässä
                    // Haetaan vielä ottelunTulos -kenttään tulospisteet tuplana (0=tappio,1=tasapeli,2=voitto)
                    if ((syotteet.ottelunTulos = tarkista_ottelun_tulos()) == Vakiot.VIRHE_TULOS)
                        break;
                }

                status = true; // kaikki OK, jos päästy tänne asti

            } while (false);

            return status;
        }


        // Laske tulokset, yhden vastustajan ja useamman vastustajan tapaukset erillisiä
        // Ensin tarkistetaan syöte ja jos kaikki OK, niin lasketaan.
        private bool suorita_laskenta()
        {
            bool status;

            Syotetiedot syotteet = new Syotetiedot(); // tiedot nollataan

            // jos annettu yksi ottelu, niin vastustajan tiedot:
            //     vastustajanSelo ja pisteet
            // jos annettu useampi ottelu, niin ottelut ovat listassa ja vastustajien_lukumaara_listalla > 0
            // Lista on luokassa seloPelaaja (private ottelut_list).
            status = tarkistaSyote(syotteet);

            if (status) {

                // asettaa omat tiedot (selo ja pelimäärä) seloPelaaja-luokkaan, nollaa tilastotiedot ym.
                shakinpelaaja.aloita_laskenta(syotteet);

                //  *** NYT LASKETAAN ***

                // Lasketaanko yhtä ottelua vai turnausta?
                // Huom! Turnauksessakin voi olla vain yksi ottelu listalla.
                if (ottelulista.vastustajienLukumaara == 0) // tyhjä lista?
                    shakinpelaaja.pelaa_ottelu(syotteet.vastustajanSelo, syotteet.ottelunTulos); // pelaa yksi tietty ottelu
                else
                    shakinpelaaja.pelaa_ottelu(ottelulista);       // pelaa kaikki (turnauksen) ottelut listalta
            }

            return status; 
        }


        void nayta_tulokset()
        {
            // Laskettiinko yhtä ottelua vai turnausta?
            if (ottelulista.vastustajienLukumaara == 0) {
                // tyhjä lista, joten yksi ottelu -> näytä tässä ensin odotustulos ja kerroin
                seloEro_output.Text =
                    Math.Abs(shakinpelaaja.selo_alkuperainen - shakinpelaaja.viimeisin_vastustaja).ToString();
                odotustulos_output.Text = (shakinpelaaja.odotustulos / 100F).ToString("0.00");
                kerroin_output.Text = shakinpelaaja.kerroin.ToString();
                vaihteluvali_output.Text = "";  // ei vaihteluväliä, koska vain yksi luku laskettu

            } else {

                // Laskettiin turnauksena syötettyä vastustajien joukkoa (jossa tosin voi olla myös vain yksi ottelu)
                // tyhjennä yksittäisen ottelun tuloskentät
                seloEro_output.Text = "";
                // laskettu odotustulos näytetään, jos ei ollut uuden pelaajan laskenta
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
                // Vaihteluväli on vain, jos tulokset formaatissa "+1622 -1880 =1633"
                if (shakinpelaaja.syotetty_turnauksen_tulos < 0 && shakinpelaaja.turnauksen_ottelumaara > 1) {
                    vaihteluvali_output.Text =
                        shakinpelaaja.min_selo.ToString() + " - " + shakinpelaaja.max_selo.ToString();
                } else
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


        // Suoritetaan laskenta -button
        private void Laske_button_Click(object sender, EventArgs e)
        {
            if (suorita_laskenta()) {
                nayta_tulokset();

                // 12.12.1027: Annettu teksti talteen (jos ei ennestään ollut) -> Drop-down Combo box
                if (!vastustajanSelo_comboBox.Items.Contains(vastustajanSelo_comboBox.Text))
                    vastustajanSelo_comboBox.Items.Add(vastustajanSelo_comboBox.Text);
            }
        }


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
            if (pelimaara > Vakiot.VIRHE_PELIMAARA) {
                // vain, jos pelimaara oli annettu (muutoin on jo valmiiksi tyhjä)
                pelimaara_input.Text = pelimaara.ToString();
            }
            vastustajanSelo_comboBox.Select();
        }


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
            if (suorita_laskenta())
                nayta_tulokset();
        }

        private void tulosVoitto_Button_Enter(object sender, EventArgs e)
        {
            tulosVoitto_Button.Checked = true;
            if (suorita_laskenta())
                nayta_tulokset();
        }

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


        private void miettimisaika_vaihda_tekstit(Vakiot.vaihda_miettimisaika_enum suunta)
        {
            string alkup, uusi;

            if (suunta == Vakiot.vaihda_miettimisaika_enum.VAIHDA_SELOKSI) {
                alkup = "PELO";
                uusi = "SELO";
                TuloksetPistemaaranKanssa_teksti.Font = new Font(TuloksetPistemaaranKanssa_teksti.Font, FontStyle.Regular);
            } else {
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
            miettimisaika_vaihda_tekstit(Vakiot.vaihda_miettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_60_89_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(Vakiot.vaihda_miettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_11_59_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(Vakiot.vaihda_miettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_enint10_Button_CheckedChanged(object sender, EventArgs e)
        {
            miettimisaika_vaihda_tekstit(Vakiot.vaihda_miettimisaika_enum.VAIHDA_PELOKSI);
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




        //  LOMAKKEEN KENTTIEN ARVOJEN TARKISTUKSET JA VIRHEILMOITUKSET

        // Nämä miettimisajan valintapainikkeet ovat omana ryhmänään paneelissa
        // Aina on joku valittuna. Oletuksena: vähintään 90 min
        private int tarkista_miettimisaika()
        {
            int aika;

            if (miettimisaika_vah90_Button.Checked)
                aika = Vakiot.Miettimisaika_vah90min;
            else if (miettimisaika_60_89_Button.Checked)
                aika = Vakiot.Miettimisaika_60_89min;
            else if (miettimisaika_11_59_Button.Checked)
                aika = Vakiot.Miettimisaika_11_59min;
            else
                aika = Vakiot.Miettimisaika_enint10min;

            return aika;
        }

        // Tarkista Oma SELO -kenttä, oltava numero ja rajojen sisällä
        // Paluuarvo joko kelvollinen SELO (MIN_SELO .. MAX_SELO) tai negatiivinen virhestatus
        private int tarkista_nykyinenselo()
        {
            bool status = true;
            int tulos;

            nykyinenSelo_input.Text = nykyinenSelo_input.Text.Trim();  // remove leading and trailing white spaces

            // onko numero ja jos on, niin onko sallittu numero
            if (int.TryParse(nykyinenSelo_input.Text, out tulos) == false)
                status = false;
            else if (tulos < Vakiot.MIN_SELO || tulos > Vakiot.MAX_SELO)
                status = false;

            if (!status) {
                string message = String.Format("VIRHE: Nykyisen SELOn oltava numero {0}-{1}.",
                    Vakiot.MIN_SELO, Vakiot.MAX_SELO);
                nykyinenSelo_input.ForeColor = Color.Red;
                MessageBox.Show(message);
                nykyinenSelo_input.ForeColor = Color.Black;

                // Tyhjennä liian täysi kenttä? Takaisin kenttään ja virhestatus
                if (nykyinenSelo_input.Text.Length > Vakiot.MAX_PITUUS)
                    nykyinenSelo_input.Text = "";
                nykyinenSelo_input.Select();
                tulos = Vakiot.VIRHE_SELO;
            }
            return tulos;
        }

        //
        // tarkista pelimäärä
        // Saa olla tyhjä, mutta jos annettu, oltava numero, joka on 0-9999.
        // Käytetään uuden pelaajan laskentakaavaa, jos pelimäärä on 0-10.
        // Paluuarvo joko kelvollinen pelimäärä, PELIMAARA_TYHJA tai VIRHE_PELIMAARA.
        //
        private int tarkista_pelimaara()
        {
            bool status = true;
            int tulos;

            pelimaara_input.Text = pelimaara_input.Text.Trim();  // remove leading and trailing white spaces

            if (string.IsNullOrWhiteSpace(pelimaara_input.Text)) {
                tulos = Vakiot.PELIMAARA_TYHJA; // Tyhjä kenttä on OK
            } else {
                // onko numero ja jos on, niin onko sallittu numero
                if (int.TryParse(pelimaara_input.Text, out tulos) == false)
                    status = false;
                else if (tulos < Vakiot.MIN_PELIMAARA || tulos > Vakiot.MAX_PELIMAARA)
                    status = false;

                if (!status) {
                    string message = String.Format("VIRHE: pelimäärän voi olla numero väliltä {0}-{1} tai tyhjä.",
                        Vakiot.MIN_PELIMAARA, Vakiot.MAX_PELIMAARA);
                    pelimaara_input.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    pelimaara_input.ForeColor = Color.Black;

                    // Tyhjennä liian täysi kenttä? Takaisin kenttään ja virhestatus
                    if (pelimaara_input.Text.Length > Vakiot.MAX_PITUUS)
                        pelimaara_input.Text = "";
                    pelimaara_input.Select();
                    tulos = Vakiot.VIRHE_PELIMAARA;
                }
            }
            return tulos;
        }

        // Tarkista Vastustajan SELO -kenttä
        //
        // Syöte voi olla annettu kolmella eri formaatilla:
        //  1)  1720   -> ja sitten tulos valintanapeilla
        //  2)  2,5 1624 1700 1685 1400    Eli aloitetaan kokonaispistemäärällä.
        //                                 SELOt ilman erillisiä tuloksia.
        //  3)  +1624 -1700 =1685 +1400    jossa  '+' voitto, '=' tasapeli ja '-' tappio.
        //                                 Tasapeli voidaan myös antaa ilman '='-merkkiä.
        //
        // Yhden ottelun tulos voidaan antaa kolmella tavalla:
        //   1)  1720      ja tulos erikseen valintanapeilla, esim. 1=voitto, 1/2=tasapeli tai 0=tappio
        //   2)  -1720  (tappio), =1720    (tasapeli) tai +1720  (voitto)
        //   3)  0 1720 (tappio), 0.5 1720 (tasapeli) tai 1 1720 (voitto)
        //
        // Kahden tai useamman ottelun tulos voidaan syöttää kahdella eri tavalla
        //   1) 2,5 1624 1700 1685 1400
        //   2) +1624 -1700 =1685 +1400  (Huom! myös -1624 +1700 +1685 1400 laskee saman vahvuusluvun)
        // HUOM! Jos tuloksessa on desimaalit väärin, esim. 2.37 tai 0,9,
        //       niin ylimääräiset desimaalit "pyöristyvät" alas -> 2,0 tai 0,5.

        // XXX: Saako tämän moduulin jaettua pienempiin osiin?
        //
        // TODO: Tässä mietittävää, kun on kaksi eri tallennustapaa
        //    JOS YKSI OTTELU,   saadaan muuttujassa vastustajanSelo vastustajan vahvuusluku,
        //                       ottelun tulosta ei voida tietää vielä
        //    JOS MONTA OTTELUA, ottelut tallennetaan tuloksineen listaan, jossa tuloksetkin ovat mukana
        //
        // Paluuarvo joko kelvollinen seloluku, PELIMAARA_TYHJA tai virhestatus VIRHE_SELO.
        private int tarkista_vastustajanSelo()
        {
            bool status = true;
            int tulos = 0;           // palautettava vastustajan selo
            int virhekoodi = 0;  // tai tästä saatu virhestatus

            bool onko_turnauksen_tulos = false;  // oliko tulos ensimmäisenä?
            float syotetty_tulos = 0F;           // tähän sitten sen tulos desimaalilukuna (esim. 2,5)

            vastustajanSelo_comboBox.Text = vastustajanSelo_comboBox.Text.Trim();  // remove leading and trailing white spaces

            // Ensin helpot tarkistukset:
            // 1) Kenttä ei saa olla tyhjä
            if (string.IsNullOrWhiteSpace(vastustajanSelo_comboBox.Text)) {
                status = false;
            } else if (vastustajanSelo_comboBox.Text.Length == Vakiot.SELO_PITUUS) {
                if (int.TryParse(vastustajanSelo_comboBox.Text, out tulos) == false) {
                    // 2) Jos on annettu neljä merkkiä (esim. 1728), niin sen on oltava numero
                    status = false;
                } else if (tulos < Vakiot.MIN_SELO || tulos > Vakiot.MAX_SELO) {
                    // 3) Numeron on oltava sallitulla lukualueella
                    //    Mutta jos oli OK, niin vastustajanSelo sisältää nyt sallitun vahvuusluvun eikä tulla tähän
                    status = false;
                }
                // NOTE! Yhden ottelun tulosta ei tallenneta listaan, koska tässä ei tiedetä tulosta
                //       Tai tiedettäisiin, jos tarkistettaisiin painikkeet.
                //
                // Jos status = true, niin
                //   vastustajanSelo = annettu vastustajan vahvuusluku ja tulos saadaan tulos-painikkeista
                // Jos status == false, niin virhestatuksena käytetään oletusstatusta VIRHE_SELO

            } else {
                // Jäljellä vielä hankalammat tapaukset:
                // 4) turnauksen tulos+vahvuusluvut, esim. 2,5 1624 1700 1685 1400
                // 5) vahvuusluvut, joissa kussakin tulos  +1624 -1700 =1685 +1400

                // kentässä voidaan antaa alussa turnauksen tulos, esim. 0.5, 2.0, 2.5, 7.5 eli saadut pisteet
                shakinpelaaja.set_syotetty_turnauksen_tulos(-1.0F);  // oletus: ei annettu

                // poista sanojen väleistä ylimääräiset välilyönnit
                string pattern = "\\s+";    // \s = any whitespace, + one or more repetitions
                string replacement = " ";   // tilalle vain yksi välilyönti
                Regex rx = new Regex(pattern);
                vastustajanSelo_comboBox.Text = rx.Replace(vastustajanSelo_comboBox.Text, replacement);

                // Nyt voidaan jakaa syöte välilyönneillä erotettuihin merkkijonoihin
                List<string> selo_lista = vastustajanSelo_comboBox.Text.Split(' ').ToList();

                // Apumuuttujat
                int selo1 = Vakiot.MIN_SELO;
                int tulos1 = 0;
                bool ensimmainen = true;

                // Tutki vastustajanSelo_comboBox-kenttä
                // Tallenna listaan selo_lista vastustajien SELO:t ja tulokset merkkijonona
                foreach (string vastustaja in selo_lista) {
                    if (ensimmainen) {
                        string tempString = vastustaja;
                        // 4) Onko annettu kokonaispistemäärä? (eli useamman ottelun yhteistulos)
                        ensimmainen = false;
                        // Auttavatkohan nuo NumberStyles ja CultureInfo... testaa
                        // XXX: käykö desimaaliluvun formaatti 1,5 tai 1.5?
                        //if (float.TryParse(tulos, out syotetty_tulos)) 

                        // Laita molemmat 1.5 ja 1,5 toimimaan, InvariantCulture
                        if (tempString.Contains(','))  // korvaa pilkku pisteellä...
                            tempString = tempString.Replace(',', '.');
                        if (float.TryParse(tempString, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out syotetty_tulos) == true) {
                            if (syotetty_tulos >= 0.0F && syotetty_tulos <= 99.5F) {
                                // HUOM! Jos tuloksessa on desimaalit väärin, esim. 2.37 tai 0,9,
                                //       niin ylimääräiset desimaalit "pyöristyvät" alas -> 2,0 tai 0,5.
                                onko_turnauksen_tulos = true;
                                shakinpelaaja.set_syotetty_turnauksen_tulos(syotetty_tulos);

                                // alussa oli annettu turnauksen lopputulos, jatka SELOjen tarkistamista
                                // Nyt selojen on oltava ilman tulosmerkintää!
                                continue;
                            }
                            // Jos ei saatu kelvollista lukua, joka käy tuloksena, niin jatketaan
                            // ja katsotaan, saadaanko vahvuusluku sen sijaan (jossa voi olla +/=/-)
                        }
                    }

                    // Tarkista yksittäiset vastustajien vahvuusluvut

                    // merkkijono voi alkaa merkillä '+', '=' tai '-'
                    // Mutta tasapeli voidaan antaa myös ilman '='-merkkiä
                    // Jos oli annettu turnauksen tulos, niin selot on syötettävä näin ilman tulosta
                    if (vastustaja.Length == Vakiot.SELO_PITUUS) {  // numero (4 merkkiä)
                        if (int.TryParse(vastustaja, out selo1) == false) {
                            virhekoodi = Vakiot.VIRHE_SELO;  // -> virheilmoitus, ei ollut numero
                            status = false;
                            break;
                        } else if (selo1 < Vakiot.MIN_SELO || selo1 > Vakiot.MAX_SELO) {
                            virhekoodi = Vakiot.VIRHE_SELO;  // -> virheilmoitus, ei sallittu numero
                            status = false;
                            break;
                        }

                        // Tallennetaan tasapelinä, ei ollut +:aa tai -:sta
                        ottelulista.LisaaOttelunTulos(selo1, Vakiot.TASAPELIx2);

                    } else if (onko_turnauksen_tulos == false && vastustaja.Length == Vakiot.MAX_PITUUS) {
                        // 5)
                        // Erillisten tulosten antaminen hyväksytään vain, jos turnauksen
                        // lopputulosta ei oltu jo annettu (turnauksen_tulos false)
                        if (vastustaja[0] >= '0' && vastustaja[0] <= '9') {
                            // tarkistetaan, voidaan olla annettu viisinumeroinen luku
                            // 10000 - 99999... joten anna virheilmoitus vahvuusluvusta
                            virhekoodi = Vakiot.VIRHE_SELO;
                            status = false;
                        } else {
                            // Ensimmäinen merkki kertoo tuloksen
                            switch (vastustaja[0]) {
                                case '+':
                                    tulos1 = Vakiot.VOITTOx2;
                                    break;
                                case '=':
                                    tulos1 = Vakiot.TASAPELIx2;
                                    break;
                                case '-':
                                    tulos1 = Vakiot.TAPPIOx2;
                                    break;
                                default: // ei sallittu tuloksen kertova merkki
                                    virhekoodi = Vakiot.VIRHE_YKSITTAINEN_TULOS;
                                    status = false;
                                    break;
                            }
                        }

                        // jos virhe, pois foreach-loopista
                        if (!status)
                            break;

                        // Selvitä vielä tuloksen perässä oleva numero
                        // tarkista sitten, että on sallitulla alueella
                        if (int.TryParse(vastustaja.Substring(1), out selo1) == false) {
                            virhekoodi = Vakiot.VIRHE_SELO;  // -> virheilmoitus, ei ollut numero
                            status = false;
                        } else if (selo1 < Vakiot.MIN_SELO || selo1 > Vakiot.MAX_SELO) {
                            status = false;
                            break;
                        }
                        // lisää vahvuusluku ja tulos listaan
                        ottelulista.LisaaOttelunTulos(selo1, tulos1);
                    } else {
                        // pituus ei ollut
                        //   - SELO_PITUUS (esim. 1234) 
                        //   - MAX_PITUUS (esim. +1234) silloin kun tulos voidaan antaa
                        // Tähän tullaan myös, jos turnauksen kokonaistulos oli annettu ennen vahvuuslukuja,
                        // koska silloin annetaan vain pelkät vahvuusluvut ilman yksittäisiä tuloksia.
                        // Ei ole sallittu  2,5 +1624 =1700 -1685 +1400 (oikein on 2,5 1624 1700 1685 1400)
                        virhekoodi = Vakiot.VIRHE_SELO;
                        status = false;
                        break;
                    }

                    // Oliko asetettu virhe, mutta ei vielä poistuttu foreach-loopista?
                    if (!status)
                        break;

                } // foreach (käydään läpi syötteen numerot)

                // Lisää tarkastuksia
                // 6) Annettu turnauksen tulos ei saa olla suurempi kuin pelaajien lukumäärä
                if (status && onko_turnauksen_tulos) {
                    // Vertailu kokonaislukuina, esim. syötetty tulos 3.5 ja pelaajia 4, vertailu 7 > 8.
                    if ((int)(2 * syotetty_tulos + 0.01F) > 2 * ottelulista.vastustajienLukumaara) {
                        virhekoodi = Vakiot.VIRHE_TURNAUKSEN_TULOS;  // tästä oma virheilmoitus
                        status = false;
                    }
                }
            }

            // VIRHEILMOITUKSET
            // Kolme mahdollista virhestatusta
            //    - virheellinen kokonaispistemäärä
            //    - väärin annettu ottelun tulos
            //    - virheellinen vahvuusluku, ei numero tai ei 
            //     
            if (!status) {
                string message;
                if (virhekoodi == Vakiot.VIRHE_TURNAUKSEN_TULOS) {
                    message =
                        String.Format("VIRHE: Turnauksen pistemäärä (annettu {0}) voi olla enintään sama kuin vastustajien lukumäärä ({1}).",
                        syotetty_tulos, ottelulista.vastustajienLukumaara);

                } else if (virhekoodi == Vakiot.VIRHE_YKSITTAINEN_TULOS) {
                    message =
                        String.Format("VIRHE: Yksittäisen ottelun tulos voidaan antaa merkeillä +(voitto), =(tasapeli) tai -(tappio), esim. +1720. Tasapeli voidaan antaa muodossa =1720 ja 1720.");
                } else {
                    // oletuksena tulostettava virhestatus, esim. jos kenttä oli tyhjä tai numero oli rajojen ulkopuolella
                    message =
                        String.Format("VIRHE: Vahvuusluvun on oltava numero {0}-{1}.", Vakiot.MIN_SELO, Vakiot.MAX_SELO);
                }

                vastustajanSelo_comboBox.ForeColor = Color.Red;
                MessageBox.Show(message);
                vastustajanSelo_comboBox.ForeColor = Color.Black;

                // Ei tyhjennetä kenttää, jotta sitä on helpompi korjata
                //              if (vastustajanSelo_comboBox.Text.Length > MAX_PITUUS)
                //                  vastustajanSelo_comboBox.Text = "";
                // Kentästä on kuitenkin jo poistettu ylimääräiset välilyönnit
                vastustajanSelo_comboBox.Select();

                tulos = Vakiot.VIRHE_SELO;  // jos epäonnistui, palautetaan yksi virhestatus
            }

            return tulos;
        }

        // Tarkista ottelun tulos -painikkeet ja tallenna niiden vaikutus
        // pisteet: tappiosta 0, tasapelistä puoli ja voitosta yksi
        //          palautetaan kokonaislukuna 0, 1 ja 2
        public int tarkista_ottelun_tulos()
        {
            int pisteet = -1;

            if (tulosTappio_Button.Checked)
                pisteet = Vakiot.TAPPIOx2;
            else if (tulosTasapeli_Button.Checked)
                pisteet = Vakiot.TASAPELIx2;
            else if (tulosVoitto_Button.Checked)
                pisteet = Vakiot.VOITTOx2;
            else {
                MessageBox.Show("Ottelun tulosta ei annettu!");
                tulosTappio_Button.Select();   // siirry ensimmäiseen valintanapeista
                pisteet = Vakiot.VIRHE_TULOS;
            }
            return pisteet;
        }
    }

} // END Form1.cs
