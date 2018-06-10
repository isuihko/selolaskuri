//
// Luokka shakinpelaajaan laskentaa varten
//
// Luotu 7.1.2018 Ismo Suihko
//
// Muokattu
//   1.-7.4.2018 Laskennan korjauksia, uuden pelaajan laskenta turnauksen tuloksesta
//   10.6.2018   Laskennan korjaukset (pelimäärä vaikutti tulokseen)
//
// TODO: Tarkista apumuuttujien tarpeellisuus
//

using System;
using System.Linq;
//using System.Windows.Forms;  // MessageBox.Show()

namespace Selolaskuri
{
    // class seloPelaaja 
    //   - selo
    //   - pelimäärä (merkitystä, jos 0-10)
    //     ym.
    //
    // pelaa shakkiotteluita, joissa on vastustaja (selo) ja tulos (tappio, tasapeli tai voitto)
    //
    // vastustajien_lukumaara_listalla = listan alkioiden lukumaara
    //
    // turnauksen_ottelumaara = turnauksen_vastustajien_lkm
    //     = ottelujen lukumäärä, joka päivitetään laskennan edetessä
    //
    public class SeloPelaaja
    {
        Syotetiedot alkuperaisetSyotteet;

        // kun lasketaan kerralla useamman ottelun (turnauksen matsit) vaikutus
        private int kasiteltyjenOttelujenLkm;    // päivitetään laskennan edetessä
        public int selo { get; set; }
        public int pelimaara { get; private set; }

        public SeloPelaaja(int selo, int pelimaara)
        {
            this.selo = selo;
            this.pelimaara = pelimaara;
        }

        // AloitaLaskenta
        //
        // Ennen laskentaa alustetaan muuttujat saatujen syötteiden mukaan
        public void AloitaLaskenta(Syotetiedot syotteet)
        {
            this.alkuperaisetSyotteet = syotteet;  // selo, pelimaara, miettimisaika

            // laskettavat tiedot
            this.selo = syotteet.nykyinenSelo;
            this.pelimaara = syotteet.nykyinenPelimaara;

            // vaihteluvälin alustus näin, jotta min ja max toimisivat
            minSelo = Vakiot.MAX_SELO;
            maxSelo = Vakiot.MIN_SELO;

            // alusta apumuuttujat
            kasiteltyjenOttelujenLkm = 0;

            laskettuPelimaara = -1;          // lasketaan vain jos on syötetty pelimäärä
            laskettuTurnauksenTulos = 0;     // lasketaan otteluista kokonaislukuna
            odotustuloksienSumma = 0;
        }


        public void TallennaTurnauksenTulos(float f)
        {
            // tallennetaan kokonaislukuna
            //  0 = 0, tasapeli on 0.5:n sijaan 1, voitto on 1:n sijaan 2
            // Huom! Jos tulos on annettu irheellisesti esim. 0,9 tai 2,4, 
            //       niin ne muuttuvat tällä kaavalla 0,5:ksi ja 2,0:ksi.
            annettuTurnauksenTulos = (int)(2 * f + 0.01F); // pyöristys kuntoon
        }

        public int annettuTurnauksenTulos { get; private set; }

        private Vakiot.Miettimisaika_enum miettimisaika {
            get {
                return alkuperaisetSyotteet.miettimisaika;
            }
        }

        public int alkuperainenSelo {
            get {
                return this.alkuperaisetSyotteet.nykyinenSelo;
            }
        }

        public bool OnkoUudenPelaajanLaskenta()
        {
            return (pelimaara >= Vakiot.MIN_PELIMAARA && pelimaara <= Vakiot.MAX_UUSI_PELAAJA);
        }

        // laskennan aputiedot
        public int odotustulos { get; private set; }
        public int odotustuloksienSumma { get; private set; }
        public int kerroin { get; private set; }

        // XXX: Hm... onko turhia muuttujia, kun on myös selo ja pelimaara
        public int laskettuSelo { get; private set; }
        public int laskettuPelimaara { get; private set; }

        // Lasketun selon vaihteluväli, jos vastustajien selot ja tulokset formaatissa: +1622 -1880 =1633
        // Nämä saanee myös hakea koko joukosta hakemalla minimit ja maksimit.
        public int minSelo { get; private set; }
        public int maxSelo { get; private set; }

        public int kasitellytOttelut {
            get {
                return kasiteltyjenOttelujenLkm;  // päivitetty laskennan edetessä
            }
            set {
                kasiteltyjenOttelujenLkm = value;
            }
        }

        public int laskettuTurnauksenTulos { get; private set; }




