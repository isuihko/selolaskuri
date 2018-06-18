//
// Luokka shakinpelaajan laskentaa varten
//
// Public:
//      PelaaKaikkiOttelut
//
// Luotu 7.1.2018 Ismo Suihko
//
// Muokattu
//   1.-7.4.2018    Laskennan korjauksia, uuden pelaajan laskenta turnauksen tuloksesta
//   15.6.2018      Laskennan korjaukset (alkuperäinen pelimäärä vaikutti tulokseen)
//   15.-18.6.2018  Laskennassa käytettyjen apumuuttujien käytön tarkistusta ja turhiksi jääneiden poistoa,
//                  sekä kommenttien päivitystä (laskentaa muutettu aika paljon).
//                  Tarkistettu näkyvyyttä (public vs. private).
//

using System;
using System.Linq;

namespace Selolaskuri
{
    // class SeloPelaaja 
    //
    // pelaa shakkiotteluita, joissa on vastustaja (vastustajan selo) ja tulos (tappio, tasapeli tai voitto)
    //
    public class SeloPelaaja
    {
        public SeloPelaaja()
        {
        }

        // AlustaLaskenta tallentaa tähän alkuperäiset syötteet, joissa mm.
        // alkuperäinen vahvuusluku ja pelimäärä sekä ottelulista.
        // Käytetään tämän luokan rutiineissa suoraan.
        private Syotetiedot alkuperaisetSyotteet;

        // --------------------------------------------------------------------------------
        // Laskennan aikana päivitettävät tiedot, jotka kopioidaan tuloksiin
        // ks. SeloLaskuriOperations.SuoritaLaskenta sekä struct Tulokset
        // --------------------------------------------------------------------------------

        public int uusiSelo { get; private set; }
        public int uusiPelimaara { get; private set; }

        // Lasketun selon vaihteluväli, jos vastustajien selot ja tulokset formaatissa: +1622 -1880 =1633
        public int minSelo { get; private set; }
        public int maxSelo { get; private set; }
        
        // laskennan aputiedot
        public int laskettuOdotustulos { get; private set; }  // jos usea vastustaja, niin summa
        public int laskettuKerroin { get; private set; }

        // Selvitetään tulos, jos ottelut formaatissa "+1525 =1600 -1611 +1558", josta esim. saadaan
        // tulokseksi 2,5 (2 voittoa ja 1 tasapeli). Tallennetaan kokonaislukuna tuplana 2*2,5 -> (int)5.
        public int laskettuTurnauksenTulos { get; private set; }

        // Turnauksen tulos voidaan antaa vastustajan SELO-kentässä ensimmäisenä
        // Esim. "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684"
        //
        // Tallennetaan kokonaislukuna tuplana: esim. 10,5 -> 21
        // Huom! Jos tulos on annettu virheellisesti esim. 0,9 tai 2,4, niin pyöristys alas
        // (0,5 ja 2,0) ja tallentuvat kokonaislukuina 1 ja 4.
        private int annettuTurnauksenTulos;

        // Tarvitaan oma erillinen setter, koska tehdään muunnos float -> kokonaisluku
        public void SetAnnettuTurnauksenTulos(float f)
        {
            annettuTurnauksenTulos = (int)(2 * f + 0.01F); // pyöristys
        }


        // --------------------------------------------------------------------------------
        // Laskenta
        // --------------------------------------------------------------------------------

        // AlustaLaskenta
        //
        // Ennen laskentaa alustetaan muuttujat saatujen syötteiden mukaan
        //
        private void AlustaLaskenta(Syotetiedot syotteet)
        {
            alkuperaisetSyotteet = syotteet;  // selo, pelimaara, miettimisaika

            // laskettavat tiedot, selon ja pelimaaran laskenta aloitetaan syötetyistä arvoista
            uusiSelo         = syotteet.alkuperainenSelo;
            uusiPelimaara    = syotteet.alkuperainenPelimaara;

            laskettuTurnauksenTulos = 0;  // lasketaan otteluista kokonaislukuna
            laskettuOdotustulos     = 0;  // summa odotustuloksista

            // vaihteluvälin alustus
            minSelo = Vakiot.MAX_SELO;
            maxSelo = Vakiot.MIN_SELO;
        }


