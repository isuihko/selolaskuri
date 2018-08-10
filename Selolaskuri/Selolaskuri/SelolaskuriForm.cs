//
// The form with input fields and menus, calling of input checking and calculations, showing the results
//
// 10.6.2018 Ismo Suihko (earlier version history removed)
//
// Modifcations
//  10.6.2018   Syötteen tarkistukset ja laskenta siirretty omaan moduuliinsa SelolaskuriOperations.cs,
//              jotta niitä voidaan kutsua yksikkötestauksesta (Selolaskuri.Tests / UnitTest1.cs).
//              Syötteet haetaan lomakkeelta erikseen ja sitten kutsutaan tarkistusrutiinia,
//              jonka jälkeen (jos oli OK), kutsutaan laskentaa.
//              Virheilmoitusikkunat tulostetaan tarkistuksista saatujen statuksien mukaan.
//              Aiemmalla rakenteella ei automaattinen testaus olisi ollut mahdollista ainakaan tätä kautta.
//
// Publish --> Versio 2.0.0.0, myös github
//
//  11.6.2018       Jos ei annettu pelimääärää, niin uusi pelimäärä pitää olla tyhjä
//  15.6.2018       Korjattu vastustajanSelo_combobox:iin virheellisen syötteen tallentuminen (vastustajanSelo_combobox_KeyDown)
//  16.-18.6.2018   Tarkistettu laskennassa käytettyjä apumuuttujia ja poistettu turhia. Päivitetty kommentteja.
//                  Tarkistettu näkyvyyttä (public vs. private).
//
// Publish --> Versio 2.0.0.3, myös github
//
// 19.6.2018        Otettu käyttöön Visual Studio Community 2017 + Git
// 19.7.2018        Muokattu ohjetekstejä, pientä järjestelyä
//
// Publish --> Versio 2.0.0.4, myös github
//
// 20.-24.7.2018    Muutoksia mm. luokassa Selopelaaja ja yksikkötestauksessa, koodia selkeytetty ym.
//
// 25.7.2018        Tulostietoja järjestetty lomakkeella -> vastaavat tiedot ovat samalla rivillä.
//                  Laskennassa käytetty kerroin poistettu näytöltä.
//
// Publish --> Versio 2.0.0.5, myös github
//
// 26.7.2018        Lomakkeen kenttiä järjestelty vielä: ottelun tulospainikkeiden järjestys muutettu.
//                  Pelimäärän viereen tulostuu ilmoitusteksti uuden pelaajan laskennan käyttämisestä.
//
// Publish --> Versio 2.0.0.7, myös github
//
// 31.7.-1.8.2018   Koodin järjestelyä, jota tehty samalla kun tein tästä Java-versiota. Rutiinit samaan järjestykseen.
//                  HaeSyotteetLomakkeelta(): Syötekentistä poistetaan mahdolliset ylimääräiset välilyönnit ja kentät päivitetään näytölle.
//                  Vähennetty Tuple:n käyttöä, koska Javassakaan sitä ei ole (vielä Ottelulista ja UnitTest1).
//
// Publish --> Versio 2.0.0.8, myös github
//
// 2.8.2018         TarkistaVastustajanSelo() ei antanut virhestatusta, jos kenttä oli tyhjä.
//
// Publish --> Versio 2.0.0.9, myös github
//
// 4.8.2018         - Syöte voidaan antaa CSV(comma-separated values) eli pilkulla erotettuna listana
//                    vastustajat-kenttään.Listassa 2, 3, 4 tai 5 merkkijonoa, ks.HaeSyotteetLomakkeelta()
//                  - Myös uusi virheilmoitus, jos CSV-formaatissa liikaa pilkkuja.
//                  - Muutama yksikkötesti CSV-formaatin testaamiseen
//
// Publish --> Versio 2.0.0.10, myös github
//
// 5.8.2018         - Poista piippaus, kun painettu enter vastustajanSelo-kentässä
//                  - CSV:ssä voitu vaihtaa miettimisaika, joten päivitä SELO/PELO-tekstit
//
// 10.8.2018        - vastustajanSelo-kentässä voidaan antaa komentoja. Tarkistus enterin painalluksen jälkeen.
//                      clear - nollaa kaiken syötteen ja tulosteen
//                      test  - tallentaa vastustajanSelo_comboBox:iin testidataa ikkunakaappauksien ottoa varten
//                  - lomakkeen teksteihin muutoksia: vastustajanSelo-kenttään ohje Enter=laskenta
//
// Publish --> Versio 2.0.0.12, myös github
//
//
//
// TODO: Voidaan tehdä tarkempaa yksikkötestausta, mm. syötteen tarkistamisen jälkeen voidaan tarkistaa ottelulista
//

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions; // Regex rx, rx.Replace (remove extra white spaces)
using System.Collections.Generic;
using System.Linq;

