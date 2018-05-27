//
// Selolaskuri   https://github.com/isuihko/selolaskuri
//
// 27.5.2018 Ismo Suihko  -  Huhtikuusta alkaen järjestetty koodia melkoisesti!
//
// C#/.NET, Visual Studio Community 2015, Windows 7.
//
// Ensimmäinen C#/.NET -ohjelmani.
// Sisältää käyttöliittymän, syötteen tarkistuksen, laskentaa ja tuloksien näyttämisen.
//
// Ohjelma laskee pelaajalle vahvuusluvun miettimisaikojen vähintään 90, 60-89 ja 11-59 minuutin peleistä,
// sekä pikapelistä, kun miettimisaika on enintään 10 minuuttia.
//
// Jos pelimäärä 0-10, käytetään uuden pelaajan selon kaavaa. Jos pelataan turnaus, niin kaikissa turnauksen otteluissa.
// Jos pelimäärä tyhjä tai > 10, käytetään normaalia laskukaavaa.
//
//
// HUOM! Käännetty binääri/asennusohjelma setup.exe vaatinee nyt .NET Framework 4.6:n.
//
// KOODIA JÄRJESTELLÄÄN JA OPTIMOIDAAN VIELÄ, MUTTA TÄMÄN PITÄISI NYT TOIMIA AIKA HYVIN.
// Isoimmat tulevat muutokset liittyvät automaattiseen testaukseen, joka puuttuu vielä kokonaan.
//
// JAVA-versio: Tein ohjelmasta vastaavan version Javalla, mutta se on pahasti kesken ja
//              siitä puuttuu osa ominaisuuksista (lista syötteistä ja menut).
//              Ja myös se pitäisi järjestää uusiksi.
//
// Kuvia ohjelman toiminnasta:
//
// Ensimmäinen kuva "Selolaskuri PELOn laskenta turnauksesta.png" - pikashakin laskenta
// https://github.com/isuihko/selolaskuri/blob/master/Selolaskuri%20PELOn%20laskenta%20turnauksesta.png
//      Miettimiaika     : enint. 10 min
//      Oma PELO         : 1996
//      Oma pelimäärä    : tyhjä
//      Vastustajan PELO : 10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684
// Tulos
//      Uusi PELO        : 2033 +37 (jos lasketaan miettimisajalla väh. 90 min, saadaan tulos 2048 +52)
//
// Toinen kuva "Selolaskuri uuden pelaajan SELO.png" - uuden pelaajan vahvuusluvun laskenta
// https://raw.githubusercontent.com/isuihko/selolaskuri/master/Selolaskuri%20uuden%20pelaajan%20SELO.png
//      Miettimisaika    : väh. 90 min.
//      Oma SELO         : 1525
//      Oma pelimäärä    : 0
//      Vastustajan SELO : +1525 +1441 -1973 +1718 -1784 -1660 -1966
// Tulos
//      Uusi SELO        : 1695 +170 (1683 - 1764)
//
// Samaan tulokseen päästään jälkimmäisessä, jos annetaan tulokset formaatissa
//      Vastustajan SELO : 3 1525 1441 1973 1718 1784 1660 1966
// Tai lähtemällä tilanteesta 1525 ja 0 ja sitten syöttämällä kukin ottelu erikseen
// (vastustajan selo ja valintanapeista tulos, esim. 1525 (x) 1 = voitto)
// ja klikkaamalla Käytä uutta SELOa jatkolaskennassa (seuraava 1441 (x) 1 = voitto jne.).
//
//
// Lomakkeella kentät tässä järjestyksessä ylhäältä alas:
//
//    Valintanapit: Miettimisaika: väh. 90 min, 60-89 min, 11-59 min ja enint. 10 min.
//
//    Numerot: Oma SELO (1000-2999)
//             Oma pelimäärä (tyhjä tai jos uusi pelaaja: 0-10)
//    Numero/numeroita/merkkijono, joka muistaa laskennassa käytetyt syötteet (comboBox):
//             Vastustajan SELO. Tai monta tuloksineen: +1725 -1810 =1612 (tai 1612)
//             (ohjetekstiä)   Montaa vahvuuslukua syötettäessä voitto +, tasapeli = tai tyhjä, tappio -
//                             Tai pistemäärä ja vastustajien SELOt: 1.5 1725 1810 1612
//
//    Valintanapit: Ottelun tulos, jossa vaihtoehdot: 0 = tappio, 1/2 = tasapeli ja 1 = voitto.
//
// Painikkeet:
//    - Laske uusi SELO
//    - Käytä uutta SELOa jatkolaskennassa -> kopioi tuloksen & pelimäärän uutta laskentaa varten
//                                            Jos ei vielä laskettu, asettaa arvot 1525 ja 0.
// Lisäksi laskenta tehdään, kun vastustajan SELO-kentässä painetaan Enter sekä
// yhden vastustajan tapauksessa kun selataan ottelun tuloksia valintapainikkeista.
//
// Kun valittu miettimisajaksi enintään 10 minuuttia eli pikashakki,
// niin lomakkeen SELO-tekstit vaihdetaan -> PELO.
//
// Laskentakaavat:  http://skore.users.paivola.fi/selo.html  (odotustulos, kerroin, SELO)
// Käytössä on kaavat miettimisajoille väh. 90 min, 60-89 min ja 11-59 min
// sekä PELO:n laskenta enintään 10 min.
//
// Tulostiedot:
//                  Uusi SELO (PELO)      laskettu uusi vahvuusluku
//                  Vahvuusuvun muutos sekä useamman vastustajan tapauksessa SELOn vaihteluväli laskennan aikana.
//                  Uusi pelimäärä        jos annettu, niin vastustajien lukumäärällä lisättynä
//                  Piste-ero             annettujen selojen ero (vain jos yksi vastustaja)
//                  Odotustulos           voiton todennäköisyys/turnauksessa kaikkien odotustuloksien summa
//                  Kerroin               laskennassa käytetty kerroin
//                  Vastustajien keskivahvuus
//                  Ottelun/turnauksen tulos
//
//
// XXX: Versiohistoriaa voi ehkä tiivistääkin
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
// 24.-27.11.2017 -Muutettu lomaketta, jotta voitaisiin jatkossa syöttää useamman ottelun tulokset
//             -Lisätty tuloskentät: Vastustajien keskivahvuus ja Tulos.
//             -Muutettu varsinainen laskenta omaksi aliohjelmakseen, jotta sitä voidaan kutsua
//              jokaiselle tulokselle erikseen.
//             -Luotu luokka: SeloPelaaja
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
//            -Vaihdettu ohjelman nimi ikkunasta "SELO-laskuri" -> "Selolaskuri".
//            -Myös namespace SELO_laskuri -> Selolaskuri, tiedostot: Form1.cs ja Program.cs
//            -Nyt käännetty ohjelma tulee nimelle "Selolaskuri.exe".
//  Publish --> Versio 1.0.0.8 -> github/isuihko/selolaskuri (ensimmäinen versio siellä)
//            -Pyöristyksiä: get_turnauksen_keskivahvuus() jakolaskuun +0.5F ennen kokonaisluvuksi muuttamista.
//            -Shakki.net:n selolaskentasivujen tuloksiin vertaamalla huomattu, että pikashakin laskentakaavaan 
//             tarvittiin korjauksia. Nyt laskee oikein!
//  Publish --> Versio 1.0.0.9, myös github
// 30.11.2017 -Oma luokka vakioille, jotta niitä ei tarvitse määritellä kaikissa luokissa.
//             Kaikki vakiot nyt ovat luokassa constants (myöhemmin nimetty Vakiot).
//
// 12.12.2017 -VastustajanSelo-kentästä tehty ComboBox, josta voidaan nähdä ja valita aiempiakin syötteitä
//               Korvattu kenttä vastustajanSelo_in kentällä vastustajanSelo_combobox.
//               Uusi teksti lisätään listaan silloin kun se on kelvollinen, eikä ole siellä ennestään.
//            -pelaaja_class muutettu nimelle seloPelaaja
//            -vakioiden luokan nimi muutetut -> vakiot (koska ohjelma on muutenkin suomenkielinen)
//            -muutettu käyttämään uudempaa .NET Framework 4.6:tta, oli 4.5.2.
//  Publish --> Versio 1.0.0.10, myös github
//
// 7.1.2018   -Käytä C# properties: selo, pelimaara, miettimisaika ja selo_alkuperainen.
//            -Turhat apumuuttujat pois, kun tulos voidaan sijoittaa suoraankin
//                Esim. nykyinenSelo_in.Text = nykyinenSelo_in.Text.Trim();
//            -Math-rutiineja käyttöön (Min, Max ja Round)
//            -Luokat omiin tiedostoihinsa: -> seloPelaaja.cs ja vakiot.cs
//            -Lisätty valikot: Menu-> Ohjeita/Tietoja ohjelmasta/Laskentakaavat/Sulje ohjelma
//            -Varmistetaan ohjelmasta poistuminen, kun valitaan Menu->Sulje ohjelma tai suljetaan ikkuna.
//  Publish --> Versio 1.0.0.12, myös github
//
// 1.4.-6.4.2018 -Koodin järjestämistä ja siistimistä, mm. moduulijakoa uusittu
//              -"out"-parametrien käyttö pois syötetietojen tarkastuksista. Tuloksien
//               välittämisessä käytetään luokkaa, jossa on int-kentät mm. selo, pelimäärä.
//  Publish --> Versio 1.0.0.19, myös github
//
// 7.4.2018 -melkoisesti koodin järjestämistä, uusia moduuleja ja luokkia, muuttujien uudelleen nimeämistä, ...
//          -uuden pelaajan selon laskenta toimii nyt myös formaatista 1.5 1722 1581 1608
//           Samaa laskentakaavaa käytetään koko turnauksesta, jos pelaaja on alussa ns. uusi pelaaja!
//  Publish --> Versio 1.0.1.0, myös github
// 
// 27.5.2018 - Hieman koodin järjestelyä ja dokumentointia, ei näkyviä muutoksia toimintaan
//  Publish --> Versio 1.0.1.5, myös github
//
// TODO:
//          -testaa testaa testaa, tarkista tarkista tarkista
//          -automaattinen testaus (Unit Test Project), joka vaatinee hieman enemmän koodin muokkausta
//          -lisää koodi siistimistä ja refaktorointia, sekä mieti luokkien käyttöä
//

