//
// Routines to calculate the results
//
// Public:
//      PelaaKaikkiOttelut  - go through all the matches and calculate
//
// 7.1.2018 Ismo Suihko
//
// Modifications
//   1.-7.4.2018    Laskennan korjauksia, uuden pelaajan laskenta turnauksen tuloksesta
//   15.6.2018      Laskennan korjaukset (alkuperäinen pelimäärä vaikutti tulokseen)
//   15.-19.6.2018  Laskennassa käytettyjen apumuuttujien käytön tarkistusta ja turhiksi jääneiden poistoa,
//                  sekä kommenttien päivitystä (laskentaa muutettu aika paljon).
//                  Tarkistettu näkyvyyttä (public vs. private).
//   18.7.2018      Nyt Selopelaaja-luokka käyttää Tulokset-luokkaa pohjana eikä esittele samoja kenttiä uudestaan laskentaa varten.
//                  Lisäksi selvitetään vastustajienLkm ja turnauksenKeskivahvuus, joita tarvitaan laskennassa.
//   19.-20.7.2018  Luokan Ottelulista sisältö piilotettu, joten käytetään mm. Lukumaara, HaeEnsimmainen() ym.
//   15.8.2018      Laskennassa erikoiskäsittely TULOS_EI_ANNETTU, joka tarkoittaa sitä että vastustajat-kentässä ei selon
//                  yhteydessä ollut merkkiä +,- tai =. Tuo käsitellään tasapelinä laskennassa.
//

using System;