namespace Selolaskuri
{
    public partial class SelolaskuriForm : Form
    {
        SelolaskuriOperations so = new SelolaskuriOperations();   //  Check the input data, calculate the results

        public SelolaskuriForm()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------------------
        // LOMAKKEEN KENTTIEN ARVOJEN HAKEMINEN
        // --------------------------------------------------------------------------------
        // Tietoja ei tarkisteta tässä
        // Miettimisaika on aina kelvollinen, mutta merkkijonot eivät välttämättä
        // Myös ottelun tulos voi/saa olla antamatta, joten silloin se on määrittelemätön
        private Syotetiedot HaeSyotteetLomakkeelta()
        {
            // Remove all leading and trailing white spaces from the form
            selo_in.Text = selo_in.Text.Trim();
            pelimaara_in.Text = pelimaara_in.Text.Trim();

            // NOTE! In Java version this comboBox could return also null, so there have to check for null value
            vastustajanSelo_comboBox.Text = vastustajanSelo_comboBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(vastustajanSelo_comboBox.Text) == false)
            {
                // poista sanojen väleistä ylimääräiset välilyönnit
                string pattern = "\\s+";    // \s = any whitespace, + one or more repetitions
                string replacement = " ";   // tilalle vain yksi välilyönti
                Regex rx = new Regex(pattern);
                vastustajanSelo_comboBox.Text = rx.Replace(vastustajanSelo_comboBox.Text, replacement);


                // Tarkista, onko csv ja jos on, niin unohda muut syötteet
                // Paitsi jos on väärässä formaatissa, palautetaan null ja kutsuvalla tasolla virheilmoitus
                //
                // CSV 
                // 90,1525,0,1725,1
                // Jos 5 merkkijonoa:  minuutit,selo,pelimäärä,vastustajat,jos_yksi_selo_niin_tulos
                // Jos 4: ottelun tulosta ei anneta, käytetään TULOS_MAARITTELEMATON
                // Jos 3: miettimisaikaa ei anneta, käytetään lomakkeelta valittua miettimisaikaa
                // Jos 2: pelimäärää ei anneta, käytetään oletuksena tyhjää ""
                //
                string csv = vastustajanSelo_comboBox.Text;

                List<string> data = csv.Split(',').ToList();
                if (data.Count == 5)
                {
                    return new Syotetiedot(so.SelvitaMiettimisaika(data[0]), data[1], data[2], data[3], so.SelvitaTulos(data[4]));
                }
                else if (data.Count == 4)
                {
                    return new Syotetiedot(so.SelvitaMiettimisaika(data[0]), data[1], data[2], data[3], Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
                }
                else if (data.Count == 3)
                {
                    return new Syotetiedot(HaeMiettimisaika(), data[0], data[1], data[2], Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
                }
                else if (data.Count == 2)
                {
                    return new Syotetiedot(HaeMiettimisaika(), data[0], "", data[1], Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON);
                } else if (data.Count > 1)
                {
                    // CSV FORMAT ERROR, ILLEGAL DATA
                    return null;
                }
            }

            return new Syotetiedot(HaeMiettimisaika(), selo_in.Text, pelimaara_in.Text, vastustajanSelo_comboBox.Text, HaeOttelunTulos());
        }

        // Nämä miettimisajan valintapainikkeet ovat omana ryhmänään paneelissa
        // Aina on joku valittuna, joten ei voi olla virhetilannetta.
        private Vakiot.Miettimisaika_enum HaeMiettimisaika()
        {
            Vakiot.Miettimisaika_enum valinta;

            if (miettimisaika_vah90_btn.Checked)
                valinta = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;
            else if (miettimisaika_60_89_btn.Checked)
                valinta = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN;
            else if (miettimisaika_11_59_btn.Checked)
                valinta = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN;
            else
                valinta = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN;

            return valinta;
        }

        // Ottelun tulos voi olla valittu radiobuttoneilla tai valitsematta (MAARITTELEMATON)
        private Vakiot.OttelunTulos_enum HaeOttelunTulos()
        {
            Vakiot.OttelunTulos_enum valinta;

            if (tulosVoitto_btn.Checked)
                valinta = Vakiot.OttelunTulos_enum.TULOS_VOITTO;          
            else if (tulosTasapeli_btn.Checked)
                valinta = Vakiot.OttelunTulos_enum.TULOS_TASAPELI;
            else if (tulosTappio_btn.Checked)
                valinta = Vakiot.OttelunTulos_enum.TULOS_TAPPIO;
            else
                valinta = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON;

            return valinta;
        }


        // --------------------------------------------------------------------------------
        // Painikkeiden toiminta
        // --------------------------------------------------------------------------------
        //    Laske uusi SELO  (pikashakissa Laske uusi PELO)
        //    Käytä uutta SELOa jatkolaskennassa

        // Suoritetaan laskenta -button
        private void Laske_btn_Click(object sender, EventArgs e)
        {
            if (LaskeOttelunTulosLomakkeelta()) { 
                // Annettu teksti talteen (jos ei ennestään ollut) -> Drop-down Combo box
                // Tallennus kun klikattu Laske SELO tai painettu enter vastustajan selo-kentässä
                if (!vastustajanSelo_comboBox.Items.Contains(vastustajanSelo_comboBox.Text))
                    vastustajanSelo_comboBox.Items.Add(vastustajanSelo_comboBox.Text);
            }
        }

        // Kopioi lasketun uuden selon ja mahdollisen pelimäärän käytettäväksi, jotta
        // laskentaa voidaan jatkaa helposti syöttämällä uusi vastustajan selo ja ottelun tulos.
        // Ja sitten siirrytään valmiiksi vastustajan SELO-kenttään.
        //
        // Jos painiketta oli painettu ennen ensimmäistäkään laskentaa, niin
        // saadaan pelaajaa luodessa käytetyt alkuarvot SELO 1525 ja pelimääärä 0.
        private void KaytaTulosta_btn_Click(object sender, EventArgs e)
        {
            int selo1 = so.HaeViimeksiLaskettuSelo();
            int pelimaara1 = so.HaeViimeksiLaskettuPelimaara();

            selo_in.Text = selo1.ToString();

            // vain, jos pelimaara oli annettu (muutoin on jo valmiiksi tyhjä)
            if (pelimaara1 != (int)Vakiot.PELIMAARA_TYHJA)
                pelimaara_in.Text = pelimaara1.ToString();

            vastustajanSelo_comboBox.Select();
        }


        // Näyttää virheen mukaisen ilmoituksen sekä siirtää kursorin kenttään, jossa virhe
        // Virheellisen kentän arvo näytetään punaisella kunnes ilmoitusikkuna kuitataan
        private void NaytaVirheilmoitus(int virhestatus)
        {
            string message;

            switch (virhestatus) {
                case Vakiot.SYOTE_STATUS_OK:
                    break;
                case Vakiot.SYOTE_VIRHE_OMA_SELO:
                    message =
                        String.Format("VIRHE: Nykyisen SELOn oltava numero {0}-{1}.",
                                Vakiot.MIN_SELO, Vakiot.MAX_SELO);
                    selo_in.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    selo_in.ForeColor = Color.Black;

                    // Tyhjennä liian täysi kenttä? Tyhjennä
                    if (selo_in.Text.Length > Vakiot.MAX_PITUUS)
                        selo_in.Text = "";
                    selo_in.Select();
                    break;

                case Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO:
                    message =
                        String.Format("VIRHE: Vastustajan vahvuusluvun on oltava numero {0}-{1}.",
                                Vakiot.MIN_SELO, Vakiot.MAX_SELO);
                    vastustajanSelo_comboBox.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    vastustajanSelo_comboBox.ForeColor = Color.Black;
                    vastustajanSelo_comboBox.Select();
                    break;

                case Vakiot.SYOTE_VIRHE_PELIMAARA:
                    message =
                        String.Format("VIRHE: pelimäärän voi olla numero väliltä {0}-{1} tai tyhjä.",
                                Vakiot.MIN_PELIMAARA, Vakiot.MAX_PELIMAARA);
                    pelimaara_in.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    pelimaara_in.ForeColor = Color.Black;

                    // Tyhjennä liian täysi kenttä? Tyhjennä
                    if (pelimaara_in.Text.Length > Vakiot.MAX_PITUUS)
                        pelimaara_in.Text = "";
                    pelimaara_in.Select();
                    break;

                // tulos puuttuu painonapeista, siirry ensimmäiseen valintanapeista
                case Vakiot.SYOTE_VIRHE_BUTTON_TULOS:  
                    MessageBox.Show("Ottelun tulosta ei valittu!");
                    tulosVoitto_btn.Select();  // ensimmäinen tulos-painikkeista
                    break;

                case Vakiot.SYOTE_VIRHE_YKSITTAINEN_TULOS:
                    message =
                        String.Format("VIRHE: Yksittäisen ottelun tulos voidaan antaa merkeillä +(voitto), =(tasapeli) tai -(tappio), esim. +1720. Tasapeli voidaan antaa muodossa =1720 ja 1720.");
                    vastustajanSelo_comboBox.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    vastustajanSelo_comboBox.ForeColor = Color.Black;
                    vastustajanSelo_comboBox.Select();
                    break;

                case Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS:
                    message =
                        String.Format("VIRHE: Turnauksen pistemäärä voi olla enintään sama kuin vastustajien lukumäärä.");
                    vastustajanSelo_comboBox.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    vastustajanSelo_comboBox.ForeColor = Color.Black;
                    vastustajanSelo_comboBox.Select();
                    break;

                case Vakiot.SYOTE_VIRHE_CSV_FORMAT:
                    message =
                        String.Format("VIRHE: CSV-formaattivirhe, ks. Menu->Ohjeita");
                    vastustajanSelo_comboBox.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    vastustajanSelo_comboBox.ForeColor = Color.Black;
                    vastustajanSelo_comboBox.Select();
                    break;
            }
        }


        // --------------------------------------------------------------------------------
        // Lomakkeen tietojen käsittely, laskenta ja tuloksien näyttäminen
        // --------------------------------------------------------------------------------
        // Haetaan tiedot lomakkeelta
        // Tarkistataan ne
        // Lasketaan uusi vahvuusluku ja ottelumäärä sekä muut tulokset
        // Näytetään tulokset
        //
        // Virhetarkastus ja laskenta erillisessä luokassa SelolaskuriOperations,
        // jotta niitä voidaan kutsua myös yksikkötestauksesta
        //
        private bool LaskeOttelunTulosLomakkeelta()
        {
            bool status = true;
            int tulos;

            // hakee syötetyt tekstit ja tehdyt valinnat, ei virhetarkastusta
            Syotetiedot syotteet = HaeSyotteetLomakkeelta();

            if (syotteet == null)
            {
                NaytaVirheilmoitus(Vakiot.SYOTE_VIRHE_CSV_FORMAT);
                status = false;
            } else if ((tulos = so.TarkistaSyote(syotteet)) != Vakiot.SYOTE_STATUS_OK) {
                NaytaVirheilmoitus(tulos);
                status = false;
            } else {
                Selopelaaja tulokset = so.SuoritaLaskenta(syotteet);

                // Tuloksissa näytetään myös selo-muutos
                NaytaTulokset(tulokset);
            }

            return status;
        }


        // Lasketut tulokset lomakkeelle
        //   Uusi vahvuusluku ja sen muutos +/- NN pistettä
        //   uusi pelimäärä tai tyhjä
        //   piste-ero
        private void NaytaTulokset(Selopelaaja tulokset)
        {
            //   Uusi vahvuusluku ja sen muutos +NN, -NN tai 0 pistettä
            uusiSelo_out.Text = tulokset.UusiSelo.ToString();
            selomuutos_out.Text = (tulokset.UusiSelo - tulokset.AlkuperainenSelo).ToString("+#;-#;0");

            //   uusi pelimäärä tai tyhjä
            if (tulokset.UusiPelimaara != Vakiot.PELIMAARA_TYHJA)
                uusiPelimaara_out.Text = tulokset.UusiPelimaara.ToString();
            else
                uusiPelimaara_out.Text = "";

            // piste-ero turnauksen keskivahvuuteen nähden
            pisteEro_out.Text = Math.Abs(tulokset.AlkuperainenSelo - tulokset.TurnauksenKeskivahvuus).ToString();

            // Vastustajien vahvuuslukujen keskiarvo
            keskivahvuus_out.Text = tulokset.TurnauksenKeskivahvuus.ToString();

            // Turnauksen loppupisteet yhdellä desimaalilla / ottelujen lkm, esim.  2.5 / 6 tai 2.0 / 6
            turnauksenTulos_out.Text =
                (tulokset.TurnauksenTulos / 2F).ToString("0.0") + " / " + tulokset.VastustajienLkm;

            // Vahvuusluku on voinut vaihdella laskennan edetessä, jos vastustajat ovat olleet formaatissa "+1622 -1880 =1633"
            // Vaihteluväliä ei ole, jos laskenta on tehty yhdellä lausekkeella tai on ollut vain yksi vastustaja
            if (tulokset.MinSelo < tulokset.MaxSelo)
                vaihteluvali_out.Text = tulokset.MinSelo.ToString() + " - " + tulokset.MaxSelo.ToString();
            else
                vaihteluvali_out.Text = "";

            // Odotustulosta tai sen summaa ei näytetä uudelle pelaajalle, koska vahvuusluku on vielä provisional
            // Uuden pelaajan laskennasta annetaan ilmoitusteksti
            if (tulokset.UudenPelaajanLaskenta) {
                odotustulos_out.Text = "";
                UudenPelaajanLaskenta_txt.Visible = true;
            } else {
                odotustulos_out.Text = (tulokset.Odotustulos / 100F).ToString("0.00");
                UudenPelaajanLaskenta_txt.Visible = false;
            }


            // kerroin on laskettu alkuperäisestä omasta selosta (laskennan aputieto)
            //XXX: Poistettu lomakkeelta kerroin_out.Text = tulokset.Kerroin.ToString();

            // Jos ei käytetty tulospainikkeita, niin tuloksen valintanapit varmuuden vuoksi pois päältä
            // Tulospainikkeita käytettäessä yksi vastustajan selo, eikä tulosta annettu muodossa "1.0 1434" tai "+1434"
            if (!tulokset.KaytettiinkoTulospainikkeita) {
                tulosVoitto_btn.Checked = false;
                tulosTasapeli_btn.Checked = false;
                tulosTappio_btn.Checked = false;
            }

            // Jos käytetty CSV-formaattia, on voitu antaa eri miettimisaika kuin mitä valittu buttoneilla,
            // joten varmuuden vuoksi päivitetään SELO- ja PELO-tekstit (vaikka voivat jo olla oikein)
            // Turhan päivittämisen voisi estää lisäämällä flag syötetietoihin kertomaan, oliko csv:ssä miettimisaika.
            //
            // Ei riitä tarkistaa, onko valittu eri kuin näytöllä, koska tekstit on voitu vaihtaa välillä
            //if (tulokset.Miettimisaika != HaeMiettimisaika()) {
                if (tulokset.Miettimisaika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN)
                    vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_PELOKSI);
                else
                    vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
            //}

        }

