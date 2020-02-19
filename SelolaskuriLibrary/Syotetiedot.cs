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
//

using System;

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
        public Syotetiedot(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos,  bool doTrim)
        {
            // XXX: Could check arguments everywhere: arg == null or sometimes string.IsNullOrEmpty(arg)
            if (selo == null)
                throw new ArgumentNullException(nameof(selo));
            if (pelimaara == null)
                throw new ArgumentNullException(nameof(pelimaara));
            if (vastustajat == null)
                throw new ArgumentNullException(nameof(vastustajat));

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
    }
}
