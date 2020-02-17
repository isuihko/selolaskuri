//
// Calls the routines in Selopelaaja to check the input fields and calculate results
// The business logic of Selolaskuri
//
// Originally the routines of this class were in SelolaskuriForm.cs
// Created a separate module so that routines can be called from the unit testing too
//
// Public:
//      HaeViimeksiLasketutTulokset  - get the latest calculated selo and game count
//      TarkistaSyote                - check the input data like SELO and number of games, opponents and results
//      SiistiVastustajatKentta      - poistaa ylimääräiset välilyönnit Vastustajat-kentästä
//      SelvitaCSV                   - erota CSV-formaatin merkkijonosta syotetiedot (miettimisaika, oma selo, ...)
//      SelvitaMiettimisaikaCSV      - muuta CSV-formaatissa annettu merkkijono miettimisajaksi
//      SelvitaTulosCSV              - muuta CSV-formaatissa annettu merkkijono ottelun tulokseksi
//      SuoritaLaskenta              - calculate the results
//
// 10.6.2018 Ismo Suihko
// Modifications:
//  11.-12.6.2018 Comments, constants
//  17.-19.6.2018 refactoring, documenting
//  18.-22.7.2018 SuoritaLaskenta() now returns Selopelaaja. Earlier had separate (unnecessary) class for the results
//  15.8.2018     CSV-formaatin tarkistamista
//                Laskennan ja tallennuksen muutokset yksikkötestauksen helpottamiseksi (=1234 on eri tapaus kuin 1234).
//  16.-17.2.2020 Calculation for new player so that after certain defined point can continue with normal old player calculations
//

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions; // Regex rx, rx.Replace (remove extra white spaces)

namespace SelolaskuriLibrary {
    // SelolaskuriOperations must be public to be used in Selolaskuri.Tests
    // Called from
    //    -SelolaskuriForms button handling
    //    -Unit Tests
    public class SelolaskuriOperations
    {
        Selopelaaja selopelaaja;   // store and return the results, calculations

        // Initialize calculation, clear selopelaaja etc
        public SelolaskuriOperations()
        {
            selopelaaja = new Selopelaaja();
        }

        // Palautetaan laskennasta saadut arvot, selo ja pelimäärä
        public int HaeViimeksiLaskettuSelo()
        {
            int i = selopelaaja.UusiSelo;
            if (i == 0)
                i = Vakiot.UUDEN_PELAAJAN_ALKUSELO;
            return i;
        }

        public int HaeViimeksiLaskettuPelimaara()
        {
            int i = selopelaaja.UusiPelimaara;
            return i;
        }

