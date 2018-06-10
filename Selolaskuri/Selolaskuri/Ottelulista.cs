//
// Luokka ottelujen tallentamiseen
//
// Luotu 2.4.2018 Ismo Suihko
//

using System.Collections;
using System.Collections.Generic;


namespace Selolaskuri
{
    //
    // Ottelujen tietojen tallennus listaan, vastustajan vahvuusluku ja ottelun tulos
    //
    public class Ottelulista : IEnumerable
    {
        public struct Ottelu
        {
            public Ottelu(int selo, Vakiot.OttelunTulos_enum tulos)  // tallentaa
            {
                vastustajanSelo = selo;
                ottelunTulos = tulos;
                //ensimmainen = true;
            }

            public int vastustajanSelo { get; }
            public Vakiot.OttelunTulos_enum ottelunTulos { get; }
            //private bool ensimmainen;
        }

        // Lista vastustajien tiedoille ja ottelutuloksille
        public IList<Ottelu> tallennetutOttelut = new List<Ottelu>();

        // Listan tyhjennys ennen kuin siihen tallennetaan uusia otteluita
        public void Tyhjenna()
        {
            tallennetutOttelut.Clear();
        }

        // Ottelutuloksen (vastustaja ja tulos) lisääminen listaan
        public void LisaaOttelunTulos(int vastustajanSelo, Vakiot.OttelunTulos_enum ottelunTulos)
        {
            Ottelu ottelu = new Ottelu(vastustajanSelo, ottelunTulos);
            tallennetutOttelut.Add(ottelu);
            ///*DEBUG*/
            //System.Windows.Forms.MessageBox.Show("Lisää vastustaja " + vastustajanSelo + " tulos " + ottelunTulos + " otteluita " + vastustajienLukumaara);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)tallennetutOttelut).GetEnumerator();
        }

        //
        // Kun pelaajat on syötetty listaan, niin tämä on sama kuin turnauksen_vastustajien_lkm
        // Mutta yhden vastustajan tapauksessa tämä on nolla, koska ei ole käytetty listaa!
        public int vastustajienLukumaara {
            get {
                return tallennetutOttelut.Count;
            }
        }
    }
}