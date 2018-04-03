using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; // Color
using System.Windows.Forms; // MessageBox
using System.Text.RegularExpressions;  // Regex rx, rx.Replace

namespace Selolaskuri
{
    public partial class Form1
    {
        //  LOMAKKEEN KENTTIEN ARVOJEN TARKISTUKSET JA VIRHEILMOITUKSET

        // Nämä miettimisajan valintapainikkeet ovat omana ryhmänään paneelissa
        // Aina on joku valittuna. Oletuksena: vähintään 90 min
        private void tarkista_miettimisaika()
        {
            int aika;

            if (miettimisaika_vah90_Button.Checked)
                aika = vakiot.Miettimisaika_vah90min;
            else if (miettimisaika_60_89_Button.Checked)
                aika = vakiot.Miettimisaika_60_89min;
            else if (miettimisaika_11_59_Button.Checked)
                aika = vakiot.Miettimisaika_11_59min;
            else
                aika = vakiot.Miettimisaika_enint10min;

            shakinpelaaja.miettimisaika = aika;
        }

        // Tarkista Oma SELO -kenttä, oltava numero ja rajojen sisällä
        // Paluuarvo joko kelvollinen SELO (MIN_SELO .. MAX_SELO) tai negatiivinen virhestatus
        private int tarkista_nykyinenselo()
        {
            bool status = true;
            int tulos;

            nykyinenSelo_input.Text = nykyinenSelo_input.Text.Trim();  // remove leading and trailing white spaces

            // onko numero ja jos on, niin onko sallittu numero
            if (int.TryParse(nykyinenSelo_input.Text, out tulos) == false)
                status = false;
            else if (tulos < vakiot.MIN_SELO || tulos > vakiot.MAX_SELO)
                status = false;

            if (!status) {
                string message = String.Format("VIRHE: Nykyisen SELOn oltava numero {0}-{1}.",
                    vakiot.MIN_SELO, vakiot.MAX_SELO);
                nykyinenSelo_input.ForeColor = Color.Red;
                MessageBox.Show(message);
                nykyinenSelo_input.ForeColor = Color.Black;

                // Tyhjennä liian täysi kenttä? Takaisin kenttään ja virhestatus
                if (nykyinenSelo_input.Text.Length > vakiot.MAX_PITUUS)
                    nykyinenSelo_input.Text = "";
                nykyinenSelo_input.Select();
                tulos = vakiot.VIRHE_SELO;
            }
            return tulos;
        }

        //
        // tarkista pelimäärä
        // Saa olla tyhjä, mutta jos annettu, oltava numero, joka on 0-9999.
        // Käytetään uuden pelaajan laskentakaavaa, jos pelimäärä on 0-10.
        // Paluuarvo joko kelvollinen pelimäärä, PELIMAARA_TYHJA tai VIRHE_PELIMAARA.
        //
        private int tarkista_pelimaara()
        {
            bool status = true;
            int tulos;

            pelimaara_input.Text = pelimaara_input.Text.Trim();  // remove leading and trailing white spaces

            if (string.IsNullOrWhiteSpace(pelimaara_input.Text)) {
                tulos = vakiot.PELIMAARA_TYHJA; // Tyhjä kenttä on OK
            } else {
                // onko numero ja jos on, niin onko sallittu numero
                if (int.TryParse(pelimaara_input.Text, out tulos) == false)
                    status = false;
                else if (tulos < vakiot.MIN_PELIMAARA || tulos > vakiot.MAX_PELIMAARA)
                    status = false;

                if (!status) {
                    string message = String.Format("VIRHE: pelimäärän voi olla numero väliltä {0}-{1} tai tyhjä.",
                        vakiot.MIN_PELIMAARA, vakiot.MAX_PELIMAARA);
                    pelimaara_input.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    pelimaara_input.ForeColor = Color.Black;

                    // Tyhjennä liian täysi kenttä? Takaisin kenttään ja virhestatus
                    if (pelimaara_input.Text.Length > vakiot.MAX_PITUUS)
                        pelimaara_input.Text = "";
                    pelimaara_input.Select();
                    tulos = vakiot.VIRHE_PELIMAARA;
                }
            }
            return tulos;
        }