namespace SelolaskuriLibrary {
    // class Selopelaaja 
    //
    // Used in SelolaskuriOperations, SelolaskuriForm and UnitTesting.
    //
    // pelaa shakkiotteluita, joissa on vastustaja (vastustajan selo) ja tulos (tappio, tasapeli tai voitto)
    //
    public class Selopelaaja // Tulokset: UusiSelo, UusiPelimäärä, MinSelo, MaxSelo,
                             //           Odotustulos, Kerroin, TurnauksenTulos,
                             //           VastustajienLkm, TurnauksenKeskivahvuus
    {
        //// --------------------------------------------------------------------------------
        //// Laskennan aikana päivitettävät tiedot, jotka kopioidaan tuloksiin
        //// ks. SeloLaskuriOperations.SuoritaLaskenta sekä struct Tulokset
        //// --------------------------------------------------------------------------------

        public int UusiSelo { get; private set; }       // Calculated new chess rating
        public int UusiPelimaara { get; private set; }  // new number of played games

        // Selon vaihteluväli mahdollinen selvittää, jos ottelut formaatissa: +1622 -1880 =1633
        public int MinSelo { get; private set; }        // minumum rating during calculation
        public int MaxSelo { get; private set; }        // maximum rating during calculation

        // laskennan aputiedot
        public int Odotustulos { get; private set; }    // expected score
        public int Kerroin { get; private set; }        // factor used in calculations

        // Turnauksen tulos
        //
        // Syötteistä laskettu tulos
        // Selvitetään tulos, jos ottelut formaatissa "+1525 =1600 -1611 +1558", josta esim. saadaan
        // tulokseksi 2,5 (2 voittoa ja 1 tasapeli). Tallennetaan kokonaislukuna tuplana (int)(2*2,5) eli 5.
        public int TurnauksenTulos { get; private set; }    // result from all the matches/tournament

        public int VastustajienLkm { get; private set; }    // number of the opponents/matches
        public int TurnauksenKeskivahvuus { get; private set; } // average opponent strength

        // Tuloksien näyttämisessä tarvitaan alkuperäistä seloa -> muutos, sekä ero keskivahvuuteen
        public int AlkuperainenSelo {                       // original chess rating
            get {
                return alkuperaisetSyotteet.AlkuperainenSelo;
            }
        }

        // Tuloksien näyttämisessä tarvitaan tieto, koska odotustulosta ei näytetä uudelle pelaajalle
        public bool UudenPelaajanLaskenta {                 // use the new player's calculation?
            get {
                return alkuperaisetSyotteet.UudenPelaajanLaskenta();
            }
        }

        // Tuloksien näyttämisessä tarvitaan tieto, koska tulospainikkeet tyhjennetään varalta, jos niitä ei käytetty
        // Voivat jäädä päälle yhtä ottelua syötettäessä, jos vaihdettu tuloksen syöttötapa -> "1.0 1434" tai "+1434"
        public bool KaytettiinkoTulospainikkeita {          // one match, were the result buttons used?
            get {
                return (alkuperaisetSyotteet.YksiVastustajaTulosnapit != 0);
            }
        }

        // Tuloksien näyttämisessä tarkista miettimisaika, jos csv-formaattia käytettäessä vaihdettu
        // Päivitä näytölle oikeat SELO- tai PELO-tekstit
        public Vakiot.Miettimisaika_enum Miettimisaika {
            get {
                return alkuperaisetSyotteet.Miettimisaika;
            }
        }
       
        // Mahdollisesti annettu turnauksen tulos voi olla 0 - vastustajienlkm
        // Tallennetaan kokonaislukuna tuplana: esim. (int)(2*10,5) eli 21
        //
        // Turnauksen tulos voidaan antaa vastustajan SELO-kentässä ensimmäisenä
        // Esim. "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684"
        //
        // Huom! Jos tulos on annettu virheellisesti esim. 0,9 tai 2,4, niin pyöristys alas
        // em. syötteistä saadaan 0,5 tai 2,0 (tallennus 1 tai 4)
        // Jos ei annettu, arvo on TURNAUKSEN_TULOS_ANTAMATTA (eli negatiivinen luku)
        private int annettuTurnauksenTulos;                 // possible given tournament result (-1 if not given)

        // Tarvitaan oma erillinen setter, koska tehdään muunnos float -> kokonaisluku
        public void SetAnnettuTurnauksenTulos(float f)      // set the tournament result
        {
            annettuTurnauksenTulos = (int)Math.Round(2.0F * f + 0.01);
        }

        private bool OnkoAnnettuTurnauksenTulos {
            get { 
                // riittää tarkistaa, että on suurempi kuin asetettu vakio
                // Tuohan oli vieläpä tallennettu 2.0:lla kerrottuna eli oli -2
                return annettuTurnauksenTulos > Vakiot.TURNAUKSEN_TULOS_ANTAMATTA;
            }
        }

        public Selopelaaja()
        {
        }


        // The initial input data is stored here to be used in calcuations.
        // Also some data is used when displaying the results in SelolaskuriForm.
        private Syotetiedot alkuperaisetSyotteet;           // store the initial input data with matches


        // --------------------------------------------------------------------------------
        // Laskenta
        // --------------------------------------------------------------------------------

        // AlustaLaskenta
        //
        // Ennen laskentaa alustetaan muuttujat saatujen syötteiden mukaan
        //
        private void AlustaLaskenta(Syotetiedot syotteet)
        {
            alkuperaisetSyotteet = syotteet;  // selo, pelimaara, miettimisaika, lomakkeelle mm. seloero

            // laskettavat tiedot, selon ja pelimaaran laskenta aloitetaan syötetyistä arvoista
            UusiSelo         = syotteet.AlkuperainenSelo;
            UusiPelimaara    = syotteet.AlkuperainenPelimaara;

            TurnauksenTulos = 0;  // lasketaan otteluista kokonaislukuna
            Odotustulos     = 0;  // summa yksittäisten otteluiden odotustuloksista

            // palautettava kerroin alkuperäisen selon mukaan
            // laskennassa käytetään sen hetkisestä selosta laskettua kerrointa
            Kerroin = MaaritaKerroin(UusiSelo);  

            // vaihteluvälin alustus
            MinSelo = Vakiot.MAX_SELO;
            MaxSelo = Vakiot.MIN_SELO;

            // Lisäksi selvitä syötetiedoista (tarvitaan laskennassa, tulostetaan lomakkeelle)
            //   - vastustajien eli otteluiden lkm
            //   - turnauksen eli vastustajien keskivahvuus
            VastustajienLkm        = syotteet.Ottelut.Lukumaara;
            TurnauksenKeskivahvuus = syotteet.Ottelut.Keskivahvuus; 
        }



        // Pelaa kaikki listalta (syotteet.Ottelut) löytyvät ottelut!
        //
        // Tapaukset: 
        // 1) Uuden pelaajan laskenta, jossa tulokset formaatissa "1.5 1622 1880 1683"
        // 2) Normaali laskenta, jossa käydään kaikki listan ottelut läpi, tulokset "+1525 =1600 -1611 +1558"
        // 3) Uuden pelaajan laskenta, jossa tulokset formaatissa "1.5 1622 1880 1683"
        //
        // Päivittää: UusiSelo, UusiPelimaara, turnauksenTulos, MinSelo ja MaxSelo
        // Palauttaa: -
        //
        public void PelaaKaikkiOttelut(Syotetiedot syotteet)
        {
            Ottelulista ottelulista = syotteet.Ottelut;

            // asettaa omat tiedot (selo ja pelimäärä) seloPelaaja-luokkaan, nollaa tilastotiedot ym.
            AlustaLaskenta(syotteet);

            // XXX: Kun ensimmäinen ottelu, niin UusiSelo ja UusiPelimaara ovat käyttäjän antamat alkuarvot omaSelo ja pelimaara
            // XXX: Laskennan edetessä niitä päivitetään

            // Erikoistapauksena uuden pelaajan tuloksien laskenta turnauksesta,
            // jossa tulokset on ilmoitettu formaatissa "1.5 1622 1880 1683"
            //

            if (OnkoAnnettuTurnauksenTulos && UudenPelaajanLaskenta) {
                //  selo += pistemäärä - ottelut/2 * 200
                // 1 ottelu:
                //    1525 + 0.5 1525 -> tulos 1525    
                // 2 ottelua:
                //  2    1525 1441   summa: 2966  keskim. 1483   tulos on keskim+200
                // keskitulos/matsi = 1
                
                // apumuuttujia (lausekkeiden selkiyttämiseksi ja lyhentämiseksi)
                float keskimTulos = (annettuTurnauksenTulos / 2F) / VastustajienLkm;   // 0.0 - 1.0
                float muutos = 400 * (keskimTulos - 0.5F) + 0.5F;   // tuloksella tasapeli pysytään samassa kuin keskimTulos

                // vanhan selon painoarvo ja uuden lasketun selon painoarvo riippuvat pelimääristä
                UusiSelo       = ((UusiSelo * UusiPelimaara) + (int)(TurnauksenKeskivahvuus + muutos) * VastustajienLkm) / (UusiPelimaara + VastustajienLkm);
                UusiPelimaara += VastustajienLkm;

                // turnauksen tulos annettu, joten ei laskettavaa
                TurnauksenTulos = annettuTurnauksenTulos;

                // koska laskenta tehtiin kerralla, ei saatu minSeloa ja maxSeloa
                MinSelo = UusiSelo;
                MaxSelo = UusiSelo;

                return;
            }


            // Varsinainen laskenta: Käydään läpi kaikki listan ottelut, jotka olivat formaatissa
            // "+1525 =1600 -1611 +1558". Tällöin myös MinSelo ja MaxSelo voidaan selvittää.
            //
            var ottelu = ottelulista.HaeEnsimmainen(); // vastustajanSelo, ottelunTulos

            // Kun lista on tyhjä, saadaan ottelun tulos TULOS_MAARITTELEMATON
            while (ottelu.Item2 != Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON) {
                
                // päivitä seloa ja tilastoja jokaisen ottelun laskennassa, myös laske Odotustulos
                UusiSelo = PelaaOttelu(ottelu.Item1, ottelu.Item2);

                // päivitä pelimäärää vain jos oli annettu
                if (UusiPelimaara != Vakiot.PELIMAARA_TYHJA)
                    UusiPelimaara++;

                ottelu = ottelulista.HaeSeuraava();
            }


            // Entä jos vanhan pelaajan ottelut olivatkin formaatissa "1.5 1622 1880 1683"?
            // Jos näin oli, niin unohdetaan vanha laskenta, josta käytetään vain Odotustulos sekä UusiPelimaara.
            //
            // HUOM! Seuraava ei toimisi uudella pelaajalla, mutta se erikoistapaus onkin käsitelty aiemmin
            //
            if (OnkoAnnettuTurnauksenTulos) {
                //
                // Aiemmasta laskennasta tarvitaan Odotustulos
                // apumuuttuja selo, koska sitä tarvitaan kaavassa usein
                //
                int vanha = alkuperaisetSyotteet.AlkuperainenSelo; // aloitetaan alusta, oma apumuuttuja
                TurnauksenTulos = annettuTurnauksenTulos; // turnauksen tulos annettu, joten ei laskettavaa

                if (alkuperaisetSyotteet.Miettimisaika <= Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN) {
                    //
                    // PELO: pikashakilla on oma laskentakaavansa
                    //
                    // http://skore.users.paivola.fi/selo.html kertoo:
                    // Pikashakin laskennassa Odotustulos lasketaan samoin, mutta ilman 0,85 - sääntöä.
                    // Itse laskentakaava onkin sitten hieman vaikeampi:
                    // pelo = vanha pelo + 200 - 200 * e(Odotustulos - tulos) / 10 , kun saavutettu tulos on odotustulosta suurempi
                    // pelo = vanha pelo - 200 + 200 * e(tulos - Odotustulos) / 10 , kun saavutettu tulos on odotustulosta pienempi
                    //            Loppuosan pitää olla e((tulos - Odotustulos) / 10)  eli sulut lisää, jakolasku ensin.
                    //
                    // turnauksen tulos on kokonaislukuna, pitää jakaa 2:lla
                    // Odotustulos on kokonaisluku ja pitää jakaa 100:lla
                    //
                    // Laskentakaavaan lisätty pyöristys Math.Round, jonka jälkeen kaikista Joukkuepikashakin laskennoista saadaan samat tulokset
                    if ((annettuTurnauksenTulos / 2.0) > (Odotustulos / 100.0)) {
                        UusiSelo =
                            (int)Math.Round(vanha + 200.0 - 200.0 * Math.Pow(Math.E, (Odotustulos / 100.0 - annettuTurnauksenTulos / 2.0) / 10.0) + 0.0001);
                    } else {
                        UusiSelo =
                            (int)Math.Round(vanha - 200.0 + 200.0 * Math.Pow(Math.E, (annettuTurnauksenTulos / 2.0 - Odotustulos / 100.0) / 10.0) + 0.0001);
                    }
                } else {
                    //
                    // SELO: pidemmän miettimisajan pelit eli > 10 min
                    //
                    float lisakerroin = MaaritaLisakerroin(vanha, alkuperaisetSyotteet.Miettimisaika);
                    // Lisätään vielä pelattujen pelien lkm * 0.1
                    UusiSelo =
                        (int)Math.Round((vanha + MaaritaKerroin(vanha) * lisakerroin * (annettuTurnauksenTulos / 2.0 - Odotustulos / 100.0)) + ottelulista.Lukumaara * 0.1 + 0.0001);
                }

                // koska laskenta tehtiin kerralla, ei saatu minSeloa ja maxSeloa
                MinSelo = UusiSelo;
                MaxSelo = UusiSelo;
            }
        }


        // pelatusta shakkiottelusta lasketaan vahvuusluku
        //
        // IN: vastustajan_selo 1000-2999
        // IN: ottelun tulos: 0 = tappio, 1 = tasapeli, 2 = voitto (oikeasti 0, 1/2 ja 1)
        //
        // Käyttää: uusiSelo ja UusiPelimaara
        // Päivittää: Odotustulos, MinSelo ja MaxSelo

        // Palauttaa: uusi vahvuusluku
        //
        private int PelaaOttelu(int vastustajanSelo, Vakiot.OttelunTulos_enum tulos)
        {
            int odotustulos1;  // yhden ottelun Odotustulos, lisätään turnauksen odotustulokseen
            int selo;

            // Erikoistapaus
            if (tulos == Vakiot.OttelunTulos_enum.TULOS_EI_ANNETTU)
                tulos = Vakiot.OttelunTulos_enum.TULOS_TASAPELI;

            // Vanhan pelaajan SELOn laskennassa käytetään odotustulosta ja kerrointa
            //
            odotustulos1    = MaaritaOdotustulos(alkuperaisetSyotteet.AlkuperainenSelo, vastustajanSelo);

            Odotustulos     += odotustulos1;  // monta ottelua, niin summa kunkin ottelun odotustuloksista
            TurnauksenTulos += (int)tulos;

            if (alkuperaisetSyotteet.UudenPelaajanLaskenta()) {
                //
                // Uuden pelaajan laskennassa käytetään vastustajan seloa tuloksen mukaan -200 / +0 / +200

                int[] selomuutos = { -200, 0, 200 };  // indeksinä tulos 2*(0, 1/2, 1) -> indeksi (0, 1, 2)

                // Uuden pelaajan SELO, kun pelimäärä 0-10
                // XXX: Kun ensimmäinen ottelu, niin UusiSelo ja UusiPelimaara ovat käyttäjän antamat alkuarvot omaSelo ja pelimaara
                // XXX: Laskennan edetessä niitä päivitetään

                // Jos pelimäärä on 0, niin omalla selolla (selo-kenttä) ei ole merkitystä (UusiSelo * 0 on nolla)
                selo = (int)Math.Round((UusiSelo * UusiPelimaara + (vastustajanSelo + selomuutos[(int)tulos])) / (UusiPelimaara + 1F) + 0.01);

            } else {

                // Vanhan pelaajan laskenta
                //
                // XXX: Käytetään kerrointa ja lisäkerrointa, jotka lasketaan laskennan aikaisesta vahvuusluvusta, OK?
                int kerroin1 = MaaritaKerroin(UusiSelo);
                float lisakerroin = MaaritaLisakerroin(UusiSelo, alkuperaisetSyotteet.Miettimisaika);

                // vanhan pelaajan SELO, kun pelimäärä jätetty tyhjäksi tai on yli 10.
                selo = (int)Math.Round((UusiSelo + kerroin1 * lisakerroin * (((int)tulos / 2F) - (odotustulos1 / 100F)) + 0.1F) + 0.01);
            }

            // tallenna vaihteluväli (jos yksi ottelu, niin jäävät samoiksi)
            MinSelo = Math.Min(MinSelo, selo);
            MaxSelo = Math.Max(MaxSelo, selo);

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

            // laske Odotustulos taulukon paikkaa eli indeksiä käyttäen
            // jos ei löytynyt, niin index 50 ja Odotustulos 0 (0,00) tai 100 (1,00)
            int odotustulos = 50 + sign * index;

            // Pikashakissa ei odotustulosta rajoiteta 92:een (kun miettimisaika <= 10 min)
            return (odotustulos > 92 && alkuperaisetSyotteet.Miettimisaika >= Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN) ? 92 : odotustulos;
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
        private float MaaritaLisakerroin(int selo, Vakiot.Miettimisaika_enum aika)
        {
            float f = 1.0F;

            // Tämä ei vaikuta uuden pelaajan SELOn laskentaan
            if (aika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN)
                f = 0.5F;
            else if (aika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN)
                f = (selo < 2300) ? 0.3F : 0.15F;
            return f;
        }
    }
}
