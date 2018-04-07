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
        public Vakiot.Miettimisaika_enum miettimisaika;

        public int nykyinenSelo;
        public int nykyinenPelimaara;

        public int vastustajanSelo;
        public Vakiot.OttelunTulos_enum ottelunTulos;
        
        public string vastustajienSelot_str;  // vastustajan/vastustajien tiedot ja tulokset

        public Syotetiedot()
        {
            miettimisaika = 0;
            nykyinenSelo = 0;
            nykyinenPelimaara = 0;
            vastustajanSelo = 0;
            ottelunTulos = Vakiot.OttelunTulos_enum.MAARITTELEMATON;
            vastustajienSelot_str = null;
        }
    }
}
