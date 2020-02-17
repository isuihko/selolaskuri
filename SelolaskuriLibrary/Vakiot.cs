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
//   4.9.2018       enum Selolaskuri Winforms, WPF_XAML and XBAP. Needed in FormOperations.
//   16.-17.2.2020  added new error status for new + old player calculation in one go format,
//                  error messages defined as constants -> same for each version
//    

namespace SelolaskuriLibrary {

    public class Vakiot {

        public enum Selolaskuri_enum {
            WINFORMS,
            WPF_XAML,
            XBAP_WEB             
        }

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
        public const int SYOTE_VIRHE_MIETTIMISAIKA_CSV  = -1;
        public const int SYOTE_VIRHE_OMA_SELO           = -2;
        public const int SYOTE_VIRHE_VASTUSTAJAN_SELO   = -3;
        public const int SYOTE_VIRHE_PELIMAARA          = -4;
        public const int SYOTE_VIRHE_BUTTON_TULOS       = -5;
        public const int SYOTE_VIRHE_YKSITTAINEN_TULOS  = -6;
        public const int SYOTE_VIRHE_TURNAUKSEN_TULOS   = -7;
        public const int SYOTE_VIRHE_CSV_FORMAT         = -8;
        public const int SYOTE_VIRHE_UUDEN_PELAAJAN_OTTELUT_ENINT_10 = -9;
        public const int SYOTE_VIRHE_UUDEN_PELAAJAN_OTTELUT_VAHINT_11 = -10;
        public const int SYOTE_VIRHE_UUDEN_PELAAJAN_OTTELUT_KAKSI_KAUTTAMERKKIA = -11;

        // Edellä oleville negatiivisille virhestatuksille virheilmoitustekstit
        // HUOM! Oltava samassa numerojärjestyksessä kuin yllä
        public static string[] SYOTE_VIRHEET_text = new string[] 
        {
            "OK", // ensin indeksi 0
            "VIRHE: CSV-formaatissa annettu virheellinen miettimisaika. Annettava minuutit. Ks. Menu->Ohjeita",
            $"VIRHE: Nykyisen SELOn oltava numero {Vakiot.MIN_SELO}-{Vakiot.MAX_SELO}.",
            $"VIRHE: Vastustajan vahvuusluvun on oltava numero {Vakiot.MIN_SELO}-{Vakiot.MAX_SELO}.",
            $"VIRHE: pelimäärän voi olla numero väliltä {Vakiot.MIN_PELIMAARA}-{Vakiot.MAX_PELIMAARA} tai tyhjä.",
            "Ottelun tulosta ei valittu!",
            "VIRHE: Yksittäisen ottelun tulos voidaan antaa merkeillä +(voitto), =(tasapeli) tai -(tappio), esim. +1720. Tasapeli voidaan antaa muodossa =1720 ja 1720.",
            "VIRHE: Turnauksen pistemäärä voi olla enintään sama kuin vastustajien lukumäärä.",
            "VIRHE: CSV-formaattivirhe, ks. Menu->Ohjeita",
            "VIRHE: Uuden pelaajan laskenta / normaali laskenta, alkup. pelimäärä voi olla enintään 10, ks. Menu->Ohjeita",
            "VIRHE: Uuden pelaajan laskenta / normaali laskenta, ei riittävästi uuden pelaajan pelejä, ks. Menu->Ohjeita",
            "VIRHE: Uuden pelaajan laskenta / normaali laskenta, voi olla vain yksi '/', ks. Menu->Ohjeita"
        };
        public static int SYOTE_VIRHE_MAX = -1 * SYOTE_VIRHEET_text.Length; // ei voinut olla const


        public const int PELIMAARA_TYHJA = -1; // OK, muilla kuin uusilla pelaajilla voi olla tyhjä

        public const int LEIKEKIRJA_MAX_RIVINPITUUS     = 1000;
        public const int LEIKEKIRJA_MAX_RIVIMAARA       = 100;

        public const int TURNAUKSEN_TULOS_ANTAMATTA     = -1;
        // käytännössä maksimi on pelaajien lkm, koska ei voida saada enempää pisteitä kuin on otteluita
        // Jos on pelattu kahdesti samaa pelaajaa vastaan, niin ne annetaan erillisinä otteluina
        public const float TURNAUKSEN_TULOS_MAX         = 199.5F;  


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

