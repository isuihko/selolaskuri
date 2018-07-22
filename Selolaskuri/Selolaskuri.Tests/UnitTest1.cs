//
// Yksikkötestaukset - laskennan muutoksien testaus
//
// Luotu 10.6.2018 Ismo Suihko
//
// TOIMII!! Nyt on helppo tarkistaa muutoksien jälkeen, onko syötteen tarkastus ja laskenta kunnossa!
// Yksi virhekin tuli korjattua. Pelimäärä saattoi joillakin syötteillä vaikuttaa tulokseen.
//
// Tuloksista voidaan tarkistaa
//      1) laskettu vahvuusluku
//      2) uusi pelimäärä
//      3) turnauksen pistemäärä/tulos, huom! kaksinkertaisena jotta saadaan kokonaislukuna (esim. 1,5 tulee lukuna 3)
//      4) vastustajien keskivahvuus
//      5) vastustajien lukumäärä
//      6) Odotustulos
//
// Ks. apurutiini Testaa(), johon voidaan lisätä muitakin tarkistettavia tuloksia (mm. kerroin).
//
// Tuloksia on verrattu muiden laskentaohjelmien tuloksiin
//      http://shakki.kivij.info/performance_calculator.shtml
//      http://www.shakki.net/kerhot/salsk/ohjelmat/selo.html
// sekä selolaskennassa kerrottujen turnauksien ja otteluiden tuloksiin.
//
// Tuloksissa voi olla yhden pisteen verran heittoa eri laskentaohjelmien välillä pyöristyksistä johtuen,
// jolloin virallisen selolaskennan tulos on määräävä ja johon on tässä ohjelmassa myös pyritty.
//
// Muutokset:
//   11.6.2018  Järjestetty aiempia testitapauksia ja lisätty uusia
//   15.6.2018  Lisätty tarkistettavia tietoja: pistemäärä ja keskivahvuus
//   17.6.2018  Lisätty tarkistettavia tietoja: vastustajien lkm, Odotustulos
//              Odotustulosta ei näytetä uuden pelaajan tuloksissa, mutta sekin on laskettu ja voidaan tarkistaa
//              Myös lisätty testitapauksia (turnauksen tuloksen virheet). Nyt niitä on 22 kpl eli aika kattavasti.
//   18.6.2018  Tarkistettu näkyvyyttä -> private Testaa()
//   18.7.2018  Selkeytetty
//

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selolaskuri.Tests
{
    [TestClass]
    public class UnitTest1
    {

        // --------------------------------------------------------------------------------
        // Testataan vahvuusluvun ym. laskentaa
        // --------------------------------------------------------------------------------

        [TestMethod]
        public void UudenPelaajanOttelutYksittain()
        {
            // Testataan uuden pelaajan vahvuusluvun muutokset ottelu kerrallaan.
            // Seuraavaan testiin otetaan edellisestä saatu vahvuusluku ja pelimäärä.

            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(t.Item1, 1725);  // uusi vahvuusluku
            Assert.AreEqual(t.Item2, 1);     // uusi pelimäärä 0+1=1
            Assert.AreEqual(t.Item3, 1 * 2); // tulos voitto (tulee kokonaislukuna 2)
            Assert.AreEqual(t.Item4, 1525);  // keskivahvuus
            Assert.AreEqual(t.Item5, 1);     // yksi vastustaja
            Assert.AreEqual(t.Item6, 50);    // 0,50*100  odotustulos palautuu 100-kertaisena

            t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, t.Item1.ToString(), t.Item2.ToString(), "1441", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(t.Item1, 1683);
            Assert.AreEqual(t.Item2, 2);     // uusi pelimäärä 1+1=2
            Assert.AreEqual(t.Item3, 1 * 2);
            Assert.AreEqual(t.Item4, 1441);
            Assert.AreEqual(t.Item6, 84);    // 0,84*100

            t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, t.Item1.ToString(), t.Item2.ToString(), "1973", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(t.Item1, 1713);
            Assert.AreEqual(t.Item2, 3);
            Assert.AreEqual(t.Item3, 0);
            Assert.AreEqual(t.Item4, 1973);
            Assert.AreEqual(t.Item6, 16);   // 0,16*100

            t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, t.Item1.ToString(), t.Item2.ToString(), "1718", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(t.Item1, 1764);
            Assert.AreEqual(t.Item2, 4);
            Assert.AreEqual(t.Item3, 1 * 2);
            Assert.AreEqual(t.Item4, 1718);

            t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, t.Item1.ToString(), t.Item2.ToString(), "1784", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(t.Item1, 1728);
            Assert.AreEqual(t.Item2, 5);
            Assert.AreEqual(t.Item3, 0);
            Assert.AreEqual(t.Item4, 1784);

            t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, t.Item1.ToString(), t.Item2.ToString(), "1660", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(t.Item1, 1683);
            Assert.AreEqual(t.Item2, 6);
            Assert.AreEqual(t.Item3, 0);
            Assert.AreEqual(t.Item4, 1660);

            t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, t.Item1.ToString(), t.Item2.ToString(), "1966", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(t.Item1, 1695);
            Assert.AreEqual(t.Item2, 7);
            Assert.AreEqual(t.Item3, 0);
            Assert.AreEqual(t.Item4, 1966);
        }

        [TestMethod]
        public void UudenPelaajanOttelutKerralla1()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 1695);
            Assert.AreEqual(t.Item2, 7);
            Assert.AreEqual(t.Item3, 3 * 2);
            Assert.AreEqual(t.Item4, 1724); // keskivahvuus
            Assert.AreEqual(t.Item5, 7);    // seitsemän vastustajaa
            Assert.AreEqual(t.Item6, 199);  // odotustulos 1,99*100
        }

        [TestMethod]
        public void UudenPelaajanOttelutKerralla2()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "3 1525 1441 1973 1718 1784 1660 1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 1695);
            Assert.AreEqual(t.Item2, 7);
            Assert.AreEqual(t.Item3, 3 * 2);
            Assert.AreEqual(t.Item4, 1724);// keskivahvuus
        }

        // Tässä lasketaan samat ottelut kuin uudelle pelaajalle, mutta vanhan pelaajan kaavalla (pelimäärä "")
        [TestMethod]
        public void SamatOttelutKuinUudella1() // Turnauksen tulos lasketaan otteluista
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "", "+1525 +1441 -1973 +1718 -1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 1571);
            Assert.AreEqual(t.Item2, Vakiot.PELIMAARA_TYHJA);  // pelimäärää ei laskettu
            Assert.AreEqual(t.Item3, 3 * 2);
            Assert.AreEqual(t.Item4, 1724); // keskivahvuus
            Assert.AreEqual(t.Item5, 7);    // seitsemän vastustajaa
            Assert.AreEqual(t.Item6, 199);  // odotustulos 1,99*100
        }

        [TestMethod]
        public void SamatOttelutKuinUudella2() // Turnauksen tulos annettu numerona
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "", "3 1525 1441 1973 1718 1784 1660 1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 1571);
            Assert.AreEqual(t.Item2, Vakiot.PELIMAARA_TYHJA);  // pelimäärää ei laskettu
            Assert.AreEqual(t.Item3, 3 * 2);
            Assert.AreEqual(t.Item4, 1724);
            Assert.AreEqual(t.Item5, 7);    // seitsemän vastustajaa
            Assert.AreEqual(t.Item6, 199);  // odotustulos 1,99*100
        }

        // Kolme tapaa syöttää ottelun tulos
        [TestMethod]
        public void TulosPainikkeilla()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1800", "", "1900", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(t.Item1, 1823);
            Assert.AreEqual(t.Item6, 36);   // odotustulos 0,36*100
        }

        [TestMethod]
        public void TulosSelossa()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1800", "", "+1900", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 1823);
            Assert.AreEqual(t.Item6, 36);   // odotustulos 0,36*100
        }

        [TestMethod]
        public void TulosNumeronaEnnenSeloa()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1800", "", "1.0 1900", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 1823);
            Assert.AreEqual(t.Item6, 36);   // odotustulos 0,36*100
        }

        // Merkkijonoissa ylimääräisiä välilyöntejä
        [TestMethod]
        public void UudenPelaajanOttelutValilyonteja()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "    1525  ", "0  ", "     +1525  +1441           -1973 +1718    -1784 -1660     -1966   ", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 1695);
            Assert.AreEqual(t.Item2, 7);
            Assert.AreEqual(t.Item3, 3 * 2);
            Assert.AreEqual(t.Item4, 1724);
        }

        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksesta()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 2033);
            Assert.AreEqual(t.Item2, Vakiot.PELIMAARA_TYHJA);  // pelimäärää ei laskettu
            Assert.AreEqual(t.Item3, (int)(10.5F * 2));
            Assert.AreEqual(t.Item4, 1827);  // (1977+2013+1923+1728+1638+1684+1977+2013+1923+1728+1638+1684)/12 = 1827,167
            Assert.AreEqual(t.Item5, 12);    // 12 vastustajaa
            Assert.AreEqual(t.Item6, 840);   // odotustulos 8,40*100
        }

        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "75", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 2033);
            Assert.AreEqual(t.Item2, 87);    // 75 + 12 ottelua = 87
            Assert.AreEqual(t.Item3, (int)(10.5F * 2));
            Assert.AreEqual(t.Item4, 1827);
            Assert.AreEqual(t.Item5, 12);
            Assert.AreEqual(t.Item6, 840);   // odotustulos 8,40*100
        }

        [TestMethod]
        public void ShakinVahvuuslukuTurnauksesta()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1996", "", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 2050);
            Assert.AreEqual(t.Item2, Vakiot.PELIMAARA_TYHJA);  // pelimäärää ei laskettu
        }

        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void ShakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1996", "150", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, 2050);
            Assert.AreEqual(t.Item2, 162);   // 150 + 12 ottelua  = 162
        }


        // --------------------------------------------------------------------------------
        // Testataan virheellisiä syötteitä, joista pitää saada virheen mukainen virhestatus
        // --------------------------------------------------------------------------------

        // Testataan virheellinen syöte, tässä virheellinen oma vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteOmaSELO()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "15zz5", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(t.Item1, Vakiot.SYOTE_VIRHE_OMA_SELO);
        }

        // Testataan virheellinen syöte, tässä virheellinen vastustajan vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteVastustajanSELO()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "c5sdffew25", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(t.Item1, Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO);
        }

        // Pelimäärä virheellinen, annettu liian suureksi
        [TestMethod]
        public void VirheellinenSyoteOmaPelimaara()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "123456", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(t.Item1, Vakiot.SYOTE_VIRHE_PELIMAARA);
        }

        // Ei ole annettu ottelun tulosta valintanapeilla tappio, tasapeli tai voitto
        [TestMethod]
        public void VirheellinenSyoteEiTulosta()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1600", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, Vakiot.SYOTE_VIRHE_BUTTON_TULOS);
        }

        // Virheellinen yksittäinen tulos turnauksen tuloksissa. Oltava + (voitto), - (tappio) tai = (tasan).
        [TestMethod]
        public void VirheellinenSyoteTurnauksessaVirheellinenTulos()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "+1525 +1441 -1973 +1718 /1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, Vakiot.SYOTE_VIRHE_YKSITTAINEN_TULOS);
        }

        // Annettu isompi pistemäärä (20) kuin mitä on otteluita (12 kpl)
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos1()  
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "20 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS);
        }

        // Annettu isompi pistemäärä (99) kuin mitä on otteluita (12 kpl)
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos2()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "99 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(t.Item1, Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS);
        }

        // Annettu negatiivinen pistemäärä.
        // Palautuu ilmoituksena virheellisestä vastustajan selosta, kun ensimmäinen 
        // luku käsitellään numerona eikä tarkistuksessa voida tietää, kumpaa on tarkoitettu.
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos3()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "-6 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            //Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS);
            Assert.AreEqual(t.Item1, Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO);
        }

        // Annettu isompi pistemäärä (150) kuin mitä on otteluita (12 kpl)
        // Palautuu ilmoituksena virheellisestä vastustajan selosta, kun ensimmäinen 
        // luku käsitellään numerona eikä tarkistuksessa voida tietää, kumpaa on tarkoitettu.
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos4()
        {
            var t = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "150 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            //Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS);
            Assert.AreEqual(t.Item1, Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO);
        }


        // --------------------------------------------------------------------------------
        // Testauksen apurutiini
        // --------------------------------------------------------------------------------

        // Tuloksista voidaan tarkistaa
        //      1) laskettu vahvuusluku
        //      2) uusi pelimäärä
        //      3) turnauksen pistemäärä/tulos, huom! kaksinkertaisena jotta saadaan kokonaislukuna (esim. 1,5 tulee lukuna 3)
        //      4) vastustajien keskivahvuus
        //      5) vastustajien lukumäärä
        //      6) odotustulos
        //
        // Tietorakenteesta saisi otettua myös muitakin laskettuja tietoja tarkistettavaksi, ks. Tulokset.cs
        //
        // Virhetilanteessa palautetaan vain virhestatus
        //
        // Käytetään Tuple:n aiempaa versiota, koska Visual Studio Community 2015:ssa ei ole käytössä C# 7.0:aa
        private Tuple<int, int, int, int, int, int> Testaa(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            SelolaskuriOperations so = new SelolaskuriOperations();
            Syotetiedot syotetiedot = new Syotetiedot(aika, selo, pelimaara, vastustajat, tulos);
            int status;

            if ((status = so.TarkistaSyote(syotetiedot)) != Vakiot.SYOTE_STATUS_OK) {
                // Virhestatus
                return Tuple.Create(status, 0, 0, 0, 0, 0);
            } else {
                // Syötteet OK, joten voidaan edetä laskentaan, saadaan Selopelaaja tulokset 
                Selopelaaja tulokset = so.SuoritaLaskenta(syotetiedot);

                // Lasketut tiedot tarkastettavaksi
                return Tuple.Create(tulokset.UusiSelo, tulokset.UusiPelimaara,
                                    tulokset.TurnauksenTulos, tulokset.TurnauksenKeskivahvuus,
                                    tulokset.VastustajienLkm, tulokset.Odotustulos);
            }
        }
    }  
}
