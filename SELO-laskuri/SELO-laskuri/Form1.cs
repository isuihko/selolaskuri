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
//      -Tietoja on tuplana eri luokissa. Tutki ja korjaa.
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
// 2.4.2018   -Lisää koodin siistimistä ja järjestelyä,
//             mm. syötteen tarkastus omaan moduuliin Form1.Tarkistukset.cs
//            -Tarkempia tarkastuksia, tarkempia virheilmoituksia.
//  Publish --> Versio 1.0.0.16, myös github
//
// 3.4.2018   -"out"-parametrien käyttö pois syötetietojen tarkastuksista. Tuloksien
//              välittämisessä käytetään luokkaa, jossa on int-kentät mm. selo, pelimäärä.
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
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing; // Color
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms; // MessageBox


namespace Selolaskuri
{
    // lomakkeen käsittely, syötteen tarkastuksen ja laskennan kutsuminen

    public partial class Form1 : Form
    {

        // Luodaan shakinpelaaja, jolla tietoina mm. SELO ja pelimäärä.
        // Uudella pelaajalla SELO 1525, pelimäärä 0
        // Nämä arvot kopioidaan käyttöön, jos valitaan "Käytä uutta SELOa jatkolaskennassa"
        // ilman, että on suoritettu laskentaa sitä ennen.

        seloPelaaja shakinpelaaja = new seloPelaaja(1525, 0);

        // Tietorakenne syötetiedoille
        //
        // XXX: Muuten OK, mutta entä jos on syötetty useita vastustajia,
        // XXX: niin ne ovat tiedossa tuloksineen vain seloPelaaja-luokassa.
        // XXX: Ja nämä tiedot ovat kaikki seloPelaaja-luokassa. Toistoa.
        private class Syotetiedot
        {
            public int nykyinenSelo;
            public int nykyinenPelimaara;
            public int vastustajanSelo;
            public int ottelunTulos;

            public Syotetiedot()
            {
                nykyinenSelo = 0;
                nykyinenPelimaara = 0;
                vastustajanSelo = 0;
                ottelunTulos = 0;
            }
        }

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
        private bool tarkistaInput(Syotetiedot syotteet)
        {
            bool status = false;

            // ************ TARKISTA SYÖTE ************

            // ENSIN TARKISTA MIETTIMISAIKA. Tässä ei voi olla virheellista tietoa.
            tarkista_miettimisaika();

            do {
                // Hae ensin oma nykyinen vahvuusluku ja pelimäärä
                if ((syotteet.nykyinenSelo = tarkista_nykyinenselo()) == vakiot.VIRHE_SELO)
                    break;
                if ((syotteet.nykyinenPelimaara = tarkista_pelimaara()) == vakiot.VIRHE_PELIMAARA)
                    break;

                // TODO: Tässä mietittävää, kun on kaksi eri tallennustapaa
                //    JOS YKSI OTTELU,   saadaan muuttujassa vastustajanSelo vastustajan vahvuusluku,
                //                       ottelun tulosta ei voida tietää vielä
                //    JOS MONTA OTTELUA, ottelut tallennetaan tuloksineen listaan, jossa tuloksetkin ovat mukana
                //
                if ((syotteet.vastustajanSelo = tarkista_vastustajanSelo()) == vakiot.VIRHE_SELO)
                    break;

                // DEBUG:   MessageBox.Show("Syöte: " + nykyinenSelo + " " + pelimaara + " " + vastustajanSelo);

                // jos otteluita listalla, niin ottelutuloksen valintanapeilla ei ole merkitystä
                if (shakinpelaaja.vastustajien_lukumaara_listalla > 0) {
                    status = true;
                    break;
                }

                // Jos oli vain yksi ottelu, niin sen yhden vastustajan vahvuusluku on muuttujassa vastustajanSelo.
                // Haetaan vielä ottelun tulos muuttujaan pisteet
                if ((syotteet.ottelunTulos = tarkista_ottelun_tulos()) == vakiot.VIRHE_TULOS)
                    break;

                status = true; // kaikki OK

            } while (false);

            return status;
        }


        // Laske tulokset, yhden vastustajan ja useamman vastustajan tapaukset erillisiä
        // Ensin tarkistetaan syöte ja jos kaikki OK, niin lasketaan.
        private bool suorita_laskenta()
        {
            Syotetiedot syotteet = new Syotetiedot(); // tiedot nollataan

            // tyhjennä ottelulista!
            shakinpelaaja.listan_alustus();

            // jos annettu yksi ottelu, niin vastustajan tiedot:
            //     vastustajanSelo ja pisteet
            // jos annettu useampi ottelu, niin ottelut ovat listassa ja vastustajien_lukumaara_listalla > 0
            // Lista on luokassa seloPelaaja (private ottelut_list).
            if (tarkistaInput(syotteet) == false)
                return false;

            // asettaa omat tiedot (selo ja pelimäärä) seloPelaaja-luokkaan, nollaa tilastotiedot ym.
            shakinpelaaja.aloita_laskenta(syotteet.nykyinenSelo, syotteet.nykyinenPelimaara);

            //  *** NYT LASKETAAN ***

            // Lasketaanko yhtä ottelua vai turnausta?
            // Huom! Turnauksessakin voi olla vain yksi ottelu listalla.
            if (shakinpelaaja.vastustajien_lukumaara_listalla == 0) // tyhjä lista?
                shakinpelaaja.pelaa_ottelu(syotteet.vastustajanSelo, syotteet.ottelunTulos); // pelaa yksi tietty ottelu
            else
                shakinpelaaja.pelaa_ottelu();       // pelaa kaikki (turnauksen) ottelut listalta

            return true;
        }

        void nayta_tulokset()
        {
            // Laskettiinko yhtä ottelua vai turnausta?
            if (shakinpelaaja.vastustajien_lukumaara_listalla == 0) {
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
            if (pelimaara > vakiot.VIRHE_PELIMAARA) {
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

        private enum vaihda_miettimisaika_enum { VAIHDA_SELOKSI, VAIHDA_PELOKSI };

        private void miettimisaika_vaihda_tekstit(vaihda_miettimisaika_enum suunta)
        {
            string alkup, uusi;

            if (suunta == vaihda_miettimisaika_enum.VAIHDA_SELOKSI) {
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