        // pelaaja pelaa shakkiotteluita
        //
        // IN: vastustajan_selo 1000-2999
        // IN: ottelun tulos: 0 = tappio, 1 = tasapeli, 2 = voitto (oikeasti 0, 1/2 ja 1)
        //
        // -> selo muuttuu
        // -> pelimaara kasvaa yhdellä (jos ei ollut -1)
        //
        // XXX: TARKISTA TÄMÄ LASKENTA
        public int PelaaOttelu(int vastustajanSelo, Vakiot.OttelunTulos_enum tulos)
        {
            // Vanhan pelaajan SELOn laskennassa käytetään odotustulosta ja kerrointa
            // Mutta nämä lasketaan täällä, koska ne näytetään myös uuden pelaajan tuloksissa
            odotustulos = MaaritaOdotustulos(vastustajanSelo);
            kerroin = MaaritaKerroin();

            ///*DEBUG*/System.Windows.Forms.MessageBox.Show("Odotus " + odotustulos +
            //    " kerroin " + kerroin + " selo " + selo +
            //    " pelimäärä " + pelimaara + " vastus " + vastustajanSelo + " tulos " + tulos);

            //if (pelimaara >= Vakiot.MIN_PELIMAARA && pelimaara <= Vakiot.MAX_UUSI_PELAAJA) 
            if (OnkoUudenPelaajanLaskenta()) {
                // Uuden pelaajan laskennassa käytetään vastustajan seloa tuloksen mukaan -200 / +0 / +200
                int[] selomuutos = { -200, 0, 200 };  // indeksinä tulos 2*(0, 1/2, 1) -> indeksi (0, 1, 2)

                // Uuden pelaajan SELO, kun pelimäärä 0-10
                // Jos pelimäärä on 0, niin nykyinenSelo-kentän arvolla ei ole merkitystä
                // XXX: pyöristykset jakolaskun jälkeen?
                laskettuSelo = (int)Math.Round((selo * pelimaara + (vastustajanSelo + selomuutos[(int)tulos])) / (pelimaara + 1F));
            } else {
                float lisakerroin = MaaritaLisakerroin();
                // vanhan pelaajan SELO, kun pelimäärä jätetty tyhjäksi tai on yli 10.
                // XXX: SELOn pyöristys? lisätään 0.5F, kaavassa lisäksi +0.1F
                laskettuSelo = (int)Math.Round((selo + kerroin * lisakerroin * (((int)tulos / 2F) - (odotustulos / 100F)) + 0.1F));
            }

            // päivitä pelimäärää, jos oli annettu
            // XXX: Tarkista, käytetäänkö turhaan eri muuttujia
            if (pelimaara >= Vakiot.MIN_PELIMAARA)
                laskettuPelimaara = pelimaara + 1;

            // laskenta etenee!
            kasiteltyjenOttelujenLkm++;
            odotustuloksienSumma += odotustulos;  // = vastustajien_lkm * odotustulos
            laskettuTurnauksenTulos += (int)tulos;   // enum tulos 

            // tallenna vaihteluväli
            minSelo = Math.Min(minSelo, laskettuSelo);
            maxSelo = Math.Max(maxSelo, laskettuSelo);

            return laskettuSelo;
        }


