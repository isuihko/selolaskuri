
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest2_TarkistaSyote
    {
        private UnitTest u = new UnitTest();

        // --------------------------------------------------------------------------------
        // Testataan virheellisiä syötteitä, joista pitää saada virheen mukainen virhestatus
        // --------------------------------------------------------------------------------

        // Testataan virheellinen syöte, tässä virheellinen oma vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteOmaSELO1()
        {
            var t = u.Testaa("15zz5", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO, t.Item1);
        }

        // Testataan virheellinen syöte, tässä virheellinen oma vahvuusluku. Oltava vähintään 1000.
        [TestMethod]
        public void VirheellinenSyoteOmaSELO2()
        {
            var t = u.Testaa("999", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO, t.Item1);
        }

        // Testataan virheellinen syöte, tässä virheellinen oma vahvuusluku. Oltava enintään 2999.
        [TestMethod]
        public void VirheellinenSyoteOmaSELO3()
        {
            var t = u.Testaa("3000", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO, t.Item1);
        }

        [TestMethod]
        public void VirheellinenSyoteOmaSELOtyhja()
        {
            var t = u.Testaa("", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_OMA_SELO, t.Item1);
        }

        // Testataan virheellinen syöte, tässä virheellinen vastustajan vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteVastustajanSELO()
        {
            var t = u.Testaa("1525", "0", "c5sdffew25", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        // Testataan virheellinen syöte, tässä virheellinen vastustajan vahvuusluku (oltava vähintään 1000)
        [TestMethod]
        public void VirheellinenSyoteVastustajanSELO2()
        {
            var t = u.Testaa("1525", "", "999", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        // Testataan virheellinen syöte, tässä virheellinen vastustajan vahvuusluku (oltava enintään 2999)
        [TestMethod]
        public void VirheellinenSyoteVastustajanSELO3()
        {
            var t = u.Testaa("1525", "", "3000", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        [TestMethod]
        public void VirheellinenSyoteVastustajanSELOTyhja()
        {
            var t = u.Testaa("1525", "0", "", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        // Pelimäärä virheellinen, annettu liian suureksi (asetettu rajoitus 9999)
        [TestMethod]
        public void VirheellinenSyoteOmaPelimaara()
        {
            var t = u.Testaa("1525", "123456", "1600", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_PELIMAARA, t.Item1);
        }

        // Pelimäärä virheellinen, annettu liian suureksi (asetettu rajoitus 9999)
        [TestMethod]
        public void VirheellinenSyoteOmaPelimaara2()
        {
            var t = u.Testaa("1525", "10000", "1600", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_PELIMAARA, t.Item1);
        }

        // Pelimäärä virheellinen, annettu negatiiviseksi
        [TestMethod]
        public void VirheellinenSyoteOmaPelimaara3()
        {
            var t = u.Testaa("1525", "-1", "1600", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_PELIMAARA, t.Item1);
        }

        // Ei ole annettu ottelun tulosta valintanapeilla tappio, tasapeli tai voitto, annettu TULOS_MAARITTELEMATON
        [TestMethod]
        public void VirheellinenSyoteEiTulosta()
        {
            var t = u.Testaa("1525", "0", "1600", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_BUTTON_TULOS, t.Item1);
        }

        // Virheellinen yksittäinen tulos turnauksen tuloksissa. Oltava + (voitto), - (tappio) tai = (tasan).
        [TestMethod]
        public void VirheellinenSyoteTurnauksessaVirheellinenTulos()
        {
            var t = u.Testaa("1525", "0", "+1525 +1441 -1973 +1718 /1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_YKSITTAINEN_TULOS, t.Item1);
        }

        // Annettu isompi pistemäärä (20) kuin mitä on otteluita (12 kpl)
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos1()
        {
            var t = u.Testaa("1996", "20 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS, t.Item1);
        }

        // Annettu isompi pistemäärä (199.5) kuin mitä on otteluita (12 kpl).
        // 199.5 (Vakiot.TURNAUKSEN_TULOS_MAX) on maksi, joka käsitellään turnauksen tuloksena.
        // Isommat tuloksena annetut luvut, jos ovat alle 1000, käsitellään vastustajan selon virheenä
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos2()
        {
            var t = u.Testaa("1996", "199.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS, t.Item1);
        }

        // Annettu turnauksen tulos 200, joka on suurempi kuin Vakiot.TURNAUKSEN_TULOS_MAX.
        // Palautuu ilmoituksena virheellisestä vastustajan selosta, kun ensimmäinen 
        // luku käsitellään numerona eikä tarkistuksessa voida tietää, kumpaa on tarkoitettu.
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos3()
        {
            var t = u.Testaa("1996", "200 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        // Annettu negatiivinen pistemäärä.
        // Palautuu ilmoituksena virheellisestä vastustajan selosta, kun ensimmäinen 
        // luku käsitellään numerona eikä tarkistuksessa voida tietää, kumpaa on tarkoitettu.
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos4()
        {
            var t = u.Testaa("1996", "-6 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        // Turnauksen tulos 2½ syötetty ½2, jolloin se tarkastuksessa tulkitaan vastustajan seloksi
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos5()
        {
            var t = u.Testaa("1525", "0", "½2 1505 1600 1611 1558");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        // Turnauksen tulos 2½ syötetty 2 ½, jolloin 2 on turnauksen tulos ja "½" tulkitaan vastustajan seloksi
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos6()
        {
            var t = u.Testaa("1525", "0", "2 ½ 1505 1600 1611 1558");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }

        // Turnauksen tulos 2½ syötetty 2.½, jolloin "2.½" tulkitaan vastustajan seloksi
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos7()
        {
            var t = u.Testaa("1525", "0", "2.½ 1505 1600 1611 1558");
            Assert.AreEqual(Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO, t.Item1);
        }
    }
}

