using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selolaskuri
{
    // Tietorakenne syötetiedoille
    //
    // Tähän talteen annetut syötetiedot, jotka välitetään SeloPelaaja-luokkaan
    // XXX: Mutta syötteenä annettua merkkijonoa ei tallenneta tähän,
    // XXX: vaan se jaetaan yksittäisiin otteluihin ja on listassa SeloPelaaja-luokassa.
    //
    public class Syotetiedot
    {
        public int nykyinenSelo;
        public int nykyinenPelimaara;
        public int vastustajanSelo;
        public int ottelunTulos;
        public int miettimisaika;

        public string vastustajienSelot_str;

        public Syotetiedot()
        {
            nykyinenSelo = 0;
            nykyinenPelimaara = 0;
            vastustajanSelo = 0;
            ottelunTulos = 0;
            miettimisaika = 0;
            vastustajienSelot_str = null;
        }
    }
}
