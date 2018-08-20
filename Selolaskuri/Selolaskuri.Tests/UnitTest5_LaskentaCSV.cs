
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest5_LaskentaCSV
    {
        private UnitTest u = new UnitTest();

        [TestMethod]
        public void CSV_UudenPelaajanOttelutYksittain1()
        {
            var t = u.Testaa("90,1525,0,1525,1");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1725, t.Item2.UusiSelo);                  // uusi vahvuusluku
            Assert.AreEqual(1, t.Item2.UusiPelimaara);             // uusi pelimäärä 0+1 = 1
            Assert.AreEqual(1 * 2, t.Item2.TurnauksenTulos);           // tulos voitto kaksinkertaisena
            Assert.AreEqual(1525, t.Item2.TurnauksenKeskivahvuus);    // keskivahvuus
            Assert.AreEqual(1, t.Item2.VastustajienLkm);            // yksi vastustaja
            Assert.AreEqual(50, t.Item2.Odotustulos);               // 0,50*100  odotustulos palautuu 100-kertaisena
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);         // yksi ottelu, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);         // yksi ottelu, sama kuin UusiSelo           
        }        

        [TestMethod]
        public void CSV_UudenPelaajanOttelutYksittain2()
        {
            var t = u.Testaa("90,1725,1,1441,1");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1683, t.Item2.UusiSelo);
            Assert.AreEqual(2, t.Item2.UusiPelimaara);             // uusi pelimäärä 1+1 = 2
            Assert.AreEqual(1 * 2, t.Item2.TurnauksenTulos);
            Assert.AreEqual(1441, t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(84, t.Item2.Odotustulos);               // 0,84*100
        }

        [TestMethod]
        public void CSV_TasapeliOttelustaUusiPelaaja()
        {
            var t = u.Testaa("90,1525,0,1812,0.5");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1812, t.Item2.UusiSelo);
            Assert.AreEqual(1, t.Item2.UusiPelimaara);             // uusi pelimäärä 1+1 = 2
            Assert.AreEqual(0.5 * 2, t.Item2.TurnauksenTulos);
            Assert.AreEqual(1812, t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(16, t.Item2.Odotustulos);
        }

        [TestMethod]
        public void CSV_TasapeliOttelustaUusiPelaaja2()
        {
            var t = u.Testaa("90,1525,0,1812,½");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1812, t.Item2.UusiSelo);
            Assert.AreEqual(1, t.Item2.UusiPelimaara);             // uusi pelimäärä 1+1 = 2
            Assert.AreEqual(0.5 * 2, t.Item2.TurnauksenTulos);
            Assert.AreEqual(1812, t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(16, t.Item2.Odotustulos);
        }

        // Tarkistettu http://shakki.kivij.info/performance_calculator.shtml
        [TestMethod]
        public void CSV_TurnauksenLaskenta()
        {
            var t = u.Testaa("90,1525,20,2.5 1505 1600 1611 1558");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1559, t.Item2.UusiSelo);                // uusi vahvuusluku
            Assert.AreEqual(24, t.Item2.UusiPelimaara);             // uusi pelimäärä 0+1 = 1
            Assert.AreEqual(2.5F * 2, t.Item2.TurnauksenTulos);     // tulos voitto kaksinkertaisena
            Assert.AreEqual(1569, t.Item2.TurnauksenKeskivahvuus);  // 1568,5 -> Round 1569
            Assert.AreEqual(4, t.Item2.VastustajienLkm);
            Assert.AreEqual(176, t.Item2.Odotustulos);              // 1,76*100  odotustulos palautuu 100-kertaisena
        }

        [TestMethod]
        public void CSV_TurnauksenLaskentaPuolikas()
        {
            var t = u.Testaa("90,1525,20,2½ 1505 1600 1611 1558");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1559, t.Item2.UusiSelo);                // uusi vahvuusluku
            Assert.AreEqual(24, t.Item2.UusiPelimaara);             // uusi pelimäärä 0+1 = 1
            Assert.AreEqual(2.5F * 2, t.Item2.TurnauksenTulos);     // tulos voitto kaksinkertaisena
            Assert.AreEqual(1569, t.Item2.TurnauksenKeskivahvuus);  // 1568,5 -> Round 1569
            Assert.AreEqual(4, t.Item2.VastustajienLkm);
            Assert.AreEqual(176, t.Item2.Odotustulos);              // 1,76*100  odotustulos palautuu 100-kertaisena
        }

        [TestMethod]
        public void CSV_TurnauksenLaskentaValilyonnit1()
        {
            var t = u.Testaa("    90   ,   1525,    20,    2.5    1505    1600 1611 1558   ");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1559, t.Item2.UusiSelo);                // uusi vahvuusluku
            Assert.AreEqual(24, t.Item2.UusiPelimaara);             // uusi pelimäärä 0+1 = 1
            Assert.AreEqual(2.5F * 2, t.Item2.TurnauksenTulos);     // tulos voitto kaksinkertaisena
            Assert.AreEqual(1569, t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(4, t.Item2.VastustajienLkm);
            Assert.AreEqual(176, t.Item2.Odotustulos);              // 1,76*100  odotustulos palautuu 100-kertaisena
        }

        [TestMethod]
        public void CSV_TurnauksenLaskentaValilyonnit1Puolikas()
        {
            var t = u.Testaa("    90   ,   1525,    20,    2½    1505    1600 1611 1558   ");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1559, t.Item2.UusiSelo);                // uusi vahvuusluku
            Assert.AreEqual(24, t.Item2.UusiPelimaara);             // uusi pelimäärä 0+1 = 1
            Assert.AreEqual(2.5F * 2, t.Item2.TurnauksenTulos);     // tulos voitto kaksinkertaisena
            Assert.AreEqual(1569, t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(4, t.Item2.VastustajienLkm);
            Assert.AreEqual(176, t.Item2.Odotustulos);              // 1,76*100  odotustulos palautuu 100-kertaisena
        }

        [TestMethod]
        public void CSV_TurnauksenLaskentaValilyonnit2()
        {
            var t = u.Testaa("90 , 1525 ,  20  ,   2.5 1505 1600 1611 1558");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1559, t.Item2.UusiSelo);                // uusi vahvuusluku
            Assert.AreEqual(24, t.Item2.UusiPelimaara);             // uusi pelimäärä 0+1 = 1
            Assert.AreEqual(2.5F * 2, t.Item2.TurnauksenTulos);     // tulos voitto kaksinkertaisena
            Assert.AreEqual(1569, t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(4, t.Item2.VastustajienLkm);
            Assert.AreEqual(176, t.Item2.Odotustulos);              // 1,76*100  odotustulos palautuu 100-kertaisena
        }

        [TestMethod]
        public void CSV_TurnauksenLaskentaValilyonnit3()
        {
            var t = u.Testaa("  90   ,1525    ,20       ,2.5 1505 1600    1611    1558  ");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1559, t.Item2.UusiSelo);                // uusi vahvuusluku
            Assert.AreEqual(24, t.Item2.UusiPelimaara);             // uusi pelimäärä 0+1 = 1
            Assert.AreEqual(2.5F * 2, t.Item2.TurnauksenTulos);     // tulos voitto kaksinkertaisena
            Assert.AreEqual(1569, t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(4, t.Item2.VastustajienLkm);
            Assert.AreEqual(176, t.Item2.Odotustulos);              // 1,76*100  odotustulos palautuu 100-kertaisena
        }

        [TestMethod]
        public void CSV_UudenPelaajanOttelutKerralla1()
        {
            var t = u.Testaa("90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695, t.Item2.UusiSelo);
            Assert.AreEqual(7, t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2, t.Item2.TurnauksenTulos);
            Assert.AreEqual(1724, t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7, t.Item2.VastustajienLkm);
            Assert.AreEqual(199, t.Item2.Odotustulos);         // odotustulos 1,99*100
            Assert.AreEqual(1683, t.Item2.MinSelo);             // laskennan aikainen minimi
            Assert.AreEqual(1764, t.Item2.MaxSelo);             // laskennan aikainen maksimi
        }

        [TestMethod]
        public void CSV_UudenPelaajanOttelutKerralla2()
        {
            var t = u.Testaa("90,1525,0,3 1525 1441 1973 1718 1784 1660 1966");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(1695, t.Item2.UusiSelo);
            Assert.AreEqual(7, t.Item2.UusiPelimaara);
            Assert.AreEqual(3 * 2, t.Item2.TurnauksenTulos);
            Assert.AreEqual(1724, t.Item2.TurnauksenKeskivahvuus);
            Assert.AreEqual(7, t.Item2.VastustajienLkm);
            // Assert.AreEqual(199, t.Item2.Odotustulos);           // ei lasketa uudelle pelaajalle tästä formaatista
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);     // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);     // selo laskettu kerralla, sama kuin UusiSelo
        }

        [TestMethod]
        public void CSV_PikashakinVahvuuslukuTurnauksesta()
        {
            var t = u.Testaa("5,1996,,10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2034, t.Item2.UusiSelo);  // tarkista, oliko 2033 (yhden pisteen virhe laskennassa mahdollinen)
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual((int)(10.5F * 2), t.Item2.TurnauksenTulos);
            Assert.AreEqual(1827, t.Item2.TurnauksenKeskivahvuus);  // 
            Assert.AreEqual(12, t.Item2.VastustajienLkm);         // 12 vastustajaa eli ottelua
            Assert.AreEqual(840, t.Item2.Odotustulos);             // odotustulos 8,40*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);       // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);       // selo laskettu kerralla, sama kuin UusiSelo
        }

        // Kuten edellä CSV_PikashakinVahvuuslukuTurnauksesta(), mutta ei anneta miettimisaikaa 5 vaan käytetään oletusta 90 min -> eri tulos
        // Tässä voidaan jättää myös oma pelimäärä antamatta kokonaan. Edellisessä testitapauksessa annettu tyhjä.
        [TestMethod]
        public void CSV_VahvuuslukuTurnauksesta()
        {
            var t = u.Testaa("1996,10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2050, t.Item2.UusiSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual((int)(10.5F * 2), t.Item2.TurnauksenTulos);
            Assert.AreEqual(1827, t.Item2.TurnauksenKeskivahvuus);  // 
            Assert.AreEqual(12, t.Item2.VastustajienLkm);         // 12 vastustajaa eli ottelua
            Assert.AreEqual(840, t.Item2.Odotustulos);             // odotustulos 8,40*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);       // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);       // selo laskettu kerralla, sama kuin UusiSelo
        }

        // Kuten edellä CSV_PikashakinVahvuuslukuTurnauksesta(), mutta ei anneta miettimisaikaa 5 vaan käytetään oletusta 90 min -> eri tulos
        // Tässä voidaan jättää myös oma pelimäärä antamatta kokonaan. Edellisessä testitapauksessa annettu tyhjä.
        [TestMethod]
        public void CSV_VahvuuslukuTurnauksestaPuolikas()
        {
            var t = u.Testaa("1996,10½ 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreNotEqual(null, t);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, t.Item1);
            Assert.AreEqual(2050, t.Item2.UusiSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, t.Item2.UusiPelimaara);  // pelimäärää ei laskettu
            Assert.AreEqual((int)(10.5F * 2), t.Item2.TurnauksenTulos);
            Assert.AreEqual(1827, t.Item2.TurnauksenKeskivahvuus);  // 
            Assert.AreEqual(12, t.Item2.VastustajienLkm);         // 12 vastustajaa eli ottelua
            Assert.AreEqual(840, t.Item2.Odotustulos);             // odotustulos 8,40*100
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MinSelo);       // selo laskettu kerralla, sama kuin UusiSelo
            Assert.AreEqual(t.Item2.UusiSelo, t.Item2.MaxSelo);       // selo laskettu kerralla, sama kuin UusiSelo
        }
    }
}
