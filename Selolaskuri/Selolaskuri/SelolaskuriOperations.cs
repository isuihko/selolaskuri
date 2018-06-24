//
// Luokka syötteen tarkistamiseen ja laskennan suorittamiseen
//
// Public:
//      HaeViimeksiLasketutTulokset  (käytetty lomakkeesta)
//      TarkistaSyote
//      SuoritaLaskenta
//
// Tämän luokan rutiinit olivat alunperin lomakkeen moduulissa. Kun nämä nyt erotettiin lomakkeesta,
// myös yksikkötestaus (Selolaskuri.Tests) on mahdollista ilman että tarvitaan ikkunaa ja lomaketta.
//
// Varsinainen laskenta suoritetaan kutsumalla luokan Selopelaaja rutiineja.
//
// Luotu 10.6.2018 Ismo Suihko
// Muutokset:
//  11.-12.6.2018 Kommentit, muutama vakio
//  17.-19.6.2018 Järjestelyä, dokumentointia
//

using System;
using System.Linq;
using System.Text.RegularExpressions; // Regex rx, rx.Replace (ylimääräisten välilyöntien poisto)

namespace Selolaskuri
{
    // SelolaskuriOperations must be public to be used in Selolaskuri.Tests

    // This has business logic of Selolaskuri
    // Called from
    //    -SelolaskuriForms button handling
    //    -Unit Tests
    public class SelolaskuriOperations
    {
        SeloPelaaja selopelaaja;

        // Initialize calculation, clear selopelaaja etc
        public SelolaskuriOperations()
        {
            selopelaaja = new SeloPelaaja();
        }

        // Käytetään Tuple:n aiempaa versiota, koska Visual Studio Community 2015:ssa ei ole käytössä C# 7.0:aa
        public Tuple<int, int> HaeViimeksiLasketutTulokset()
        {
            int selo      = selopelaaja.uusiSelo;
            int pelimaara = selopelaaja.uusiPelimaara;

            // Jos ei vielä ollut laskentaa, palautetaan uuden pelaajan alkuselo
            if (selo == 0 && pelimaara == 0)
                selo = Vakiot.UUDEN_PELAAJAN_ALKUSELO;  // tiedetty vakio 1525

            return Tuple.Create(selo, pelimaara);
        }


