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
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest1
    {
        SelolaskuriOperations so = new SelolaskuriOperations();

        // --------------------------------------------------------------------------------
        // Laskennan testauksia erilaisin syöttein(oma selo, vastustajat, ottelutulokset, ...)
        // Virhestatuksien testauksia erilaisin virhein
        // --------------------------------------------------------------------------------

        [TestMethod]
        public void UudenPelaajanOttelutYksittain()
        {
            // Testataan uuden pelaajan vahvuusluvun muutokset ottelu kerrallaan.

            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            // Jos pitkä peli, niin jatkossa käytetään lyhyempää muotoa
            // var t = Testaa("1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            // jossa miettimisaika on oletuksena MIETTIMISAIKA_VAH_90MIN
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1725,   t.Item2.UusiSelo);                // uusi vahvuusluku
            Assert.AreEqual(1,      t.Item2.UusiPelimaara);              // uusi pelimäärä 0+1 = 1
            Assert.AreEqual(1 * 2,  t.Item2.TurnauksenTulos);        // tulos voitto 
            Assert.AreEqual(1525,   t.Item2.TurnauksenKeskivahvuus);  // keskivahvuus
            Assert.AreEqual(1,      t.Item2.VastustajienLkm);            // yksi vastustaja
            Assert.AreEqual(50,     t.Item2.Odotustulos);               // 0,50*100  odotustulos palautuu 100-kertaisena
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // yksi ottelu, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // yksi ottelu, sama kuin UusiSelo

            // Ja tästä eteenpäin käytetään edellisestä laskennasta saatuja UusiSelo ja UusiPelimaara
            t = Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1441", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1683,   t.Item2.UusiSelo);
            Assert.AreEqual(2,      t.Item2.UusiPelimaara);              // uusi pelimäärä 1+1 = 2
            Assert.AreEqual(1 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1441,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(84,     t.Item2.Odotustulos);               // 0,84*100

            t = Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1973", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1713,   t.Item2.UusiSelo);
            Assert.AreEqual(3,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos);
            Assert.AreEqual(1973,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(16,     t.Item2.Odotustulos);               // 0,16*100

            t = Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1718", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1764,   t.Item2.UusiSelo);
            Assert.AreEqual(4,      t.Item2.UusiPelimaara);
            Assert.AreEqual(1 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1718,   t.Item2.TurnauksenKeskivahvuus);

            t = Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1784", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1728,   t.Item2.UusiSelo);
            Assert.AreEqual(5,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos);
            Assert.AreEqual(1784,   t.Item2.TurnauksenKeskivahvuus);

            t = Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1660", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1683,   t.Item2.UusiSelo);
            Assert.AreEqual(6,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos);
            Assert.AreEqual(1660,   t.Item2.TurnauksenKeskivahvuus);

            t = Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1966", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos);
            Assert.AreEqual(1966,   t.Item2.TurnauksenKeskivahvuus);
        }

        // Calculation from the format ""+1525 +1441 -1973 +1718..." takes more time than from "3 1525 1441 1973 1718..."
        [TestMethod]
        public void UudenPelaajanOttelutKerralla1()
        {
            var t = Testaa("1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            // Jos pitkä peli ja tulos määrittelematon, niin jatkossa käytetään lyhyempää muotoa, jossa 
            // var t = Testaa("1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            // jossa miettimisaika on oletuksena MIETTIMISAIKA_VAH_90MIN ja tulos TULOS_MAARITTELEMATON
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7,      t.Item2.VastustajienLkm);
            Assert.AreEqual(199,    t.Item2.Odotustulos);          // odotustulos 1,99*100
            Assert.AreEqual(1683,   t.Item2.MinSelo);             // laskennan aikainen minimi
            Assert.AreEqual(1764,   t.Item2.MaxSelo);             // laskennan aikainen maksimi
        }

        [TestMethod]
        public void UudenPelaajanOttelutKerralla2()
        {
            var t = Testaa("1525", "0", "3 1525 1441 1973 1718 1784 1660 1966");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7,      t.Item2.VastustajienLkm);
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }

        // Tässä lasketaan samat ottelut kuin uudelle pelaajalle laskettiin, mutta vanhan pelaajan kaavalla (pelimäärä antamatta)
        [TestMethod]
        public void SamatOttelutKuinUudella1() // Turnauksen tulos lasketaan otteluista
        {
            var t = Testaa("1525", "+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1571,   t.Item2.UusiSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7,      t.Item2.VastustajienLkm);        // seitsemän vastustajaa
            Assert.AreEqual(199,    t.Item2.Odotustulos);          // odotustulos 1,99*100
            Assert.AreEqual(1548,   t.Item2.MinSelo);             // laskennan aikainen minimi
            Assert.AreEqual(1596,   t.Item2.MaxSelo);             // laskennan aikainen maksimi
        }

        // Tässä lasketaan samat ottelut kuin uudelle pelaajalle laskettiin (pelimäärä antamatta), turnauksen tulos pistemääränä ennen vastustajien vahvuuslukuja
        [TestMethod]
        public void SamatOttelutKuinUudella2() // Turnauksen tulos annettu numerona
        {
            var t = Testaa("1525", "3 1525 1441 1973 1718 1784 1660 1966");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1571,   t.Item2.UusiSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7,      t.Item2.VastustajienLkm);        // seitsemän vastustajaa
            Assert.AreEqual(199,    t.Item2.Odotustulos);          // odotustulos 1,99*100
        }

        // Kolme tapaa syöttää ottelun tulos
        [TestMethod]
        public void TulosPainikkeilla()
        {
            var t = Testaa("1800", "1900", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1823,   t.Item2.UusiSelo);
            Assert.AreEqual(36,     t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        [TestMethod]
        public void TulosSelossa()
        {
            var t = Testaa("1800", "+1900");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1823,   t.Item2.UusiSelo);
            Assert.AreEqual(36,     t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        [TestMethod]
        public void TulosNumeronaEnnenSeloa()
        {
            var t = Testaa("1800", "1.0 1900");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1823,   t.Item2.UusiSelo);
            Assert.AreEqual(36,     t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        // Merkkijonoissa ylimääräisiä välilyöntejä
        [TestMethod]
        public void UudenPelaajanOttelutValilyonteja()
        {
            var t = Testaa("    1525  ", "0  ", "     +1525  +1441           -1973 +1718    -1784 -1660     -1966   ");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
        }

        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksesta()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2033,   t.Item2.UusiSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual((int)(10.5F * 2), t.Item2.TurnauksenTulos);
            Assert.AreEqual(1827,   t.Item2.TurnauksenKeskivahvuus);  // 
            Assert.AreEqual(12,     t.Item2.VastustajienLkm);           // 12 vastustajaa eli ottelua
            Assert.AreEqual(840,    t.Item2.Odotustulos);              // odotustulos 8,40*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }

        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "75", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2033,   t.Item2.UusiSelo);
            Assert.AreEqual(87,     t.Item2.UusiPelimaara);             // 75 + 12 ottelua = 87
            Assert.AreEqual((int)(10.5F * 2), t.Item2.TurnauksenTulos);
            Assert.AreEqual(1827,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(12,     t.Item2.VastustajienLkm);
            Assert.AreEqual(840,    t.Item2.Odotustulos);              // odotustulos 8,40*100
        }

        // Tässä edellinen pikashakin laskenta pitkällä miettimisajalla, jolloin saadaan eri tulos 2050 (pikashakissa 2033)
        [TestMethod]
        public void ShakinVahvuuslukuTurnauksesta()
        {
            var t = Testaa("1996", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2050,   t.Item2.UusiSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
        }

        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void ShakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var t = Testaa("1996", "150", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2050,   t.Item2.UusiSelo);
            Assert.AreEqual(162,    t.Item2.UusiPelimaara);        // 150 + 12 ottelua  = 162
        }


        // --------------------------------------------------------------------------------
        // Testataan virheellisiä syötteitä, joista pitää saada virheen mukainen virhestatus
        // --------------------------------------------------------------------------------

        // Testataan virheellinen syöte, tässä virheellinen oma vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteOmaSELO()
        {
            var t = Testaa("15zz5", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO, t.Item1);
        }

        [TestMethod]
        public void VirheellinenSyoteOmaSELOtyhja()
        {
            var t = Testaa("", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO, t.Item1);
        }

        // Testataan virheellinen syöte, tässä virheellinen vastustajan vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteVastustajanSELO()
        {
            var t = Testaa("1525", "0", "c5sdffew25", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        [TestMethod]
        public void VirheellinenSyoteVastustajanSELOTyhja()
        {
            var t = Testaa("1525", "0", "", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }


        // Pelimäärä virheellinen, annettu liian suureksi
        [TestMethod]
        public void VirheellinenSyoteOmaPelimaara()
        {
            var t = Testaa("1525", "123456", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_PELIMAARA, t.Item1);
        }

        // Ei ole annettu ottelun tulosta valintanapeilla tappio, tasapeli tai voitto
        [TestMethod]
        public void VirheellinenSyoteEiTulosta()
        {
            var t = Testaa("1525", "0", "1600", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_BUTTON_TULOS, t.Item1);
        }

        // Virheellinen yksittäinen tulos turnauksen tuloksissa. Oltava + (voitto), - (tappio) tai = (tasan).
        [TestMethod]
        public void VirheellinenSyoteTurnauksessaVirheellinenTulos()
        {
            var t = Testaa("1525", "0", "+1525 +1441 -1973 +1718 /1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_YKSITTAINEN_TULOS, t.Item1);
        }

        // Annettu isompi pistemäärä (20) kuin mitä on otteluita (12 kpl)
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos1()
        {
            var t = Testaa("1996", "20 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS, t.Item1);
        }

        // Annettu isompi pistemäärä (99) kuin mitä on otteluita (12 kpl)
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos2()
        {
            var t = Testaa("1996", "99 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS, t.Item1);
        }

        // Annettu negatiivinen pistemäärä.
        // Palautuu ilmoituksena virheellisestä vastustajan selosta, kun ensimmäinen 
        // luku käsitellään numerona eikä tarkistuksessa voida tietää, kumpaa on tarkoitettu.
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos3()
        {
            var t = Testaa("1996", "-6 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        // Annettu isompi pistemäärä (150) kuin mitä on otteluita (12 kpl)
        // Palautuu ilmoituksena virheellisestä vastustajan selosta, kun ensimmäinen 
        // luku käsitellään numerona eikä tarkistuksessa voida tietää, kumpaa on tarkoitettu.
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos4()
        {
            var t = Testaa("1996", "150 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        [TestMethod]
        public void CSV_UudenPelaajanOttelutYksittain1()
        {
            var t = Testaa("90,1525,0,1525,1");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1725,   t.Item2.UusiSelo);                // uusi vahvuusluku
            Assert.AreEqual(1,      t.Item2.UusiPelimaara);              // uusi pelimäärä 0+1 = 1
            Assert.AreEqual(1 * 2,  t.Item2.TurnauksenTulos);        // tulos voitto 
            Assert.AreEqual(1525,   t.Item2.TurnauksenKeskivahvuus);  // keskivahvuus
            Assert.AreEqual(1,      t.Item2.VastustajienLkm);            // yksi vastustaja
            Assert.AreEqual(50,     t.Item2.Odotustulos);               // 0,50*100  odotustulos palautuu 100-kertaisena
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // yksi ottelu, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // yksi ottelu, sama kuin UusiSelo           
        }

        [TestMethod]
        public void CSV_UudenPelaajanOttelutKerralla1()
        {
            var t = Testaa("90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7,      t.Item2.VastustajienLkm);
            Assert.AreEqual(199,    t.Item2.Odotustulos);         // odotustulos 1,99*100
            Assert.AreEqual(1683,   t.Item2.MinSelo);             // laskennan aikainen minimi
            Assert.AreEqual(1764,   t.Item2.MaxSelo);             // laskennan aikainen maksimi
        }

        [TestMethod]
        public void CSV_UudenPelaajanOttelutKerralla2()
        {
            var t = Testaa("90,1525,0,3 1525 1441 1973 1718 1784 1660 1966");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7,      t.Item2.VastustajienLkm);
            // Assert.AreEqual(199, t.Item2.Odotustulos);           // ei lasketa uudelle pelaajalle tästä formaatista
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }

        [TestMethod]
        public void CSV_PikashakinVahvuuslukuTurnauksesta()
        {
            var t = Testaa("5,1996,,10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2033,   t.Item2.UusiSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual((int)(10.5F * 2), t.Item2.TurnauksenTulos);
            Assert.AreEqual(1827,   t.Item2.TurnauksenKeskivahvuus);  // 
            Assert.AreEqual(12,     t.Item2.VastustajienLkm);           // 12 vastustajaa eli ottelua
            Assert.AreEqual(840,    t.Item2.Odotustulos);              // odotustulos 8,40*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }


        // --------------------------------------------------------------------------------
        // Testauksen apurutiinit
        //
        // Näissä kaikissa on string selo ja string vastustajat
        // Muiden parametrien puuttumisen varalta on useita versioita
        // --------------------------------------------------------------------------------

        // Use old Tuple, because Visual Studio Community 2015 has older C#
        private Tuple<int, Selopelaaja> Testaa(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
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
        private Tuple<int, Selopelaaja> Testaa(string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, selo, pelimaara, vastustajat, tulos);
        }

        // Jos aikaa, pelimäärää ei annettu, oletuksena 90 minuuttia ja ""
        private Tuple<int, Selopelaaja> Testaa(string selo, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, selo, "", vastustajat, tulos);
        }

        // Jos aikaa ja yksittäistä tulosta ei annettu, oletus 90 minuuttia ja TULOS_MAARITTELEMATON
        private Tuple<int, Selopelaaja> Testaa(string selo, string pelimaara, string vastustajat)
        {
            return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, selo, pelimaara, vastustajat, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        // Jos pelimäärää ja tulosta ei annettu, oletus "" ja TULOS_MAARITTELEMATON
        private Tuple<int, Selopelaaja> Testaa(Vakiot.Miettimisaika_enum aika, string selo, string vastustajat)
        {
            return Testaa(aika, selo, "", vastustajat, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        // Jos tulosta ei annettu, oletus TULOS_MAARITTELEMATON
        private Tuple<int, Selopelaaja> Testaa(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat)
        {
            return Testaa(aika, selo, pelimaara, vastustajat, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        // Jos aikaa, pelimäärää ja yksittäistä tulosta ei annettu, oletus 90 minuuttia, "" ja TULOS_MAARITTELEMATON
        private Tuple<int, Selopelaaja> Testaa(string selo, string vastustajat)
        {
            return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, selo, "", vastustajat, Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
        }

        // Use old Tuple, because Visual Studio Community 2015 has older C#
        private Tuple<int, Selopelaaja> Testaa(string csv)
        {
            List<string> data = csv.Split(',').ToList();
            if (data.Count == 5) {
                return Testaa(so.SelvitaMiettimisaika(data[0]), data[1], data[2], data[3], so.SelvitaTulos(data[4]));
            } else if (data.Count == 4) {
                return Testaa(so.SelvitaMiettimisaika(data[0]), data[1], data[2], data[3], Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            } else if (data.Count == 3) {
                return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, data[0], data[1], data[2], Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            } else if (data.Count == 2) {
                return Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, data[0], "", data[1], Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            } else {
                Assert.AreEqual(5, data.Count); // -> Illegal CSV
                return null;
            }
        }
    }
}
