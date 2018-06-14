//
// Yksikkötestaukset
//
// Luotu 10.6.2018 Ismo Suihko
//
// TOIMII!! Nyt on helppo tarkistaa muutoksien jälkeen, onko laskenta kunnossa!
//
// Yksi virhekin tuli korjattua. Pelimäärä saattoi joillakin syötteillä vaikuttaa tulokseen.
//
// Muutokset:
//  11.6.2018  Järjestetty aiempia testitapauksia ja lisätty uusia
//

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest1
    {
        // Testataan uuden pelaajan vahvuusluvun muutokset ottelu kerrallaan.
        // Seuraavaan testiin otetaan edellisestä saatu vahvuusluku ja pelimäärä.
        [TestMethod]
        public void UudenPelaajanOttelutYksittain()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, 1725);
            Assert.AreEqual(tulokset.Item2, 1);

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1441", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, 1683);
            Assert.AreEqual(tulokset.Item2, 2);

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1973", Vakiot.OttelunTulos_enum.TULOS_TAPPIOx2);
            Assert.AreEqual(tulokset.Item1, 1713);
            Assert.AreEqual(tulokset.Item2, 3);

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1718", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, 1764);
            Assert.AreEqual(tulokset.Item2, 4);

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1784", Vakiot.OttelunTulos_enum.TULOS_TAPPIOx2);
            Assert.AreEqual(tulokset.Item1, 1728);
            Assert.AreEqual(tulokset.Item2, 5);

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1660", Vakiot.OttelunTulos_enum.TULOS_TAPPIOx2);
            Assert.AreEqual(tulokset.Item1, 1683);
            Assert.AreEqual(tulokset.Item2, 6);

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1966", Vakiot.OttelunTulos_enum.TULOS_TAPPIOx2);
            Assert.AreEqual(tulokset.Item1, 1695);
            Assert.AreEqual(tulokset.Item2, 7);
        }

        [TestMethod]
        public void UudenPelaajanOttelutKerralla1()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1695);
            Assert.AreEqual(tulokset.Item2, 7);
        }

        [TestMethod]
        public void UudenPelaajanOttelutKerralla2()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "3 1525 1441 1973 1718 1784 1660 1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1695);
            Assert.AreEqual(tulokset.Item2, 7);
        }

        // Kolme tapaa syöttää ottelun tulos
        [TestMethod]
        public void TulosPainikkeilla()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1800", "", "1900", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, 1823);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        [TestMethod]
        public void TulosSelossa()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1800", "", "+1900", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1823);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        [TestMethod]
        public void TulosNumeronaEnnenSeloa()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1800", "", "1.0 1900", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1823);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        // Merkkijonoissa ylimääräisiä välilyöntejä
        [TestMethod]
        public void UudenPelaajanOttelutValilyonteja()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "    1525  ", "0  ", "     +1525  +1441           -1973 +1718    -1784 -1660     -1966   ", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1695);
            Assert.AreEqual(tulokset.Item2, 7);
        }

        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksesta()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2033);
            Assert.AreEqual(tulokset.Item2, 0);  // Pelimaara 0, koska sitä ei annettu ("") eikä siten laskettu
        }

        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "75", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2033);
            Assert.AreEqual(tulokset.Item2, 87);  // 75 + 12 ottelua = 87
        }

        [TestMethod]
        public void ShakinVahvuuslukuTurnauksesta()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1996", "", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2050);
            Assert.AreEqual(tulokset.Item2, 0);  // Pelimaara 0, koska sitä ei annettu ("") eikä siten laskettu        
        }

        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void ShakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1996", "150", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2050);
            Assert.AreEqual(tulokset.Item2, 162);   // 150 + 12 ottelua  = 162
        }


        // Testataan virheellisiä syötteitä, joista saadaan virheen mukainen virhestatus
   
        // Testataan virheellinen syöte, tässä virheellinen oma vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteOmaSELO()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "15zz5", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_OMA_SELO);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        // Testataan virheellinen syöte, tässä virheellinen vastustajan vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteVastustajanSELO()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "c5sdffew25", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_VAST_SELO);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        // Pelimäärä virheellinen, annettu liian suureksi
        [TestMethod]
        public void VirheellinenSyoteOmaPelimaara()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "123456", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_PELIMAARA);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        // Ei ole annettu ottelun tulosta valintanapeilla tappio, tasapeli tai voitto
        [TestMethod]
        public void VirheellinenSyoteEiTulosta()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1600", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_BUTTON_TULOS);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        // Virheellinen yksittäinen tulos turnauksen tuloksissa. Oltava + (voitto), - (tappio) tai = (tasan).
        [TestMethod]
        public void VirheellinenSyoteTurnauksessaVirheellinenTulos()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "+1525 +1441 -1973 +1718 /1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_YKSITTAINEN_TULOS);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        // Annettu isompi pistemäärä (20) kuin mitä on otteluita (12 kpl)
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos()  
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "20 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS);
            Assert.AreEqual(tulokset.Item2, 0);
        }


        // TODO: Lisää erilaisia testitapauksia laskennoista ja virhetilanteista


        // Testauksen apurutiini, joka palauttaa tuloksista lasketun vahvuusluvun ja pelimäärän
        // Tietorakenteesta Tulokset saisi otettua myös muitakin laskettuja tietoja tarkistettavaksi
        // Esim. -otteluista saatu yhteispistemäärä (tulokset.laskettuTurnauksenTulos)
        //       -vastustajien vahvuuslukujen keskiarvo (tulokset.turnauksenKeskivahvuus)
        //
        // Virhetilanteessa palautetaan virhestatus ja nolla
        // Käytetään Tuple:n aiempaa versiota, koska Visual Studio Community 2015:ssa ei ole käytössä C# 7.0:aa
        public Tuple<int, int> Testaa(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            SelolaskuriOperations so = new SelolaskuriOperations();
            Syotetiedot ottelu = new Syotetiedot(aika, selo, pelimaara, vastustajat, tulos);
            Tulokset tulokset = new Tulokset();
            int status;

            if ((status = so.TarkistaSyote(ottelu)) != Vakiot.SYOTE_STATUS_OK) {
                return Tuple.Create(status, 0);             // Palautetaan virhestatus
            } else {
                so.SuoritaLaskenta(ottelu, ref tulokset);   // tarvitaan ref
                // Palautetaan uusi vahvuusluku ja pelimäärä tarkistettavaksi
                return Tuple.Create(tulokset.laskettuSelo, tulokset.laskettuPelimaara);
            }
        }
    }  
}
