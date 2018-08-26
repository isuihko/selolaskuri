
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelolaskuriLibrary;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest1_SyotteenKasittely
    {
        SelolaskuriOperations so = new SelolaskuriOperations();

        // Apurutiini, muodostaa seloluvusta ja tuloksesta merkkijonon,
        // Esim. selo 1441, tulos voitto -> "+1441"
        private string MuodostaOttelu(int selo, Vakiot.OttelunTulos_enum tulos)
        {
            string ottelu;

            switch (tulos) {
                case Vakiot.OttelunTulos_enum.TULOS_VOITTO:
                    ottelu = "+";
                    break;
                case Vakiot.OttelunTulos_enum.TULOS_TASAPELI:
                    ottelu = "=";
                    break;
                case Vakiot.OttelunTulos_enum.TULOS_TAPPIO:
                    ottelu = "-";
                    break;
                case Vakiot.OttelunTulos_enum.TULOS_EI_ANNETTU:
                    ottelu = "";
                    break;
                default:
                    ottelu = "*";  // VIRHE
                    break;
            }
            ottelu += selo.ToString();
            return ottelu;
        }

        //
        // Tämä tarkistaa, ovat syötteet ja ottelulista oikein, kun tulokset annettu formaatissa
        // "+1525 +1441 -1973 +1718 -1784 -1660 -1966",
        // jossa kunkin vastustajan vahvuusluvun kanssa on annettu ottelun tulos
        //
        [TestMethod]
        public void TarkistaOttelulistaErillisinTuloksin()
        {
            // input
            Vakiot.Miettimisaika_enum aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;
            string selo = "1525";
            string pelimaara = "";
            string vastustajat = "+1525 +1441 -1973 +1718 -1784 -1660 -1966 =1234 1555";
            Vakiot.OttelunTulos_enum tulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;


            Syotetiedot s = new Syotetiedot(aika, selo, pelimaara, vastustajat, tulos);

            // 
            // Check that the input was stored correctly!
            // 
            Assert.AreEqual(aika, s.Miettimisaika);
            Assert.AreEqual(selo, s.AlkuperainenSelo_str);
            Assert.AreEqual(pelimaara, s.AlkuperainenPelimaara_str);
            Assert.AreEqual(vastustajat, s.VastustajienSelot_str);
            Assert.AreEqual(tulos, s.OttelunTulos);

            //
            // parse strings into numbers, create an array of opponents
            //
            int status = so.TarkistaSyote(s);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, status);

            int seloInt;
            if (int.TryParse(selo, out seloInt) == false)
                Assert.AreEqual("Illegal test data (number)", selo);

            Assert.AreEqual(seloInt, s.AlkuperainenSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, s.AlkuperainenPelimaara);

            //
            // Tarkista ottelulista, ensin lukumäärä
            //
            string[] vastustajatStr = vastustajat.Split(' ');
            Assert.AreEqual(vastustajatStr.Length, s.Ottelut.Lukumaara);

            // Käy sitten tallennetut ottelut läpi

            int i = 0;
            Ottelulista lista = s.Ottelut;

            var ottelu = lista.HaeEnsimmainen(); // vastustajanSelo, ottelunTulos            
            while (ottelu.Item2 != Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON) {
                Assert.AreEqual(MuodostaOttelu(ottelu.Item1, ottelu.Item2), vastustajatStr[i]);              
                ottelu = lista.HaeSeuraava();
                i++;
            }
        }


        //
        // Tämä tarkistaa, ovat syötteet ja ottelulista oikein, kun tulokset annettu
        // formaatissa "1.5 1525 1441 1973 1718 1784 1660 1966",
        // jossa on turnauksen tulos ensin ja vastustajien vahvuusluvut ovat ilman erillistä tulosta
        //
        [TestMethod]
        public void TarkistaOttelulistaTurnauksenTuloksella()
        {
            // input
            Vakiot.Miettimisaika_enum aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;
            string selo = "1525";
            string pelimaara = "";
            string vastustajat = "1.5 1525 1441 1973 1718 1784 1660 1966";
            Vakiot.OttelunTulos_enum tulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;

            Syotetiedot s = new Syotetiedot(aika, selo, pelimaara, vastustajat, tulos);

            // 
            // Check that the input was stored correctly!
            // 
            Assert.AreEqual(aika, s.Miettimisaika);
            Assert.AreEqual(selo, s.AlkuperainenSelo_str);
            Assert.AreEqual(pelimaara, s.AlkuperainenPelimaara_str);
            Assert.AreEqual(vastustajat, s.VastustajienSelot_str);
            Assert.AreEqual(tulos, s.OttelunTulos);

            //
            // parse strings into numbers, create an array of opponents
            //
            int status = so.TarkistaSyote(s);
            Assert.AreEqual(Vakiot.SYOTE_STATUS_OK, status);

            int seloInt;
            if (int.TryParse(selo, out seloInt) == false)
                Assert.AreEqual("Illegal test data (number)", selo);

            Assert.AreEqual(seloInt, s.AlkuperainenSelo);
            Assert.AreEqual(Vakiot.PELIMAARA_TYHJA, s.AlkuperainenPelimaara);

            //
            // Tarkista ottelulista, ensin lukumäärä
            //
            // Huom! Nyt vastustajatStr sisältää myös turnauksen tuloksen, joten vähennettävä lukumäärää yhdellä
            //
            string[] vastustajatStr = vastustajat.Split(' ');
            Assert.AreEqual(vastustajatStr.Length - 1, s.Ottelut.Lukumaara);

            // Käy sitten tallennetut ottelut läpi

            // Huom! Koska vastustajatStr sisältää myös turnauksen tuloksen, aloitetaan indeksistä 1
            int i = 1;
            Ottelulista lista = s.Ottelut;

            var ottelu = lista.HaeEnsimmainen(); // vastustajanSelo, ottelunTulos            
            while (ottelu.Item2 != Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON) {
                // Koska käytetty turnauksen tulosta, niin listaan tallennettuihin otteluihin ei ole annettu tulosta
                Assert.AreEqual(MuodostaOttelu(ottelu.Item1, ottelu.Item2), vastustajatStr[i]);
                ottelu = lista.HaeSeuraava();
                i++;
            }
        }
    }
}
