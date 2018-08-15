using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest1_SyotteenKasittely
    {
        SelolaskuriOperations so = new SelolaskuriOperations();

        [TestMethod]
        public void TarkistaOttelulistaErillisinTuloksin()
        {
            // input
            Vakiot.Miettimisaika_enum aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;
            string selo = "1525";
            string pelimaara = "";
            string vastustajat = "+1525 +1441 -1973 +1718 -1784 -1660 -1966";
            Vakiot.OttelunTulos_enum tulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;


            Syotetiedot s = new Syotetiedot(aika, selo, pelimaara, vastustajat, tulos);

            // was the class Syotetiedot initialized correctly?
            Assert.AreEqual(aika, s.Miettimisaika);
            Assert.AreEqual(selo, s.AlkuperainenSelo_str);
            Assert.AreEqual(pelimaara, s.AlkuperainenPelimaara_str);
            Assert.AreEqual(vastustajat, s.VastustajienSelot_str);
            Assert.AreEqual(tulos, s.OttelunTulos);

            // parse strings into numbers, create an array of opponents
            int status = so.TarkistaSyote(s);

            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, status);

            int seloInt;
            int[] vastustajatInt;
            if (int.TryParse(selo, out seloInt) == false)
                Assert.AreEqual("Illegal test data (number)", selo);
            string[] vastustajatStr = vastustajat.Split(' ');

            Assert.AreEqual(seloInt, s.AlkuperainenSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, s.AlkuperainenPelimaara);
            Assert.AreEqual(vastustajatStr.Length, s.Ottelut.Lukumaara);


        }
    }
}