        // --------------------------------------------------------------------------------
        // Miettimisajan valinnan mukaan tekstit: SELO (pidempi peli) vai PELO (pikashakki)
        //
        // XXX: Hm... onko liian monta vaihdettavaa otsikkokenttää? Esim. Laske uusi SELO -> Laske uusi vahvuusluku
        // --------------------------------------------------------------------------------
        private void vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum suunta)
        {
            string alkup, uusi;

            if (suunta == Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI)
            {
                alkup = "PELO";
                uusi = "SELO";
                TuloksetPistemaaranKanssa_teksti.Font = new Font(TuloksetPistemaaranKanssa_teksti.Font, FontStyle.Regular);
            }
            else
            {
                alkup = "SELO";
                uusi = "PELO";
                // korosta PELO-ohje
                TuloksetPistemaaranKanssa_teksti.Font = new Font(TuloksetPistemaaranKanssa_teksti.Font, FontStyle.Bold);
            }

            OmaVahvuusluku_teksti.Text = OmaVahvuusluku_teksti.Text.Replace(alkup, uusi);
            VastustajanVahvuusluku_teksti.Text = VastustajanVahvuusluku_teksti.Text.Replace(alkup, uusi);
            TuloksetPistemaaranKanssa_teksti.Text = TuloksetPistemaaranKanssa_teksti.Text.Replace(alkup, uusi);
            UusiSELO_teksti.Text = UusiSELO_teksti.Text.Replace(alkup, uusi);
            Laske_btn.Text = Laske_btn.Text.Replace(alkup, uusi);
            KaytaTulosta_btn.Text = KaytaTulosta_btn.Text.Replace(alkup, uusi);
        }