        // TarkistaSyote
        //
        // Tarkistaa
        //      -miettimisaika-valintanapit (ei voi olla virhettä)
        //      -nykyinen SELO eli oma alkuselo (onko kelvollinen numero)
        //      -nykyinen oma pelimäärä   (kelvollinen numero tai tyhjä)
        //      -vastustajan SELO (onko numero) tai vastustajien SELOT (onko turnauksen tulos+selot tai selot tuloksineen)
        //      -yhtä ottelua syötettäessä tuloksen valintanapit (jos yksi vastustaja, niin tulos pitää valita)
        //
        // Tuloksena
        //    Syotetiedot syotteet  = oma nykyinen selo ja pelimäärä, vastustajan selo, ottelun tulos sekä merkkijono
        //    syotteet.ottelut sisältää listan vastustajista tuloksineen: ottelu(selo, tulos) 
        //
        // Virhetilanteet:
        //    Kenttiä tarkistetaan yo. järjestyksessä ja lopetetaan, kun kohdataan ensimmäinen virhe.
        //    Palautetaan virhestatus ja virheilmoitukset näytetään ylemmällä tasolla.
        //
        public int TarkistaSyote(Syotetiedot syotteet)
        {
            int tulos = Vakiot.SYOTE_STATUS_OK;

            // tyhjennä ottelulista, johon tallennetaan vastustajat tuloksineen
            //syotteet.ottelut.Tyhjenna();  Ei tarvitse, kun aiempi new Syotetiedot() tyhjentää

            // ************ TARKISTA SYÖTE ************

            // ENSIN TARKISTA MIETTIMISAIKA.
            // Jo haettu lomakkeelta eikä tässä voi olla virhettä, joten ei tehdä virhetarkastusta.
            syotteet.miettimisaika = TarkistaMiettimisaika(syotteet.miettimisaika);

            do {
                // Hae ensin oma nykyinen vahvuusluku ja pelimäärä
                if ((tulos = TarkistaNykyinenSelo(syotteet.alkuperainenSelo_str)) == Vakiot.SYOTE_VIRHE_OMA_SELO)
                    break;
                syotteet.alkuperainenSelo = tulos;

                if ((tulos = TarkistaPelimaara(syotteet.alkuperainenPelimaara_str)) == Vakiot.SYOTE_VIRHE_PELIMAARA)
                    break;
                syotteet.alkuperainenPelimaara = tulos;  // Voi olla PELIMAARA_TYHJA tai numero >= 0


                //    JOS YKSI OTTELU,   saadaan muuttujassa vastustajanSeloYksittainen vastustajan vahvuusluku,
                //                       ottelun tulosta ei voida tietää vielä
                //    JOS MONTA OTTELUA, palautuu 0 ja ottelut on tallennettu tuloksineen listaan
                //
                if ((tulos = TarkistaVastustajanSelo(syotteet.ottelut, syotteet.vastustajienSelot_str)) < Vakiot.SYOTE_STATUS_OK)
                    break;
                syotteet.vastustajanSeloYksittainen = tulos; // tässä tulos siis on vahvuusluku

                // vain jos otteluita ei jo ole listalla (ja TarkistaVastustajanSelo palautti kelvollisen vahvuusluvun),
                // niin tarkista ottelutuloksen valintanapit -> TarkistaOttelunTulos()
                if (syotteet.ottelut.tallennetutOttelut.Count == 0) {
                    //
                    // Vastustajan vahvuusluku on nyt vastustajanSeloYksittainen-kentässä
                    // Haetaan vielä ottelunTulos -kenttään tulospisteet tuplana (0=tappio,1=tasapeli,2=voitto)

                    // Tarvitaan tulos (voitto, tasapeli tai tappio)
                    if ((tulos = TarkistaOttelunTulos(syotteet.ottelunTulos)) == Vakiot.SYOTE_VIRHE_BUTTON_TULOS)
                        break;

                    // Nyt voidaan tallentaa ainoan ottelun tiedot listaan (vastustajanSelo, ottelunTulos), josta
                    // ne on helppo hakea laskennassa.
                    // Myös vastustajanSeloYksittainen jää alustetuksi, koska siitä nähdään että vahvuusluku oli
                    // annettu erikseen, jolloin myös ottelun tuloksen on oltava annettuna valintapainikkeilla.
                    syotteet.ottelut.LisaaOttelunTulos(syotteet.vastustajanSeloYksittainen, syotteet.ottelunTulos);
                }

                tulos = Vakiot.SYOTE_STATUS_OK; // syötekentät OK, jos päästy tänne asti

            } while (false);

            // Virheen käsittelyt ja virheilmoitus ovat kutsuvissa rutiineissa
            return tulos;
        }


        // Nämä miettimisajan valintapainikkeet ovat omana ryhmänään paneelissa
        // Aina on joku valittuna, joten ei voi olla virhetilannetta.
        private Vakiot.Miettimisaika_enum TarkistaMiettimisaika(Vakiot.Miettimisaika_enum aika)
        {
            return aika;   // Ei tarkistusta, koska ei voi olla virhetilanteita
        }

        // Tarkista Oma SELO -kenttä, oltava numero ja rajojen sisällä
        // Paluuarvo joko kelvollinen SELO (MIN_SELO .. MAX_SELO) tai negatiivinen virhestatus
        private int TarkistaNykyinenSelo(string syote)
        {
            bool status = true;
            int tulos;

            syote = syote.Trim();  // remove leading and trailing white spaces

            // onko numero ja jos on, niin onko sallittu numero
            if (int.TryParse(syote, out tulos) == false)
                status = false;
            else if (tulos < Vakiot.MIN_SELO || tulos > Vakiot.MAX_SELO)
                status = false;

            if (!status) {
                tulos = Vakiot.SYOTE_VIRHE_OMA_SELO;
            }
            return tulos;
        }

        // tarkista pelimäärä
        // Saa olla tyhjä, mutta jos annettu, oltava numero, joka on 0-9999.
        // Jos pelimäärä on 0-10, tullaan käyttämään uuden pelaajan laskentakaavaa.
        // Paluuarvo joko kelvollinen pelimäärä, PELIMAARA_TYHJA tai VIRHE_PELIMAARA.
        private int TarkistaPelimaara(string syote)
        {
            bool status = true;
            int tulos;

            syote = syote.Trim();  // remove leading and trailing white spaces

            if (string.IsNullOrWhiteSpace(syote)) {
                tulos = Vakiot.PELIMAARA_TYHJA; // Tyhjä kenttä on OK, kun ei ole uusi pelaaja
            } else {
                // onko numero ja jos on, niin onko sallittu numero
                if (int.TryParse(syote, out tulos) == false)
                    status = false;
                else if (tulos < Vakiot.MIN_PELIMAARA || tulos > Vakiot.MAX_PELIMAARA)
                    status = false;

                if (!status) {
                    tulos = Vakiot.SYOTE_VIRHE_PELIMAARA;
                }
            }
            return tulos;
        }

