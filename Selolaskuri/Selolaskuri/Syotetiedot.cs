//
// Luokka syötetietoja varten
//
// Public:
//      Syotetiedot() ilman parametreja ja parametrein
//      UudenPelaajanLaskenta
//
// Luotu 5.4.2018 Ismo Suihko
//
// Muutokset:
// 16.-17.6.2018  pientä järjestelyä, lisätty tarkistus UudenPelaajanLaskenta()

namespace Selolaskuri
{
    // Luokka/tietorakenne syötetiedoille
    //
    // Tähän talteen annetut syötetiedot, jotka välitetään SeloPelaaja-luokkaan
    // Myös merkkijonomuotoisen syötteen tarkastuksen jälkeen numeroiksi muutetut tiedot
    // sekä lista otteluista (vastustajan vahvuusluku, ottelun tulos)
    //
    public class Syotetiedot
    {
        // Alkuperäiset syötteet (sama järjestys kuin näytöllä)
        public Vakiot.Miettimisaika_enum  miettimisaika;
        public string alkuperainenSelo_str;
        public string alkuperainenPelimaara_str;
        public string vastustajienSelot_str;  // vastustajan/vastustajien tiedot ja tulokset
        public Vakiot.OttelunTulos_enum ottelunTulos;

        // Tarkastuksessa merkkijonot muutettu numeroiksi
        public int alkuperainenSelo;
        public int alkuperainenPelimaara;
        public int vastustajanSeloYksittainen;

        public Ottelulista ottelut;   // sis. vastustajien selot ja ottelutulokset

        public Syotetiedot()
        {
            miettimisaika              = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;  // =oletus
            alkuperainenSelo           = 0;
            alkuperainenPelimaara      = 0;
            vastustajanSeloYksittainen = 0;
            vastustajienSelot_str      = null;
            ottelunTulos               = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
            ottelut                    = new Ottelulista();
        }

        // KÄYTETÄÄN TESTATTAESSA (UnitTest)
        // Esim. Syotetiedot ottelu =
        //    new Syotetiedot(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
        public Syotetiedot(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            miettimisaika              = aika;
            alkuperainenSelo_str       = selo;
            alkuperainenPelimaara_str  = pelimaara;
            vastustajienSelot_str      = vastustajat;
            ottelunTulos               = tulos;
            ottelut                    = new Ottelulista();
        }

        // Uuden pelaajan laskennassa ja tulostuksissa joitain erikoistapauksia
        // Tarkistetaan alkuperäisestä pelimäärästä
        public bool UudenPelaajanLaskenta()
        {
            return (alkuperainenPelimaara >= Vakiot.MIN_PELIMAARA &&
                    alkuperainenPelimaara <= Vakiot.MAX_PELIMAARA_UUSI_PELAAJA);
        }
    }
}
