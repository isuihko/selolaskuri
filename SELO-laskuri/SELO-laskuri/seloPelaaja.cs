﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class seloPelaaja
    {
        //  XXX: NÄMÄ ON MUUTETTY -> PROPERTY
        //
        // pelaajan omat tiedot
        //private int miettimisaika;  // miettimisajan mukaan laskukaavat, 90 / 60 / 15 / 5
        //private int selo;           // shakinpelaajan vahvuusluku, 1000-2999
        //private int pelimaara;      // pelimaara tai -1, jolloin sitä ei huomioida
        //private int selo_alkuperainen; // SELO, jolla laskenta aloitettiin

        // nämä lasketaan
        //private int uusi_selo;
        //private int uusi_pelimaara;

        // vaihteluväli, jos vastustajien selot ja tulokset formaatissa: +1622 -1880 =1633
        //private int min_selo;
        //private int max_selo;

        // laskennan aputiedot
        //private int viimeisin_vastustaja;
        //private int odotustulos;
        //private int kerroin;

        // kun lasketaan kerralla useamman ottelun (turnauksen matsit) vaikutus
        private int turnauksen_vastustajien_lkm;       // päivitetään laskennan edetessä
        private int turnauksen_vastustajien_selosumma; // keskiarvoa varten
        //private int odotustuloksien_summa;             // laskettu summa
        //private int turnauksen_tulos;                  // laskettu tulos

        // Jos tulos ja vastustajien selo on annettu formaatissa: 1.5 1622 1880 1633
        //private int syotetty_turnauksen_tulos;


        public seloPelaaja(int selo, int pelimaara)
        {
            this.selo = selo;
            this.pelimaara = pelimaara;
        }

        // Ottelujen tietojen tallennus, vastustajan vahvuusluku ja ottelun tulos
        private struct ottelu_table
        {
            //private int vastustajan_selo;
            //private int ottelun_tulos;

            public ottelu_table(int selo, int tulos)
            {
                vastustajan_selo = selo;
                ottelun_tulos = tulos;
            }

            public int vastustajan_selo {
                get; private set;
            }

            public int ottelun_tulos {
                get; private set;
            }
        }
        // Lista vastustajien tiedoille ja ottelutuloksille
        private IList<ottelu_table> ottelut_list = null;


        // Ottelutuloksen (vastustaja ja tulos) lisääminen listaan
        public void lista_lisaa_ottelun_tulos(int vastustajan_selo, int tulos)
        {
            ottelu_table ottelu = new ottelu_table(vastustajan_selo, tulos);
            ottelut_list.Add(ottelu);
        }


        // Listan tyhjennys, jotta siihen voidaan tallentaa ottelujen tiedot
        public void listan_alustus()
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
        public int vastustajien_lukumaara_listalla
        {
            get {
                return ottelut_list.Count;
            }
        }


        // FUNKTIO: aloita_laskenta(int selo, int pelimaara)
        //
        // Ennen laskennan aloittamista asetetaan omat tiedot ja nollataan apumuuttujat.
        public void aloita_laskenta(int selo, int pelimaara)
        {
            this.selo = selo;
            this.pelimaara = pelimaara;
            selo_alkuperainen = selo;  // tästä aloitettiin!
            
            // vaihteluvälin alustus näin, jotta min ja max toimisivat
            min_selo = vakiot.MAX_SELO;
            max_selo = vakiot.MIN_SELO;

            uusi_pelimaara = -1;
            turnauksen_vastustajien_lkm = 0;
            turnauksen_vastustajien_selosumma = 0;
            turnauksen_tulos = 0;
            odotustuloksien_summa = 0;
        }


        //   SETTERS & GETTERS

        public int selo {
            get; set;
        }

        public int pelimaara {
            get; private set;
        }

        public void set_syotetty_turnauksen_tulos(float f)
        {
            // tallennetaan kokonaislukuna
            //  0 = 0, tasapeli on 0.5:n sijaan 1, voitto on 1:n sijaan 2
            syotetty_turnauksen_tulos = (int)(2 * f + 0.01F); // pyöristys kuntoon
        }

        public int syotetty_turnauksen_tulos
        {
            get; private set;
        }


        //   SETTERS ONLY (private get)

        public int miettimisaika {
            private get; set;
        }

        //   GETTERS ONLY (private set)

        public int selo_alkuperainen {
            get; private set;
        }

        public int viimeisin_vastustaja {
            get; private set;
        }

        public int odotustulos {
            get; private set;
        }

        public int odotustuloksien_summa {
            get; private set;
        }

        public int kerroin {
            get; private set;
        }

        public int uusi_selo {
            get; private set;
        }

        public int uusi_pelimaara {
            get; private set;
        }

        public int min_selo {
            get; private set;
        }

        public int max_selo {
            get; private set;
        }

        public int turnauksen_ottelumaara {
            get {
                return turnauksen_vastustajien_lkm;  // päivitetty laskennan edetessä
            }
            set {
                turnauksen_vastustajien_lkm = value;
            }
        }

        public int turnauksen_keskivahvuus {
            get {
                return (int)((float)turnauksen_vastustajien_selosumma / turnauksen_vastustajien_lkm + 0.5F);
            }
        }

        public int turnauksen_tulos {
            get; private set;
        }


        // Eri miettimisajoilla voi olla omia kertoimia
        private float maarita_lisakerroin()
        {
            float f = 1.0F;

            // Tämä ei vaikuta uuden pelaajan SELOn laskentaan
            if (miettimisaika == vakiot.Miettimisaika_60_89min)
                f = 0.5F;
            else if (miettimisaika == vakiot.Miettimisaika_11_59min)
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
            kerroin = maarita_kerroin();

            // DEBUG:   MessageBox.Show("Odotus " + odotustulos + " kerroin " + kerroin + " selo " + selo + " pelim " + pelimaara + " vastus " + vastustajan_selo);

            if (pelimaara >= vakiot.MIN_PELIMAARA && pelimaara <= vakiot.MAX_UUSI_PELAAJA) {
                // Uuden pelaajan SELO, kun pelimäärä 0-10
                // Jos pelimäärä on 0, niin nykyinenSelo-kentän arvolla ei ole merkitystä
                // XXX: tarvitseeko pyöristää jakolaskun jälkeen? (+0,5F)
                uusi_selo = (int)Math.Round((selo * pelimaara + (vastustajan_selo + selomuutos[tulos])) / (pelimaara + 1F));
            }
            else {
                // vanhan pelaajan SELO, kun pelimäärä jätetty tyhjäksi tai on yli 10.
                // XXX: SELOn pyöristys? lisätään 0.5F, kaavassa lisäksi +0.1F
                uusi_selo = (int)Math.Round((selo + kerroin * lisakerroin * ((tulos / 2F) - (odotustulos / 100F)) + 0.1F));
            }

            if (pelimaara >= vakiot.MIN_PELIMAARA)
                uusi_pelimaara = pelimaara + 1;

            // laskenta etenee!
            turnauksen_vastustajien_selosumma += vastustajan_selo;
            turnauksen_vastustajien_lkm++;   // tässä tieto myös kun vastustajia on vain yksi
            odotustuloksien_summa += odotustulos;  // = vastustajien_lkm * odotustulos
            turnauksen_tulos += tulos;

            // tallenna vaihteluväli
            min_selo = Math.Min(min_selo, uusi_selo);
            max_selo = Math.Max(max_selo, uusi_selo);

            return uusi_selo;
        }

        // FUNKTIO: pelaa_ottelu
        //
        // pelaa kaikki listalta ottelut_list löytyvät ottelut!
        public int pelaa_ottelu()
        {
            // ottelu_table ottelu1 = new ottelu_table();

            foreach (ottelu_table ottelu1 in ottelut_list) {
                // tarkista, että tiedot ovat alkuperäisessä järjestyksessään  -> OK!
                // MessageBox.Show(" vastustaja: " + ottelu1.get_vastustajan_selo() + "tulos: " + ottelu1.get_ottelun_tulos());

                // päivitä seloa jokaisella ottelulla, jotta käytetään laskennassa aina viimesintä
                selo = pelaa_ottelu(ottelu1.vastustajan_selo, ottelu1.ottelun_tulos);

                // päivitä pelimäärää, jos oli annettu
                if (pelimaara != vakiot.PELIMAARA_TYHJA)
                    pelimaara++;
            }


            // Pikashakin laskentakaavaan mennään täällä eli sitä käytetään vain
            // jos ottelun pisteet on annettu ensimmäisenä
            if (syotetty_turnauksen_tulos >= 0) {
                // DEBUG         MessageBox.Show("laske turnauksen tulos: " + syotetty_turnauksen_tulos + "selo/alkup " + selo + "/" + selo_alkuperainen);

                // unohdetaan aiempi selolaskenta!
                // Mutta sieltä saadaan odotustuloksien_summa ja pelimaara valmiiksi!
                selo = selo_alkuperainen;
                turnauksen_tulos = syotetty_turnauksen_tulos;

                if (miettimisaika <= vakiot.Miettimisaika_enint10min) {
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
                    if ((syotetty_turnauksen_tulos / 2F) > (odotustuloksien_summa / 100F)) {
                        uusi_selo =
                            (int)(selo + 200 - 200 * Math.Pow(Math.E, (odotustuloksien_summa / 100F - syotetty_turnauksen_tulos / 2F) / 10F));
                    }
                    else {
                        uusi_selo =
                            (int)(selo - 200 + 200 * Math.Pow(Math.E, (syotetty_turnauksen_tulos / 2F - odotustuloksien_summa / 100F) / 10F));
                    }
                }
                else {
                    //
                    // pidemmän miettimisajan pelit eli > 10 min
                    //
                    float lisakerroin = maarita_lisakerroin();
                    // myös 0.5F pyöristystä varten
                    uusi_selo =
                        (int)((selo + maarita_kerroin() * lisakerroin * (syotetty_turnauksen_tulos / 2F - odotustuloksien_summa / 100F)) + (pelimaara * 0.1F) + 0.5F);
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
            while (index < difftable.Length) {
                if (diff < difftable[index])
                    break;
                index++;
            }

            // laske odotustulos taulukon paikkaa eli indeksiä käyttäen
            // jos ei löytynyt, niin index 50 ja odotustulos 0 (0,00) tai 100 (1,00)
            odotustulos = 50 + sign * index;

            // Pikashakissa ei odotustulosta rajoiteta 92:een (kun miettimisaika <= 10 min)
            return (odotustulos > 92 && miettimisaika >= vakiot.Miettimisaika_11_59min) ? 92 : odotustulos;   // enintään 92 (0,92)
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
}
