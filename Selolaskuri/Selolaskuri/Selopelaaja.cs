//
// Luokka shakinpelaajaan laskentaa varten
//
// Luotu 7.1.2018 Ismo Suihko
//
// Muokattu
//   1.-7.4.2018   Laskennan korjauksia, uuden pelaajan laskenta turnauksen tuloksesta
//   15.6.2018     Laskennan korjaukset (alkuperäinen pelimäärä vaikutti tulokseen)
//   15.-16.6.2018 Laskennassa käytettyjen apumuuttujien käytön tarkistusta ja turhiksi jääneiden poistoa (jatkuu)
//
// XXX: Laskentaa on muutettu ja osa apumuuttujista voi olla turhaan käytössä, tarkista!
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
        Syotetiedot alkuperaisetSyotteet; // mm. alkuperäinen vahvuusluku ja pelimäärä

        // laskennan edetessä päivitetään:
        private int lasketaanOttelujenLkm;
        public int  lasketaanSelo { get; set; }
        public int  lasketaanPelimaara { get; private set; }

        public SeloPelaaja(int selo, int pelimaara)
        {
            this.lasketaanSelo      = selo;
            this.lasketaanPelimaara = pelimaara;
        }

        // AloitaLaskenta
        //
        // Ennen laskentaa alustetaan muuttujat saatujen syötteiden mukaan
        public void AloitaLaskenta(Syotetiedot syotteet)
        {
            this.alkuperaisetSyotteet = syotteet;  // selo, pelimaara, miettimisaika

            // laskettavat tiedot
            this.lasketaanOttelujenLkm = 0;
            this.lasketaanSelo         = syotteet.alkuperainenSelo;
            this.lasketaanPelimaara    = syotteet.alkuperainenPelimaara;

            // alusta apumuuttujat
            // XXX: Tarkista näiden käyttö
            //this.laskettuPelimaara       = -1;     // lasketaan vain jos on syötetty pelimäärä
            this.laskettuTurnauksenTulos = 0;  // lasketaan otteluista kokonaislukuna
            this.laskettuOdotustulos     = 0;      // tai summa odotustuloksista

            // vaihteluvälin alustus näin, jotta min ja max toimisivat
            this.minSelo = Vakiot.MAX_SELO;
            this.maxSelo = Vakiot.MIN_SELO;
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
                return this.alkuperaisetSyotteet.alkuperainenSelo;
            }
        }

        // tarkistetaan alkuperäisestä pelimäärästä vaikka pelimäärä laskennan edetessä menisikin rajan yli
        public bool OnkoUudenPelaajanLaskenta()
        {
            return (alkuperaisetSyotteet.alkuperainenPelimaara >= Vakiot.MIN_PELIMAARA &&
                    alkuperaisetSyotteet.alkuperainenPelimaara <= Vakiot.MAX_UUSI_PELAAJA);
        }

        // laskennan aputiedot
        public int laskettuOdotustulos { get; private set; }  // jos usea vastustaja, niin summa
        public int laskettuKerroin { get; private set; }

        // XXX: Ovat turhia muuttujia, kun on myös lasketaanSelo ja lasketaanPelimaara
        //public int laskettuSelo { get; private set; }
        //public int laskettuPelimaara { get; private set; }

        // Lasketun selon vaihteluväli, jos vastustajien selot ja tulokset formaatissa: +1622 -1880 =1633
        // Nämä saanee myös hakea koko joukosta hakemalla minimit ja maksimit.
        public int minSelo { get; private set; }
        public int maxSelo { get; private set; }

        public int kasitellytOttelut {
            get {
                return lasketaanOttelujenLkm;  // päivitetty laskennan edetessä
            }
            set {
                lasketaanOttelujenLkm = value;
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
            int odotustulos;
            int selo;

            // Vanhan pelaajan SELOn laskennassa käytetään odotustulosta ja kerrointa
            // Mutta nämä lasketaan täällä, koska ne näytetään myös uuden pelaajan tuloksissa
            odotustulos = MaaritaOdotustulos(alkuperainenSelo, vastustajanSelo);
            laskettuKerroin = MaaritaKerroin(lasketaanSelo);

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
                selo = (int)Math.Round((lasketaanSelo * lasketaanPelimaara + (vastustajanSelo + selomuutos[(int)tulos])) / (lasketaanPelimaara + 1F));
            } else {
                float lisakerroin = MaaritaLisakerroin(miettimisaika);
                // vanhan pelaajan SELO, kun pelimäärä jätetty tyhjäksi tai on yli 10.
                // XXX: SELOn pyöristys? lisätään 0.5F, kaavassa lisäksi +0.1F
                selo = (int)Math.Round((lasketaanSelo + laskettuKerroin * lisakerroin * (((int)tulos / 2F) - (odotustulos / 100F)) + 0.1F));
            }

            // päivitä pelimäärää, jos oli annettu
            // XXX: Tarkista, käytetäänkö turhaan eri muuttujia
            //if (lasketaanPelimaara >= Vakiot.MIN_PELIMAARA)
            //    laskettuPelimaara = lasketaanPelimaara + 1;

            // laskenta etenee!
            this.lasketaanOttelujenLkm++;
            this.laskettuOdotustulos += odotustulos;
            laskettuTurnauksenTulos += (int)tulos;   // OttelunTulos_enum tulos

            // tallenna vaihteluväli (jos yksi ottelu, niin jäävät samoiksi)
            this.minSelo = Math.Min(minSelo, selo);
            this.maxSelo = Math.Max(maxSelo, selo);

            return selo;
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
                lasketaanSelo = ((lasketaanSelo * lasketaanPelimaara) + (int)(keskimSelo + muutos) * pelaajia) / (lasketaanPelimaara + pelaajia);
                lasketaanPelimaara += pelaajia;

                //laskettuSelo = lasketaanSelo;
                //laskettuPelimaara = lasketaanPelimaara;

                laskettuTurnauksenTulos = annettuTurnauksenTulos;
                kasitellytOttelut = pelaajia;

                // /*DEBUG*/MessageBox.Show(" Selo " + selo + " pelimaara " + pelimaara);

                return;
            }

            foreach (Ottelulista.Ottelu ottelu in ottelulista) {

                // päivitä seloa ja pelimäärää jokaisen ottelun jälkeen
                // laskennassa mukana myös aiemmin tallennetut miettimisaika ja pelimäärä
                lasketaanSelo = PelaaOttelu(ottelu.vastustajanSelo, ottelu.ottelunTulos);

                // päivitä pelimäärää vain jos oli annettu
                if (lasketaanPelimaara != Vakiot.PELIMAARA_TYHJA)
                    lasketaanPelimaara++;
            }

            // Laskenta, jos on annettu ensin turnauksen pistemäärä ja sen perään selot ilman erillistä tulostietoa
            // Esim. 1.5 1622 1880 1683
            //
            // HUOM! Seuraava laskenta ei toimi uudella pelaajalla, joka onkin käsitelty aiemmin
            //
            if (annettuTurnauksenTulos >= 0 &&
                (alkuperaisetSyotteet.alkuperainenPelimaara > Vakiot.MAX_UUSI_PELAAJA ||
                 alkuperaisetSyotteet.alkuperainenPelimaara == Vakiot.PELIMAARA_TYHJA))
            {
                ///*DEBUG*/
                //System.Windows.Forms.MessageBox.Show("annettu turnauksen tulos " + annettuTurnauksenTulos +
                //    " laskettu turnauksen tulos " + laskettuTurnauksenTulos +
                //    " uusi selo " + selo + " alkup selo " + alkuperainenSelo +
                //    "uusi pelimäärä " + pelimaara);

                // unohdetaan aiempi selolaskenta!
                // Mutta sieltä saadaan odotustuloksien summa (laskettuOdotustulos) ja pelimaara valmiiksi!
                lasketaanSelo = alkuperainenSelo;
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
                    // odotustuloksien_summa on kokonaisluku ja pitää jakaa 100:lla
                    if ((annettuTurnauksenTulos / 2F) > (laskettuOdotustulos / 100F)) {
                        lasketaanSelo =
                            (int)(lasketaanSelo + 200 - 200 * Math.Pow(Math.E, (laskettuOdotustulos / 100F - annettuTurnauksenTulos / 2F) / 10F));
                    } else {
                        lasketaanSelo =
                            (int)(lasketaanSelo - 200 + 200 * Math.Pow(Math.E, (annettuTurnauksenTulos / 2F - laskettuOdotustulos / 100F) / 10F));
                    }
                } else {
                    //
                    // pidemmän miettimisajan pelit eli > 10 min
                    //
                    // Laskentakaava ei toimi uudella pelaajalla, joka onkin käsitelty aiemmin
                    //
                    float lisakerroin = MaaritaLisakerroin(miettimisaika);
                    // Lisätään vielä pelattujen pelien lkm * 0.1
                    // 0.5F pyöristystä varten
                    lasketaanSelo =
                        (int)((lasketaanSelo + MaaritaKerroin(lasketaanSelo) * lisakerroin * (annettuTurnauksenTulos / 2F - laskettuOdotustulos / 100F)) + (lasketaanOttelujenLkm * 0.1F) + 0.5F);
                    minSelo = maxSelo = lasketaanSelo;  // tässä ei voida laskea minimi- eikä maksimiseloa
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
        // odotustulokset lasketaan aina alkuperäisellä selolla
        private int MaaritaOdotustulos(int alkuperainenSelo, int vastustajanSelo)
        {
            int SELO_diff = alkuperainenSelo - vastustajanSelo;
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
            int odotustulos = 50 + sign * index;

            // Pikashakissa ei odotustulosta rajoiteta 92:een (kun miettimisaika <= 10 min)
            return (odotustulos > 92 && miettimisaika >= Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN) ? 92 : odotustulos;
        }

        // Kerroin määritetään alkuperäisen selon mukaan.
        // ks. kerrointaulukko http://skore.users.paivola.fi/selo.html
        private int MaaritaKerroin(int selo)
        {
            if (selo >= 2050)
                return 20;
            if (selo < 1650)
                return 45;
            return 40 - 5 * ((selo - 1650) / 100);
        }

        // Eri miettimisajoilla voi olla omia kertoimia
        private float MaaritaLisakerroin(Vakiot.Miettimisaika_enum aika)
        {
            float f = 1.0F;

            // Tämä ei vaikuta uuden pelaajan SELOn laskentaan
            if (aika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN)
                f = 0.5F;
            else if (aika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN)
                f = (lasketaanSelo < 2300) ? 0.3F : 0.15F;
            return f;
        }
    }
}