using System;
using System.Collections.Generic; // List<string>
using System.Drawing;        // Color
using System.Windows.Forms;  // MessageBox mm. menuista tulostettavat ikkunat
using System.Text.RegularExpressions; // Regex rx, rx.Replace
using System.Linq;           // tempString.Contains(',')


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


        // tarkistaSyote
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
        private bool TarkistaSyote(Syotetiedot syotteet)
        {
            bool status = false;

            // tyhjennä ottelulista, johon tallennetaan vastustajat tuloksineen
            ottelulista.Tyhjenna();

            // ************ TARKISTA SYÖTE ************

            // ENSIN TARKISTA MIETTIMISAIKA. Tässä ei voi olla virheellista tietoa.
            syotteet.miettimisaika = TarkistaMiettimisaika();

            do {
                // Hae ensin oma nykyinen vahvuusluku ja pelimäärä
                if ((syotteet.nykyinenSelo = TarkistaNykyinenSelo()) == Vakiot.VIRHE_SELO)
                    break;
                if ((syotteet.nykyinenPelimaara = TarkistaPelimaara()) == Vakiot.VIRHE_PELIMAARA)
                    break;

                // TODO: Tässä mietittävää, kun on kaksi eri tallennustapaa
                //    JOS YKSI OTTELU,   saadaan muuttujassa vastustajanSelo vastustajan vahvuusluku,
                //                       ottelun tulosta ei voida tietää vielä
                //    JOS MONTA OTTELUA, ottelut tallennetaan tuloksineen listaan, jossa tuloksetkin ovat mukana
                //
                if ((syotteet.vastustajanSelo = TarkistaVastustajanSelo()) == Vakiot.VIRHE_SELO)
                    break;

                // merkkijonokin talteen sitten kun siitä on poistettu ylimääräiset välilyönnit
                syotteet.vastustajienSelot_str = vastustajanSelo_comboBox.Text;

                // vain jos otteluita ei ole listalla, niin tarkista ottelutuloksen valintanapit
                if (ottelulista.vastustajienLukumaara == 0) {
                    //
                    // Jos oli vain yksi ottelu, niin vastustajan vahvuusluku on vastustajanSelo-kentässä
                    // Haetaan vielä ottelunTulos -kenttään tulospisteet tuplana (0=tappio,1=tasapeli,2=voitto)
                    if ((syotteet.ottelunTulos = TarkistaOttelunTulos()) == Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON)
                        break;

                    ///* DEBUG */MessageBox.Show("Syotteet: " + syotteet.nykyinenSelo +
                    //                ", " + syotteet.nykyinenPelimaara +
                    //                ", " + syotteet.vastustajanSelo +
                    //                ", " + syotteet.ottelunTulos);

                    // Nyt voidaan tallentaa ottelun tiedot (vastustajanSelo, ottelunTulos)
                    ottelulista.LisaaOttelunTulos(syotteet.vastustajanSelo, syotteet.ottelunTulos);
                }

                status = true; // syötekentät OK, jos päästy tänne asti

            } while (false);

            return status;
        }


        // Laske tulokset, yhden vastustajan ja useamman vastustajan tapaukset erillisiä
        // Ensin tarkistetaan syöte ja jos kaikki OK, niin lasketaan.
        private bool SuoritaLaskenta()
        {
            bool status;

            Syotetiedot syotteet = new Syotetiedot(); // tiedot nollataan

            // jos annettu yksi ottelu, niin vastustajan tiedot:
            //     vastustajanSelo ja pisteet
            // jos annettu useampi ottelu, niin ottelut ovat listassa ja vastustajien_lukumaara_listalla > 0
            // Lista on luokassa seloPelaaja (private ottelut_list).
            status = TarkistaSyote(syotteet);

            if (status) {
                // asettaa omat tiedot (selo ja pelimäärä) seloPelaaja-luokkaan, nollaa tilastotiedot ym.
                shakinpelaaja.AloitaLaskenta(syotteet);

                //  *** NYT LASKETAAN ***

                shakinpelaaja.PelaaKaikkiOttelut(ottelulista);       // pelaa kaikki ottelut listalta
            }

            return status; 
        }


        void NaytaTulokset()
        {

            // vastustajien vahvuuslukujen keskiarvo
            int turnauksenKeskivahvuus = (int)Math.Round(ottelulista.tallennetutOttelut.Average(x => x.vastustajanSelo));


            // Laskettiinko yhtä ottelua vai turnausta?
            if (ottelulista.vastustajienLukumaara == 1) {
                // yksi ottelu -> näytä tässä piste-ero vastustajaan, odotustulos ja kerroin
                ///*DEBUG*/MessageBox.Show("Näytä: " +
                //    shakinpelaaja.alkuperainenSelo + ", " +
                //    shakinpelaaja.turnauksenKeskivahvuus + ", " +
                //    shakinpelaaja.laskettuSelo);

                pisteEro_out.Text =
                    Math.Abs(shakinpelaaja.alkuperainenSelo - turnauksenKeskivahvuus).ToString();
                odotustulos_out.Text = (shakinpelaaja.odotustulos / 100F).ToString("0.00");
                kerroin_out.Text = shakinpelaaja.kerroin.ToString();
                vaihteluvali_out.Text = "";  // ei vaihteluväliä, koska vain yksi luku laskettu

            } else {

                // Laskettiin turnauksena syötettyä vastustajien joukkoa (jossa tosin voi olla myös vain yksi ottelu)
                // tyhjennä yksittäisen ottelun tuloskentät
                pisteEro_out.Text = "";
                // laskettu odotustulos näytetään, jos ei ollut uuden pelaajan laskenta
                if (shakinpelaaja.pelimaara < 0 || shakinpelaaja.pelimaara > 10)
                    odotustulos_out.Text = (shakinpelaaja.odotustuloksienSumma / 100F).ToString("0.00");
                else
                    odotustulos_out.Text = "";

                // kerroin on laskettu alkuperäisestä omasta selosta
                kerroin_out.Text = shakinpelaaja.kerroin.ToString();

                // Valintanapeilla ei merkitystä, kun käsitellään turnausta eli valinnat pois
                tulosTappio_btn.Checked = false;
                tulosTasapeli_btn.Checked = false;
                tulosVoitto_btn.Checked = false;

                // Näytä laskennan aikainen vahvuusluvun vaihteluväli
                // Jos oli annettu turnauksen tulos, niin laskenta tehdään yhdellä lauseella eikä vaihteluväliä ole
                // Vaihteluväliä ei ole myöskään, jos oli laskettu yhden ottelun tulosta
                // Vaihteluväli on vain, jos tulokset formaatissa "+1622 -1880 =1633"
                if (shakinpelaaja.annettuTurnauksenTulos < 0 && shakinpelaaja.kasitellytOttelut > 1) {
                    vaihteluvali_out.Text =
                        shakinpelaaja.minSelo.ToString() + " - " + shakinpelaaja.maxSelo.ToString();
                } else
                    vaihteluvali_out.Text = "";  // muutoin siis tyhjä
            }

            // Näytä uusi vahvuusluku ja pelimäärä. Näytä myös vahvuusluvun muutos +/-NN pistettä,
            // sekä vastustajien keskivahvuus ja omat pisteet.
            uusiSelo_out.Text = shakinpelaaja.laskettuSelo.ToString();
            selomuutos_out.Text =
                (shakinpelaaja.laskettuSelo - shakinpelaaja.alkuperainenSelo).ToString("+#;-#;0");
            if (shakinpelaaja.laskettuPelimaara >= 0)
                uusiPelimaara_out.Text = shakinpelaaja.laskettuPelimaara.ToString();
            else
                uusiPelimaara_out.Text = "";

            keskivahvuus_out.Text = turnauksenKeskivahvuus.ToString();

            // Turnauksen loppupisteet / ottelujen lkm, esim.  2.5 / 6
            turnauksenTulos_out.Text =
                (shakinpelaaja.laskettuTurnauksenTulos / 2F).ToString("0.0") + " / " + shakinpelaaja.kasitellytOttelut;
        }


        // Suoritetaan laskenta -button
        private void Laske_button_Click(object sender, EventArgs e)
        {
            if (SuoritaLaskenta()) {
                NaytaTulokset();

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
            int selo = shakinpelaaja.laskettuSelo;
            int pelimaara = shakinpelaaja.laskettuPelimaara;

            if (selo == 0) { // ei ollut vielä laskentaa (1525, 0)
                selo = shakinpelaaja.selo;
                pelimaara = shakinpelaaja.pelimaara;
            }

            nykyinenSelo_in.Text = selo.ToString();
            if (pelimaara != Vakiot.PELIMAARA_TYHJA) {
                // vain, jos pelimaara oli annettu (muutoin on jo valmiiksi tyhjä)
                pelimaara_in.Text = pelimaara.ToString();
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
            tulosTappio_btn.Checked = true;
            if (SuoritaLaskenta())
                NaytaTulokset();
        }

        private void tulosTasapeli_Button_Enter(object sender, EventArgs e)
        {
            tulosTasapeli_btn.Checked = true;
            if (SuoritaLaskenta())
                NaytaTulokset();
        }

        private void tulosVoitto_Button_Enter(object sender, EventArgs e)
        {
            tulosVoitto_btn.Checked = true;
            if (SuoritaLaskenta())
                NaytaTulokset();
        }

        // Kun painettu Enter vastustajan SELO-kentässä, suoritetaan laskenta
        //
        private void vastustajanSelo_combobox_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter painettu vastustajan selojen tai useammankin syöttämisen jälkeen?
            if (e.KeyCode == Keys.Enter) {
                // Kun painettu Enter, niin lasketaan
                // siirrytäänkö myös kentän loppuun? Nyt jäädään samaan paikkaan, mikä myös OK.
                if (SuoritaLaskenta())
                    NaytaTulokset();

                // 12.12.1027: Annettu teksti talteen -> Drop-down combobox
                if (!vastustajanSelo_comboBox.Items.Contains(vastustajanSelo_comboBox.Text))
                        vastustajanSelo_comboBox.Items.Add(vastustajanSelo_comboBox.Text);
            }
        }


        // Normaalissa laskennassa käytetään tekstiä SELO, pikashakissa PELO
        private void vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum suunta)
        {
            string alkup, uusi;

            if (suunta == Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI) {
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
            vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_60_89_Button_CheckedChanged(object sender, EventArgs e)
        {
            vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_11_59_Button_CheckedChanged(object sender, EventArgs e)
        {
            vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_enint10_Button_CheckedChanged(object sender, EventArgs e)
        {
            vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_PELOKSI);
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
        // Aina on joku valittuna, joten ei voi olla virhetilannetta.
        private Vakiot.Miettimisaika_enum TarkistaMiettimisaika()
        {
            Vakiot.Miettimisaika_enum aika;

            if (miettimisaika_vah90_btn.Checked)
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;
            else if (miettimisaika_60_89_btn.Checked)
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN;
            else if (miettimisaika_11_59_btn.Checked)
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN;
            else
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN;

            return aika;
        }

        // Tarkista Oma SELO -kenttä, oltava numero ja rajojen sisällä
        // Paluuarvo joko kelvollinen SELO (MIN_SELO .. MAX_SELO) tai negatiivinen virhestatus
        private int TarkistaNykyinenSelo()
        {
            bool status = true;
            int tulos;

            nykyinenSelo_in.Text = nykyinenSelo_in.Text.Trim();  // remove leading and trailing white spaces

            // onko numero ja jos on, niin onko sallittu numero
            if (int.TryParse(nykyinenSelo_in.Text, out tulos) == false)
                status = false;
            else if (tulos < Vakiot.MIN_SELO || tulos > Vakiot.MAX_SELO)
                status = false;

            if (!status) {
                string message = String.Format("VIRHE: Nykyisen SELOn oltava numero {0}-{1}.",
                    Vakiot.MIN_SELO, Vakiot.MAX_SELO);
                nykyinenSelo_in.ForeColor = Color.Red;
                MessageBox.Show(message);
                nykyinenSelo_in.ForeColor = Color.Black;

                // Tyhjennä liian täysi kenttä? Takaisin kenttään ja virhestatus
                if (nykyinenSelo_in.Text.Length > Vakiot.MAX_PITUUS)
                    nykyinenSelo_in.Text = "";
                nykyinenSelo_in.Select();
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
        private int TarkistaPelimaara()
        {
            bool status = true;
            int tulos;

            pelimaara_in.Text = pelimaara_in.Text.Trim();  // remove leading and trailing white spaces

            if (string.IsNullOrWhiteSpace(pelimaara_in.Text)) {
                tulos = Vakiot.PELIMAARA_TYHJA; // Tyhjä kenttä on OK
            } else {
                // onko numero ja jos on, niin onko sallittu numero
                if (int.TryParse(pelimaara_in.Text, out tulos) == false)
                    status = false;
                else if (tulos < Vakiot.MIN_PELIMAARA || tulos > Vakiot.MAX_PELIMAARA)
                    status = false;

                if (!status) {
                    string message = String.Format("VIRHE: pelimäärän voi olla numero väliltä {0}-{1} tai tyhjä.",
                        Vakiot.MIN_PELIMAARA, Vakiot.MAX_PELIMAARA);
                    pelimaara_in.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    pelimaara_in.ForeColor = Color.Black;

                    // Tyhjennä liian täysi kenttä? Takaisin kenttään ja virhestatus
                    if (pelimaara_in.Text.Length > Vakiot.MAX_PITUUS)
                        pelimaara_in.Text = "";
                    pelimaara_in.Select();
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
        private int TarkistaVastustajanSelo()
        {
            bool status = true;
            int tulos = 0;        // palautettava vastustajan selo
            int virhekoodi = 0;   // tai tästä saatu virhestatus

            bool onko_turnauksen_tulos = false;  // oliko tulos ensimmäisenä?
            float syotetty_tulos = 0F;           // tähän sitten sen tulos desimaalilukuna (esim. 2,5)

            vastustajanSelo_comboBox.Text = vastustajanSelo_comboBox.Text.Trim();  // remove leading and trailing white spaces

            // kentässä voidaan antaa alussa turnauksen tulos, esim. 0.5, 2.0, 2.5, 7.5 eli saadut pisteet
            shakinpelaaja.TallennaTurnauksenTulos(-1.0F);  // oletus: ei annettu turnauksen tulosta

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

                // poista sanojen väleistä ylimääräiset välilyönnit
                string pattern = "\\s+";    // \s = any whitespace, + one or more repetitions
                string replacement = " ";   // tilalle vain yksi välilyönti
                Regex rx = new Regex(pattern);
                vastustajanSelo_comboBox.Text = rx.Replace(vastustajanSelo_comboBox.Text, replacement);

                // Nyt voidaan jakaa syöte välilyönneillä erotettuihin merkkijonoihin
                List<string> selo_lista = vastustajanSelo_comboBox.Text.Split(' ').ToList();

                // Apumuuttujat
                int selo1 = Vakiot.MIN_SELO;
                Vakiot.OttelunTulos_enum tulos1 = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
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
                                shakinpelaaja.TallennaTurnauksenTulos(syotetty_tulos);

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
                        ottelulista.LisaaOttelunTulos(selo1, Vakiot.OttelunTulos_enum.TULOS_TASAPELIx2);

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
                                    tulos1 = Vakiot.OttelunTulos_enum.TULOS_VOITTOx2;
                                    break;
                                case '=':
                                    tulos1 = Vakiot.OttelunTulos_enum.TULOS_TASAPELIx2;
                                    break;
                                case '-':
                                    tulos1 = Vakiot.OttelunTulos_enum.TULOS_TAPPIOx2;
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
                //    Jos tulos on sama kuin pelaajien lkm, on voitettu kaikki ottelut.
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
                // Kentästä on jo poistettu ylimääräiset välilyönnit
                vastustajanSelo_comboBox.Select();

                tulos = Vakiot.VIRHE_SELO;  // jos epäonnistui, palautetaan virhestatus
            }

            return tulos;
        }

        // Tarkista ottelun tulos -painikkeet ja tallenna niiden vaikutus
        // pisteet: tappiosta 0, tasapelistä puoli ja voitosta yksi
        //          palautetaan kokonaislukuna 0, 1 ja 2
        public Vakiot.OttelunTulos_enum TarkistaOttelunTulos()
        {
            Vakiot.OttelunTulos_enum pisteet = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;

            if (tulosTappio_btn.Checked)
                pisteet = Vakiot.OttelunTulos_enum.TULOS_TAPPIOx2;
            else if (tulosTasapeli_btn.Checked)
                pisteet = Vakiot.OttelunTulos_enum.TULOS_TASAPELIx2;
            else if (tulosVoitto_btn.Checked)
                pisteet = Vakiot.OttelunTulos_enum.TULOS_VOITTOx2;
            else {
                MessageBox.Show("Ottelun tulosta ei annettu!");
                tulosTappio_btn.Select();   // siirry ensimmäiseen valintanapeista
            }
            return pisteet;
        }
    }

} // END Selolaskuri.cs
