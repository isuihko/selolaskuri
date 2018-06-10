//
// Luokka syötetietoja varten
//
// Luotu 5.4.2018 Ismo Suihko
//

namespace Selolaskuri
{
    // Luokka/tietorakenne syötetiedoille
    //
    // Tähän talteen annetut syötetiedot, jotka välitetään SeloPelaaja-luokkaan
    //
    // Tarkoitus on, että tätä luokkaa/tietorakennetta käytetään automaattissa testauksessa!
    // Luokan constructor voi siksi muuttua vielä.
    //
    public class Syotetiedot
    {

        // Alkuperäiset syötteet
        public Vakiot.Miettimisaika_enum miettimisaika;
        public string nykyinenSelo_str { get; set; }
        public string nykyinenPelimaara_str { get; set; }
        public string vastustajienSelot_str { get; set; }  // vastustajan/vastustajien tiedot ja tulokset
        public Vakiot.OttelunTulos_enum ottelunTulos { get; set; }


        // Tarkastetut syötteet, merkkijonot muutettu numeroiksi
        public int nykyinenSelo { get; set; }
        public int nykyinenPelimaara { get; set; }
        public int vastustajanSelo { get; set; }

        public Syotetiedot()
        {
            miettimisaika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;
            nykyinenSelo = 0;
            nykyinenPelimaara = 0;
            vastustajanSelo = 0;
            ottelunTulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
            //vastustajienSelot_str = null;
        }

        // KÄYTETÄÄN TESTATTAESSA, Tiedot syötetään constructorille
        // Syotetiedot ottelu1 = new Syotetiedot(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
        public Syotetiedot(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            this.miettimisaika = aika;
            this.nykyinenSelo_str = selo;
            this.nykyinenPelimaara_str = pelimaara;
            this.vastustajienSelot_str = vastustajat;
            this.ottelunTulos = tulos;
        }
    }
}