        // Pelaa kaikki listalta löytyvät ottelut!
        //
        public void PelaaKaikkiOttelut(Syotetiedot syotteet)
        {
            Ottelulista ottelulista = syotteet.ottelut;

            // asettaa omat tiedot (selo ja pelimäärä) seloPelaaja-luokkaan, nollaa tilastotiedot ym.
            AlustaLaskenta(syotteet);

            // Erikoistapauksena uuden pelaajan tuloksien laskenta turnauksesta,
            // jossa tulokset on ilmoitettu formaatissa "1.5 1622 1880 1683"
            //

            if (annettuTurnauksenTulos >= 0 && alkuperaisetSyotteet.UudenPelaajanLaskenta()) {
                //  selo += pistemäärä - ottelut/2 * 200
                // 1 ottelu:
                //    1525 + 0.5 1525 -> tulos 1525    
                // 2 ottelua:
                //  2    1525 1441   summa: 2966  keskim. 1483   tulos on keskim+200
                // keskitulos/matsi = 1
                
                // apumuuttujia (lausekkeiden selkiyttämiseksi ja lyhentämiseksi)
                int keskimSelo    = (int)Math.Round(ottelulista.tallennetutOttelut.Average(x => x.vastustajanSelo));  // Linq
                int pelaajia      = ottelulista.tallennetutOttelut.Count;
                float keskimTulos = (annettuTurnauksenTulos / 2F) / pelaajia;   // 0-1
                float muutos = 400 * (keskimTulos - 0.5F) + 0.5F;   // tuloksella tasapeli pysytään samassa kuin keskiSelo

                // vanhan selon painoarvo ja uuden lasketun selon painoarvo riippuvat pelimääristä
                uusiSelo       = ((uusiSelo * uusiPelimaara) + (int)(keskimSelo + muutos) * pelaajia) / (uusiPelimaara + pelaajia);
                uusiPelimaara += pelaajia;

                // turnauksen tulos annettu, joten ei laskettavaa
                laskettuTurnauksenTulos = annettuTurnauksenTulos;  

                return;
            }


            // Varsinainen laskenta: Käydään läpi kaikki listan ottelut, jotka olivat formaatissa
            // "+1525 =1600 -1611 +1558". Tällöin myös minSelo ja maxSelo voidaan selvittää.
            //
            foreach (Ottelulista.Ottelu ottelu in ottelulista) {

                // päivitä seloa ja tilastoja jokaisen ottelun laskennassa, myös laske odotustulos
                uusiSelo = PelaaOttelu(ottelu.vastustajanSelo, ottelu.ottelunTulos);

                // päivitä pelimäärää vain jos oli annettu
                if (uusiPelimaara != Vakiot.PELIMAARA_TYHJA)
                    uusiPelimaara++;
            }


            // Entä jos vanhan pelaajan ottelut olivatkin formaatissa "1.5 1622 1880 1683"?
            // Jos näin oli, niin unohdetaan vanha laskenta, josta käytetään vain odotustulos.
            //
            // HUOM! Seuraava ei toimisi uudella pelaajalla, mutta se erikoistapaus onkin käsitelty aiemmin
            //
            if (annettuTurnauksenTulos >= 0 && !alkuperaisetSyotteet.UudenPelaajanLaskenta()) {
                //
                // Aiemmasta laskennasta tarvitaan laskettuOdotustulos.
                //
                uusiSelo = alkuperaisetSyotteet.alkuperainenSelo; // aloitetaan alusta
                laskettuTurnauksenTulos = annettuTurnauksenTulos; // turnauksen tulos annettu, joten ei laskettavaa

                if (alkuperaisetSyotteet.miettimisaika <= Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN) {
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
                        uusiSelo =
                            (int)(uusiSelo + 200 - 200 * Math.Pow(Math.E, (laskettuOdotustulos / 100F - annettuTurnauksenTulos / 2F) / 10F));
                    } else {
                        uusiSelo =
                            (int)(uusiSelo - 200 + 200 * Math.Pow(Math.E, (annettuTurnauksenTulos / 2F - laskettuOdotustulos / 100F) / 10F));
                    }
                } else {
                    //
                    // pidemmän miettimisajan pelit eli > 10 min
                    //
                    float lisakerroin = MaaritaLisakerroin(alkuperaisetSyotteet.miettimisaika);
                    // Lisätään vielä pelattujen pelien lkm * 0.1
                    uusiSelo =
                        (int)((uusiSelo + MaaritaKerroin(uusiSelo) * lisakerroin * (annettuTurnauksenTulos / 2F - laskettuOdotustulos / 100F)) + (ottelulista.tallennetutOttelut.Count * 0.1F) + 0.5F);
                }

                // tällä tavalla saaduista tuloksista ei voida tietää minSeloa ja maxSeloa (koska laskenta tehdään kerralla),
                // joten nyt ne alustetaan samoiksi ettei niitä näytettäisi tuloksissa
                minSelo = uusiSelo;
                maxSelo = uusiSelo;
            }
        }


