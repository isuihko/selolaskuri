//
// Yksikkötestaukset
//
// Luotu 10.6.2018 Ismo Suihko
//
// TOIMII!! Nyt on helppo tarkistaa muutoksien jälkeen, onko laskenta kunnossa!
//
// Yksi virhekin tuli korjattua. Pelimäärä saattoi joillakin syötteillä vaikuttaa tulokseen.
//

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UudenPelaajanOttelutYksittain()
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, 1725);
            Assert.AreEqual(tulokset.Item2, 1);

            tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1441", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, 1683);
            Assert.AreEqual(tulokset.Item2, 2);

            tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1973", Vakiot.OttelunTulos_enum.TULOS_TAPPIOx2);
            Assert.AreEqual(tulokset.Item1, 1713);
            Assert.AreEqual(tulokset.Item2, 3);

            tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1718", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, 1764);
            Assert.AreEqual(tulokset.Item2, 4);

            tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1784", Vakiot.OttelunTulos_enum.TULOS_TAPPIOx2);
            Assert.AreEqual(tulokset.Item1, 1728);
            Assert.AreEqual(tulokset.Item2, 5);

            tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1660", Vakiot.OttelunTulos_enum.TULOS_TAPPIOx2);
            Assert.AreEqual(tulokset.Item1, 1683);
            Assert.AreEqual(tulokset.Item2, 6);

            tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1966", Vakiot.OttelunTulos_enum.TULOS_TAPPIOx2);
            Assert.AreEqual(tulokset.Item1, 1695);
            Assert.AreEqual(tulokset.Item2, 7);
        }

        [TestMethod]
        public void UudenPelaajanOttelutKerralla()
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1695);
            Assert.AreEqual(tulokset.Item2, 7);
        }

        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksesta()
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2033);
            Assert.AreEqual(tulokset.Item2, 0);  // Pelimaara 0, koska sitä ei annettu ("") eikä siten laskettu
        }

        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "75", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2033);
            Assert.AreEqual(tulokset.Item2, 87);  // 75 + 12 ottelua = 87
        }

        [TestMethod]
        public void ShakinVahvuuslukuTurnauksesta()
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1996", "", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2050);
            Assert.AreEqual(tulokset.Item2, 0);  // Pelimaara 0, koska sitä ei annettu ("") eikä siten laskettu        
        }

        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void ShakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1996", "150", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2050);
            Assert.AreEqual(tulokset.Item2, 162);   // 150 + 12 ottelua  = 162
        }

        // Testataan virheellisiä syötteitä, joista saatujen tuloksien on oltava nollia
        //     tässä ei ole annettu ottelun tulosta
        [TestMethod]
        public void VirheellinenSyoteEiTulosta()
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1600", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_BUTTON_TULOS);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        // Testataan virheellinen syöte, tässä virheellinen oma vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteOmaSELO()
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "15zz5", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_OMA_SELO);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        // Testataan virheellinen syöte, tässä virheellinen oma vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteVastustajanSELO()
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "15zz5", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_OMA_SELO);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        [TestMethod]
        public void VirheellinenSyoteVastustajanPelimaara()
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "12345", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTOx2);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_PELIMAARA);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos()  // Annetaan isompi pistemäärä kuin mitä on otteluita
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "20 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        [TestMethod]
        public void VirheellinenSyoteTurnauksessaVirheellinenTulos()  // tuloksen oltava + (voitto), - (tappio) tai = (tasan)
        {
            var tulokset = TestaaLaskentaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "+1525 +1441 -1973 +1718 /1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_YKSITTAINEN_TULOS);
            Assert.AreEqual(tulokset.Item2, 0);
        }

        // TODO: Lisää erilaisia testitapauksia virhetilanteista


        // Testauksen apurutiini, joka palauttaa tuloksista lasketun vahvuusluvun ja pelimäärän
        // Tietorakenteesta Tulokset saisi otettua myös muitakin laskettuja tietoja tarkistettavaksi
        //
        // Virhetilanteessa palautetaan virhestatus ja nolla
        public Tuple<int, int> TestaaLaskentaa(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            SelolaskuriOperations so = new SelolaskuriOperations();
            Syotetiedot ottelu = new Syotetiedot(aika, selo, pelimaara, vastustajat, tulos);
            Tulokset tulokset = new Tulokset();
            int status;

            if ((status = so.TarkistaSyote(ottelu)) != Vakiot.SYOTE_STATUS_OK) {
                return Tuple.Create(status, 0);             // Palautetaan virhestatus
            } else {
                so.SuoritaLaskenta(ottelu, ref tulokset);   // Pitääkö olla ref, koska on eri luokassa?
                // Palautetaan uusi vahvuusluku ja pelimäärä tarkistettavaksi
                return Tuple.Create(tulokset.laskettuSelo, tulokset.laskettuPelimaara);
            }
        }
    }  
}
