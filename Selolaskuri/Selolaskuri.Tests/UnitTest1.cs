//
// Yksikkötestaukset
//
// Luotu 10.6.2018 Ismo Suihko
//
// TOIMII!! Nyt on helppo tarkistaa muutoksien jälkeen, onko syötteen tarkastus ja laskenta kunnossa!
// Yksi virhekin tuli korjattua. Pelimäärä saattoi joillakin syötteillä vaikuttaa tulokseen.
//
// Tuloksista voidaan tarkistaa
//      1) lasketun vahvuusluvun
//      2) uuden pelimäärän
//      3) turnauksen pistemäärän, huom! kaksinkertaisena jotta saadaan kokonaislukuna (esim. 1,5 tulee lukuna 3)
//      4) vastustajien keskivahvuuden
//      5) vastustajien lukumäärän
//      6) odotustuloksen tai niiden summan, jos useita otteluita
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
//  11.6.2018  Järjestetty aiempia testitapauksia ja lisätty uusia
//  15.6.2018  Lisätty tarkistettavia tietoja: pistemäärä ja keskivahvuus
//  17.6.2018  Lisätty tarkistettavia tietoja: vastustajien lkm, odotustulos
//             Odotustulosta ei näytetä uuden pelaajan tuloksissa, mutta sekin on laskettu ja voidaan tarkistaa
//             Myös lisätty testitapauksia (turnauksen tuloksen virheet). Nyt niitä on 22 kpl eli aika kattavasti.
// 
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
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(tulokset.Item1, 1725);  // uusi vahvuusluku
            Assert.AreEqual(tulokset.Item2, 1);     // uusi pelimäärä 0+1=1
            Assert.AreEqual(tulokset.Item3, 1 * 2); // tulos voitto (tulee kokonaislukuna 2)
            Assert.AreEqual(tulokset.Item4, 1525);  // keskivahvuus
            Assert.AreEqual(tulokset.Item5, 1);     // yksi vastustaja
            Assert.AreEqual(tulokset.Item6, 50);    // 0,50*100  odotustulos palautuu 100-kertaisena

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1441", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(tulokset.Item1, 1683);
            Assert.AreEqual(tulokset.Item2, 2);     // uusi pelimäärä 1+1=2
            Assert.AreEqual(tulokset.Item3, 1 * 2);
            Assert.AreEqual(tulokset.Item4, 1441);
            Assert.AreEqual(tulokset.Item6, 84);  // 0,84*100

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1973", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(tulokset.Item1, 1713);
            Assert.AreEqual(tulokset.Item2, 3);
            Assert.AreEqual(tulokset.Item3, 0);
            Assert.AreEqual(tulokset.Item4, 1973);
            Assert.AreEqual(tulokset.Item6, 16);  // 0,16*100

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1718", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(tulokset.Item1, 1764);
            Assert.AreEqual(tulokset.Item2, 4);
            Assert.AreEqual(tulokset.Item3, 1 * 2);
            Assert.AreEqual(tulokset.Item4, 1718);

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1784", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(tulokset.Item1, 1728);
            Assert.AreEqual(tulokset.Item2, 5);
            Assert.AreEqual(tulokset.Item3, 0);
            Assert.AreEqual(tulokset.Item4, 1784);

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1660", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(tulokset.Item1, 1683);
            Assert.AreEqual(tulokset.Item2, 6);
            Assert.AreEqual(tulokset.Item3, 0);
            Assert.AreEqual(tulokset.Item4, 1660);

            tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, tulokset.Item1.ToString(), tulokset.Item2.ToString(), "1966", Vakiot.OttelunTulos_enum.TULOS_TAPPIO);
            Assert.AreEqual(tulokset.Item1, 1695);
            Assert.AreEqual(tulokset.Item2, 7);
            Assert.AreEqual(tulokset.Item3, 0);
            Assert.AreEqual(tulokset.Item4, 1966);
        }

        [TestMethod]
        public void UudenPelaajanOttelutKerralla1()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "+1525 +1441 -1973 +1718 -1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1695);
            Assert.AreEqual(tulokset.Item2, 7);
            Assert.AreEqual(tulokset.Item3, 3 * 2);
            Assert.AreEqual(tulokset.Item4, 1724);
            Assert.AreEqual(tulokset.Item5, 7);    // seitsemän vastustajaa
            Assert.AreEqual(tulokset.Item6, 199);  // 1,99*100
        }

        [TestMethod]
        public void UudenPelaajanOttelutKerralla2()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "3 1525 1441 1973 1718 1784 1660 1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1695);
            Assert.AreEqual(tulokset.Item2, 7);
            Assert.AreEqual(tulokset.Item3, 3 * 2);
            Assert.AreEqual(tulokset.Item4, 1724);
        }

        // Tässä lasketaan samat ottelut kuin uudelle pelaajalle, mutta vanhan pelaajan kaavalla (pelimäärä "")
        [TestMethod]
        public void SamatOttelutKuinUudella1()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "", "+1525 +1441 -1973 +1718 -1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1571);
            Assert.AreEqual(tulokset.Item2, 0);    // pelimäärää ei ole laskettu
            Assert.AreEqual(tulokset.Item3, 3 * 2);
            Assert.AreEqual(tulokset.Item4, 1724);
            Assert.AreEqual(tulokset.Item5, 7);    // seitsemän vastustajaa
            Assert.AreEqual(tulokset.Item6, 199);  // 1,99*100
        }

        [TestMethod]
        public void SamatOttelutKuinUudella2()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "", "3 1525 1441 1973 1718 1784 1660 1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1571);
            Assert.AreEqual(tulokset.Item2, 0);    // pelimäärää ei ole laskettu
            Assert.AreEqual(tulokset.Item3, 3 * 2);
            Assert.AreEqual(tulokset.Item4, 1724);
            Assert.AreEqual(tulokset.Item5, 7);    // seitsemän vastustajaa
            Assert.AreEqual(tulokset.Item6, 199);  // 1,99*100
        }

        // Kolme tapaa syöttää ottelun tulos
        [TestMethod]
        public void TulosPainikkeilla()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1800", "", "1900", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(tulokset.Item1, 1823);
            Assert.AreEqual(tulokset.Item6, 36);   // 0,36*100
        }

        [TestMethod]
        public void TulosSelossa()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1800", "", "+1900", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1823);
            Assert.AreEqual(tulokset.Item6, 36);   // 0,36*100
        }

        [TestMethod]
        public void TulosNumeronaEnnenSeloa()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1800", "", "1.0 1900", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1823);
            Assert.AreEqual(tulokset.Item6, 36);   // 0,36*100
        }

        // Merkkijonoissa ylimääräisiä välilyöntejä
        [TestMethod]
        public void UudenPelaajanOttelutValilyonteja()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "    1525  ", "0  ", "     +1525  +1441           -1973 +1718    -1784 -1660     -1966   ", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 1695);
            Assert.AreEqual(tulokset.Item2, 7);
            Assert.AreEqual(tulokset.Item3, 3 * 2);
            Assert.AreEqual(tulokset.Item4, 1724);
        }


        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksesta()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2033);
            Assert.AreEqual(tulokset.Item2, 0);  // pelimäärä 0, koska sitä ei annettu ("") eikä siten laskettu (nolla ei voi olla laskettukaan tulos)
            Assert.AreEqual(tulokset.Item3, (int)(10.5F * 2));
            Assert.AreEqual(tulokset.Item4, 1827);  // (1977+2013+1923+1728+1638+1684+1977+2013+1923+1728+1638+1684)/12 = 1827,167
            Assert.AreEqual(tulokset.Item5, 12);    // 12 vastustajaa
            Assert.AreEqual(tulokset.Item6, 840);   // 8,40*100
        }

        // Testataan eri pelimäärillä, ettei tulos riipu pelimäärästä silloin kun ei ole uuden pelaajan laskenta
        [TestMethod]
        public void PikashakinVahvuuslukuTurnauksestaPelimaaralla()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "75", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2033);
            Assert.AreEqual(tulokset.Item2, 87);    // 75 + 12 ottelua = 87
            Assert.AreEqual(tulokset.Item3, (int)(10.5F * 2));
            Assert.AreEqual(tulokset.Item4, 1827);
            Assert.AreEqual(tulokset.Item5, 12);
            Assert.AreEqual(tulokset.Item6, 840);   // 8,40*100
        }

        [TestMethod]
        public void ShakinVahvuuslukuTurnauksesta()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1996", "", "10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, 2050);
            Assert.AreEqual(tulokset.Item2, 0);
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
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "15zz5", "0", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_OMA_SELO);
        }

        // Testataan virheellinen syöte, tässä virheellinen vastustajan vahvuusluku
        [TestMethod]
        public void VirheellinenSyoteVastustajanSELO()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "c5sdffew25", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_VAST_SELO);
        }

        // Pelimäärä virheellinen, annettu liian suureksi
        [TestMethod]
        public void VirheellinenSyoteOmaPelimaara()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "123456", "1525", Vakiot.OttelunTulos_enum.TULOS_VOITTO);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_PELIMAARA);
        }

        // Ei ole annettu ottelun tulosta valintanapeilla tappio, tasapeli tai voitto
        [TestMethod]
        public void VirheellinenSyoteEiTulosta()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "1600", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_BUTTON_TULOS);
        }

        // Virheellinen yksittäinen tulos turnauksen tuloksissa. Oltava + (voitto), - (tappio) tai = (tasan).
        [TestMethod]
        public void VirheellinenSyoteTurnauksessaVirheellinenTulos()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN, "1525", "0", "+1525 +1441 -1973 +1718 /1784 -1660 -1966", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_YKSITTAINEN_TULOS);
        }

        // Annettu isompi pistemäärä (20) kuin mitä on otteluita (12 kpl)
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos1()  
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "20 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS);
        }

        // Annettu isompi pistemäärä (99) kuin mitä on otteluita (12 kpl)
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos2()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "99 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS);
        }

        // Annettu negatiivinen pistemäärä.
        // Palautuu ilmoituksena virheellisestä vastustajan selosta, kun ensimmäinen 
        // luku käsitellään numerona eikä tarkistuksessa voida tietää, kumpaa on tarkoitettu.
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos3()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "-6 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            //Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_VAST_SELO);
        }

        // Annettu isompi pistemäärä (150) kuin mitä on otteluita (12 kpl)
        // Palautuu ilmoituksena virheellisestä vastustajan selosta, kun ensimmäinen 
        // luku käsitellään numerona eikä tarkistuksessa voida tietää, kumpaa on tarkoitettu.
        [TestMethod]
        public void VirheellinenSyoteTurnauksenTulos4()
        {
            var tulokset = Testaa(Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN, "1996", "", "150 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684", Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            //Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS);
            Assert.AreEqual(tulokset.Item1, Vakiot.SYOTE_VIRHE_VAST_SELO);
        }



        // Testauksen apurutiini
        //
        // Tuloksista voidaan tarkistaa
        //      1) lasketun vahvuusluvun
        //      2) uuden pelimäärän
        //      3) turnauksen pistemäärän, huom! kaksinkertaisena jotta saadaan kokonaislukuna (esim. 1,5 tulee lukuna 3)
        //      4) vastustajien keskivahvuuden
        //      5) vastustajien lukumäärän
        //      6) odotustuloksen tai niiden summan, jos useita otteluita
        // Tietorakenteesta saisi otettua myös muitakin laskettuja tietoja tarkistettavaksi, kuten vastustajien lukumäärä.
        //
        // Virhetilanteessa palautetaan virhestatus ja muuten nollaa
        //
        // Käytetään Tuple:n aiempaa versiota, koska Visual Studio Community 2015:ssa ei ole käytössä C# 7.0:aa
        public Tuple<int, int, int, int, int, int> Testaa(Vakiot.Miettimisaika_enum aika, string selo, string pelimaara, string vastustajat, Vakiot.OttelunTulos_enum tulos)
        {
            SelolaskuriOperations so = new SelolaskuriOperations();
            Syotetiedot ottelu = new Syotetiedot(aika, selo, pelimaara, vastustajat, tulos);
            Tulokset t = new Tulokset();
            int status;

            if ((status = so.TarkistaSyote(ottelu)) != Vakiot.SYOTE_STATUS_OK) {
                return Tuple.Create(status, 0, 0, 0, 0, 0);       // Palautetaan vain virhestatus
            } else {
                so.SuoritaLaskenta(ottelu, ref t);   // tarvitaan ref
                // Palautetaan lasketut tiedot tarkastettavaksi
                return Tuple.Create(t.uusiSelo, t.uusiPelimaara, t.laskettuTurnauksenTulos, t.turnauksenKeskivahvuus, t.vastustajienLkm, t.odotustulos);
            }
        }
    }  
}
