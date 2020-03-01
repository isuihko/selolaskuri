
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelolaskuriLibrary;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest3_Laskenta
    {
        private UnitTest u = new UnitTest();

        // --------------------------------------------------------------------------------
        // Laskennan testauksia erilaisin syöttein(oma selo, vastustajat, ottelutulokset, ...)
        // --------------------------------------------------------------------------------

       [TestMethod]
       public void UudenPelaajanOttelutYksittain()
       {
            // Testataan uuden pelaajan vahvuusluvun muutokset ottelu kerrallaan.

            var t = u.Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            // Jos pitkä peli, niin jatkossa käytetään lyhyempää muotoa
            // var t = u.Testaa("1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            // jossa miettimisaika on oletuksena MIETTIMISAIKA_VAH_90MIN
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1725,   t.Item2.UusiSelo);                // uusi vahvuusluku
            Assert.AreEqual(1,      t.Item2.UusiPelimaara);              // uusi pelimäärä 0+1 = 1
            Assert.AreEqual(1 * 2,  t.Item2.TurnauksenTulos2x);        // tulos voitto 
            Assert.AreEqual(1525,   t.Item2.TurnauksenKeskivahvuus);  // keskivahvuus
            Assert.AreEqual(1,      t.Item2.VastustajienLkm);            // yksi vastustaja
            Assert.AreEqual(50,     t.Item2.Odotustulos);               // 0,50*100  odotustulos palautuu 100-kertaisena
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // yksi ottelu, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // yksi ottelu, sama kuin UusiSelo

            // Ja tästä eteenpäin käytetään edellisestä laskennasta saatuja UusiSelo ja UusiPelimaara
            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1441", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1683,   t.Item2.UusiSelo);
            Assert.AreEqual(2,      t.Item2.UusiPelimaara);              // uusi pelimäärä 1+1 = 2
            Assert.AreEqual(1 * 2,  t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1441,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(84,     t.Item2.Odotustulos);               // 0,84*100

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1973", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1713,   t.Item2.UusiSelo);
            Assert.AreEqual(3,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1973,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(16,     t.Item2.Odotustulos);               // 0,16*100

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1718", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1764,   t.Item2.UusiSelo);
            Assert.AreEqual(4,      t.Item2.UusiPelimaara);
            Assert.AreEqual(1 * 2,  t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1718,   t.Item2.TurnauksenKeskivahvuus);

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1784", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1728,   t.Item2.UusiSelo);
            Assert.AreEqual(5,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1784,   t.Item2.TurnauksenKeskivahvuus);

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1660", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1683,   t.Item2.UusiSelo);
            Assert.AreEqual(6,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1660,   t.Item2.TurnauksenKeskivahvuus);

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1966", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1966,   t.Item2.TurnauksenKeskivahvuus);

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1321", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1673, t.Item2.UusiSelo);
            Assert.AreEqual(8, t.Item2.UusiPelimaara);
            Assert.AreEqual(1 * 2, t.Item2.TurnauksenTulos2x);   // voitto (1 piste) mutta kaksinkertaisena
            Assert.AreEqual(1321, t.Item2.TurnauksenKeskivahvuus);
        }

        // Calculation from the format ""+1525 +1441 -1973 +1718..." takes more time than from "3 1525 1441 1973 1718..."
        [TestMethod]
        public void UudenPelaajanOttelutKerralla1()
        {
            var t = u.Testaa("1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            // Jos pitkä peli ja tulos määrittelematon, niin jatkossa käytetään lyhyempää muotoa, jossa 
            // var t = u.Testaa("1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            // jossa miettimisaika on oletuksena MIETTIMISAIKA_VAH_90MIN ja tulos TULOS_MAARITTELEMATON
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7,      t.Item2.VastustajienLkm);
            Assert.AreEqual(199,    t.Item2.Odotustulos);          // odotustulos 1,99*100
            Assert.AreEqual(1683,   t.Item2.MinSelo);             // laskennan aikainen minimi
            Assert.AreEqual(1764,   t.Item2.MaxSelo);             // laskennan aikainen maksimi
        }

        // Calculation from the format ""+1525 +1441 -1973 +1718..." takes more time than from "3 1525 1441 1973 1718..."
        [TestMethod]
        public void UudenPelaajanOttelutKerralla1b()
        {
            var t = u.Testaa("1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966 +1321");
            // Jos pitkä peli ja tulos määrittelematon, niin jatkossa käytetään lyhyempää muotoa, jossa 
            // var t = u.Testaa("1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            // jossa miettimisaika on oletuksena MIETTIMISAIKA_VAH_90MIN ja tulos TULOS_MAARITTELEMATON
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1673, t.Item2.UusiSelo);
            Assert.AreEqual(8, t.Item2.UusiPelimaara);
            Assert.AreEqual(4 * 2, t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1674, t.Item2.TurnauksenKeskivahvuus);  // oikeasti 1673,5
            Assert.AreEqual(8, t.Item2.VastustajienLkm);
            Assert.AreEqual(275, t.Item2.Odotustulos);          // odotustulos 2,75*100
            Assert.AreEqual(1673, t.Item2.MinSelo);             // laskennan aikainen minimi
            Assert.AreEqual(1764, t.Item2.MaxSelo);             // laskennan aikainen maksimi
        }


        [TestMethod]
        public void UudenPelaajanOttelutKerralla2()
        {
            var t = u.Testaa("1525", "0", "3 1525 1441 1973 1718 1784 1660 1966");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7,      t.Item2.VastustajienLkm);
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }

        [TestMethod]
        public void UudenPelaajanOttelutKerralla2b()
        {
            var t = u.Testaa("1525", "0", "4 1525 1441 1973 1718 1784 1660 1966 1321");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1674, t.Item2.UusiSelo);  // XXX: Virallisessa laskennassa on 1673 mutta tällä kaavalla tulos on 1673,5 -> 1674
            Assert.AreEqual(8, t.Item2.UusiPelimaara);
            Assert.AreEqual(4 * 2, t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1674, t.Item2.TurnauksenKeskivahvuus);  // oikeasti 1673,5
            Assert.AreEqual(8, t.Item2.VastustajienLkm);
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }


        // SEURAAVAT NELJÄ TESTIÄ (17.2.2020) 
        // - ensimmäinen laskee uuden pelaajan vahvuusluvun
        // - toinen jatkaa laskentaa normaalilla kaavalla edellisen tuloksesta, tulokset vahvuusluvuissa +-=
        // - kolmas laskee saman kuin toinen, mutta tulos on ensin ja sitten vahvuusluvut, eikä anneta pelimäärää
        // - neljäs (=uusi ominaisuus) laskee ensin uuden pelaajan kaavalla ja jatkaa normaalilla kaavalla
        // Lisäksi moduulissa UnitTest1_SyotteenKasittely.cs on testattu virhetilanteet
        [TestMethod]
        public void UudenPelaajanOttelutKerralla3()
        {
            var t = u.Testaa("1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966 +1321 -1678 -1864 -1944");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1660, t.Item2.UusiSelo); 
            Assert.AreEqual(11, t.Item2.UusiPelimaara);
            Assert.AreEqual(4 * 2, t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1716, t.Item2.TurnauksenKeskivahvuus);  // oikeasti 1715,8
            Assert.AreEqual(11, t.Item2.VastustajienLkm);
            Assert.AreEqual(1651, t.Item2.MinSelo);
            Assert.AreEqual(1764, t.Item2.MaxSelo);
        }

        [TestMethod]
        public void UudenPelaajanOttelujenJalkeenLisaa()
        {
            var t = u.Testaa("1660", "11", "-1995 +1695 -1930 1901");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1682, t.Item2.UusiSelo);   // XXX: Virallisessa laskennassa on 1683
            Assert.AreEqual(15, t.Item2.UusiPelimaara);
            Assert.AreEqual(1.5 * 2, t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1880, t.Item2.TurnauksenKeskivahvuus);  // oikeasti 1715,8
            Assert.AreEqual(4, t.Item2.VastustajienLkm);
            Assert.AreEqual(1655, t.Item2.MinSelo);
            Assert.AreEqual(1682, t.Item2.MaxSelo); // XXX: Virallisessa laskennassa on 1683
        }

        [TestMethod]
        public void UudenPelaajanOttelujenJalkeenLisaa2()
        {
            var t = u.Testaa("1660", "", "1,5 1995 1695 1930 1901");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1683, t.Item2.UusiSelo);   // näin saadaan sama kuin virallisessa laskennassa
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);
            Assert.AreEqual(1.5 * 2, t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1880, t.Item2.TurnauksenKeskivahvuus);  // oikeasti 1715,8
            Assert.AreEqual(4, t.Item2.VastustajienLkm);
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }


        // Nyt voidaan laskea uuden pelaajan vahvuusluku uuden pelaajan kaavalla ja
        // jatkaa sitten määrätystä kohdasta normaalilla laskennalla, kunhan uuden pelaajan kaaavalla tulee
        // vähintään 11 peliä ja alkupelimäärä on enintään 10, jotta voidaan käyttää uuden pelaajan kaavaa
        // Virhetilanteiden testaus -> UnitTest2_TarkistaSyote.cs
        [TestMethod]
        public void UudenPelaajanOttelutKerralla3jatkuuNormaali()
        {
            var t = u.Testaa("1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966 +1321 -1678 -1864 -1944 / -1995 +1695 -1930 1901");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1683, t.Item2.UusiSelo); 
            Assert.AreEqual(15, t.Item2.UusiPelimaara);
            Assert.AreEqual(5.5 * 2, t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1760, t.Item2.TurnauksenKeskivahvuus);  // oikeasti 1759,7
            Assert.AreEqual(15, t.Item2.VastustajienLkm);
            Assert.AreEqual(1651, t.Item2.MinSelo);
            Assert.AreEqual(1764, t.Item2.MaxSelo);
        }


        // Tässä lasketaan samat ottelut kuin uudelle pelaajalle laskettiin, mutta vanhan pelaajan kaavalla (pelimäärä antamatta)
        [TestMethod]
        public void SamatOttelutKuinUudella1() // Turnauksen tulos lasketaan otteluista
        {
            var t = u.Testaa("1525", "+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1571,   t.Item2.UusiSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos2x);
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
            var t = u.Testaa("1525", "3 1525 1441 1973 1718 1784 1660 1966");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1571,   t.Item2.UusiSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7,      t.Item2.VastustajienLkm);        // seitsemän vastustajaa
            Assert.AreEqual(199,    t.Item2.Odotustulos);          // odotustulos 1,99*100
        }


        // Kolme tapaa syöttää ottelun tulos
        [TestMethod]
        public void TulosPainikkeilla()
        {
            var t = u.Testaa("1800", "1900", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1823,   t.Item2.UusiSelo);
            Assert.AreEqual(36,     t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        [TestMethod]
        public void TulosSelossa()
        {
            var t = u.Testaa("1800", "+1900");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1823,   t.Item2.UusiSelo);
            Assert.AreEqual(36,     t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        // Seuraavan kahden laskennan tarkistus: http://shakki.kivij.info/performance_calculator.shtml
        [TestMethod]
        public void TulosNumeronaEnnenSeloaDesimPiste()
        {
            var t = u.Testaa("1800", "1.0 1900");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1823,   t.Item2.UusiSelo);
            Assert.AreEqual(36,     t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        [TestMethod]
        public void TulosNumeronaEnnenSeloaPilkku()
        {
            var t = u.Testaa("1800", "1,0 1900");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1823, t.Item2.UusiSelo);
            Assert.AreEqual(36, t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        // Seuraavan kahden laskennan tarkistus: http://shakki.kivij.info/performance_calculator.shtml
        [TestMethod]
        public void TulosNumeronaEnnenSeloaDesimPiste2()
        {
            var t = u.Testaa("1800", "0.5 1900");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1805, t.Item2.UusiSelo);
            Assert.AreEqual(36, t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        [TestMethod]
        public void TulosNumeronaEnnenSeloaPilkku2()
        {
            var t = u.Testaa("1800", "0,5 1900");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1805, t.Item2.UusiSelo);
            Assert.AreEqual(36, t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        [TestMethod]
        public void TulosNumeronaEnnenSeloaPuolikas()
        {
            var t = u.Testaa("1800", "½ 1900");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1805, t.Item2.UusiSelo);
            Assert.AreEqual(36, t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        // Merkkijonoissa ylimääräisiä välilyöntejä
        [TestMethod]
        public void UudenPelaajanOttelutValilyonteja()
        {
            var t = u.Testaa("    1525  ", "0  ", "     +1525  +1441           -1973 +1718    -1784 -1660     -1966   ");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
        }

        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksestaDesimPiste()
        {
            var t = u.Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2034,   t.Item2.UusiSelo);  // tarkista, oliko 2033 (yhden pisteen virhe laskennassa mahdollinen)
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual((int)(10.5F * 2), t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1827,   t.Item2.TurnauksenKeskivahvuus);  // 
            Assert.AreEqual(12,     t.Item2.VastustajienLkm);           // 12 vastustajaa eli ottelua
            Assert.AreEqual(840,    t.Item2.Odotustulos);              // odotustulos 8,40*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }

        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksestaPilkku()
        {
            var t = u.Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "10,5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2034, t.Item2.UusiSelo);  // tarkista, oliko 2033 (yhden pisteen virhe laskennassa mahdollinen)
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual((int)(10.5F * 2), t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1827, t.Item2.TurnauksenKeskivahvuus);  // 
            Assert.AreEqual(12, t.Item2.VastustajienLkm);           // 12 vastustajaa eli ottelua
            Assert.AreEqual(840, t.Item2.Odotustulos);              // odotustulos 8,40*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }

        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksestaPuolikas()
        {
            var t = u.Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "10½ 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2034, t.Item2.UusiSelo);  // tarkista, oliko 2033 (yhden pisteen virhe laskennassa mahdollinen)
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual((int)(10.5F * 2), t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1827, t.Item2.TurnauksenKeskivahvuus);  // 
            Assert.AreEqual(12, t.Item2.VastustajienLkm);           // 12 vastustajaa eli ottelua
            Assert.AreEqual(840, t.Item2.Odotustulos);              // odotustulos 8,40*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }

        // Esimerkki Joukkuepikashakin SM 2018 alkukilpailut, alkukilpailuryhmä C  4.8.2018, LauttSSK 1 pöytä 1
        // Kilpailuryhmä C: http://www.shakki.net/cgi-bin/selo?do=turnaus&turnaus_id=5068
        // Kaikki täsmää
        [TestMethod]
        public void PikashakinVahvuuslukuSMTurnauksesta1()
        {
            var t = u.Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "2180", "2054", "14.5 1914 2020 1869 2003 2019 1979 2131 2161 2179 2392 1590 1656 1732 1944 1767 1903 1984 2038 2083 2594 2324 1466 1758");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2148, t.Item2.UusiSelo);       // Saadaan sama tulos kuin shakkiliiton sivulla
            Assert.AreEqual(2077, t.Item2.UusiPelimaara);  // pelimäärä
            Assert.AreEqual((int)(14.5F * 2), t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1979, t.Item2.TurnauksenKeskivahvuus);  // Summa 45506 / 23 = 1978,521739 pyöristys 1979
            Assert.AreEqual(23, t.Item2.VastustajienLkm);           // 23 vastustajaa eli ottelua
            Assert.AreEqual(1624, t.Item2.Odotustulos);              // odotustulos 16,24*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }

        // Esimerkki Joukkuepikashakin SM 2018 alkukilpailut, alkukilpailuryhmä C  4.8.2018, LauttSSK 1 pöytä 4
        // Kilpailuryhmä C: http://www.shakki.net/cgi-bin/selo?do=turnaus&turnaus_id=5068
        // Kaikki täsmää
        [TestMethod]
        public void PikashakinVahvuuslukuSMTurnauksesta2()
        {
            var t = u.Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "2045", "1225", "19.5 1548 1560 1699 1737 1735 1880 1856 2019 2102 2177 1539 1531 1672 1592 1775 1842 1847 1905 1970 2308 1988 1454 1481");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2083, t.Item2.UusiSelo);       // 
            Assert.AreEqual(1248, t.Item2.UusiPelimaara);  // pelimäärä
            Assert.AreEqual((int)(19.5F * 2), t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1792, t.Item2.TurnauksenKeskivahvuus);  // Summa 41217 / 23 = 1792.043
            Assert.AreEqual(23, t.Item2.VastustajienLkm);           // 23 vastustajaa eli ottelua
            Assert.AreEqual(1740, t.Item2.Odotustulos);              // odotustulos 17,40*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }

        // Esimerkki Joukkuepikashakin SM 2018 sijoituskilpailut 5.8.2018, sijoitusryhmä C, LauttSSK 4 pöytä 4
        // Sijoitusryhmä 5: http://www.shakki.net/cgi-bin/selo?do=turnaus&turnaus_id=5068
        // Kaikki täsmää
        [TestMethod]
        public void PikashakinVahvuuslukuSMTurnauksesta3()
        {
            var t = u.Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1262", "11 1623 1591 1318 1560 1493 1417 1343 1493 1524 1227 1716 1490 1454 1479 1329 1429 1444 1289 1576 1445 1280");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1345, t.Item2.UusiSelo); 
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärä
            Assert.AreEqual((int)(11F * 2), t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1453, t.Item2.TurnauksenKeskivahvuus);  // Summa 30520 / 21 = 1453.333
            Assert.AreEqual(21, t.Item2.VastustajienLkm);           // 21 vastustajaa eli ottelua
            Assert.AreEqual(564, t.Item2.Odotustulos);              // odotustulos 5,64*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }



        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var t = u.Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "75", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2034, t.Item2.UusiSelo);  // tarkista, oliko 2033 (yhden pisteen virhe laskennassa mahdollinen)
            Assert.AreEqual(87,     t.Item2.UusiPelimaara);             // 75 + 12 ottelua = 87
            Assert.AreEqual((int)(10.5F * 2), t.Item2.TurnauksenTulos2x);
            Assert.AreEqual(1827,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(12,     t.Item2.VastustajienLkm);
            Assert.AreEqual(840,    t.Item2.Odotustulos);              // odotustulos 8,40*100
        }

        // Tässä edellinen pikashakin laskenta pitkällä miettimisajalla, jolloin saadaan eri tulos 2050 (pikashakissa 2033)
        [TestMethod]
        public void ShakinVahvuuslukuTurnauksesta()
        {
            var t = u.Testaa("1996", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2050,   t.Item2.UusiSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
        }

        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void ShakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var t = u.Testaa("1996", "150", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2050,   t.Item2.UusiSelo);
            Assert.AreEqual(162,    t.Item2.UusiPelimaara);        // 150 + 12 ottelua  = 162
        }

        // Testataan, että sallittu minimiselo (1000) käy syötteessä
        // Testataan, että tulos 0 on OK turnauksen tuloksena (pienin sallittu tulos).
        // Testataan, entä jos tulos menee alle minimiselon. Ei ongelmaa.
        [TestMethod]
        public void LaskettuVahvuuslukuAlleMinimin1()
        {
            var t = u.Testaa("1000", "0 1100");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(984, t.Item2.UusiSelo);
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }
        
        [TestMethod]
        public void LaskettuVahvuuslukuAlleMinimin2()
        {
            var t = u.Testaa("1000", "-1100 +1005 -1002");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(985, t.Item2.UusiSelo);
            Assert.AreEqual(984, t.Item2.MinSelo);      // laskennan aikainen minimi
            Assert.AreEqual(1007, t.Item2.MaxSelo);     // laskennan aikainen maksimi
        }

        // Testataan, että sallittu maksimiselo (2999) käy syötteessä
        // Testataan, entä jos tulos menee yli maksimiselon. Ei ongelmaa.
        [TestMethod]
        public void LaskettuVahvuuslukuYliMaksimin1()
        {
            var t = u.Testaa("2999", "1 2700");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(3002, t.Item2.UusiSelo);
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }
        
        [TestMethod]
        public void LaskettuVahvuuslukuYliMaksimin2()
        {
            var t = u.Testaa("2999", "+2700 -2991 +2988");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(3002, t.Item2.UusiSelo);
            Assert.AreEqual(2992, t.Item2.MinSelo);      // laskennan aikainen minimi
            Assert.AreEqual(3002, t.Item2.MaxSelo);     // laskennan aikainen maksimi
        }
    }
}
