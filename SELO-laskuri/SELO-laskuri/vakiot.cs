using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selolaskuri
{
    static class vakiot
    {
        // YLEISET VAKIOT, joilla määrätään syötteen rajat
        // Käytetään pääasiassa tarkista_input() -rutiinissa
        public const int MIN_SELO = 1000;
        public const int MAX_SELO = 2999;
        public const int MIN_PELIMAARA    = 0;
        public const int MAX_PELIMAARA    = 9999;
        public const int MAX_UUSI_PELAAJA = 10;

        // syötteen tarkastuksessa käytetyt statukset, eivät ole syötteiden arvoarvoalueella
        public const int VIRHE_SELO              = -1;
        public const int VIRHE_PELIMAARA         = -2;
        public const int VIRHE_TULOS             = -3;
        public const int VIRHE_YKSITTAINEN_TULOS = -4;
        public const int VIRHE_TURNAUKSEN_TULOS  = -5;
        public const int PELIMAARA_TYHJA         = -1;

        // input-kentän syötteen maksimipituus. Tarkistetaan virhetilanteissa
        // ja jos merkkejä yli tuon, niin tyhjennetään kenttä
        public const int MAX_PITUUS  = 5;
        public const int SELO_PITUUS = 4;

        // voisi käyttää myös enum
        public const int VOITTOx2   = 2;
        public const int TASAPELIx2 = 1;
        public const int TAPPIOx2   = 0;

        // Miettimisajat (numeroilla ei merkitystä, kunhan menevät miettimisajan mukaan suuruusjärjestyksessä)
        public const int Miettimisaika_vah90min   = 90;
        public const int Miettimisaika_60_89min   = 60;
        public const int Miettimisaika_11_59min   = 11;
        public const int Miettimisaika_enint10min = 10;
    }
}

