//
// Luokka syötteen tarkistamiseen ja laskennan kutsumiseen
//
// Tämän luokan avulla lomake ja käsiteltävät tiedot ja toiminnat erotetaan toisistaan.
// Nyt myös yksikkötestaus (Selolaskuri.Tests) on mahdollista ilman että tarvitaan ikkunaa ja lomaketta.
//
// Luotu 10.6.2018 Ismo Suihko
// Muutokset:
//  11.-12.6.2018 Kommentit, muutama vakio
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions; // Regex rx, rx.Replace (ylimääräisten välilyöntien poisto)

namespace Selolaskuri
{
    // Selolaskuri must be public to be used in Selolaskuri.Tests

    // This has business logic of Selolaskuri
    // Called from SelolaskuriForms button handling
    public class SelolaskuriOperations
    {

        SeloPelaaja shakinpelaaja;
        Ottelulista ottelulista;

        // Initialize calculation etc
        public SelolaskuriOperations()
        {
            shakinpelaaja = new SeloPelaaja(1525, 0); // Uuden pelaajan alkuperäinen selo ja pelimäärä
            ottelulista = new Ottelulista();
        }

        // Käytetään Tuple:n aiempaa versiota, koska Visual Studio Community 2015:ssa ei ole käytössä C# 7.0:aa
        public Tuple<int, int> KopioiLasketutTulokset()
        {
            int selo      = shakinpelaaja.lasketaanSelo;
            int pelimaara = shakinpelaaja.lasketaanPelimaara;

            // XXX: tarkista
            //if (selo == 0) {
            //    // Jos ei ollut vielä laskentaa, niin laitetaan luotaessa käytetyt arvot 1525,0
            //    selo      = shakinpelaaja.lasketaanSelo;
            //    pelimaara = shakinpelaaja.lasketaanPelimaara;
            //}

            return Tuple.Create(selo, pelimaara);
        }


        // tarkistaSyote
        //
        // Tarkistaa
        //      -miettimisaika-valintanapit
        //      -nykyinen SELO eli oma alkuselo
        //      -nykyinen oma pelimäärä
        //      -vastustajan SELO tai vastustajien SELOT
        //      -yhtä ottelua syötettäessä tuloksen valintanapit
        //
        // Tuloksena
        //    Syotetiedot syotteet  = oma nykyinen selo ja pelimäärä, vastustajan selo, ottelun tulos sekä merkkijono
        //    1) Jos syötetty yksi ottelu, niin palautetaan vastustajan selo ja ottelun pisteet
        //    2) Jos syötetty monen ottelun tulokset, niin selo_lista sisältää selot ja tulokset
        //
        // VIrhetilanteet:
        //    Jos jokin syötekenttä on virheellinen, annetaan virheilmoitus, siirrytään ko kenttään ja keskeytetään.
        //    Kenttiä tarkistetaan yo. järjestyksessä ja vain ensimmäisestä virheestä annetaan virheilmoitus.
        //
        //    Virhetarkastukset ja -käsittelyt (ilmoitusikkuna) ovat kutsuttavissa rutiineissa.
        //