        // pelaa kaikki listalta löytyvät ottelut!
        public void PelaaKaikkiOttelut(Ottelulista ottelulista)
        {

            if (annettuTurnauksenTulos >= 0 && OnkoUudenPelaajanLaskenta()) {
                // ERIKOISTAPAUS, joka käsitellään omana tapauksenaan ja poistutaan
                //
                //  selo += pistemäärä - ottelut/2 * 200
                // 1 ottelu:
                //    1525 + 0.5 1525 -> tulos 1525    
                // 2 ottelua:
                //  2    1525 1441   summa: 2966  keskim. 1483   tulos on keskim+200
                // keskitulos/matsi = 1
                //
                int keskimSelo = (int)Math.Round(ottelulista.tallennetutOttelut.Average(x => x.vastustajanSelo));
                int pelaajia = ottelulista.tallennetutOttelut.Count;
                float keskimTulos = (annettuTurnauksenTulos / 2F) / pelaajia;   // 0-1
                float muutos = 400 * (keskimTulos - 0.5F) + 0.5F;   // tuloksella tasapeli pysytään samassa kuin keskiSelo

                ///*DEBUG*/
                //MessageBox.Show(" Selo " + selo + " pelimaara " + pelimaara + " Keskiselo " + keskiselo + " pelaajia " + pelaajia + " annettu tulos " + annettuTurnauksenTulos +
                //    " keskimtulos " + keskimtulos + " muutos " + muutos);

                // vanhan selon painoarvo ja uuden lasketun selon painoarvo riippuvat pelimääristä
                selo = ((selo * pelimaara) + (int)(keskimSelo + muutos) * pelaajia) / (pelimaara + pelaajia);
                pelimaara += pelaajia;

                laskettuSelo = selo;
                laskettuPelimaara = pelimaara;

                laskettuTurnauksenTulos = annettuTurnauksenTulos;
                kasitellytOttelut = pelaajia;

                // /*DEBUG*/MessageBox.Show(" Selo " + selo + " pelimaara " + pelimaara);

                return;
            }

            foreach (Ottelulista.Ottelu ottelu1 in ottelulista) {

                // päivitä seloa ja pelimäärää jokaisen ottelun jälkeen
                // laskennassa mukana myös aiemmin tallennetut miettimisaika ja pelimäärä
                selo = PelaaOttelu(ottelu1.vastustajanSelo, ottelu1.ottelunTulos);

                // päivitä pelimäärää, jos oli annettu
                // XXX: Tarkista tämä
                if (pelimaara != Vakiot.PELIMAARA_TYHJA)
                    pelimaara++;
            }

            // Laskenta, jos on annettu ensin turnauksen pistemäärä ja sen perään selot ilman erillistä tulostietoa
            // Esim. 1.5 1622 1880 1683
            //
            // HUOM! Tämä kaava ei toimi uudella pelaajalla, joten tarkistetaan alkuperäinen pelimäärä
            //
            if (annettuTurnauksenTulos >= 0 &&
                (alkuperaisetSyotteet.nykyinenPelimaara > Vakiot.MAX_UUSI_PELAAJA ||
                 alkuperaisetSyotteet.nykyinenPelimaara == Vakiot.PELIMAARA_TYHJA))
            {
                ///*DEBUG*/
                //System.Windows.Forms.MessageBox.Show("annettu turnauksen tulos " + annettuTurnauksenTulos +
                //    " laskettu turnauksen tulos " + laskettuTurnauksenTulos +
                //    " uusi selo " + selo + " alkup selo " + alkuperainenSelo +
                //    "uusi pelimäärä " + pelimaara);

                // unohdetaan aiempi selolaskenta!
                // Mutta sieltä saadaan odotustuloksien_summa ja pelimaara valmiiksi!
                selo = alkuperainenSelo;
                laskettuTurnauksenTulos = annettuTurnauksenTulos;

                if (miettimisaika <= Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN) {
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
                    if ((annettuTurnauksenTulos / 2F) > (odotustuloksienSumma / 100F)) {
                        laskettuSelo =
                            (int)(selo + 200 - 200 * Math.Pow(Math.E, (odotustuloksienSumma / 100F - annettuTurnauksenTulos / 2F) / 10F));
                    } else {
                        laskettuSelo =
                            (int)(selo - 200 + 200 * Math.Pow(Math.E, (annettuTurnauksenTulos / 2F - odotustuloksienSumma / 100F) / 10F));
                    }
                } else {
                    //
                    // pidemmän miettimisajan pelit eli > 10 min
                    //
                    // XXX: laskentakaava ei toimi uudella pelaajalla
                    //
                    float lisakerroin = MaaritaLisakerroin();
                    // Lisätään vielä pelattujen pelien lkm * 0.1
                    // 0.5F pyöristystä varten
                    laskettuSelo =
                        (int)((selo + MaaritaKerroin() * lisakerroin * (annettuTurnauksenTulos / 2F - odotustuloksienSumma / 100F)) + (kasiteltyjenOttelujenLkm * 0.1F) + 0.5F);
                    minSelo = maxSelo = laskettuSelo;  // tässä ei voida laskea minimi- eikä maksimiseloa
                }
            }
        }


        // Selvitä ottelun odotustulos vertaamalla SELO-lukuja
        //    50 (eli 0,50), jos samantasoiset eli SELO-ero 0-3 pistettä
        //    > 50, jos voitto odotetumpi, esim. 51 jos 4-10 pistettä parempi
        //    < 50, jos tappio odotetumpi, esim. 49, jos 4-10 pistettä alempi
        //
        // Odotustulos voi olla enintään 92. Paitsi pikashakissa voi olla jopa 100.
        // ks. ohje http://skore.users.paivola.fi/selo.html
        private int MaaritaOdotustulos(int vastustajanSelo)
        {
            int odotustulos;
            // odotustulokset lasketaan aina alkuperäisellä selolla
            int SELO_diff = alkuperainenSelo - vastustajanSelo;    // XXX: selo_alkuperainen
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
            while (index < difftable.Length) {
                if (diff < difftable[index])
                    break;
                index++;
            }

            // laske odotustulos taulukon paikkaa eli indeksiä käyttäen
            // jos ei löytynyt, niin index 50 ja odotustulos 0 (0,00) tai 100 (1,00)
            odotustulos = 50 + sign * index;

            // Pikashakissa ei odotustulosta rajoiteta 92:een (kun miettimisaika <= 10 min)
            return (odotustulos > 92 && miettimisaika >= Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN) ? 92 : odotustulos;
        }

        // Kerroin määritetään alkuperäisen selon mukaan.
        // ks. kerrointaulukko http://skore.users.paivola.fi/selo.html
        private int MaaritaKerroin()
        {
            if (selo >= 2050)
                return 20;
            if (selo < 1650)
                return 45;
            return 40 - 5 * ((selo - 1650) / 100);
        }

        // Eri miettimisajoilla voi olla omia kertoimia
        private float MaaritaLisakerroin()
        {
            float f = 1.0F;

            // Tämä ei vaikuta uuden pelaajan SELOn laskentaan
            if (miettimisaika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN)
                f = 0.5F;
            else if (miettimisaika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN)
                f = (selo < 2300) ? 0.3F : 0.15F;
            return f;
        }
    }
}
