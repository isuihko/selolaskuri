using System;
using System.Collections;
//using System.Collections.IENumerable;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Selolaskuri
{
    // Ottelujen tietojen tallennus, vastustajan vahvuusluku ja ottelun tulos
    public class Ottelulista : IEnumerable
    {
        public struct Ottelu
        {
            public Ottelu(int selo, int tulos)  // tallentaa
            {
                vastustajanSelo = selo;
                ottelunTulos    = tulos;
                //ensimmainen = true;
            }

            public int vastustajanSelo { get; }
            public int ottelunTulos    { get; }
            //private bool ensimmainen;
        }

        // Lista vastustajien tiedoille ja ottelutuloksille
        public IList<Ottelu> ottelulista = new List<Ottelu>();

        // Listan tyhjennys ennen kuin siihen tallennetaan uusia otteluita
        public void Tyhjenna()
        {
            ottelulista.Clear();
        }

        // Ottelutuloksen (vastustaja ja tulos) lisääminen listaan
        public void LisaaOttelunTulos(int vastustajanSelo, int ottelunTulos)
        {
            Ottelu ottelu = new Ottelu(vastustajanSelo, ottelunTulos);
            ottelulista.Add(ottelu);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)ottelulista).GetEnumerator();
        }


        // FUNKTIO: get_vastustajien_lkm_listassa()
        //
        // Kun pelaajat on syötetty listaan, niin tämä on sama kuin turnauksen_vastustajien_lkm
        // Mutta yhden vastustajan tapauksessa tämä on nolla, koska ei ole käytetty listaa!
        public int vastustajienLukumaara {
            get {
                return ottelulista.Count;
            }
        }
    }
}