        // TarkistaSyote
        //
        // Kutsuttu: 
        //      -SelolaskuriForm.cs
        //      -Selolaskuri.Tests/UnitTest1.cs
        //
        // Tarkistaa
        //      -miettimisaika-valintanapit (ei voi olla virhettä)
        //      -oma SELO eli nykyinen vahvuusluku (onko kelvollinen numero)
        //      -oma pelimäärä (kelvollinen numero tai tyhjä)
        //      -vastustajan SELO (onko numero) tai vastustajien SELOT (onko turnauksen tulos+selot tai selot tuloksineen)
        //      -yhtä ottelua syötettäessä tuloksen valintanapit (jos yksi vastustaja, niin tulos pitää valita)
        //
        // Syotetiedot syotteet  = oma nykyinen selo ja pelimäärä, vastustajan selo, ottelun tulos sekä merkkijono
        //
        // Tuloksena
        //    syotteet.ottelut sisältää listan vastustajista tuloksineen: ottelu(selo, tulos) 
        //    syotteet.VastustajanSeloYksittainen on joko yhden vastustajan selo tai 0 (jos monta ottelua)
        //
        // Virhetilanteet:
        //    Kenttiä tarkistetaan yo. järjestyksessä ja lopetetaan, kun kohdataan ensimmäinen virhe.
        //    Palautetaan tarkka virhestatus ja virheilmoitukset näytetään ylemmällä tasolla.
        //
        public int TarkistaSyote(Syotetiedot syotteet)
        {
            int tulos; // = Vakiot.SYOTE_STATUS_OK;

            // tyhjennä ottelulista, johon tallennetaan vastustajat tuloksineen
            //syotteet.ottelut.Tyhjenna();  Ei tarvitse, kun aiempi new Syotetiedot() tyhjentää

            // ************ TARKISTA SYÖTE ************

            do {
                // ENSIN TARKISTA MIETTIMISAIKA.
                if ((tulos = TarkistaMiettimisaika(syotteet.Miettimisaika)) == Vakiot.SYOTE_VIRHE_MIETTIMISAIKA_CSV)
                    break;

                // Hae ensin oma nykyinen vahvuusluku ja pelimäärä
                if ((tulos = TarkistaOmaSelo(syotteet.AlkuperainenSelo_str)) == Vakiot.SYOTE_VIRHE_OMA_SELO)
                    break;
                syotteet.AlkuperainenSelo = tulos;

                if ((tulos = TarkistaPelimaara(syotteet.AlkuperainenPelimaara_str)) == Vakiot.SYOTE_VIRHE_PELIMAARA)
                    break;
                syotteet.AlkuperainenPelimaara = tulos;  // Voi olla PELIMAARA_TYHJA tai numero >= 0

                //    JOS YKSI OTTELU,   saadaan sen yhden vastustajan vahvuusluku, eikä otteluja ole listassa.
                //      ottelumäärän tarkistamisen jälkeen tässä tehdään yhden ottelun lista
                //    JOS MONTA OTTELUA, palautuu 0 ja ottelut on tallennettu tuloksineen listaan
                //
                //    JOS MONTA OTTELUA ja VÄLISSÄ '/'-merkki, NIIN SYÖTETTY ENSIN UUDEN PELAAJAN LASKENTA JA SITTEN NORMAALI LASKENTA
                //    Tällöin syotteet,.AlkuperainenPelimaara oltava enintään 10

                selopelaaja.UudenPelaajanPelitLKM = 0; // XXX: oletus, ei kahta eri laskentaa

                if ((tulos = TarkistaVastustajanSelo(syotteet.Ottelut, syotteet.VastustajienSelot_str)) < Vakiot.SYOTE_STATUS_OK)
                    break;

                if (selopelaaja.UudenPelaajanPelitLKM > 0) {
                    if (syotteet.AlkuperainenPelimaara > 10) {
                        // ei voinut olla uuden pelaajan laskenta, jos alkuperäinen pelimäärä oli yli 10
                        tulos = Vakiot.SYOTE_VIRHE_UUDEN_PELAAJAN_OTTELUT_ENINT_10;
                        break;
                    }
                    if (selopelaaja.UudenPelaajanPelitLKM + syotteet.AlkuperainenPelimaara < 11) {
                        // jos alkuperäinen pelimäärä + nyt uuden pelaajan laskentaan saatu pelimäärä eivät ole vähintään 11
                        tulos = Vakiot.SYOTE_VIRHE_UUDEN_PELAAJAN_OTTELUT_VAHINT_11;
                        break;
                    }
                    syotteet.UudenPelaajanPelitEnsinLKM = selopelaaja.UudenPelaajanPelitLKM;
                }

                // tässä siis voi olla vahvuusluku tai 0
                syotteet.YksiVastustajaTulosnapit = tulos;

                // vain jos otteluita ei jo ole listalla (ja TarkistaVastustajanSelo palautti kelvollisen vahvuusluvun),
                // niin tarkista ottelutuloksen valintanapit -> TarkistaOttelunTulos()
                // ja sitten lisää tämä yksi ottelu listaan!
                if (syotteet.Ottelut.Lukumaara == 0) {
                    //
                    // Vastustajan vahvuusluku on nyt vastustajanSeloYksittainen-kentässä
                    // Haetaan vielä ottelunTulos -kenttään tulospisteet tuplana (0=tappio,1=tasapeli,2=voitto)

                    // Tarvitaan tulos (voitto, tasapeli tai tappio)
                    if ((tulos = TarkistaOttelunTulos(syotteet.OttelunTulos)) == Vakiot.SYOTE_VIRHE_BUTTON_TULOS)
                        break;

                    // Nyt voidaan tallentaa ainoan ottelun tiedot listaan (vastustajanSelo, ottelunTulos), josta
                    // ne on helppo hakea laskennassa.
                    // Myös vastustajanSeloYksittainen jää alustetuksi, koska siitä nähdään että vahvuusluku oli
                    // annettu erikseen, jolloin myös ottelun tuloksen on oltava annettuna valintapainikkeilla.
                    syotteet.Ottelut.LisaaOttelunTulos(syotteet.YksiVastustajaTulosnapit, syotteet.OttelunTulos);
                }

                tulos = Vakiot.SYOTE_STATUS_OK; // syötekentät OK, jos päästy tänne asti ja ottelu/ottelut ovat listassa

            } while (false);

            // Virheen käsittelyt ja virheilmoitus ovat kutsuvissa rutiineissa
            return tulos;
        }


