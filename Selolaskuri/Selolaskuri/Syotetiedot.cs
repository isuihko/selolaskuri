//
// Luokka syötetietoja varten
//
// Public:
//      Syotetiedot()           konstruktori lomakkeelle ilman parametreja ja yksikkötestauksessa käytettäväksi parametrein
//      UudenPelaajanLaskenta   tarkistaa, tarvitaanko pelimäärän mukaan uuden pelaajan laskusääntöä
//
// Luotu 5.4.2018 Ismo Suihko
//
// Muutokset:
// 16.-17.6.2018  pientä järjestelyä, lisätty tarkistus UudenPelaajanLaskenta()
// 19.6.2018      constructor: poistettu toistoa käyttämällä konstruktorien ketjutusta (constructor chaining)

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
        public Vakiot.Miettimisaika_enum  Miettimisaika { get; private set; }
        public string AlkuperainenSelo_str { get; private set; }
        public string AlkuperainenPelimaara_str { get; private set; }
        public string VastustajienSelot_str { get; private set; }
        public Vakiot.OttelunTulos_enum OttelunTulos { get; private set; }

        // Tarkastuksessa merkkijonot muutettu numeroiksi
        public int AlkuperainenSelo { get; set; }
        public int AlkuperainenPelimaara { get; set; }
        public int VastustajanSeloYksittainen { get; set; }

        public Ottelulista Ottelut { get; private set; }   // sis. vastustajien selot ja ottelutulokset


        // Oikeastaan kaikkea ei tarvitsisi alustaa, koska tiedot täytetään lomakkeelta
        // ks. SelolaskuriForm.cs/HaeSyotteetLomakkeelta()
        //
        // Ottelulista pitää kuitenkin luoda ja tehdään nyt kaikki muukin alustus. Constructor chaining.
        public Syotetiedot() : this(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, null, null, null, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON)
        {          
        }

        // KÄYTETÄÄN TESTATTAESSA (UnitTest1.cs)
        // esim. Syotetiedot ottelu =
        //   new Syotetiedot(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1725", "1", "1441", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
        public Syotetiedot(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            Miettimisaika              = aika;
            AlkuperainenSelo_str       = selo;
            AlkuperainenPelimaara_str  = pelimaara;
            VastustajienSelot_str      = vastustajat;
            OttelunTulos               = tulos;

            // Clear these too although not actually needed
            AlkuperainenSelo            = 0;
            AlkuperainenPelimaara       = 0;
            VastustajanSeloYksittainen  = 0;
            
            // Create en empty list for matches (opponent's selo, match result)
            Ottelut = new Ottelulista();
        }


        // Uuden pelaajan laskennassa ja tulostuksissa joitain erikoistapauksia
        //
        // Tarkistetaan alkuperäisestä pelimäärästä, sillä turnauksen laskenta tehdään
        // uuden pelaajan kaavalla vaikka pelimäärä laskennan aikana ylittäisikin rajan
        public bool UudenPelaajanLaskenta()
        {
            return (AlkuperainenPelimaara >= Vakiot.MIN_PELIMAARA &&
                    AlkuperainenPelimaara <= Vakiot.MAX_PELIMAARA_UUSI_PELAAJA);
        }
    }
}
