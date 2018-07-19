//
// Luokka ottelujen tallentamiseen
//
// Public:
//      LisaaOttelunTulos
//      Lukumaara
//      Keskivahvuus
//      HaeEnsimmainen
//      HaeSeuraava
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
//  19.7.2018  Piilotettu tallennettujen ottelujen lista -> private ... tallennetutOttelut ... ja private struct Ottelu
//             ja lisätty Lukumaara, Keskivahvuus, HaeEnsimmainen ja HaeSeuraava
//

using System;  // needed for Tuple
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Selolaskuri
{
    //
    // Ottelujen tietojen tallennus listaan, vastustajan vahvuusluku ja ottelun tulos
    //
    public class Ottelulista : IEnumerable
    {
        private struct Ottelu
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
        private IList<Ottelu> tallennetutOttelut = new List<Ottelu>();

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

        private int index;

        // Hae ottelut taulukkomuotoisesti
        public Tuple<int, Vakiot.OttelunTulos_enum> HaeEnsimmainen()
        {
            index = 0;
            if (index < Lukumaara)
                return Tuple.Create(tallennetutOttelut[index].vastustajanSelo, tallennetutOttelut[index].ottelunTulos);
            else
                return Tuple.Create(0, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        public Tuple<int, Vakiot.OttelunTulos_enum> HaeSeuraava()
        {
            index++;
            if (index < Lukumaara)
                return Tuple.Create(tallennetutOttelut[index].vastustajanSelo, tallennetutOttelut[index].ottelunTulos);
            else
                return Tuple.Create(0, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        public int Lukumaara {
            get {
                return tallennetutOttelut.Count;
            }
        }

        public int Keskivahvuus {
            get {
                return (int)System.Math.Round(tallennetutOttelut.Average(x => x.vastustajanSelo)); // Linq
            }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)tallennetutOttelut).GetEnumerator();
        }
    }
}