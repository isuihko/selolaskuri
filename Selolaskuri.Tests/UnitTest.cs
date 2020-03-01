//
// Unit testing of Selolaskuri's parameter checking and calculation routines
//
// 10.6.2018 Ismo Suihko
//
// Now with this it is easy to check the checking of input and calculating results.
//
// You can check from the results
//      1) new SELO
//      2) new game count
//      3) amount of points won from the given opponents (1/2 from draw, 1 from win). Stored as double so can use integer.
//      4) average strengt of the opponents
//      5) number of opponents
//      6) expected results (odotustulos)
//      7) minumum selo during calculations (if not possible to calculate, then same as new selo)
//      7) maximum selo during calculations (if not possible to calculate, then same as new selo)
//
//
// The results were compared and can be compared to the other calcuating programs
//      http://shakki.kivij.info/performance_calculator.shtml
//      http://www.shakki.net/kerhot/salsk/ohjelmat/selo.html
// and also chess tournament results.
//
// Occasionally there can be one point difference to official calcuations, maybe caused by rounding.
//
// Uses:  Assert.AreEqual(expected, actual)
// 
// Modifications:
//   11.6.2018  Järjestetty aiempia testitapauksia ja lisätty uusia
//   15.6.2018  Lisätty tarkistettavia tietoja: pistemäärä ja keskivahvuus
//   17.6.2018  Lisätty tarkistettavia tietoja: vastustajien lkm, Odotustulos
//              Odotustulosta ei näytetä uuden pelaajan tuloksissa, mutta sekin on laskettu ja voidaan tarkistaa
//              Myös lisätty testitapauksia (turnauksen tuloksen virheet). Nyt niitä on 22 kpl eli aika kattavasti.
//   18.6.2018  Tarkistettu näkyvyyttä -> private Testaa()
//   18.7.2018  Selkeytetty
//   23.7.2018  Muutettu Testaa()-rutiinin paluuarvo: <syötteen tarkistuksen status, tulokset>
//              Tällöin ylemmällä tasolla voidaan viitata suoraan tulostietorakenteen kenttiin
//              Nyt voidaan tarkistaa myös MinSelo ja MaxSelo
//  2.8.2018    Kaksi testiä virhetilanteiden varalta: oma tai vastustajien selo-kenttä on tyhjä. Nyt 24 testiä.
//  4.8.2018    Lisätty testejä CSV-formaattia varten. Nyt 28 testiä.
//
//  14.8.2018   New Testaa routines for helping the testing with different number of parameters.
//              If thinking time not given, use default Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN
//              If single match result not given, use default Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON
//              If number of games not given, use default ""
//
//              Fixed parameter order of Assert.AreEqual... had used (actual, expected) but should be (expected, actual)...
//              Wrote an external program to read lines and swap text between braces.
//
//  15.8.2018   Divided into modules according to the tests. General testing routines are still in this UnitTest1.cs.
//              Tests are run in this order:
//              1. checks if input has been processed properly (new tests)
//              2. checks error statuses in input
//              3. calculation from the separately given thinking time, own selo, game count, opponents/matches, possible single match result
//              4. checking and calculation of CSV format, all match data in one string (opponents/matches field)
//
//              Additional tests for CSV format errors. Now there are 36 unit tests.
//
//              More tests for CSV format.
//              Also two test cases to check that the matches are stored in to the match list correctly (UnitTest1_SyotteenKasittely.cs).
//              Now there are 43 tests.
//
// 17.8.2018    New tests: test tournament result with allowed and too big value (SELO out of 1000-2999).
//              Checkin go result. Verify that if the value bigger than Vakiot.TURNAUKSEN_TULOS_MAX (199.5),
//              program checkes if it is allowed opponent's SELO.
//              Check if the calculation result gets below or above allowed SELO ranges (1000-2999).
//
//  1.3.2020    CSV format. Csv checking code is now in class Syotetiedot and 
//              couple of return statuses were changed in UnitTest4_TarkistaCSV.
//

using SelolaskuriLibrary;
using System;

namespace Selolaskuri.Tests
{
    public class UnitTest
    {
        private SelolaskuriOperations so = new SelolaskuriOperations();

        // --------------------------------------------------------------------------------
        // Parametrien tarkistuksen ja laskennan testauksen apurutiinit
        //
        // Näissä kaikissa on string selo ja string vastustajat
        // Muiden parametrien puuttumisen varalta on useita versioita
        // --------------------------------------------------------------------------------

        // Use old Tuple, because Visual Studio Community 2015 has older C#
        public Tuple<int, Selopelaaja> Testaa(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            Syotetiedot syotetiedot = new Syotetiedot(aika, selo, pelimaara, vastustajat, tulos, /*trim input strings*/true);
            int status;
            Selopelaaja tulokset = null;

            if ((status = so.TarkistaSyote(syotetiedot)) == Vakiot.SYOTE_STATUS_OK) {

                // If the input was OK, continue and calculate
                // If wasn't, then tulokset is left null and error status will be returned
                tulokset = so.SuoritaLaskenta(syotetiedot);
            }
            return Tuple.Create(status, tulokset);
        }

        // Jos aikaa ei annettu, oletus 90 minuuttia eli pitkä peli
        public Tuple<int, Selopelaaja> Testaa(string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, selo, pelimaara, vastustajat, tulos);
        }

        // Jos aikaa, pelimäärää ei annettu, oletuksena 90 minuuttia ja ""
        public Tuple<int, Selopelaaja> Testaa(string selo, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, selo, "", vastustajat, tulos);
        }

        // Jos aikaa ja yksittäistä tulosta ei annettu, oletus 90 minuuttia ja TULOS_MAARITTELEMATON
        public Tuple<int, Selopelaaja> Testaa(string selo, string pelimaara, string vastustajat)
        {
            return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, selo, pelimaara, vastustajat, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        // Jos pelimäärää ja tulosta ei annettu, oletus "" ja TULOS_MAARITTELEMATON
        public Tuple<int, Selopelaaja> Testaa(Vakiot.Miettimisaika_enum aika, string selo, string vastustajat)
        {
            return Testaa(aika, selo, "", vastustajat, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        // Jos tulosta ei annettu, oletus TULOS_MAARITTELEMATON
        public Tuple<int, Selopelaaja> Testaa(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat)
        {
            return Testaa(aika, selo, pelimaara, vastustajat, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        // Jos aikaa, pelimäärää ja yksittäistä tulosta ei annettu, oletus 90 minuuttia, "" ja TULOS_MAARITTELEMATON
        public Tuple<int, Selopelaaja> Testaa(string selo, string vastustajat)
        {
            return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, selo, "", vastustajat, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        // Use old Tuple, because Visual Studio Community 2015 has older C#
        public Tuple<int, Selopelaaja> Testaa(string csv)
        {
            return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "", "", csv, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }
    }
}
