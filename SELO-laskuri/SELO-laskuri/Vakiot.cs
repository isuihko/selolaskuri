
namespace Selolaskuri
{
    public class Vakiot
    {
        // YLEISET VAKIOT, joilla määrätään syötteen rajat
        // Käytetään pääasiassa tarkista_input() -rutiinissa
        public const int MIN_SELO      = 1000;
        public const int MAX_SELO      = 2999;
        public const int MIN_PELIMAARA = 0;
        public const int MAX_PELIMAARA = 9999;
        
        // Kun ottelumäärä on 0-10, niin käytetään uuden pelaajan laskentakaavaa
        public const int MAX_UUSI_PELAAJA = 10;

        // syötteen tarkastuksessa käytetyt statukset, eivät ole syötteiden arvoarvoalueella
        public const int VIRHE_SELO              = -1;
        public const int VIRHE_PELIMAARA         = -2;
        public const int VIRHE_TULOS             = -3;
        public const int VIRHE_YKSITTAINEN_TULOS = -4;
        public const int VIRHE_TURNAUKSEN_TULOS  = -5;

        public const int PELIMAARA_TYHJA         = -1;  // OK, ei pakko antaa

        // input-kentän syötteen maksimipituus. Tarkistetaan virhetilanteissa
        // ja jos merkkejä yli tuon, niin tyhjennetään kenttä
        public const int MAX_PITUUS  = 5;
        public const int SELO_PITUUS = 4;

        // Tallenna tulokset kokonaislukuina. Oikeasti 0, 1/2 ja 1.
        public enum OttelunTulos_enum {
            TULOS_MAARITTELEMATON = -1,
            TULOS_TAPPIOx2        = 0,
            TULOS_TASAPELIx2      = 1,
            TULOS_VOITTOx2        = 2
        };

        // Miettimisajat (oltava miettimisajan pituuden mukaan nousevassa järjestyksessä)
        public enum Miettimisaika_enum  {
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
