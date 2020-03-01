//
// Class for the input data from SelolaskuriForm or unit testing
//
// Public:
//      UudenPelaajanLaskenta()     Checks if the new player's calculation is needed
//
// 5.4.2018 Ismo Suihko
//
// Modifications:
// 16.-17.6.2018  Small organizing, added UudenPelaajanLaskenta()
// 21.-22.7.2018  Now uses only the constructor with parameters and setting most of the data is private, only by constructor.
// 1.8.2018       New constructor with new parameter doTrim. If true, extra white space are removed from character strings.
// 1.3.2020       CSV checking is now here, so applications do not need to check
//

using System;
using System.Collections.Generic; // List
using System.Linq; // ToList

namespace SelolaskuriLibrary {
    // Luokka/tietorakenne syötetiedoille
    //
    // Tähän talteen annetut syötetiedot, jotka välitetään SeloPelaaja-luokkaan
    // Myös merkkijonomuotoisen syötteen tarkastuksen jälkeen numeroiksi muutetut tiedot
    // sekä lista otteluista (vastustajan vahvuusluku, ottelun tulos)
    //
    public class Syotetiedot
    {
        // Alkuperäiset syötteet (sama järjestys kuin näytöllä)
        public Vakiot.Miettimisaika_enum  Miettimisaika { get; private set; }
        public string AlkuperainenSelo_str { get; private set; }
        public string AlkuperainenPelimaara_str { get; private set; }
        public string VastustajienSelot_str { get; private set; }
        public Vakiot.OttelunTulos_enum OttelunTulos { get; private set; }

        // Tarkastuksessa merkkijonot muutettu numeroiksi
        public int AlkuperainenSelo { get; set; }
        public int AlkuperainenPelimaara { get; set; }
        public int UudenPelaajanPelitEnsinLKM { get; set; }  // tähän selvitetään syötteestä kohta '/', jonka jälkeen vaihtuu laskentakaava
        public int YksiVastustajaTulosnapit { get; set; }

        public Ottelulista Ottelut { get; private set; }   // sis. vastustajien selot ja ottelutulokset


