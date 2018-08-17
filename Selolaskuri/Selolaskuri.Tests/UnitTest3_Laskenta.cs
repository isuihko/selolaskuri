
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest3_Laskenta
    {
        private UnitTest1 u = new UnitTest1();

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
            Assert.AreEqual(1 * 2,  t.Item2.TurnauksenTulos);        // tulos voitto 
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
            Assert.AreEqual(1 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1441,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(84,     t.Item2.Odotustulos);               // 0,84*100

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1973", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1713,   t.Item2.UusiSelo);
            Assert.AreEqual(3,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos);
            Assert.AreEqual(1973,   t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(16,     t.Item2.Odotustulos);               // 0,16*100

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1718", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1764,   t.Item2.UusiSelo);
            Assert.AreEqual(4,      t.Item2.UusiPelimaara);
            Assert.AreEqual(1 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1718,   t.Item2.TurnauksenKeskivahvuus);

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1784", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1728,   t.Item2.UusiSelo);
            Assert.AreEqual(5,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos);
            Assert.AreEqual(1784,   t.Item2.TurnauksenKeskivahvuus);

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1660", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1683,   t.Item2.UusiSelo);
            Assert.AreEqual(6,      t.Item2.UusiPelimaara);
            Assert.AreEqual(0,      t.Item2.TurnauksenTulos);
            Assert.AreEqual(1660,   t.Item2.TurnauksenKeskivahvuus);

            t = u.Testaa(t.Item2.UusiSelo.ToString(), t.Item2.UusiPelimaara.ToString(), "1966", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
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
            var t = u.Testaa("1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            // Jos pitkä peli ja tulos määrittelematon, niin jatkossa käytetään lyhyempää muotoa, jossa 
            // var t = u.Testaa("1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966");
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
            var t = u.Testaa("1525", "0", "3 1525 1441 1973 1718 1784 1660 1966");
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
            var t = u.Testaa("1525", "+1525 +1441 -1973 +1718 -1784 -1660 -1966");
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
            var t = u.Testaa("1525", "3 1525 1441 1973 1718 1784 1660 1966");
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

        [TestMethod]
        public void TulosNumeronaEnnenSeloa()
        {
            var t = u.Testaa("1800", "1.0 1900");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1823,   t.Item2.UusiSelo);
            Assert.AreEqual(36,     t.Item2.Odotustulos);   // odotustulos 0,36*100
        }

        // Merkkijonoissa ylimääräisiä välilyöntejä
        [TestMethod]
        public void UudenPelaajanOttelutValilyonteja()
        {
            var t = u.Testaa("    1525  ", "0  ", "     +1525  +1441           -1973 +1718    -1784 -1660     -1966   ");
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695,   t.Item2.UusiSelo);
            Assert.AreEqual(7,      t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2,  t.Item2.TurnauksenTulos);
            Assert.AreEqual(1724,   t.Item2.TurnauksenKeskivahvuus);
        }

        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksesta()
        {
            var t = u.Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
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
            var t = u.Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "75", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
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