        // Tarkista Vastustajan SELO -kenttä
        // Ottelut (selot ja tulokset) tallennetaan listaan
        //
        // Syöte voi olla annettu kolmella eri formaatilla:
        //  1)  1720   -> ja sitten tulos valintanapeilla
        //  2)  2,5 1624 1700 1685 1400    Eli aloitetaan kokonaispistemäärällä.
        //                                 SELOt ilman erillisiä tuloksia.
        //  3)  +1624 -1700 =1685 +1400    jossa  '+' voitto, '=' tasapeli ja '-' tappio.
        //                                 Tasapeli voidaan myös antaa ilman '='-merkkiä.
        //
        // Yhden ottelun tulos voidaan antaa kolmella tavalla:
        //   1)  1720      ja tulos erikseen valintanapeilla, esim. 1=voitto, 1/2=tasapeli tai 0=tappio
        //   2)  -1720  (tappio), =1720    (tasapeli) tai +1720  (voitto)
        //   3)  0 1720 (tappio), 0.5 1720 (tasapeli) tai 1 1720 (voitto)
        //
        // Kahden tai useamman ottelun tulos voidaan syöttää kahdella eri tavalla
        //   1) 2,5 1624 1700 1685 1400
        //   2) +1624 -1700 =1685 +1400  (Huom! myös -1624 +1700 +1685 1400 laskee saman vahvuusluvun)
        // HUOM! Jos tuloksessa on desimaalit väärin, esim. 2.37 tai 0,9,
        //       niin ylimääräiset desimaalit "pyöristyvät" alas -> 2,0 tai 0,5.
        //
        // Paluuarvo joko kelvollinen seloluku (vain yksi vastustaja annettu), nolla (jos ottelut ovat listassa) tai virhestatus.
        private int TarkistaVastustajanSelo(Ottelulista ottelut, string syote)
        {
            bool status = true;
            int vastustajanSelo = 0;   // palautettava vastustajan selo tai nolla tai virhestatus
            int virhekoodi = 0; 

            bool onko_turnauksen_tulos = false;  // oliko tulos ensimmäisenä?
            float syotetty_tulos = 0F;           // tähän sitten sen tulos desimaalilukuna (esim. 2,5)

            syote = syote.Trim();  // remove leading and trailing white spaces

            // kentässä voidaan antaa alussa turnauksen tulos, esim. 0.5, 2.0, 2.5, 7.5 eli saadut pisteet
            selopelaaja.SetAnnettuTurnauksenTulos(-1.0F);  // oletus: ei annettu turnauksen tulosta

            // Ensin helpot tarkistukset:
            // 1) Kenttä ei saa olla tyhjä
            if (string.IsNullOrWhiteSpace(syote)) {
                status = false;
            } else if (syote.Length == Vakiot.SELO_PITUUS) {
                if (int.TryParse(syote, out vastustajanSelo) == false) {
                    // 2) Jos on annettu neljä merkkiä (esim. 1728), niin sen on oltava numero
                    status = false;
                    virhekoodi = Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO;
                } else if (vastustajanSelo < Vakiot.MIN_SELO || vastustajanSelo > Vakiot.MAX_SELO) {
                    // 3) Numeron on oltava sallitulla lukualueella
                    //    Mutta jos oli OK, niin vastustajanSelo sisältää nyt sallitun vahvuusluvun eikä tulla tähän
                    status = false;
                    virhekoodi = Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO;
                }
                // Jos OK, yhden ottelun tulosta ei kuitenkaan vielä tallenneta listaan
                // vaan nyt palataan tästä funktiosta ja seuraavaksi selvitetään ottelun tulos

            } else {
                // Jäljellä vielä hankalammat tapaukset:
                // 4) turnauksen tulos+vahvuusluvut, esim. 2,5 1624 1700 1685 1400
                // 5) vahvuusluvut, joissa kussakin tulos  +1624 -1700 =1685 +1400

                // poista sanojen väleistä ylimääräiset välilyönnit
                string pattern = "\\s+";    // \s = any whitespace, + one or more repetitions
                string replacement = " ";   // tilalle vain yksi välilyönti
                Regex rx = new Regex(pattern);
                syote = rx.Replace(syote, replacement);

                // Apumuuttujat
                int selo1 = Vakiot.MIN_SELO;
                bool ensimmainen = true;  // ensimmäinen syötekentän numero tai merkkijono

                // Tutki vastustajanSelo_comboBox-kenttä välilyönnein erotettu merkkijono kerrallaan
                foreach (string vastustaja in syote.Split(' ').ToList()) {
                    if (ensimmainen) {
                        // need to use temporary variable because can't modify foreach iteration variable
                        string tempString = vastustaja;
                        
                        // 4) Onko annettu kokonaispistemäärä? (eli useamman ottelun yhteistulos)
                        ensimmainen = false;

                        // Laita molemmat 1.5 ja 1,5 toimimaan, InvariantCulture
                        if (tempString.Contains(','))  // korvaa pilkku pisteellä...
                            tempString = tempString.Replace(',', '.');
                        if (float.TryParse(tempString, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out syotetty_tulos) == true) {
                            if (syotetty_tulos >= 0.0F && syotetty_tulos <= 99.5F) {
                                // HUOM! Jos tuloksessa on desimaalit väärin, esim. 2.37 tai 0,9,
                                //       niin ylimääräiset desimaalit "pyöristyvät" alas -> 2,0 tai 0,5.
                                onko_turnauksen_tulos = true;
                                selopelaaja.SetAnnettuTurnauksenTulos(syotetty_tulos);

                                // alussa oli annettu turnauksen lopputulos, jatka SELOjen tarkistamista
                                // Nyt selojen on oltava ilman tulosmerkintää!
                                continue;
                            }
                            // Jos ei saatu kelvollista lukua, joka käy tuloksena, niin jatketaan
                            // ja katsotaan, saadaanko vahvuusluku sen sijaan (jossa voi olla +/=/-)
                        }
                    }

                    // Tarkista yksittäiset vastustajien vahvuusluvut

                    // merkkijono voi alkaa merkillä '+', '=' tai '-'
                    // Mutta tasapeli voidaan antaa myös ilman '='-merkkiä
                    // Jos oli annettu turnauksen tulos, niin selot on syötettävä näin ilman tulosta
                    if (vastustaja.Length == Vakiot.SELO_PITUUS) {  // numero (4 merkkiä)
                        if (int.TryParse(vastustaja, out selo1) == false) {
                            virhekoodi = Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO;  // -> virheilmoitus, ei ollut numero
                            status = false;
                            break;
                        } else if (selo1 < Vakiot.MIN_SELO || selo1 > Vakiot.MAX_SELO) {
                            virhekoodi = Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO;  // -> virheilmoitus, ei sallittu numero
                            status = false;
                            break;
                        }

                        // Tallennetaan ottelu tasapelinä, ei ollut +:aa tai -:sta
                        ottelut.LisaaOttelunTulos(selo1, Vakiot.OttelunTulos_enum.TULOS_TASAPELI);

                    } else if (onko_turnauksen_tulos == false && vastustaja.Length == Vakiot.MAX_PITUUS) {
                        // 5)
                        // Erillisten tulosten antaminen hyväksytään vain, jos turnauksen
                        // lopputulosta ei oltu jo annettu (turnauksen_tulos false)

                        Vakiot.OttelunTulos_enum tulos1 = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;

                        if (vastustaja[0] >= '0' && vastustaja[0] <= '9') {
                            // tarkistetaan, voidaan olla annettu viisinumeroinen luku
                            // 10000 - 99999... joten anna virheilmoitus vahvuusluvusta
                            virhekoodi = Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO;
                            status = false;
                        } else {
                            // Ensimmäinen merkki kertoo tuloksen
                            switch (vastustaja[0]) {
                                case '+':
                                    tulos1 = Vakiot.OttelunTulos_enum.TULOS_VOITTO;
                                    break;
                                case '=':
                                    tulos1 = Vakiot.OttelunTulos_enum.TULOS_TASAPELI;
                                    break;
                                case '-':
                                    tulos1 = Vakiot.OttelunTulos_enum.TULOS_TAPPIO;
                                    break;
                                default: // ei sallittu tuloksen kertova merkki
                                    virhekoodi = Vakiot.SYOTE_VIRHE_YKSITTAINEN_TULOS;
                                    status = false;
                                    break;
                            }
                        }

                        // jos virhe, pois foreach-loopista
                        if (!status)
                            break;

                        // Selvitä vielä tuloksen perässä oleva numero
                        // tarkista sitten, että on sallitulla alueella
                        if (int.TryParse(vastustaja.Substring(1), out selo1) == false) {
                            virhekoodi = Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO;  // -> virheilmoitus, ei ollut numero
                            status = false;
                        } else if (selo1 < Vakiot.MIN_SELO || selo1 > Vakiot.MAX_SELO) {
                            status = false;
                            break;
                        }

                        // Tallennetaan tulos: vahvuusluku ja +/-/=-merkeistä selvitetty tulos listaan
                        ottelut.LisaaOttelunTulos(selo1, tulos1);
                    } else {
                        // pituus ei ollut
                        //   - SELO_PITUUS (esim. 1234) 
                        //   - MAX_PITUUS (esim. +1234) silloin kun tulos voidaan antaa
                        // Tähän tullaan myös, jos turnauksen kokonaistulos oli annettu ennen vahvuuslukuja,
                        // koska silloin annetaan vain pelkät vahvuusluvut ilman yksittäisiä tuloksia.
                        // Ei ole sallittu  2,5 +1624 =1700 -1685 +1400 (oikein on 2,5 1624 1700 1685 1400)
                        virhekoodi = Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO;
                        status = false;
                        break;
                    }

                    // Oliko asetettu virhe, mutta ei vielä poistuttu foreach-loopista?
                    if (!status)
                        break;

                } // foreach (käydään läpi syötteen numerot)

                // Lisää tarkastuksia
                // 6) Annettu turnauksen tulos ei saa olla suurempi kuin pelaajien lukumäärä
                //    Jos tulos on sama kuin pelaajien lkm, on voitettu kaikki ottelut.
                if (status && onko_turnauksen_tulos) {
                    // Vertailu kokonaislukuina, esim. syötetty tulos 3.5 ja pelaajia 4, vertailu 7 > 8.
                    if ((int)(2 * syotetty_tulos + 0.01F) > 2 * ottelut.tallennetutOttelut.Count) {
                        virhekoodi = Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS;  // tästä oma virheilmoitus
                        status = false;
                    }
                }
            }

            // Palauta virhekoodi tai selvitetty yksittäisen vastustajan selo (joka on 0, jos ottelut listassa)
            return virhekoodi < 0 ? virhekoodi : vastustajanSelo;
        }


