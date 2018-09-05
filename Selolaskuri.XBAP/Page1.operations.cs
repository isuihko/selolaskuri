using SelolaskuriLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Selolaskuri.XBAP {
    public partial class Page1 : Page {

        private SelolaskuriOperations so = new SelolaskuriOperations();                   //  Check the input data, calculate the results
        private FormOperations fo = new FormOperations(Vakiot.Selolaskuri_enum.XBAP_WEB); // information and instruction windows etc.

        // --------------------------------------------------------------------------------
        // LOMAKKEEN KENTTIEN ARVOJEN HAKEMINEN
        // --------------------------------------------------------------------------------
        // Tietoja ei tarkisteta tässä
        // Miettimisaika on aina kelvollinen, mutta merkkijonot eivät välttämättä
        // Myös ottelun tulos voi/saa olla antamatta, joten silloin se on määrittelemätön
        //
        // Jos CSV-formaatti ja on liian monta arvoa, palauttaa null
        private Syotetiedot HaeSyotteetLomakkeelta()
        {
            // Remove all leading and trailing white spaces from the form
            selo_in.Text = selo_in.Text.Trim();
            pelimaara_in.Text = pelimaara_in.Text.Trim();

            // NOTE! In Java version this comboBox could return also null, so there have to check for null value
            vastustajanSelo_comboBox.Text = vastustajanSelo_comboBox.Text.Trim();

            // process opponents field and check if CSV format was used
            //
            if (string.IsNullOrWhiteSpace(vastustajanSelo_comboBox.Text) == false) {
                // poista ylimääräiset välilyönnit, korvaa yhdellä
                // poista myös välilyönnit pilkun molemmilta puolilta, jos on CSV-formaatti
                vastustajanSelo_comboBox.Text = so.SiistiVastustajatKentta(vastustajanSelo_comboBox.Text); // .Trim jo tehty

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
                //
                // Note that string in the following format is not CSV (comma can be used in tournament result)
                //    "tournamentResult selo1 selo2 ..." e.g. "2,5 1505 1600 1611 1558" or "100,5 1505 1600 1611 1558 ... "
                //
                // But that checking should not affect CSV format "thinking time,selo,..." e.g. "5,1525,0,1505 1600 ..."
                //      or "own selo,opponent selo with result" e.g. "1525,+1505"
                //      or "own selo,opponent selo,single match result" e.g. "1525,1505,0.5" <- Here must use decimal point!!!
                //
                // So check that if there is just one comma, so two values, the length of the first value must be at least 4 (length of selo)

                if (vastustajanSelo_comboBox.Text.Contains(',')) {
                    List<string> tmp = vastustajanSelo_comboBox.Text.Split(',').ToList();

                    if (tmp.Count != 2 || (tmp.Count == 2 && tmp[0].Trim().Length >= 4)) {
                        // The thinking time might be needed from the form if there are 2 or 3 values in CSV format
                        return so.SelvitaCSV(HaeMiettimisaika(), vastustajanSelo_comboBox.Text);
                    }
                }
            }

            return new Syotetiedot(HaeMiettimisaika(), selo_in.Text, pelimaara_in.Text, vastustajanSelo_comboBox.Text, HaeOttelunTulos());
        }

        // Nämä miettimisajan valintapainikkeet ovat omana ryhmänään paneelissa
        // Aina on joku valittuna, joten ei voi olla virhetilannetta.
        private Vakiot.Miettimisaika_enum HaeMiettimisaika()
        {
            Vakiot.Miettimisaika_enum valinta;

            if (miettimisaika_vah90_btn.IsChecked == true)
                valinta = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;
            else if (miettimisaika_60_89_btn.IsChecked == true)
                valinta = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN;
            else if (miettimisaika_11_59_btn.IsChecked == true)
                valinta = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN;
            else
                valinta = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN;

            return valinta;
        }

        // Ottelun tulos voi olla valittu radiobuttoneilla tai valitsematta (MAARITTELEMATON)
        private Vakiot.OttelunTulos_enum HaeOttelunTulos()
        {
            Vakiot.OttelunTulos_enum valinta;

            if (tulosVoitto_btn.IsChecked == true)
                valinta = Vakiot.OttelunTulos_enum.TULOS_VOITTO;
            else if (tulosTasapeli_btn.IsChecked == true)
                valinta = Vakiot.OttelunTulos_enum.TULOS_TASAPELI;
            else if (tulosTappio_btn.IsChecked == true)
                valinta = Vakiot.OttelunTulos_enum.TULOS_TAPPIO;
            else
                valinta = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON; // ei mahdollinen

            return valinta;
        }


        // --------------------------------------------------------------------------------
        // Painikkeiden toiminta
        // --------------------------------------------------------------------------------
        //    Laske uusi SELO  (pikashakissa Laske uusi PELO)
        //    Käytä uutta SELOa jatkolaskennassa

        // Suoritetaan laskenta -button
        private void Laske_btn_Click(object sender, RoutedEventArgs e)
        {
            if (LaskeOttelunTulosLomakkeelta()) {
                // Annettu teksti talteen (jos ei ennestään ollut) -> Drop-down Combo box
                // Tallennus kun klikattu Laske SELO tai painettu enter vastustajan selo-kentässä
                //
                // Tekstistä on poistettu ylimääräiset välilyönnit ennen tallennusta
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
        private void KaytaTulosta_btn_Click(object sender, RoutedEventArgs e)
        {
            int selo1 = so.HaeViimeksiLaskettuSelo();
            int pelimaara1 = so.HaeViimeksiLaskettuPelimaara();

            selo_in.Text = selo1.ToString();

            // vain, jos pelimaara oli annettu (muutoin on jo valmiiksi tyhjä)
            if (pelimaara1 != (int)Vakiot.PELIMAARA_TYHJA)
                pelimaara_in.Text = pelimaara1.ToString();

            vastustajanSelo_comboBox.Focus();
        }

        // Näyttää virheen mukaisen ilmoituksen sekä siirtää kursorin kenttään, jossa virhe
        // Virheellisen kentän arvo näytetään punaisella kunnes ilmoitusikkuna kuitataan
        private void NaytaVirheilmoitus(int virhestatus)
        {
            string message;

            switch (virhestatus) {
                case Vakiot.SYOTE_STATUS_OK:
                    break;

                case Vakiot.SYOTE_VIRHE_MIETTIMISAIKA:
                    message =
                        String.Format("VIRHE: CSV-formaatissa annettu virheellinen miettimisaika. Annettava minuutit. Ks. Menu->Ohjeita");
                    MessageBox.Show(message);
                    vastustajanSelo_comboBox.Focus();  // WInForms: .Select();
                    break;

                case Vakiot.SYOTE_VIRHE_OMA_SELO:
                    message =
                        String.Format("VIRHE: Nykyisen SELOn oltava numero {0}-{1}.",
                                Vakiot.MIN_SELO, Vakiot.MAX_SELO);
                    //selo_in.ForeColor = Color.Red;      // WinForms: selo_in.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    //selo_in.ForeColor = Color.Black;

                    // Tyhjennä liian täysi kenttä? Tyhjennä
                    if (selo_in.Text.Length > Vakiot.MAX_PITUUS)
                        selo_in.Text = "";
                    selo_in.Focus();
                    break;

                case Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO:
                    message =
                        String.Format("VIRHE: Vastustajan vahvuusluvun on oltava numero {0}-{1}.",
                                Vakiot.MIN_SELO, Vakiot.MAX_SELO);
                    //vastustajanSelo_comboBox.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    //vastustajanSelo_comboBox.ForeColor = Color.Black;
                    vastustajanSelo_comboBox.Focus();
                    break;

                case Vakiot.SYOTE_VIRHE_PELIMAARA:
                    message =
                        String.Format("VIRHE: pelimäärän voi olla numero väliltä {0}-{1} tai tyhjä.",
                                Vakiot.MIN_PELIMAARA, Vakiot.MAX_PELIMAARA);
                    //pelimaara_in.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    //pelimaara_in.ForeColor = Color.Black;

                    // Tyhjennä liian täysi kenttä? Tyhjennä
                    if (pelimaara_in.Text.Length > Vakiot.MAX_PITUUS)
                        pelimaara_in.Text = "";
                    pelimaara_in.Focus();
                    break;

                // tulos puuttuu painonapeista, siirry ensimmäiseen valintanapeista
                case Vakiot.SYOTE_VIRHE_BUTTON_TULOS:
                    MessageBox.Show("Ottelun tulosta ei valittu!");
                    tulosVoitto_btn.Focus();  // ensimmäinen tulos-painikkeista
                    break;

                case Vakiot.SYOTE_VIRHE_YKSITTAINEN_TULOS:
                    message =
                        String.Format("VIRHE: Yksittäisen ottelun tulos voidaan antaa merkeillä +(voitto), =(tasapeli) tai -(tappio), esim. +1720. Tasapeli voidaan antaa muodossa =1720 ja 1720.");
                    //vastustajanSelo_comboBox.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    //vastustajanSelo_comboBox.ForeColor = Color.Black;
                    vastustajanSelo_comboBox.Focus();
                    break;

                case Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS:
                    message =
                        String.Format("VIRHE: Turnauksen pistemäärä voi olla enintään sama kuin vastustajien lukumäärä.");
                    //vastustajanSelo_comboBox.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    //vastustajanSelo_comboBox.ForeColor = Color.Black;
                    vastustajanSelo_comboBox.Focus();
                    break;

                case Vakiot.SYOTE_VIRHE_CSV_FORMAT:
                    message =
                        String.Format("VIRHE: CSV-formaattivirhe, ks. Menu->Ohjeita");
                    //vastustajanSelo_comboBox.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    //vastustajanSelo_comboBox.ForeColor = Color.Black;
                    vastustajanSelo_comboBox.Focus();
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

            if (syotteet == null) {
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
                UudenPelaajanLaskenta_txt.Visibility = Visibility.Visible;  // WinForms: .Visible = true;
            } else {
                odotustulos_out.Text = (tulokset.Odotustulos / 100F).ToString("0.00");
                UudenPelaajanLaskenta_txt.Visibility = Visibility.Hidden; // WInForms: .Visible = false;
            }


            // kerroin on laskettu alkuperäisestä omasta selosta (laskennan aputieto)
            //XXX: Poistettu lomakkeelta kerroin_out.Text = tulokset.Kerroin.ToString();

            // Jos ei käytetty tulospainikkeita, niin tuloksen valintanapit varmuuden vuoksi pois päältä
            // Tulospainikkeita käytettäessä yksi vastustajan selo, eikä tulosta annettu muodossa "1.0 1434" tai "+1434"
            if (!tulokset.KaytettiinkoTulospainikkeita) {
                tulosVoitto_btn.IsChecked = false;      // WinForms:  Checked = false;
                tulosTasapeli_btn.IsChecked = false;
                tulosTappio_btn.IsChecked = false;
            }

            // Jos käytetty CSV-formaattia, on voitu antaa eri miettimisaika kuin mitä valittu buttoneilla,
            // joten varmuuden vuoksi päivitetään SELO- ja PELO-tekstit (vaikka voivat jo olla oikein)
            // Turhan päivittämisen voisi estää lisäämällä flag syötetietoihin kertomaan, oliko csv:ssä miettimisaika.
            //
            // Ei riitä tarkistaa, onko valittu eri kuin näytöllä, koska tekstit on voitu vaihtaa välillä
            if (tulokset.Miettimisaika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN)
                VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_PELOKSI);
            else
                VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        // --------------------------------------------------------------------------------
        // Miettimisajan valinnan mukaan tekstit: SELO (pidempi peli) vai PELO (pikashakki)
        //
        // XXX: Hm... onko liian monta vaihdettavaa otsikkokenttää? Esim. Laske uusi SELO -> Laske uusi vahvuusluku
        // --------------------------------------------------------------------------------
        private void VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum suunta)
        {
            string alkup, uusi;

            if (suunta == Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI) {
                alkup = "PELO";
                uusi = "SELO";
            } else {
                alkup = "SELO";
                uusi = "PELO";
            }

            OmaVahvuusluku_teksti.Text = OmaVahvuusluku_teksti.Text.Replace(alkup, uusi);
            VastustajanVahvuusluku_teksti.Text = VastustajanVahvuusluku_teksti.Text.Replace(alkup, uusi);
            UusiSELO_teksti.Text = UusiSELO_teksti.Text.Replace(alkup, uusi);
        }

        // Miettimisajan valinta (Checked) ei tee laskentaa uusiksi automaattisesti. Vaihtaa vain tekstit SELO <-> PELO.
        // Jos mentäisiin laskentaan, eikä kaikkia tietoja olisi syötetty, saataisiin puuttuvista tiedoista (selo ym.) virheilmoitus.
        private void Miettimisaika_vah90_btn_Checked(object sender, RoutedEventArgs e)
        {
            VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void Miettimisaika_60_89_btn_Checked(object sender, RoutedEventArgs e)
        {
            VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void Miettimisaika_11_59_btn_Checked(object sender, RoutedEventArgs e)
        {
            VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void Miettimisaika_enint10_btn_Checked(object sender, RoutedEventArgs e)
        {
            VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_PELOKSI);
        }

        // --------------------------------------------------------------------------------
        // Ottelun tulos-buttonit
        // --------------------------------------------------------------------------------
        //
        // XXX: Lasketaanko, kun button on valittu (Checked="tulosVoitto_btn_Checked")
        // XXX: vai kun siihen siirrytään (GotFocus="tulosVoitto_btn_GotFocus")?
        //
        // XXX: Lasketaan vasta kun kenttä valitaan!
        // XXX: Aiemmin laskettiin jo siirryttäessä ja laitettiin erikseen painike aktiiviseksi.
        //
        // Jos olisi GotFocus:
        //     Ennen laskentaa aseta nykyinen painike valituksi, koska sitä ei
        //     muutoin vielä oltu valittu kenttään siirryttäessä.
        //
        // Jos tässä vaiheessa ei ole vielä annettu SELOja, tulee virheilmoitus
        // sekä siirrytään SELO-kenttään.
        // 
        private void TulosVoitto_btn_Checked(object sender, RoutedEventArgs e)
        {
            //tulosVoitto_btn.IsChecked = true;
            LaskeOttelunTulosLomakkeelta();
        }

        private void TulosTasapeli_btn_Checked(object sender, RoutedEventArgs e)
        {
            //tulosTasapeli_btn.IsChecked = true;
            LaskeOttelunTulosLomakkeelta();
        }

        private void TulosTappio_btn_Checked(object sender, RoutedEventArgs e)
        {
            //tulosTappio_btn.IsChecked = true;
            LaskeOttelunTulosLomakkeelta();
        }
        // Tyhjennä talteen otetut vastustajien/otteluiden tiedot
        private void TyhjennaVastustajat()
        {
            // This still leaves empty lines into comboBox...
            vastustajanSelo_comboBox.Items.Clear();
            ////vastustajanSelo_comboBox.ResetText();
            vastustajanSelo_comboBox.Text = "";
            vastustajanSelo_comboBox.SelectedIndex = -1;
            //// ...but they disappear after adding new item
            ////vastustajanSelo_comboBox.Items.Add("");
            ////vastustajanSelo_comboBox.Items.Clear();
        }

        // Tyhjentää lomakkeen syötteet ja tuloskentät ja palauttaa alkuarvot, miettimisaika vähintään 90 min, ei tulospainikkeita valittuna
        // Suoritetaan kun kirjoitetaan sana "clear" (ilman lainausmerkkejä) vastustajien kenttään ja painetaan Enter
        private void TyhjennaSyotteet()
        {
            selo_in.Text = "";
            pelimaara_in.Text = "";
            miettimisaika_vah90_btn.IsChecked = true;

            tulosVoitto_btn.IsChecked = false;
            tulosTasapeli_btn.IsChecked = false;
            tulosTappio_btn.IsChecked = false;

            TyhjennaVastustajat();
        }

        // tyhjennä tuloskentät näytöltä
        private void TyhjennaTuloskentat()
        {
            uusiSelo_out.Text = "";
            selomuutos_out.Text = "";
            vaihteluvali_out.Text = "";

            uusiPelimaara_out.Text = "";
            turnauksenTulos_out.Text = "";
            odotustulos_out.Text = "";
            keskivahvuus_out.Text = "";
            pisteEro_out.Text = "";
        }

        // Tallenna testausta varten listaan vastustajien & otteluiden tietoja, ei nollata muita kenttiä
        // Suoritetaan kun kirjoitetaan sana "test" (ilman lainausmerkkejä) vastustajien kenttään ja painetaan Enter
        private void TallennaTestaustaVartenVastustajia()
        {
            TyhjennaVastustajat();

            // Tietoa testiaineistosta: SelolaskuriLibrary FormOperations.cs
            for (int i = 0; i < fo.TestaustaVartenVastustajia.Length; i++)
                vastustajanSelo_comboBox.Items.Add(fo.TestaustaVartenVastustajia[i]);
        }

        // --------------------------------------------------------------------------------
        // Kun painettu Enter vastustajan SELO-kentässä, suoritetaan laskenta
        //
        // Tarkistetaan mahdollisesti annetut komennot: clear, test
        // --------------------------------------------------------------------------------
        private void VastustajanSelo_comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter painettu vastustajan selojen tai useammankin syöttämisen jälkeen?
            if (e.Key == Key.Enter) {

                // Prevent beep sound, because Enter is accepted in this form field
                e.Handled = true;

                // Tarkista erikoistapaukset eli mahdolliset komennot
                //   clear  tyhjentää kaikki syötekentät, myös vastustajanSelo_comboBox:n
                //   test   tyhjentää kaikki syötekentät ja laittaa vastustajanSelo_comboBox:iin testausta varten aineistoa
                if (vastustajanSelo_comboBox.Text.Equals("clear")) {
                    // Huom! Jättää muistiin aiemmin lasketut vahvuusluvun ja pelimäärän, jolloin
                    // painike Käytä uutta SELOa jatkolaskennassa voi hakea ne (ei siis palauta 1525,0)
                    TyhjennaSyotteet();
                    TyhjennaTuloskentat();

                    // palauta tekstit
                    //vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
                    return;

                } else if (vastustajanSelo_comboBox.Text.Equals("test")) {
                    // testauksen helpottamista varten vastustajien tietoja
                    TallennaTestaustaVartenVastustajia();
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
        // Rivinvaihto: Environment.NewLine (sama kuin "\r\n")
        //
        private void OhjeitaToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            fo.NaytaOhjeita();
        }

        //
        private void LaskentakaavatToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            fo.NaytaLaskentakaavat();
        }

        //
        private void TietoaOhjelmastaToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {          
            fo.NaytaTietoaOhjelmasta();
        }

        // XXX: WPF/XAML -> XBAP  ei käytössä
        // Lopetuksen varmistaminen
        //      Valittu Menu->Sulje ohjelma
        private void SuljeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // App.xaml :  ShutdownMode="OnExplicitShutdown"
            // calls Window_Closing(), where exiting is confirmed and can be cancelled
            //////            this.Close();
        }

        // Lopetuksen varmistaminen
        //      Suljettu ikkuna
        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    var vastaus = MessageBox.Show("Haluatko poistua ohjelmasta?", "Exit/Close window", MessageBoxButton.YesNo);
        //    if (vastaus == MessageBoxResult.No) {
        //        e.Cancel = true;    // Ei poistutakaan
        //    } else {
        //        Application.Current.Shutdown();
        //    }
        //}


        // XXX: WPF/XAML -> XBAP  Edit-menu ei käytössä, koska leikekirjaan ei käyttöoikeuksia
        //
        // --------------------------------------------------------------------------------
        // Edit
        // --------------------------------------------------------------------------------
        //    Cut
        //    Copy
        //    Paste
        //
        // Edit-menu käsittelee vastustajanSelo-kentän listaa eli historiatietoja

        // Tyhjentää Vastustajat-historiatiedot
        private void CutVastustajatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // At first copy into clipboard
            CopyVastustajatToolStripMenuItem_Click(sender, e);
            TyhjennaVastustajat();
        }

        // Kopioi leikekirjaan Vastustajat-historian
        private void CopyVastustajatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string leikekirja = "";
            foreach (var item in vastustajanSelo_comboBox.Items)
                leikekirja += item.ToString() + Environment.NewLine;
            Clipboard.SetDataObject(leikekirja);
        }

        // Kopioi leikekirjasta Vastustajat-historiaan tekstirivit
        //
        // Ei tarkisteta, että ovatko vastustajat/tulokset oikeassa formaatissa.
        // Vain tarkistukset, että pituus on vähintään seloluvun pituus (eli 4), eikä tule kahta samaa riviä.
        // Ei saa olla myöskään liian pitkä rivi eikä liian montaa riviä.
        //
        // Osa syötteestä on tarkoitus ajaa CSV-formaatissa (silloin täydellinen tai vain miettimisaika otetaan lomakkeelta)
        // Ja osa on tarkoitettu käytettäväksi erillisesti annetun miettimisajan, oman vahvuusluvun ja pelimäärän kanssa.
        //
        // Tekstistä poistetaan ylimääräiset välilyönnit.
        private void PasteVastustajatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Haetaan data leikekirjasta
            IDataObject iData = Clipboard.GetDataObject();
            SelolaskuriOperations so = new SelolaskuriOperations();
            string[] leikekirja = null;
            int lisatytRivit = 0;

            // jos leikekirjassa on tekstiä, niin poista aiemmat vastustajat,
            // käsittele riveittäin, tarkista ja tallenna vastustajanSelo-kenttään
            if (iData.GetDataPresent(DataFormats.Text)) {

                // Tämä lisäisi kaikki rivit tarkistamatta: vastustajanSelo_comboBox.Items.AddRange(iData.GetData(DataFormats.Text).ToString().Split('\n'));
                //
                // Ei tallenneta liian pitkiä tai lyhyitä rivejä, eikä liian montaa riviä, eikä samaa riviä kahdesti.
                // Rivin on aloitettava numerolla (eli selo tai miettimisaika) tai ottelutuloksella (+, - tai =)
                leikekirja = iData.GetData(DataFormats.Text).ToString().Split('\n'); // Ei haittaa, jos on "\r\n", koska poistetaan tarkistuksessa
                foreach (string rivi in leikekirja) {
                    // poista ylimääräiset välilyönnit ennen tarkistusta ja mahdollista tallennusta
                    string rivi2 = so.SiistiVastustajatKentta(rivi.Trim());

                    if (rivi2.Length >= Vakiot.SELO_PITUUS && rivi2.Length <= Vakiot.LEIKEKIRJA_MAX_RIVINPITUUS &&
                        (rivi2[0] == '+' || rivi2[0] == '-' || rivi2[0] == '=' || (rivi2[0] >= '0' && rivi2[0] <= '9')) &&
                        !vastustajanSelo_comboBox.Items.Contains(rivi2)) {
                        // vanhat tiedot poistetaan vain, jos on kelvollista lisättävää
                        if (lisatytRivit == 0)
                            TyhjennaVastustajat();

                        // on poistanut ylimääräiset välilyönnit ennen tallennusta
                        vastustajanSelo_comboBox.Items.Add(rivi2);
                        if (++lisatytRivit >= Vakiot.LEIKEKIRJA_MAX_RIVIMAARA)
                            break;
                    }
                }
            }

            if (lisatytRivit > 0 && null != leikekirja) {
                MessageBox.Show(
                    "Vastustajiin lisätty " + lisatytRivit + (lisatytRivit == 1 ? " rivi." : " riviä.")
                    + " Leikekirjassa oli " + leikekirja.Length + (leikekirja.Length == 1 ? " rivi." : " riviä.")
                    + " Lisätään enintään " + Vakiot.LEIKEKIRJA_MAX_RIVIMAARA + " riviä."
                    + Environment.NewLine
                    + "Huom! Ei tarkistettu, onko kelvollista ottelutietoa. Tarkistettu vain, että rivi alkaa"
                    + Environment.NewLine
                    + "numerolla tai tuloksella  (+-=), rivin pituus on välillä 4 (seloluvun pituus) - " + Vakiot.LEIKEKIRJA_MAX_RIVINPITUUS
                    + Environment.NewLine
                    + "eikä lisätä samoja rivejä.");
            } else {
                MessageBox.Show("Paste: Leikekirjan sisältöä ei hyväksytty. Ei muutettu vastustajia/ottelutietoja.");
            }
        }

    }
}

