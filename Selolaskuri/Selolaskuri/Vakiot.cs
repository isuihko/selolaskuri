//
// Selolaskurissa käytetyt vakiot
//
// Luotu 7.1.2018 Ismo Suihko
//
// Muutettu:
//   1.-6.4.2018    Koodin järjestämistä, vakioiden nimet selkeämmiksi
//   10.6.2018      Uusi Selolaskuri-projekti, nimiä selkeämmiksi
//    

namespace Selolaskuri {

    public class Vakiot {
        // YLEISET VAKIOT, joilla määrätään syötteen rajat
        public const int MIN_SELO       = 1000;
        public const int MAX_SELO       = 2999;
        public const int MIN_PELIMAARA  = 0;
        public const int MAX_PELIMAARA  = 9999;

        // Vahvuuslukuarvon maksimipituus
        public const int MAX_PITUUS  = 5;   // Sisältää ottelun tuloksen kertovan +, - tai =
        public const int SELO_PITUUS = 4;

        // Kun ottelumäärä on 0-10, käytetään uuden pelaajan laskentakaavaa
        public const int MAX_UUSI_PELAAJA = 10;

        // syötteen tarkastuksessa käytetyt virhestatukset eivät ole syötteiden arvoarvoalueella
        public const int SYOTE_STATUS_OK                = 0;
        public const int SYOTE_VIRHE_OMA_SELO           = -1;
        public const int SYOTE_VIRHE_VAST_SELO          = -2;
        public const int SYOTE_VIRHE_PELIMAARA          = -3;
        public const int SYOTE_VIRHE_BUTTON_TULOS       = -4;
        public const int SYOTE_VIRHE_YKSITTAINEN_TULOS  = -5;
        public const int SYOTE_VIRHE_TURNAUKSEN_TULOS   = -6;


        public const int PELIMAARA_TYHJA = -1;  // OK, muilla kuin uusilla pelaajilla voi olla tyhjä

        // Tallenna tulokset kokonaislukuina. Laskennassa käytetään 0, 1/2 ja 1.
        public enum OttelunTulos_enum
        {
            TULOS_MAARITTELEMATON = -1,
            TULOS_TAPPIO = 0,
            TULOS_TASAPELI = 1,
            TULOS_VOITTO = 2
        };

        // Miettimisajat (miettimisajan pituuden mukaan nousevassa järjestyksessä)
        public enum Miettimisaika_enum {
            MIETTIMISAIKA_ENINT_10MIN,
            MIETTIMISAIKA_11_59MIN,
            MIETTIMISAIKA_60_89MIN,
            MIETTIMISAIKA_VAH_90MIN
        };

        // Miettimisaikojen vaihdon takia vaihdetaan kenttien tekstejä. SELO on pitkä peli ja PELO on pikashakki.
        // Esim. "Oma SELO" ja "Oma PELO".
        public enum VaihdaMiettimisaika_enum { 
            VAIHDA_SELOKSI,
            VAIHDA_PELOKSI
        };
    }
}

