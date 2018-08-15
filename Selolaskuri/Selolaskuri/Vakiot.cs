//
// All the constants
//
// 7.1.2018 Ismo Suihko
//
// Modifications:
//   17.6.2018      Added UUDEN_PELAAJAN_ALKUSELO
//   4.8.2018       Added SYOTE_VIRHE_CSV_FORMAT
//                  defined initial values for MIETTIMISAIKA constants according to minutes they represent
//   12.8.2018      Added clipboard handling related constants LEIKEKIRJA_*
//   15.8.2018      Added TULOS_EI_ANNETTU and TURNAUKSEN_TULOS_ANTAMATTA
//    

namespace Selolaskuri {

    public class Vakiot {
        // YLEISET VAKIOT, joilla määrätään syötteen rajat
        public const int MIN_SELO       = 1000;
        public const int MAX_SELO       = 2999;
        public const int MIN_PELIMAARA  = 0;
        public const int MAX_PELIMAARA  = 9999;

        // Vahvuuslukuarvon maksimipituus
        // Pituus 5  Sisältää ottelun tuloksen kertovan +, - tai =
        public const int MAX_PITUUS  = 5; 
        public const int SELO_PITUUS = 4;

        // Kun ottelumäärä on 0-10, käytetään uuden pelaajan laskentakaavaa
        public const int MAX_PELIMAARA_UUSI_PELAAJA = 10;
        public const int UUDEN_PELAAJAN_ALKUSELO    = 1525;

        // syötteen tarkastuksessa käytetyt virhestatukset eivät ole syötteiden arvoarvoalueella
        public const int SYOTE_STATUS_OK                = 0;
        public const int SYOTE_VIRHE_MIETTIMISAIKA      = -1; // Tälle ei virheilmoitusta, koska toistaiseksi ei mahdollinen
        public const int SYOTE_VIRHE_OMA_SELO           = -2;
        public const int SYOTE_VIRHE_VASTUSTAJAN_SELO   = -3;
        public const int SYOTE_VIRHE_PELIMAARA          = -4;
        public const int SYOTE_VIRHE_BUTTON_TULOS       = -5;
        public const int SYOTE_VIRHE_YKSITTAINEN_TULOS  = -6;
        public const int SYOTE_VIRHE_TURNAUKSEN_TULOS   = -7;
        public const int SYOTE_VIRHE_CSV_FORMAT         = -8;

        public const int PELIMAARA_TYHJA = -1; // OK, muilla kuin uusilla pelaajilla voi olla tyhjä

        public const int LEIKEKIRJA_MAX_RIVINPITUUS     = 1000;
        public const int LEIKEKIRJA_MAX_RIVIMAARA       = 100;

        public const int TURNAUKSEN_TULOS_ANTAMATTA     = -1;


        // Tallenna kunkin ottelun tulos kokonaislukuna kahdella kerrottuna lukujen 0, 1/2 ja 1 sijaan.
        //
        // TULOS_EI_ANNETTU: kun turnauksen tuloksissa ei ole vahvuusluvun kanssa tulosta +-=
        //                   Esim. "+1624 -1700 1685 +1400" tai "1.5 1525 1441 1973 1718 1784 1660 1966"
        //
        // TULOS_MAARITTELEMATON: Kun tulosta ei ole vielä saatu tietoon. Myös lopetusehto ottelulistaa tutkittaessa.
        //
        public enum OttelunTulos_enum
        {
            TULOS_EI_ANNETTU = -2,          
            TULOS_MAARITTELEMATON = -1,
            TULOS_TAPPIO = 0,
            TULOS_TASAPELI = 1,
            TULOS_VOITTO = 2
        };

        // Miettimisajat (miettimisajan pituuden mukaan nousevassa järjestyksessä)
        public enum Miettimisaika_enum {
            MIETTIMISAIKA_MAARITTELEMATON = -1,
            MIETTIMISAIKA_ENINT_10MIN = 10,
            MIETTIMISAIKA_11_59MIN = 59,
            MIETTIMISAIKA_60_89MIN = 89,
            MIETTIMISAIKA_VAH_90MIN = 90
        };

        // Miettimisaikojen vaihdon takia vaihdetaan kenttien tekstejä. SELO on pitkä peli ja PELO on pikashakki.
        // Esim. "Oma SELO" ja "Oma PELO".
        public enum VaihdaMiettimisaika_enum { 
            VAIHDA_SELOKSI,
            VAIHDA_PELOKSI
        };
    }
}

