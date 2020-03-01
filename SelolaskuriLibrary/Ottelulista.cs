//
// Store the played matches (one or many) and the needed routines
//
// Public:
//      LisaaOttelunTulos  - adds a new match with opponent selo and the result (lost/draw/won)
//      Lukumaara          - returns number of stored matches
//      Keskivahvuus       - returns average opponent strength / selo strength
//      HaeEnsimmainen     - returns the first match
//      HaeSeuraava        - returns the next match
//
// 2.4.2018 Ismo Suihko
//
// Created in Syotetiedot. Ottelut = new Ottelulista();
// Data is stored in SelolaskuriOperations
// Data is fetched for calculations in Selolaskuri PelaaKaikkiOttelut().
// 
// Public:
//  LisaaOttelunTulos() - store a new match
//  HaeEnsimmainen()    - return the first match
//  HaeSeuraava()       - return the next match
//  Lukumaara           - number of matches
//  Keskivahvuus        - average strength of the opponents
// 
// Modifications:
//   19.7.2018  Data is hidden, added routines Lukumaara, Keskivahvuus, HaeEnsimmainen ja HaeSeuraava
//

using System;  // needed for Tuple
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SelolaskuriLibrary {
    //
    // Ottelujen tietojen tallennus listaan, vastustajan vahvuusluku ja ottelun tulos
    //
    public class Ottelulista : IEnumerable // Should end in 'Collection'
    {
        private struct Ottelu
        {
            public int VastustajanSelo { get; }
            public Vakiot.OttelunTulos_enum OttelunTulos { get; }

            public Ottelu(int selo, Vakiot.OttelunTulos_enum tulos)  // tallentaa
            {
                VastustajanSelo = selo;
                OttelunTulos = tulos;
            }
        }

        // Lista ottelutuloksille, jossa vastustajan selo sekä ottelun tulos
        // Pitää voida lisätä alkioita (Add), käydä ne läpi alkuperäisessä järjestyksessä,
        // hakea lukumäärä (Count) sekä laskea alkioista keskiarvo (Average(x => x.vastustajanSelo))
        private IList<Ottelu> tallennetutOttelut = new List<Ottelu>();

        // Ottelutuloksen (vastustaja ja tulos) lisääminen listaan
        public void LisaaOttelunTulos(int vastustajanSelo, Vakiot.OttelunTulos_enum ottelunTulos)
        {
            Ottelu ottelu = new Ottelu(vastustajanSelo, ottelunTulos);
            tallennetutOttelut.Add(ottelu);
        }

        private int _index;

        private Tuple<int, Vakiot.OttelunTulos_enum> HaeOttelu(int index)
        {
            if (index < Lukumaara)
                return Tuple.Create(tallennetutOttelut[index].VastustajanSelo, tallennetutOttelut[index].OttelunTulos);
            else
                return Tuple.Create(0, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        // Returns the first match  HaeEnsimmainen()
        // Returns the next match   HaeSeuraava()
        // If no more matchess left, returns TULOS_MAARITTELEMATON
        public Tuple<int, Vakiot.OttelunTulos_enum> HaeEnsimmainen()
        {
            _index = 0;
            return HaeOttelu(_index);
        }

        public Tuple<int, Vakiot.OttelunTulos_enum> HaeSeuraava()
        {
            _index++;
            return HaeOttelu(_index);
        }

        public int Lukumaara {
            get {
                return tallennetutOttelut.Count;
            }
        }

        public int Keskivahvuus {
            get {
                // Found out that Math.Round(1568.5) was 1568 but should had been 1569 like in Java.
                // ...so adding 0.01 to get correct result...
                // https://stackoverflow.com/questions/977796/why-does-math-round2-5-return-2-instead-of-3
                // XXX: Need to check also other Math.Round() usages?
                return (int)Math.Round(tallennetutOttelut.Average(x => x.VastustajanSelo) + 0.01); // Linq
            }
        }

        //public int MaxVahvuus {
        //    get {
        //        return tallennetutOttelut.Max(t => t.VastustajanSelo);
        //    }
        //}

        //public int MinVahvuus {
        //    get {
        //        return tallennetutOttelut.Min(t => t.VastustajanSelo);
        //    }
        //}

        public int Keskivahvuus10x {
            get {
                return (int)Math.Round(tallennetutOttelut.Average(x => 10 * x.VastustajanSelo) + 0.01); // Linq
            }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)tallennetutOttelut).GetEnumerator();
        }
    }
}