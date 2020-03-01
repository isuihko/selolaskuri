
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelolaskuriLibrary;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest4_TarkistaCSV
    {
        private UnitTest u = new UnitTest();

        // --------------------------------------------------------------------------------
        // Laskennan testauksia erilaisin syöttein CSV-formaatista (comma separated values)
        //
        // Tässä testataan virhetilanteet (liian monta arvoa, arvot puuttuvat) ja 
        // sitten muutamalla tapauksella laskentaa, että saadaanko oikea syöte erotettua merkkijonosta.
        // Sen jälkeen laskenta menee tavalliseksi laskennaksi, vrt. UnitTest3.cs
        // --------------------------------------------------------------------------------

        [TestMethod]
        public void CSV_LiianMontaArvoa1()
        {
            var t = u.Testaa("90,1525,0,1525,1,123");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO/*Vakiot.SYOTE_VIRHE_CSV_FORMAT*/, t.Item1);
        }

        [TestMethod]
        public void CSV_LiianMontaArvoa2()
        {
            var t = u.Testaa(",,,,,,");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO /*Vakiot.SYOTE_VIRHE_CSV_FORMAT*/, t.Item1);
        }

        [TestMethod]
        public void CSV_ArvotPuuttuvatKaikki1()
        {
            var t = u.Testaa(",,,,");
            Assert.AreNotEqual(null, t);
            // four commas expected format: thinking time,own selo,own game count,opponents,single match result
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_MIETTIMISAIKA_CSV, t.Item1);
        }

        [TestMethod]
        public void CSV_ArvotPuuttuvatKaikki2()
        {
            var t = u.Testaa(",,,");
            Assert.AreNotEqual(null, t);
            // three commas expected format: thinking time,own selo,own game count,opponents
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_MIETTIMISAIKA_CSV, t.Item1);
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

        // Can't be csv format with an empty
        [TestMethod]
        public void CSV_ArvotPuuttuvatKaikki5()
        {
            var t = u.Testaa("");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO, t.Item1);
        }

        // Jos on neljä pilkkua, niin pitää olla myös miettimisaika
        [TestMethod]
        public void CSV_ArvotPuuttuvatMiettimisaika()
        {
            var t = u.Testaa(",1525,0,1525,1");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_MIETTIMISAIKA_CSV, t.Item1);
        }

        // Kolme pilkkua, neljä arvoa ja pitää olla miettimisaika ensimmäisenä
        [TestMethod]
        public void CSV_ArvotPuuttuvatMiettimisaika2()
        {
            var t = u.Testaa(",0,1525,1");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_MIETTIMISAIKA_CSV, t.Item1);
        }

        // Neljä pilkkua ja viisi arvoa, mutta oma selo tyhjä
        [TestMethod]
        public void CSV_ArvotPuuttuvatOmaSelo()
        {
            var t = u.Testaa("90,,0,1525,1");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO, t.Item1);
        }

        [TestMethod]
        public void CSV_ArvotPuuttuvatVastustajat()
        {
            var t = u.Testaa("90,1525,0,,1");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        // Yksittäisen ottelun tulos tarvitaan, jos on yksi vastustaja eikä tulosta ole kerrottu esim. "+1425"
        [TestMethod]
        public void CSV_ArvotPuuttuvatYksittainenTulos()
        {
            var t = u.Testaa("90,1525,0,1525");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_BUTTON_TULOS, t.Item1);
        }

        // Turnauksen tulos 2½ syötetty ½2, jolloin se tarkastuksessa tulkitaan vastustajan seloksi
        [TestMethod]
        public void CSV_VirheellinenTurnauksenTulos()
        {
            var t = u.Testaa("90,1525,20,½2 1505 1600 1611 1558");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }
    }
}

