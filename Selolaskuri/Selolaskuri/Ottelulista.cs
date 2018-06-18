//
// Luokka ottelujen tallentamiseen
//
// Public:
//      LisaaOttelunTulos
//      HaeVastustajienLukumaara
//
// Luotu 2.4.2018 Ismo Suihko
//
// Sisältyy luokkaan Syotetiedot
// Alustus: SelolaskuriOperations: TarkistaSyote() ja TarkistaVastustajanSelo()
// Käyttö:  SelolaskuriOperations: SuoritaLaskenta()
//          SeloPelaaja: PelaaKaikkiOttelut()
// 
// Muutokset:
//  18.6.2018  Listan tyhjennystä ei tarvita erikseen. Tyhjä luonnin jälkeen, new Syotetiedot()
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
            public int vastustajanSelo { get; }
            public Vakiot.OttelunTulos_enum ottelunTulos { get; }

            public Ottelu(int selo, Vakiot.OttelunTulos_enum tulos)  // tallentaa
            {
                vastustajanSelo = selo;
                ottelunTulos = tulos;
            }
        }

        // Lista ottelutuloksille, jossa vastustajan selo sekä ottelun tulos
        // Pitää voida lisätä alkioita (Add), käydä ne läpi alkuperäisessä järjestyksessä,
        // hakea lukumäärä (Count) sekä laskea alkioista keskiarvo (Average(x => x.vastustajanSelo))
        public IList<Ottelu> tallennetutOttelut = new List<Ottelu>();

        //// Listan tyhjennys ennen kuin siihen tallennetaan uusia otteluita
        // Ei tarvitakaan, koska lista on tyhjennetty jo luotaessa syotetietoja, new Syotetiedot()
        //public void Tyhjenna()
        //{
        //    tallennetutOttelut.Clear();
        //}

        // Ottelutuloksen (vastustaja ja tulos) lisääminen listaan
        public void LisaaOttelunTulos(int vastustajanSelo, Vakiot.OttelunTulos_enum ottelunTulos)
        {
            Ottelu ottelu = new Ottelu(vastustajanSelo, ottelunTulos);
            tallennetutOttelut.Add(ottelu);
        }

        public int HaeVastustajienLukumaara {
            get {
                return tallennetutOttelut.Count;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)tallennetutOttelut).GetEnumerator();
        }
    }
}