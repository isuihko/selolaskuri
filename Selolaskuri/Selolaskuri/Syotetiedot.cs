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
        public string alkuperainenSelo_str { get; set; }
        public string alkuperainenPelimaara_str { get; set; }
        public string vastustajienSelot_str { get; set; }  // vastustajan/vastustajien tiedot ja tulokset
        public Vakiot.OttelunTulos_enum ottelunTulos { get; set; }


        // Tarkastetut syötteet, merkkijonot muutettu numeroiksi
        public int alkuperainenSelo { get; set; }
        public int alkuperainenPelimaara { get; set; }
        public int vastustajanSeloYksittainen { get; set; }

        public Syotetiedot()
        {
            this.miettimisaika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;  // =oletus
            this.alkuperainenSelo = 0;
            this.alkuperainenPelimaara = 0;
            this.vastustajanSeloYksittainen = 0;
            this.ottelunTulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
            //this.vastustajienSelot_str = null;
        }

        // KÄYTETÄÄN TESTATTAESSA (UnitTest), Tiedot syötetään constructorille, enum ja merkkijonot
        // Syotetiedot ottelu = new Syotetiedot(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
        public Syotetiedot(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            this.miettimisaika = aika;
            this.alkuperainenSelo_str = selo;
            this.alkuperainenPelimaara_str = pelimaara;
            this.vastustajienSelot_str = vastustajat;
            this.ottelunTulos = tulos;
        }
    }
}