        // Miettimisajan valinta ei tee laskentaa uusiksi automaattisesti. Vaihtaa vain tekstit SELO <-> PELO.
        // Jos miettimisaika valitaan alussa, eikä kaikkia tietoja ei ole syötetty, niin saataisiin virheilmoitus.
        private void miettimisaika_vah90_Button_CheckedChanged(object sender, EventArgs e)
        {
            vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_60_89_Button_CheckedChanged(object sender, EventArgs e)
        {
            vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_11_59_Button_CheckedChanged(object sender, EventArgs e)
        {
            vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void miettimisaika_enint10_Button_CheckedChanged(object sender, EventArgs e)
        {
            vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_PELOKSI);
        }


        // --------------------------------------------------------------------------------
        // Ottelun tulos-buttonit
        // --------------------------------------------------------------------------------
        // Suorita laskenta aina kun siirrytään tulos-painikkeeseen.
        // Ennen laskentaa asetetaan nykyinen painike valituksi, koska sitä ei
        // muutoin vielä oltu valittu kenttään siirryttäessä.
        //
        // Jos tässä vaiheessa ei ole vielä annettu SELOja, tulee virheilmoitus
        // sekä siirrytään SELO-kenttään.
        // 
        private void tulosVoitto_Button_Enter(object sender, EventArgs e)
        {
            tulosVoitto_btn.Checked = true;
            LaskeOttelunTulosLomakkeelta();
        }

        private void tulosTasapeli_Button_Enter(object sender, EventArgs e)
        {
            tulosTasapeli_btn.Checked = true;
            LaskeOttelunTulosLomakkeelta();
        }

        private void tulosTappio_Button_Enter(object sender, EventArgs e)
        {
            tulosTappio_btn.Checked = true;
            LaskeOttelunTulosLomakkeelta();
        }

        // --------------------------------------------------------------------------------
        // Kun painettu Enter vastustajan SELO-kentässä, suoritetaan laskenta
        //
        // Tarkistetaan mahdollisesti annetut komennot
        // --------------------------------------------------------------------------------
        private void vastustajanSelo_combobox_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter painettu vastustajan selojen tai useammankin syöttämisen jälkeen?
            if (e.KeyCode == Keys.Enter) {

                // Prevent beep sound, because Enter is accepted in this form field
                e.Handled = true;
                e.SuppressKeyPress = true;

                // Tarkista erikoistapaukset eli mahdolliset komennot
                //   clear  tyhjentää kaikki syötekentät, myös vastustajanSelo_comboBox:n
                //   test   tyhjentää kaikki syötekentät ja laittaa vastustajanSelo_comboBox:iin testausta varten aineistoa
                if (vastustajanSelo_comboBox.Text.Equals("clear")) {
                    // tyhjentää lomakkeen kentät ja tulokset, sekä palauttaa alkuarvot (miettimisaika vähintään 90 min)
                    // Huom! Jättää muistiin aiemmin lasketut vahvuusluvun ja pelimäärän, jolloin
                    // Käytä uutta SELOa jatkolaskennassa voi hakea ne (ei palauteta lukuja 1525,0)
                    selo_in.Text = "";
                    pelimaara_in.Text = "";
                    miettimisaika_vah90_btn.Select();

                    tulosVoitto_btn.Checked = false;
                    tulosTasapeli_btn.Checked = false;
                    tulosTappio_btn.Checked = false;

                    // this leaves empty lines into comboBox, but they disappear after adding new item
                    vastustajanSelo_comboBox.Items.Clear();
                    vastustajanSelo_comboBox.ResetText();

                    // tyhjennä tulokset
                    uusiSelo_out.Text = "";
                    selomuutos_out.Text = "";
                    vaihteluvali_out.Text = "";

                    uusiPelimaara_out.Text = "";
                    turnauksenTulos_out.Text = "";
                    odotustulos_out.Text = "";
                    keskivahvuus_out.Text = "";                   
                    pisteEro_out.Text = "";

                    // palauta tekstit
                    vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
                    return;

                } else if (vastustajanSelo_comboBox.Text.Equals("test")) {

                    // Aseta testausta varten listaan vastustajien & otteluiden tietoja, ei nollata muita kenttiä
                    vastustajanSelo_comboBox.Items.Clear();
                    vastustajanSelo_comboBox.ResetText();

                    // Add some data (uncomplete and complete) to help running couple of test cases for window captures
                    // vastustajanSelo_comboBox.Items.Add("");  // No need to add an empty item like in Java

                    vastustajanSelo_comboBox.Items.Add("5,1996,,10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");
                    // Also Miettimisaika enint. 10 min, nykyinen SELO 1996, pelimäärä tyhjä
                    vastustajanSelo_comboBox.Items.Add("10.5 1977 2013 1923 1728 1638 1684 1977 2013 1923 1728 1638 1684");

                    vastustajanSelo_comboBox.Items.Add("90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966");
                    // Also Miettimisaika väh. 90 min, nykyinen SELO 1525, pelimäärä 0
                    vastustajanSelo_comboBox.Items.Add("+1525 +1441 -1973 +1718 -1784 -1660 -1966");

                    vastustajanSelo_comboBox.Items.Add("90,1683,2,1973,0");
                    // Also Miettimisaika väh. 90 min, nykyinen SELO 1683, pelimäärä 2, ottelun tulos 0=tappio
                    vastustajanSelo_comboBox.Items.Add("1973");

                    return;
                }

                // Kun painettu Enter, niin lasketaan
                // siirrytäänkö myös kentän loppuun? Nyt jäädään samaan paikkaan, mikä myös OK.
                if (LaskeOttelunTulosLomakkeelta()) {

                    // Jos syöte (ja siten laskenta) OK, niin tallenna kentän syöte -> Drop-down combobox
                    // Tallennus myös, kun klikattu Laske SELO
                    if (!vastustajanSelo_comboBox.Items.Contains(vastustajanSelo_comboBox.Text))
                        vastustajanSelo_comboBox.Items.Add(vastustajanSelo_comboBox.Text);
                }
            }
        }


        // --------------------------------------------------------------------------------
        // Menu
        // --------------------------------------------------------------------------------
        //    Ohjeita
        //    Laskentakaavat
        //    Tietoa ohjelmasta
        //    Sulje ohjelma
        //
        // Ohjeikkunassa ym. on MessageBox.Show:ssa oletuksena oleva OK-painike
        // Jos tarvitaan muokattua tekstiä, voitaisiin luoda uusi lomake ohjeikkunaa varten
        private void ohjeitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Shakin vahvuusluvun laskenta SELO ja PELO"
                + "\r\n"
                + "\r\n" + "Annettavat tiedot:"
                + "\r\n"
                + "\r\n" + "-Miettimisaika. Pitkä peli (väh. 90 minuuttia) on oletuksena. Jos valitset enint. 10 minuuttia, lasketaan pikashakin vahvuuslukua (PELO)"
                + "\r\n" + "-Oma vahvuusluku"
                + "\r\n" + "-Oma pelimäärä, joka tarvitaan vain jos olet pelannut enintään 10 peliä. Tällöin käytetään uuden pelaajan laskentakaavaa."
                + "\r\n" + "-Vastustajien vahvuusluvut ja tulokset jollakin kolmesta tavasta:"
                + "\r\n" + "   1) Yhden vastustajan vahvuusluku (esim. 1922) ja lisäksi ottelun tulos 1/0,5/0 nuolinäppäimillä tai hiirellä. Laskennan tulos päivittyy valinnan mukaan."
                + "\r\n" + "   2) Vahvuusluvut tuloksineen, esim. +1525 =1600 -1611 +1558, jossa + voitto, = tasan ja - tappio"
                + "\r\n" + "   3) Turnauksen pistemäärä ja vastustajien vahvuusluvut, esim. 2.5 1525 1600 1611 1558"
                + "\r\n" + "   4) CSV eli pilkulla erotettu lista, jossa 2, 3, 4 tai 5 kenttää: HUOM! Käytä tuloksissa desimaalipistettä, esim. 0.5 tai 10.5!"
                + "\r\n" + "           2: oma selo,ottelut   esim. 1712,2.5 1525 1600 1611 1558 tai 1712,+1525"
                + "\r\n" + "           3: oma selo,pelimaara,ottelut esim. 1525,0,+1525 +1441"
                + "\r\n" + "           4: minuutit,oma selo,pelimaara,ottelut  esim. 90,1525,0,+1525 +1441"
                + "\r\n" + "           5: minuutit,oma selo,pelimaara,ottelu,tulos esim. 90,1683,2,1973,0 (jossa tasapeli 1/2 tai 0.5)"
                + "\r\n" + "      Jos miettimisaika on antamatta, käytetään ikkunasta valittua"
                + "\r\n" + "      Jos pelimäärä on antamatta, käytetään tyhjää"
                + "\r\n"
                + "\r\n" + "Laskenta suoritetaan klikkaamalla laskenta-painiketta tai painamalla Enter vastustajan SELO-kentässä sekä (jos yksi vastustaja) tuloksen valinta -painikkeilla."
                + "\r\n"
                + "\r\n" + "Jos haluat jatkaa laskentaa uudella vahvuusluvulla, klikkaa Käytä uutta SELOa jatkolaskennassa. Jos ei ole vielä ollut laskentaa, saadaan uuden pelaajan oletusarvot SELO 1525 ja pelimäärä 0.",

                "Ohjeita");
        }

        // MenuItem: Sulje ohjelma
        private void laskentakaavatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Shakin vahvuusluvun laskentakaavat: http://skore.users.paivola.fi/selo.html"
                + "\r\n" + "Lisätietoa: http://www.shakkiliitto.fi/ ja http://www.shakki.net/cgi-bin/selo",
                "Laskentakaavat");
        }


        private void tietojaOhjelmastaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Shakin vahvuusluvun laskenta, ohjelmointikieli C#/.NET/WinForms"
                + "\r\n" + "Lähdekoodit ja asennusohjelma https://github.com/isuihko/selolaskuri",
                "Tietoa Selolaskurista");
        }

        // Lopetuksen varmistaminen
        //      Valittu Menu->Sulje ohjelma
        private void suljeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Application.Exit() kutsuu ikkunan FormClosing() -rutiinia, jossa varmistus ja voidaan perua poistuminen.
            System.Windows.Forms.Application.Exit();
        }

        // Lopetuksen varmistaminen
        //      Suljettu ikkuna
        private void SelolaskuriForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult vastaus = MessageBox.Show("Haluatko poistua ohjelmasta?", "Exit/Close window", MessageBoxButtons.YesNo);
            if (vastaus == DialogResult.No)
            {
                e.Cancel = true;    // Ei poistutakaan
            }
        }
    }
}
