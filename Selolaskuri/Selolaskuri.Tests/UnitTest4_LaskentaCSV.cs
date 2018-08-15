using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest4_LaskentaCSV
    {
        private UnitTest1 u = new UnitTest1();

        // --------------------------------------------------------------------------------
        // Laskennan testauksia erilaisin syöttein CSV-formaatista (comma separated values)
        // --------------------------------------------------------------------------------

        [TestMethod]
        public void CSV_LiianMontaArvoa1()
        {
            var t = u.Testaa("90,1525,0,1525,1,123");
            Assert.AreEqual(null, t);
        }

        [TestMethod]
        public void CSV_LiianMontaArvoa2()
        {
            var t = u.Testaa(",,,,,,");
            Assert.AreEqual(null, t);
        }

        [TestMethod]
        public void CSV_ArvotPuuttuvatKaikki1()
        {
            var t = u.Testaa(",,,,");
            Assert.AreNotEqual(null, t);
            // four commas expected format: thinking time,own selo,own game count,opponents,single match result
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_MIETTIMISAIKA, t.Item1);
        }

        [TestMethod]
        public void CSV_ArvotPuuttuvatKaikki2()
        {
            var t = u.Testaa(",,,");
            Assert.AreNotEqual(null, t);
            // three commas expected format: thinking time,own selo,own game count,opponents
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_MIETTIMISAIKA, t.Item1);
        }

        [TestMethod]
        public void CSV_ArvotPuuttuvatKaikki3()
        {
            var t = u.Testaa(",,");
            Assert.AreNotEqual(null, t);
            // two commas expected format: own selo,own game count,opponents
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO, t.Item1);
        }

        [TestMethod]
        public void CSV_ArvotPuuttuvatKaikki4()
        {
            var t = u.Testaa(",");
            Assert.AreNotEqual(null, t);
            // one comma expected format: own selo,opponents
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO, t.Item1);
        }

        [TestMethod]
        public void CSV_UudenPelaajanOttelutYksittain1()
        {
            var t = u.Testaa("90,1525,0,1525,1");
            Assert.AreNotEqual(null, t);
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
            var t = u.Testaa("90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966");
            Assert.AreNotEqual(null, t);
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
            var t = u.Testaa("90,1525,0,3 1525 1441 1973 1718 1784 1660 1966");
            Assert.AreNotEqual(null, t);
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
            var t = u.Testaa("5,1996,,10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreNotEqual(null, t);
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
    }
}