        // pelaaja pelaa shakkiottelun
        //
        // IN: vastustajan_selo 1000-2999
        // IN: ottelun tulos: 0 = tappio, 1 = tasapeli, 2 = voitto (oikeasti 0, 1/2 ja 1)
        //
        // -> selo muuttuu, myös odotustulos, minSelo ja maxSelo
        //
        private int PelaaOttelu(int vastustajanSelo, Vakiot.OttelunTulos_enum tulos)
        {
            int odotustulos;
            int selo;

            // Vanhan pelaajan SELOn laskennassa käytetään odotustulosta ja kerrointa
            //
            // Mutta nämä lasketaan kaikille, koska myös kerroin näytetään uuden pelaajan tuloksissa
            // Uuden pelaajan odotustuloskin on tarkistettavissa testauksessa vaikka sitä ei näytetä.
            odotustulos     = MaaritaOdotustulos(alkuperaisetSyotteet.alkuperainenSelo, vastustajanSelo);
            laskettuKerroin = MaaritaKerroin(uusiSelo);

            laskettuOdotustulos     += odotustulos;
            laskettuTurnauksenTulos += (int)tulos;

            if (alkuperaisetSyotteet.UudenPelaajanLaskenta()) {
                // Uuden pelaajan laskennassa käytetään vastustajan seloa tuloksen mukaan -200 / +0 / +200
                int[] selomuutos = { -200, 0, 200 };  // indeksinä tulos 2*(0, 1/2, 1) -> indeksi (0, 1, 2)

                // Uuden pelaajan SELO, kun pelimäärä 0-10
                // Jos pelimäärä on 0, niin nykyinenSelo-kentän arvolla ei ole merkitystä
                selo = (int)Math.Round((uusiSelo * uusiPelimaara + (vastustajanSelo + selomuutos[(int)tulos])) / (uusiPelimaara + 1F));
            } else {
                float lisakerroin = MaaritaLisakerroin(alkuperaisetSyotteet.miettimisaika);
                // vanhan pelaajan SELO, kun pelimäärä jätetty tyhjäksi tai on yli 10.
                selo = (int)Math.Round((uusiSelo + laskettuKerroin * lisakerroin * (((int)tulos / 2F) - (odotustulos / 100F)) + 0.1F));
            }

            // tallenna vaihteluväli (jos yksi ottelu, niin jäävät samoiksi)
            minSelo = Math.Min(minSelo, selo);
            maxSelo = Math.Max(maxSelo, selo);

            return selo;
        }


        // --------------------------------------------------------------------------------
        // Laskennan apurutiinit, joilla määritetään odotustulos, kerroin ja lisäkerroin
        // --------------------------------------------------------------------------------

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
            int[] difftable = {
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
            return (odotustulos > 92 && alkuperaisetSyotteet.miettimisaika >= Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN) ? 92 : odotustulos;
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
                f = (uusiSelo < 2300) ? 0.3F : 0.15F;
            return f;
        }
    }
}