        // Virheilmoitukset näytetään kutsuvalla tasolla
        public int TarkistaSyote(Syotetiedot syotteet)
        {
            int tulos = Vakiot.SYOTE_STATUS_OK;

            // tyhjennä ottelulista, johon tallennetaan vastustajat tuloksineen
            ottelulista.Tyhjenna();

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

                // TODO: Tässä mietittävää, kun on kaksi eri tallennustapaa
                //    JOS YKSI OTTELU,   saadaan muuttujassa vastustajanSelo vastustajan vahvuusluku,
                //                       ottelun tulosta ei voida tietää vielä
                //    JOS MONTA OTTELUA, ottelut tallennetaan tuloksineen listaan, jossa tuloksetkin ovat mukana
                //
                if ((tulos = TarkistaVastustajanSelo(syotteet.vastustajienSelot_str)) < Vakiot.SYOTE_STATUS_OK)
                    break;
                syotteet.vastustajanSeloYksittainen = tulos;

                // merkkijonokin talteen sitten kun siitä on poistettu ylimääräiset välilyönnit
                // XXX: ON JO TALLESSA, TARKISTA
                //syotteet.vastustajienSelot_str = vastustajanSelo_comboBox.Text;

                // vain jos otteluita ei ole listalla, niin tarkista ottelutuloksen valintanapit
                if (ottelulista.vastustajienLukumaara == 0) {
                    //
                    // Jos oli vain yksi ottelu, niin vastustajan vahvuusluku on vastustajanSelo-kentässä
                    // Haetaan vielä ottelunTulos -kenttään tulospisteet tuplana (0=tappio,1=tasapeli,2=voitto)

                    // XXX: ottelunTulos HAETTU JO! Täällä virheilmoitus
                    if ((tulos = TarkistaOttelunTulos(syotteet.ottelunTulos)) == Vakiot.SYOTE_VIRHE_BUTTON_TULOS)
                        break;
                    //syotteet.ottelunTulos = (Vakiot.OttelunTulos_enum)tulos;

                    // Nyt voidaan tallentaa ainoan ottelun tiedot (vastustajanSelo, ottelunTulos)
                    ottelulista.LisaaOttelunTulos(syotteet.vastustajanSeloYksittainen, syotteet.ottelunTulos);
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
            // Tarkistettu, ei voi olla virhetilanteita
            return aika;
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

        //
        // tarkista pelimäärä
        // Saa olla tyhjä, mutta jos annettu, oltava numero, joka on 0-9999.
        // Käytetään uuden pelaajan laskentakaavaa, jos pelimäärä on 0-10.
        // Paluuarvo joko kelvollinen pelimäärä, PELIMAARA_TYHJA tai VIRHE_PELIMAARA.
        //
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
        // Jos on useita vastustajia, niin ottelut (selot ja tulokset) tallennetaan listaan
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

        // XXX: Saako tämän moduulin jaettua pienempiin osiin?
        //
        // TODO: Tässä mietittävää, kun on kaksi eri tallennustapaa
        //    JOS YKSI OTTELU,   saadaan muuttujassa vastustajanSelo vastustajan vahvuusluku,
        //                       ottelun tulosta ei voida tietää vielä
        //    JOS MONTA OTTELUA, ottelut tallennetaan tuloksineen listaan, jossa tuloksetkin ovat mukana
        //
        // Paluuarvo joko kelvollinen seloluku, PELIMAARA_TYHJA tai virhestatus VIRHE_SELO.
        private int TarkistaVastustajanSelo(string syote)
        {
            bool status = true;   // XXX: tarkista, onko turha
            int tulos = 0;        // palautettava vastustajan selo
            int virhekoodi = 0;   // tai tästä saatu virhestatus

            bool onko_turnauksen_tulos = false;  // oliko tulos ensimmäisenä?
            float syotetty_tulos = 0F;           // tähän sitten sen tulos desimaalilukuna (esim. 2,5)

            syote = syote.Trim();  // remove leading and trailing white spaces

            // kentässä voidaan antaa alussa turnauksen tulos, esim. 0.5, 2.0, 2.5, 7.5 eli saadut pisteet
            shakinpelaaja.TallennaTurnauksenTulos(-1.0F);  // oletus: ei annettu turnauksen tulosta

            // Ensin helpot tarkistukset:
            // 1) Kenttä ei saa olla tyhjä
            if (string.IsNullOrWhiteSpace(syote)) {
                status = false;
            } else if (syote.Length == Vakiot.SELO_PITUUS) {
                if (int.TryParse(syote, out tulos) == false) {
                    // 2) Jos on annettu neljä merkkiä (esim. 1728), niin sen on oltava numero
                    status = false;
                    virhekoodi = Vakiot.SYOTE_VIRHE_VAST_SELO;
                } else if (tulos < Vakiot.MIN_SELO || tulos > Vakiot.MAX_SELO) {
                    // 3) Numeron on oltava sallitulla lukualueella
                    //    Mutta jos oli OK, niin vastustajanSelo sisältää nyt sallitun vahvuusluvun eikä tulla tähän
                    status = false;
                    virhekoodi = Vakiot.SYOTE_VIRHE_VAST_SELO;
                }
                // NOTE! Yhden ottelun tulosta ei tallenneta listaan, koska tässä ei tiedetä tulosta
                //       Tai tiedettäisiin, jos tarkistettaisiin painikkeet.
                //
                // Jos status = true, niin
                //   vastustajanSelo = annettu vastustajan vahvuusluku ja tulos saadaan tulos-painikkeista
                // Jos status == false, niin virhestatuksena käytetään oletusstatusta VIRHE_SELO

            } else {
                // Jäljellä vielä hankalammat tapaukset:
                // 4) turnauksen tulos+vahvuusluvut, esim. 2,5 1624 1700 1685 1400
                // 5) vahvuusluvut, joissa kussakin tulos  +1624 -1700 =1685 +1400

                // poista sanojen väleistä ylimääräiset välilyönnit
                string pattern = "\\s+";    // \s = any whitespace, + one or more repetitions
                string replacement = " ";   // tilalle vain yksi välilyönti
                Regex rx = new Regex(pattern);
                syote = rx.Replace(syote, replacement);

                // Nyt voidaan jakaa syöte välilyönneillä erotettuihin merkkijonoihin
                List<string> selo_lista = syote.Split(' ').ToList();

                // Apumuuttujat
                int selo1 = Vakiot.MIN_SELO;
                Vakiot.OttelunTulos_enum tulos1 = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
                bool ensimmainen = true;

                // Tutki vastustajanSelo_comboBox-kenttä
                // Tallenna listaan selo_lista vastustajien SELO:t ja tulokset merkkijonona
                foreach (string vastustaja in selo_lista) {
                    if (ensimmainen) {
                        string tempString = vastustaja;
                        // 4) Onko annettu kokonaispistemäärä? (eli useamman ottelun yhteistulos)
                        ensimmainen = false;
                        // Auttavatkohan nuo NumberStyles ja CultureInfo... testaa
                        // XXX: käykö desimaaliluvun formaatti 1,5 tai 1.5?
                        //if (float.TryParse(tulos, out syotetty_tulos)) 

                        // Laita molemmat 1.5 ja 1,5 toimimaan, InvariantCulture
                        if (tempString.Contains(','))  // korvaa pilkku pisteellä...
                            tempString = tempString.Replace(',', '.');
                        if (float.TryParse(tempString, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out syotetty_tulos) == true) {
                            if (syotetty_tulos >= 0.0F && syotetty_tulos <= 99.5F) {
                                // HUOM! Jos tuloksessa on desimaalit väärin, esim. 2.37 tai 0,9,
                                //       niin ylimääräiset desimaalit "pyöristyvät" alas -> 2,0 tai 0,5.
                                onko_turnauksen_tulos = true;
                                shakinpelaaja.TallennaTurnauksenTulos(syotetty_tulos);

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
                            virhekoodi = Vakiot.SYOTE_VIRHE_VAST_SELO;  // -> virheilmoitus, ei ollut numero
                            status = false;
                            break;
                        } else if (selo1 < Vakiot.MIN_SELO || selo1 > Vakiot.MAX_SELO) {
                            virhekoodi = Vakiot.SYOTE_VIRHE_VAST_SELO;  // -> virheilmoitus, ei sallittu numero
                            status = false;
                            break;
                        }

                        // Tallennetaan tasapelinä, ei ollut +:aa tai -:sta
                        ottelulista.LisaaOttelunTulos(selo1, Vakiot.OttelunTulos_enum.TULOS_TASAPELI);

                    } else if (onko_turnauksen_tulos == false && vastustaja.Length == Vakiot.MAX_PITUUS) {
                        // 5)
                        // Erillisten tulosten antaminen hyväksytään vain, jos turnauksen
                        // lopputulosta ei oltu jo annettu (turnauksen_tulos false)
                        if (vastustaja[0] >= '0' && vastustaja[0] <= '9') {
                            // tarkistetaan, voidaan olla annettu viisinumeroinen luku
                            // 10000 - 99999... joten anna virheilmoitus vahvuusluvusta
                            virhekoodi = Vakiot.SYOTE_VIRHE_VAST_SELO;
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
                            virhekoodi = Vakiot.SYOTE_VIRHE_VAST_SELO;  // -> virheilmoitus, ei ollut numero
                            status = false;
                        } else if (selo1 < Vakiot.MIN_SELO || selo1 > Vakiot.MAX_SELO) {
                            status = false;
                            break;
                        }
                        // lisää vahvuusluku ja tulos listaan
                        ottelulista.LisaaOttelunTulos(selo1, tulos1);
                    } else {
                        // pituus ei ollut
                        //   - SELO_PITUUS (esim. 1234) 
                        //   - MAX_PITUUS (esim. +1234) silloin kun tulos voidaan antaa
                        // Tähän tullaan myös, jos turnauksen kokonaistulos oli annettu ennen vahvuuslukuja,
                        // koska silloin annetaan vain pelkät vahvuusluvut ilman yksittäisiä tuloksia.
                        // Ei ole sallittu  2,5 +1624 =1700 -1685 +1400 (oikein on 2,5 1624 1700 1685 1400)
                        virhekoodi = Vakiot.SYOTE_VIRHE_VAST_SELO;
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
                    if ((int)(2 * syotetty_tulos + 0.01F) > 2 * ottelulista.vastustajienLukumaara) {
                        virhekoodi = Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS;  // tästä oma virheilmoitus
                        status = false;
                    }
                }
            }

            // Palauta virhekoodi tai laskettu yksittäisen vastustajan selo (voi olla 0)
            return virhekoodi < 0 ? virhekoodi : tulos;
        }

        // Tarkista ottelun tulos -painikkeet ja tallenna niiden vaikutus
        // pisteet: tappiosta 0, tasapelistä puoli ja voitosta yksi
        //          palautetaan kokonaislukuna 0, 1 ja 2
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
        public void SuoritaLaskenta(Syotetiedot syotteet, ref Tulokset tulokset)
        {
            // asettaa omat tiedot (selo ja pelimäärä) seloPelaaja-luokkaan, nollaa tilastotiedot ym.
            shakinpelaaja.AloitaLaskenta(syotteet);

            //  *** NYT LASKETAAN ***

            shakinpelaaja.PelaaKaikkiOttelut(ottelulista);       // pelaa kaikki ottelut listalta

            //
            // Siirrä kaikki tulokset tietorakenteeseen Tulokset palautettavaksi
            // 

            tulokset.vastustajienLkm         = ottelulista.vastustajienLukumaara;
            tulokset.turnauksenKeskivahvuus  = (int)Math.Round(ottelulista.tallennetutOttelut.Average(x => x.vastustajanSelo));
            tulokset.laskettuTurnauksenTulos = shakinpelaaja.laskettuTurnauksenTulos;
            //tulokset.alkuperainenSelo        = shakinpelaaja.alkuperainenSelo;

            // Laskettiinko yhtä ottelua vai turnausta?
            if (ottelulista.vastustajienLukumaara == 1) {
                tulokset.pisteero = Math.Abs(shakinpelaaja.alkuperainenSelo - tulokset.turnauksenKeskivahvuus);

            } else {
                if (shakinpelaaja.lasketaanPelimaara == Vakiot.PELIMAARA_TYHJA || shakinpelaaja.lasketaanPelimaara > Vakiot.MAX_UUSI_PELAAJA) {
                    //tulokset.odotustulos = shakinpelaaja.odotustuloksienSumma; // 100-kertainen
                }


            }

            tulokset.minSelo = shakinpelaaja.minSelo;
            tulokset.maxSelo = shakinpelaaja.maxSelo;

            // Yhden ottelun odotustulos tai useiden summa
            tulokset.odotustulos = shakinpelaaja.laskettuOdotustulos;  // 100-kertainen, tulostusta varten tullaan jakamaan 100:lla

            tulokset.kerroin = shakinpelaaja.laskettuKerroin;
            tulokset.laskettuSelo = shakinpelaaja.lasketaanSelo;

            if (shakinpelaaja.lasketaanPelimaara != Vakiot.PELIMAARA_TYHJA)
                tulokset.laskettuPelimaara = shakinpelaaja.lasketaanPelimaara;
            else
                tulokset.laskettuPelimaara = 0;
        }
    }
}