        // Konstruktorin käyttö:
        //  - Lomakkeelta (SelolaskuriForm.cs)
        //     return new Syotetiedot(HaeMiettimisaika(), selo_in.Text, pelimaara_in.Text, vastustajanSelo_comboBox.Text, HaeOttelunTulos());
        // Lomakkeella doTrim=false, koska ylimääräiset välilyönnit poistetaan jo ennen kutsua, jotta
        // ne saadaan päivitettyä myös lomakkeen kenttiin.
        public Syotetiedot(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
            : this(aika, selo, pelimaara, vastustajat, tulos, /*doTrim*/false)
        {
        }

        //  - TESTATTAESSA (UnitTest1.cs)
        //     Syotetiedot ottelu =
        //        new Syotetiedot(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1725", "1", "1441", Vakiot.OttelunTulos_enum.TULOS_VOITTO, /*doTrim*/true);
        // Testattaessa doTrim=true, koska välilyöntien poistoa ei tehdä yksikkötestauksessa vaan
        // poiston jättäminen tänne on osa testausta.
        public Syotetiedot(Vakiot.Miettimisaika_enum aika1, string selo1, string pelimaara1, string vastustajat1, Vakiot.OttelunTulos_enum tulos1,  bool doTrim)
        {
            // XXX: Could check arguments everywhere: arg == null or sometimes string.IsNullOrEmpty(arg)
            if (selo1 == null)
                throw new ArgumentNullException(nameof(selo1));
            if (pelimaara1 == null)
                throw new ArgumentNullException(nameof(pelimaara1));
            if (vastustajat1 == null)
                throw new ArgumentNullException(nameof(vastustajat1));


            // Copy arguments, might be modified
            Vakiot.Miettimisaika_enum aika = aika1;
            string selo = String.Copy(selo1);
            string pelimaara = String.Copy(pelimaara1);
            string vastustajat = String.Copy(vastustajat1);
            Vakiot.OttelunTulos_enum tulos = tulos1;


            // If CSV format, values there will override other parameters
            if (vastustajat.Contains(','))
            {
                List<string> tmp = vastustajat.Split(',').ToList();

                if (tmp.Count != 2 || (tmp.Count == 2 && tmp[0].Trim().Length >= 4))
                {
                    //vastustajat = so.SiistiVastustajatKentta(vastustajat.Trim());
                    List<string> data = vastustajat.Split(',').ToList();

                    if (data.Count == 5)
                    {
                        aika = this.SelvitaMiettimisaikaCSV(data[0]);
                        selo = data[1];
                        pelimaara = data[2];
                        vastustajat = data[3];
                        tulos = this.SelvitaTulosCSV(data[4]);
                    }
                    else if (data.Count == 4)
                    {
                        // viimeinen osa voi sisältää vastustajat tuloksineen tai jos alkuperäinen pelimäärä on enintään 10, 
                        // niin ensin lasketaan uuden pelaajan kaavalla ja loput "/"-merkin jälkeen menevät normaalilaskentaan
                        //  "90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966 +1321 -1678 -1864 -1944 / -1995 +1695 -1930 1901",
                        aika = this.SelvitaMiettimisaikaCSV(data[0]);
                        selo = data[1];
                        pelimaara = data[2];
                        vastustajat = data[3];
                        tulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
                    }
                    else if (data.Count == 3)
                    {
                        //aika = aika;
                        selo = data[0];
                        pelimaara = data[1];
                        vastustajat = data[2];
                        tulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
                    }
                    else if (data.Count == 2)
                    {
                        //aika = aika;
                        selo = data[0];
                        pelimaara = "";
                        vastustajat = data[1];
                        tulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
                    }
                    else
                    {
                        // XXX: CHECK WHAT HAPPENS HERE
                        // XXX: Virheen pitää olla Vakiot.SYOTE_VIRHE_CSV_FORMAT

                        //aika = aika;
                        selo = "";
                        pelimaara = "";
                        vastustajat = "";
                        tulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
                    }
                    // The thinking time might be needed from the form if there are 2 or 3 values in CSV format
                    //return so.SelvitaCSV(aika, vastustajat);
                }
            }

            Miettimisaika = aika;
            // if doTrim -> remove leading and trailing white spaces
            AlkuperainenSelo_str = doTrim ? selo.Trim() : selo;
            AlkuperainenPelimaara_str = doTrim ? pelimaara.Trim() : pelimaara;
            if (doTrim) {
                SelolaskuriOperations so = new SelolaskuriOperations();
                vastustajat = so.SiistiVastustajatKentta(vastustajat.Trim());
            }
            VastustajienSelot_str = vastustajat;
            OttelunTulos = tulos;

            // Clear these too although not actually needed
            AlkuperainenSelo = 0;
            AlkuperainenPelimaara = 0;
            UudenPelaajanPelitEnsinLKM = 0;
            YksiVastustajaTulosnapit = 0;

            // Create en empty list for matches (opponent's selo, match result)
            Ottelut = new Ottelulista();
        }

        // Uuden pelaajan laskennassa ja tulostuksissa joitain erikoistapauksia
        //
        // Tarkistetaan alkuperäisestä pelimäärästä, sillä turnauksen laskenta tehdään
        // uuden pelaajan kaavalla vaikka pelimäärä turnauksen laskennan aikana ylittäisikin rajan.
        public bool UudenPelaajanLaskentaAlkupPelimaara()
        {
            return (AlkuperainenPelimaara >= Vakiot.MIN_PELIMAARA &&
                    AlkuperainenPelimaara <= Vakiot.MAX_PELIMAARA_UUSI_PELAAJA);
        }


        // Miettimisaika, vain minuutit, esim. "5" tai "90"
        // Oltava kokonaisluku ja vähintään 1 minuutti
        private Vakiot.Miettimisaika_enum SelvitaMiettimisaikaCSV(string s)
        {
            Vakiot.Miettimisaika_enum aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_MAARITTELEMATON;
            int temp; // define here instead of "out int temp" for Visual Studio 2015 compatibility
            if (int.TryParse(s, out /*int*/ temp) == true)
            {
                if (temp < 1)
                {
                    // ei voida pelata ilman miettimisaikaa
                    // jo asetettu aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_MAARITTELEMATON;
                }
                else if (temp <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN)
                    aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN;
                else if (temp <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN)
                    aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN;
                else if (temp <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN)
                    aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN;
                else
                    aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;
            }
            return aika;
        }

        // Yksittäisen ottelun tulos joko "0", "0.0", "0,0", "0.5", "0,5", "1/2", "½" (alt-171), "1", "1.0" tai "1,0"
        // Toistaiseksi CSV-formaatin tuloksissa voi käyttää vain desimaalipistettä, joten ei voida syöttää 
        // tuloksia pilkun kanssa kuten "0,0", "0,5" ja "1,0". Tarkistetaan ne kuitenkin varalta.
        private Vakiot.OttelunTulos_enum SelvitaTulosCSV(string s)
        {
            Vakiot.OttelunTulos_enum tulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
            if (s.Equals("0") || s.Equals("0.0") || s.Equals("0,0"))
                tulos = Vakiot.OttelunTulos_enum.TULOS_TAPPIO;
            else if (s.Equals("0.5") || s.Equals("0,5") || s.Equals("1/2") || s.Equals("½"))
                tulos = Vakiot.OttelunTulos_enum.TULOS_TASAPELI;
            else if (s.Equals("1") || s.Equals("1.0") || s.Equals("1,0"))
                tulos = Vakiot.OttelunTulos_enum.TULOS_VOITTO;
            return tulos;
        }
    }
}