        // Tarkista valitun ottelun tulos -painikkeen kelvollisuus
        // Virhestatus palautetaan, jos oli valittu TULOS_MAARITTELEMATON
        //
        private int TarkistaOttelunTulos(Vakiot.OttelunTulos_enum ottelunTulos)
        {
            int tulos = Vakiot.SYOTE_STATUS_OK;

            if (ottelunTulos == Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON) {
                tulos = Vakiot.SYOTE_VIRHE_BUTTON_TULOS;
            }
            return tulos;
        }


        // Laske tulokset, syöte on jo tarkistettu tätä ennen
        //
        // Lisäksi kopioi lasketut tulokset tietorakenteeseen Tulokset, josta ne myöhemmin
        // näytetään (SelolaskuriForm.cs) tai ) yksikkötestauksessa tarkistetaan (Selolaskuri.Tests/UnitTest1.cs)
        //
        public void SuoritaLaskenta(Syotetiedot syotteet, ref Tulokset tulokset)
        {
            //  *** NYT LASKETAAN ***
            //
            selopelaaja.PelaaKaikkiOttelut(syotteet);   // pelaa kaikki ottelut listalta

            //  *** KOPIOI TULOKSET ***

            tulokset.uusiSelo = selopelaaja.uusiSelo;
            tulokset.uusiPelimaara = selopelaaja.uusiPelimaara;
            tulokset.minSelo = selopelaaja.minSelo;
            tulokset.maxSelo = selopelaaja.maxSelo;

            tulokset.odotustulos = selopelaaja.laskettuOdotustulos; // 100-kertainen, tulostusta varten tullaan jakamaan 100:lla
            tulokset.kerroin = selopelaaja.laskettuKerroin;

            tulokset.vastustajienLkm         = syotteet.ottelut.tallennetutOttelut.Count;
            tulokset.turnauksenKeskivahvuus  = (int)Math.Round(syotteet.ottelut.tallennetutOttelut.Average(x => x.vastustajanSelo)); // Linq

            tulokset.laskettuTurnauksenTulos = selopelaaja.laskettuTurnauksenTulos;  // tuplana, jotta on kokonaisuluku
        }
    }
}
