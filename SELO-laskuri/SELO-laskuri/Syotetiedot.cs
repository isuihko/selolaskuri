using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selolaskuri
{
    // Luokka/tietorakenne syötetiedoille
    //
    // Tähän talteen annetut syötetiedot, jotka välitetään SeloPelaaja-luokkaan
    //
    // Tarkoitus on, että tätä luokkaa/tietorakennetta käytetään automaattissa testauksessa!
    // Luokan constructor voi siksi muuttua vielä.
    //
    public class Syotetiedot
    {
        public Vakiot.Miettimisaika_enum miettimisaika;

        public int nykyinenSelo { get; set; }
        public int nykyinenPelimaara { get; set; }

        public int vastustajanSelo { get; set; }
        public Vakiot.OttelunTulos_enum ottelunTulos { get; set; }

        public string vastustajienSelot_str { get; set; }  // vastustajan/vastustajien tiedot ja tulokset

        public Syotetiedot()
        {
            miettimisaika = 0;
            nykyinenSelo = 0;
            nykyinenPelimaara = 0;
            vastustajanSelo = 0;
            ottelunTulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
            vastustajienSelot_str = null;
        }
    }
}