        // ************ TARKISTA MIETTIMISAIKA ************
        //
        // Nämä miettimisajan valintapainikkeet ovat omana ryhmänään paneelissa
        // Aina on joku valittuna, joten ei voi olla virhetilannetta.
        private int TarkistaMiettimisaika(Vakiot.Miettimisaika_enum aika)
        {
            int tulos = Vakiot.SYOTE_STATUS_OK;
            if (aika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_MAARITTELEMATON)
                tulos = Vakiot.SYOTE_VIRHE_MIETTIMISAIKA_CSV;
            return tulos;
        }

        // ************ TARKISTA NYKYINEN SELO ************
        //
        // Tarkista Oma SELO -kenttä, oltava numero ja rajojen sisällä
        // Paluuarvo joko kelvollinen SELO (MIN_SELO .. MAX_SELO) tai negatiivinen virhestatus
        private int TarkistaOmaSelo(string syote)
        {
            bool status = true;
            //int tulos;

            // onko numero ja jos on, niin onko sallittu numero
            if (int.TryParse(syote, out int tulos) == false)
                status = false;
            else if (tulos < Vakiot.MIN_SELO || tulos > Vakiot.MAX_SELO)
                status = false;

            if (!status) {
                tulos = Vakiot.SYOTE_VIRHE_OMA_SELO;
            }
            return tulos;
        }


