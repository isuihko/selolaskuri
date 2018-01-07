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
        public const int MIN_PELIMAARA = 0;
        public const int MAX_PELIMAARA = 9999;

        // input-kentän syötteen maksimipituus. Tarkistetaan virhetilanteissa
        // ja jos merkkejä yli tuon, niin tyhjennetään kenttä
        public const int MAX_PITUUS = 5;
        public const int SELO_PITUUS = 4;
    }
}
