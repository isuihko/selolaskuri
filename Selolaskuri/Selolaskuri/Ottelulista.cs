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

        // Ottelutuloksen (vastustaja ja tulos) lisääminen listaan
        public void LisaaOttelunTulos(int vastustajanSelo, Vakiot.OttelunTulos_enum ottelunTulos)
        {
            Ottelu ottelu = new Ottelu(vastustajanSelo, ottelunTulos);
            tallennetutOttelut.Add(ottelu);
        }

        private int index;

        // Returns the first match  HaeEnsimmainen()
        // Returns the next match   HaeSeuraava()
        // If no more matchess left, returns TULOS_MAARITTELEMATON
        public Tuple<int, Vakiot.OttelunTulos_enum> HaeEnsimmainen()
        {
            index = 0;
            // Same code as in HaeSeuraava()
            if (index < Lukumaara)
                return Tuple.Create(tallennetutOttelut[index].vastustajanSelo, tallennetutOttelut[index].ottelunTulos);
            else
                return Tuple.Create(0, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        public Tuple<int, Vakiot.OttelunTulos_enum> HaeSeuraava()
        {
            index++;
            // Same code as in HaeEnsimmainen()
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