        // ************ TARKISTA PELIMÄÄRÄ ************
        //
        // Saa olla tyhjä, mutta jos annettu, oltava numero, joka on 0-9999.
        // Jos pelimäärä on 0-10, tullaan käyttämään uuden pelaajan laskentakaavaa.
        // Paluuarvo joko kelvollinen pelimäärä, PELIMAARA_TYHJA tai VIRHE_PELIMAARA.
        private int TarkistaPelimaara(string syote)
        {
            bool status = true;
            int tulos;

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


        // ************ TARKISTA VASTUSTAJAN SELO-KENTTÄ ************
        //
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
        //
        // Jos syote sisältää '/' -merkin, on laskettava sitä edeltävät tulokset uuden pelaajan kaavalla
        // ja sen jälkeen normaalilla laskentakaavalla
        private int TarkistaVastustajanSelo(Ottelulista ottelut, string syote)
        {
            bool status = true;
            int vastustajanSelo = 0;   // palautettava vastustajan selo tai nolla tai virhestatus
            int virhekoodi = 0; 

            bool onko_turnauksen_tulos = false;  // oliko tulos ensimmäisenä?
            float syotetty_tulos = 0F;           // tähän sitten sen tulos desimaalilukuna (esim. 2,5)

            // kentässä voidaan antaa alussa turnauksen tulos, esim. 0.5, 2.0, 2.5, 7.5 eli saadut pisteet
            selopelaaja.SetAnnettuTurnauksenTulos(Vakiot.TURNAUKSEN_TULOS_ANTAMATTA);  // oletus: ei annettu turnauksen tulosta

            // Ensin helpot tarkistukset:
            // 1) Kenttä ei saa olla tyhjä
            if (string.IsNullOrWhiteSpace(syote)) {
                status = false;
                virhekoodi = Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO;
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
                // 6) vahvuusluvut tuloksineen ja välissä '/'-merkki +1624 -1700 / =1685 +1400

                // Apumuuttujat
                int selo1; // = Vakiot.MIN_SELO;
                bool ensimmainen = true;  // ensimmäinen syötekentän numero tai merkkijono

                // Tutki vastustajanSelo_comboBox-kenttä välilyönnein erotettu merkkijono kerrallaan
                foreach (string vastustaja in syote.Split(' ').ToList()) {

                    if (ensimmainen) {
                        // need to use temporary variable because can't modify foreach iteration variable
                        string tempString = vastustaja;
                        
                        // 4) Onko annettu kokonaispistemäärä? (eli useamman ottelun yhteistulos)
                        ensimmainen = false;

                        // Laita molemmat 1.5 ja 1,5 toimimaan, InvariantCulture
                        // Huom! Pilkkua ei voidakaan käyttää, jos halutaan
                        // että CSV-formaatti toimii - pilkulla erotetut arvot
                        if (tempString.Contains(','))  // korvaa pilkku pisteellä...
                            tempString = tempString.Replace(',', '.');

                        // Jos ottelutulon lopussa on puolikas, niin muuta se ".5":ksi, esim. 10½ -> 10.5
                        // Jos ottelutulos on ½, niin tässä se muutetaan "0.5":ksi
                        if (tempString.IndexOf('½') == tempString.Length - 1) {
                            if (tempString.Length > 1)
                                tempString = tempString.Replace("½", ".5");
                            else
                                tempString = "0.5";  // muutetaan suoraan, koska oli pelkästään "½"
                        }

                        if (float.TryParse(tempString, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out syotetty_tulos) == true) {
                            if (syotetty_tulos >= 0.0F && syotetty_tulos <= Vakiot.TURNAUKSEN_TULOS_MAX) {
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
                    } else if (vastustaja.Equals("/")) {
                        if (selopelaaja.UudenPelaajanPelitLKM > 0)
                        {
                            // joko oli kauttamerkki? Ei saa olla kahta!
                            virhekoodi = Vakiot.SYOTE_VIRHE_UUDEN_PELAAJAN_OTTELUT_KAKSI_KAUTTAMERKKIA;
                            status = false;
                            break;
                        }
                        // '/' ei voi olla ensimmäisenä
                        // '/' ei saa olla, jos oli annettu turnauksen tulos
                        // täytyy olla ainakin yksi ottelutulos ja sen jälkeen pitäisi olla (ei pakko) lisää otteluita
                        //
                        // Jos alkuperäinen pelimäärä oli 0, niin on laskettava ainakin 11 peliä uuden kaavalla
                        // tarkistus kutsuvalla tasolla
                        if (onko_turnauksen_tulos || ottelut.Lukumaara < 1) {
                            virhekoodi = Vakiot.SYOTE_VIRHE_UUDEN_PELAAJAN_OTTELUT_ENINT_10;
                            status = false;
                            break;
                        }
                        selopelaaja.UudenPelaajanPelitLKM = ottelut.Lukumaara;
                        continue;
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

                        // Hm... miten tallennus?
                        //
                        //if (onko_turnauksen_tulos) {
                        //    Jos annettu turnauksen tulos, ei merkitystä, koska tehdään uusi laskenta, jossa käytetään turnauksen tulosta
                        //    ottelut.LisaaOttelunTulos(selo1, Vakiot.OttelunTulos_enum.TULOS_EI_ANNETTU);
                        //} else {
                        //    // Tallenna tasapeli, jos tulokset formaatissa +1624 -1700 =1685 +1400, jossa tasapeli
                        //    // on annettu ilman '='-merkkiä eli vaikkapa "+1624 -1700 1685 +1400"
                        //    ottelut.LisaaOttelunTulos(selo1, Vakiot.OttelunTulos_enum.TULOS_TASAPELI);
                        //}

                        // Jos ottelun tulosta ei annettu, niin sitä ei tallenneta
                        // Laskennassa tämä otetaan kuitenkin huomioon tasapelinä, jolloin "+1624 -1700 1685 +1400" menee oikein
                        // Ks. Selopelaaja PelaaOttelu()
                        //
                        // Näin myös yksikkötestauksessa on helpompi tarkistaa tiedot,
                        // koska syötteen "=1234" on eri tapaus kuin "1234".
                        ottelut.LisaaOttelunTulos(selo1, Vakiot.OttelunTulos_enum.TULOS_EI_ANNETTU); // OK

                    } else if (onko_turnauksen_tulos == false && vastustaja.Length == Vakiot.MAX_PITUUS) {
                        // 5)
                        // Erillisten tulosten antaminen hyväksytään vain, jos turnauksen
                        // lopputulosta ei oltu jo annettu (onko_turnauksen_tulos false)

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
                    if ((int)(2 * syotetty_tulos + 0.01F) > 2 * ottelut.Lukumaara) {
                        virhekoodi = Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS;  // tästä oma virheilmoitus
                        //status = false; // no need to clear status any more
                    }
                }
            }

            // Palauta virhekoodi tai selvitetty yksittäisen vastustajan selo (joka on 0, jos ottelut listassa)
            return virhekoodi < 0 ? virhekoodi : vastustajanSelo;
        }

        // Vastustajat-kentän siistiminen
        //
        // Jo tehty .Trim() eli poistettu alusta ja lopusta välilyönnit
        // Poista ylimääräiset välilyönnit, korvaa yhdellä  "     " -> " "
        // Poista myös mahdolliset välilyönnit pilkkujen molemmilta puolilta: " , " ja ", " -> ","
        public string SiistiVastustajatKentta(string syote)
        {
            string pattern = "\\s+";    // \s = any whitespace, + one or more repetitions
            string replacement = " ";   // tilalle vain yksi välilyönti
            Regex rx = new Regex(pattern);
            string uusi = rx.Replace(syote, replacement);

            // There could still be extra spaces which would not be OK with CSV format
            // E.g. "5 ,1525, 0 , 1.5 1600 1712" -> "5,1525,0,1.5 1600 1712"
            pattern = "\\s?,\\s?";      // " , "  tai ", " tai " ,"
            replacement = ",";       // tilalle pilkku ilman välilyöntejä eli ","
            rx = new Regex(pattern);
            return rx.Replace(uusi, replacement);
        }

        //
        // Used from the form. If there are only 2 or 3 values in CSV format, also thinking time from the form is needed.
        //
        // Used from the unit tests for CSV. If there are only 2 or 3 values in CSV format, default thinking time in parameter aika is set to 90 minutes.
        //
        public Syotetiedot SelvitaCSV(Vakiot.Miettimisaika_enum aika, string csv)
        {
            // poista ylimääräiset välilyönnit, korvaa yhdellä
            // poista myös mahdolliset välilyönnit pilkkujen molemmilta puolilta
            csv = SiistiVastustajatKentta(csv.Trim());
            List<string> data = csv.Split(',').ToList();

            if (data.Count == 5) {
                return new Syotetiedot(this.SelvitaMiettimisaikaCSV(data[0]), data[1], data[2], data[3], this.SelvitaTulosCSV(data[4]));
            } else if (data.Count == 4) {
                // viimeinen osa voi sisältää vastustajat tuloksineen tai jos alkuperäinen pelimäärä on enintään 10, 
                // niin ensin lasketaan uuden pelaajan kaavalla ja loput "/"-merkin jälkeen menevät normaalilaskentaan
                //  "90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966 +1321 -1678 -1864 -1944 / -1995 +1695 -1930 1901",
                return new Syotetiedot(this.SelvitaMiettimisaikaCSV(data[0]), data[1], data[2], data[3], Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            } else if (data.Count == 3) {
                return new Syotetiedot(aika, data[0], data[1], data[2], Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            } else if (data.Count == 2) {
                return new Syotetiedot(aika, data[0], "", data[1], Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
            } else {
                return null; // CSV FORMAT ERROR, ILLEGAL DATA
            }
        }

        // Miettimisaika, vain minuutit, esim. "5" tai "90"
        // Oltava kokonaisluku ja vähintään 1 minuutti
        public Vakiot.Miettimisaika_enum SelvitaMiettimisaikaCSV(string s)
        {
            Vakiot.Miettimisaika_enum aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_MAARITTELEMATON;
            // define here to be compatible with Visual Studio 2015
            if (int.TryParse(s, out int temp) == true)
            {
                if (temp < 1)
                {
                    // ei voida pelata ilman miettimisaikaa
                    // jo asetettu aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_MAARITTELEMATON;
                }
                else if (temp <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN)
                    aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN;
                else if (temp <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN)
                    aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN;
                else if (temp <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN)
                    aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN;
                else
                    aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;
            }
            return aika;
        }

        // Yksittäisen ottelun tulos joko "0", "0.0", "0,0", "0.5", "0,5", "1/2", "½" (alt-171), "1", "1.0" tai "1,0"
        // Toistaiseksi CSV-formaatin tuloksissa voi käyttää vain desimaalipistettä, joten ei voida syöttää 
        // tuloksia pilkun kanssa kuten "0,0", "0,5" ja "1,0". Tarkistetaan ne kuitenkin varalta.
        public Vakiot.OttelunTulos_enum SelvitaTulosCSV(string s)
        {
            Vakiot.OttelunTulos_enum tulos = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;
            if (s.Equals("0") || s.Equals("0.0") || s.Equals("0,0"))
                tulos = Vakiot.OttelunTulos_enum.TULOS_TAPPIO;
            else if (s.Equals("0.5") || s.Equals("0,5") || s.Equals("1/2") || s.Equals("½"))
                tulos = Vakiot.OttelunTulos_enum.TULOS_TASAPELI;
            else if (s.Equals("1") || s.Equals("1.0") || s.Equals("1,0"))
                tulos = Vakiot.OttelunTulos_enum.TULOS_VOITTO;
            return tulos;
        }

        // Tarkista valitun ottelun tulos -painikkeen kelvollisuus
        //
        // Painikkeesta ei voida saada väärää tulosta. 
        // Mutta jos tulos oli kerrottu CSV-formaatissa, niin siellä on voinut olla virhe,
        // ks. SelvitaTulosCSV()
        //
        // Virhestatus palautetaan, jos oli TULOS_MAARITTELEMATON
        //
        // Tulos TULOS_EI_ANNETTU ei ole virhe, koska se koskee merkkijonossa annettua tulosta,
        // joka tarkoituksella on jätetty antamatta. TULOS_EI_ANNETTU lasketaan tasapelinä.
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
        // Kutsuttu: 
        //      -SelolaskuriForm.cs
        //      -Selolaskuri.Tests/UnitTest1.cs
        //
        // Lisäksi kopioi lasketut tulokset tietorakenteeseen Tulokset, josta ne myöhemmin
        // näytetään (SelolaskuriForm.cs) tai ) yksikkötestauksessa tarkistetaan (Selolaskuri.Tests/UnitTest1.cs)
        //
        public Selopelaaja SuoritaLaskenta(Syotetiedot syotteet)
        {          
            //  *** NYT LASKETAAN ***
            //
            selopelaaja.PelaaKaikkiOttelut(syotteet);   // pelaa kaikki ottelut listalta

            //  *** PALAUTA TULOKSET ***

            return selopelaaja;
         }
    }
}