        // Tarkista Vastustajan SELO -kenttä
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
        private int tarkista_vastustajanSelo()
        {
            bool status = true;
            int tulos = 0;           // palautettava vastustajan selo
            int virhekoodi = 0;  // tai tästä saatu virhestatus

            bool onko_turnauksen_tulos = false;  // oliko tulos ensimmäisenä?
            float syotetty_tulos = 0F;           // tähän sitten sen tulos desimaalilukuna (esim. 2,5)
            
            vastustajanSelo_comboBox.Text = vastustajanSelo_comboBox.Text.Trim();  // remove leading and trailing white spaces

            // Ensin helpot tarkistukset:
            // 1) Kenttä ei saa olla tyhjä
            if (string.IsNullOrWhiteSpace(vastustajanSelo_comboBox.Text)) {
                status = false;
            } else if (vastustajanSelo_comboBox.Text.Length == vakiot.SELO_PITUUS) {
                if (int.TryParse(vastustajanSelo_comboBox.Text, out tulos) == false) {
                    // 2) Jos on annettu neljä merkkiä (esim. 1728), niin sen on oltava numero
                    status = false;
                } else if (tulos < vakiot.MIN_SELO || tulos > vakiot.MAX_SELO) {
                    // 3) Numeron on oltava sallitulla lukualueella
                    //    Mutta jos oli OK, niin vastustajanSelo sisältää nyt sallitun vahvuusluvun eikä tulla tähän
                    status = false;
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

                // kentässä voidaan antaa alussa turnauksen tulos, esim. 0.5, 2.0, 2.5, 7.5 eli saadut pisteet
                shakinpelaaja.set_syotetty_turnauksen_tulos(-1.0F);  // oletus: ei annettu

                // poista sanojen väleistä ylimääräiset välilyönnit
                string pattern = "\\s+";    // \s = any whitespace, + one or more repetitions
                string replacement = " ";   // tilalle vain yksi välilyönti
                Regex rx = new Regex(pattern);
                vastustajanSelo_comboBox.Text = rx.Replace(vastustajanSelo_comboBox.Text, replacement);

                // Nyt voidaan jakaa syöte välilyönneillä erotettuihin merkkijonoihin
                List<string> selo_lista = vastustajanSelo_comboBox.Text.Split(' ').ToList();

                // Apumuuttujat
                int selo1 = vakiot.MIN_SELO;
                int tulos1 = 0;
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
                        if (tempString.Contains(','))
                            tempString = tempString.Replace(',', '.');
                        if (float.TryParse(tempString, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out syotetty_tulos) == true)
                        {
                            if (syotetty_tulos >= 0.0F && syotetty_tulos <= 99.5F) {
                                // HUOM! Jos tuloksessa on desimaalit väärin, esim. 2.37 tai 0,9,
                                //       niin ylimääräiset desimaalit "pyöristyvät" alas -> 2,0 tai 0,5.
                                onko_turnauksen_tulos = true;
                                shakinpelaaja.set_syotetty_turnauksen_tulos(syotetty_tulos);

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
                    if (vastustaja.Length == vakiot.SELO_PITUUS) {  // numero (4 merkkiä)
                        if (int.TryParse(vastustaja, out selo1) == false) {
                            virhekoodi = vakiot.VIRHE_SELO;  // -> virheilmoitus, ei ollut numero
                            status = false;
                            break;
                        } else if (selo1 < vakiot.MIN_SELO || selo1 > vakiot.MAX_SELO) {
                            virhekoodi = vakiot.VIRHE_SELO;  // -> virheilmoitus, ei sallittu numero
                            status = false;
                            break;
                        }

                        // Tallennetaan tasapelinä, ei ollut +:aa tai -:sta
                        shakinpelaaja.lista_lisaa_ottelun_tulos(selo1, vakiot.TASAPELIx2);

                    } else if (onko_turnauksen_tulos == false && vastustaja.Length == vakiot.MAX_PITUUS) {
                        // 5)
                        // Erillisten tulosten antaminen hyväksytään vain, jos turnauksen
                        // lopputulosta ei oltu jo annettu (turnauksen_tulos false)
                        if (vastustaja[0] >= '0' && vastustaja[0] <= '9') {
                            // tarkistetaan, voidaan olla annettu viisinumeroinen luku
                            // 10000 - 99999... joten anna virheilmoitus vahvuusluvusta
                            virhekoodi = vakiot.VIRHE_SELO;
                            status = false;
                        } else {
                            // Ensimmäinen merkki kertoo tuloksen
                            switch (vastustaja[0]) {
                                case '+':
                                    tulos1 = vakiot.VOITTOx2;
                                    break;
                                case '=':
                                    tulos1 = vakiot.TASAPELIx2;
                                    break;
                                case '-':
                                    tulos1 = vakiot.TAPPIOx2;
                                    break;
                                default: // ei sallittu tuloksen kertova merkki
                                    virhekoodi = vakiot.VIRHE_YKSITTAINEN_TULOS;
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
                            virhekoodi = vakiot.VIRHE_SELO;  // -> virheilmoitus, ei ollut numero
                            status = false;
                        } else if (selo1 < vakiot.MIN_SELO || selo1 > vakiot.MAX_SELO) { 
                            status = false;
                            break;
                        }
                        // lisää vahvuusluku ja tulos listaan
                        shakinpelaaja.lista_lisaa_ottelun_tulos(selo1, tulos1);
                    } else {
                        // pituus ei ollut
                        //   - SELO_PITUUS (esim. 1234) 
                        //   - MAX_PITUUS (esim. +1234) silloin kun tulos voidaan antaa
                        // Tähän tullaan myös, jos turnauksen kokonaistulos oli annettu ennen vahvuuslukuja,
                        // koska silloin annetaan vain pelkät vahvuusluvut ilman yksittäisiä tuloksia.
                        // Ei ole sallittu  2,5 +1624 =1700 -1685 +1400 (oikein on 2,5 1624 1700 1685 1400)
                        virhekoodi = vakiot.VIRHE_SELO;
                        status = false;
                        break;
                    }

                    // Oliko asetettu virhe, mutta ei vielä poistuttu foreach-loopista?
                    if (!status)
                        break;

                } // foreach (käydään läpi syötteen numerot)

                // Lisää tarkastuksia
                // 6) Annettu turnauksen tulos ei saa olla suurempi kuin pelaajien lukumäärä
                if (status && onko_turnauksen_tulos) {
                    // Vertailu kokonaislukuina, esim. syötetty tulos 3.5 ja pelaajia 4, vertailu 7 > 8.
                    if ((int)(2 * syotetty_tulos + 0.01F) > 2 * shakinpelaaja.vastustajien_lukumaara_listalla) {
                        virhekoodi = vakiot.VIRHE_TURNAUKSEN_TULOS;  // tästä oma virheilmoitus
                        status = false;
                    }
                }
            }

            // VIRHEILMOITUKSET
            // Kolme mahdollista virhestatusta
            //    - virheellinen kokonaispistemäärä
            //    - väärin annettu ottelun tulos
            //    - virheellinen vahvuusluku, ei numero tai ei 
            //     
            if (!status) {
                string message;
                if (virhekoodi == vakiot.VIRHE_TURNAUKSEN_TULOS) {
                    message =
                        String.Format("VIRHE: Turnauksen pistemäärä (annettu {0}) voi olla enintään sama kuin vastustajien lukumäärä ({1}).",
                        syotetty_tulos, shakinpelaaja.vastustajien_lukumaara_listalla);

                } else if (virhekoodi == vakiot.VIRHE_YKSITTAINEN_TULOS) {
                    message =
                        String.Format("VIRHE: Yksittäisen ottelun tulos voidaan antaa merkeillä +(voitto), =(tasapeli) tai -(tappio), esim. +1720. Tasapeli voidaan antaa muodossa =1720 ja 1720.");
                } else {
                    // oletuksena tulostettava virhestatus, esim. jos kenttä oli tyhjä tai numero oli rajojen ulkopuolella
                    message =
                        String.Format("VIRHE: Vahvuusluvun on oltava numero {0}-{1}.", vakiot.MIN_SELO, vakiot.MAX_SELO);
                }

                vastustajanSelo_comboBox.ForeColor = Color.Red;
                MessageBox.Show(message);
                vastustajanSelo_comboBox.ForeColor = Color.Black;

                // Ei tyhjennetä kenttää, jotta sitä on helpompi korjata
                //              if (vastustajanSelo_comboBox.Text.Length > MAX_PITUUS)
                //                  vastustajanSelo_comboBox.Text = "";
                // Kentästä on kuitenkin jo poistettu ylimääräiset välilyönnit
                vastustajanSelo_comboBox.Select();

                tulos = vakiot.VIRHE_SELO;  // jos epäonnistui, palautetaan yksi virhestatus
            }

            return tulos;
        }

        // Tarkista ottelun tulos -painikkeet ja tallenna niiden vaikutus
        // pisteet: tappiosta 0, tasapelistä puoli ja voitosta yksi
        //          palautetaan kokonaislukuna 0, 1 ja 2
        public int tarkista_ottelun_tulos()
        {
            int pisteet = -1;

            if (tulosTappio_Button.Checked)
                pisteet = vakiot.TAPPIOx2;
            else if (tulosTasapeli_Button.Checked)
                pisteet = vakiot.TASAPELIx2;
            else if (tulosVoitto_Button.Checked)
                pisteet = vakiot.VOITTOx2;
            else {
                MessageBox.Show("Ottelun tulosta ei annettu!");
                tulosTappio_Button.Select();   // siirry ensimmäiseen valintanapeista
                pisteet = vakiot.VIRHE_TULOS;
            }
            return pisteet;
        }
    }
}
