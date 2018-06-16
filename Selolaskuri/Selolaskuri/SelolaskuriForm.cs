//
// Shakin vahvuuslukulaskennan lomakkeen käsittely, tarkistuksen ja laskennan kutsuminen, sekä tuloksien näyttäminen
//
// Luotu 10.6.2018 Ismo Suihko (aiemmin 17.11.2017 "Selolaskuri.cs" sisältäen pitkän muutoshistorian)
//
// Muutokset:
//  10.6.2018   Syötteen tarkistukset ja laskenta siirretty omaan moduuliinsa SelolaskuriOperations.cs,
//              jotta niitä voidaan kutsua yksikkötestauksesta (Selolaskuri.Tests / UnitTest1.cs).
//              Syötteet haetaan lomakkeelta erikseen ja sitten kutsutaan tarkistusrutiinia,
//              jonka jälkeen (jos oli OK), kutsutaan laskentaa.
//              Virheilmoitusikkunat tulostetaan tarkistuksista saatujen statuksien mukaan.
//              Aiemmalla rakenteella ei automaattinen testaus olisi ollut mahdollista ainakaan tätä kautta.
//
// Publish --> Versio 2.0.0.0, myös github
//
//  11.6.2018   Jos ei annettu pelimääärää, niin uusi pelimäärä pitää olla tyhjä
//  15.6.2018   Korjattu vastustajanSelo_combobox:iin virheellisen syötteen tallentuminen (vastustajanSelo_combobox_KeyDown)
//
// TODO: Tarkista, ovatko kaikki ohjelman kommentit uuden rakenteen mukaisia
//

using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;


namespace Selolaskuri
{
    public partial class SelolaskuriForm : Form
    {

        // Käytettävät operaatiot, jotka on erotettu käyttöliittymästä
        SelolaskuriOperations so = new SelolaskuriOperations();

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
        private void HaeSyotteetLomakkeelta(Syotetiedot syotteet)
        {
            syotteet.miettimisaika             = HaeMiettimisaika();            // valittu button -> enum
            syotteet.alkuperainenSelo_str      = nykyinenSelo_in.Text;          // merkkijono
            syotteet.alkuperainenPelimaara_str = pelimaara_in.Text;             // merkkijono
            syotteet.vastustajienSelot_str     = vastustajanSelo_comboBox.Text; // merkkijono
            syotteet.ottelunTulos              = HaeOttelunTulos();             // valittu button -> enum tai määrittelemätön
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
        public Vakiot.OttelunTulos_enum HaeOttelunTulos()
        {
            Vakiot.OttelunTulos_enum valinta;

            if (tulosTappio_btn.Checked)
                valinta = Vakiot.OttelunTulos_enum.TULOS_TAPPIO;
            else if (tulosTasapeli_btn.Checked)
                valinta = Vakiot.OttelunTulos_enum.TULOS_TASAPELI;
            else if (tulosVoitto_btn.Checked)
                valinta = Vakiot.OttelunTulos_enum.TULOS_VOITTO;
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
                if (!vastustajanSelo_comboBox.Items.Contains(vastustajanSelo_comboBox.Text))
                    vastustajanSelo_comboBox.Items.Add(vastustajanSelo_comboBox.Text);
            }
        }

        // Kopioi lasketun uuden selon ja mahdollisen pelimäärän käytettäväksi, jotta
        // laskentaa voidaan jatkaa helposti syöttämällä uusi vastustajan selo ja ottelun tulos.
        // Ja sitten siirrytään valmiiksi vastustajan SELO-kenttään.
        //
        // Jos painiketta oli painettu ennen ensimmäistäkään laskentaa, niin
        // otetaan käyttöön pelaajaa luodessa käytetyt alkuarvot 1525 ja 0.
        private void KaytaTulosta_btn_Click(object sender, EventArgs e)
        {
            var lasketutTulokset = so.KopioiLasketutTulokset();  // -> selo ja pelimaara

            nykyinenSelo_in.Text = lasketutTulokset.Item1.ToString();

            // vain, jos pelimaara oli annettu (muutoin on jo valmiiksi tyhjä)
            if (lasketutTulokset.Item2 != (int)Vakiot.PELIMAARA_TYHJA)
                pelimaara_in.Text = lasketutTulokset.Item2.ToString();

            vastustajanSelo_comboBox.Select();
        }

        // Näyttää virheen mukaisen ilmoituksen sekä siirtää kursorin kenttään, jossa virhe
        // Virheellisen kentän arvo näytetään punaisella kunnes ilmoitusikkuna kuitataan
        private void NaytaVirheilmoitus(int tulos)
        {
            string message;

            switch (tulos) {
                case Vakiot.SYOTE_STATUS_OK:
                    break;
                case Vakiot.SYOTE_VIRHE_OMA_SELO:
                    message =
                        String.Format("VIRHE: Nykyisen SELOn oltava numero {0}-{1}.",
                                Vakiot.MIN_SELO, Vakiot.MAX_SELO);
                    nykyinenSelo_in.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    nykyinenSelo_in.ForeColor = Color.Black;

                    // Tyhjennä liian täysi kenttä? Tyhjennä
                    if (nykyinenSelo_in.Text.Length > Vakiot.MAX_PITUUS)
                        nykyinenSelo_in.Text = "";
                    nykyinenSelo_in.Select();
                    break;

                case Vakiot.SYOTE_VIRHE_VAST_SELO:
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
                    tulosTappio_btn.Select();
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
                        String.Format("VIRHE: Turnauksen pistemäärä voi olla enintään sama kuin vastustajien lukumäärä).");
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
        private bool LaskeOttelunTulosLomakkeelta()
        {
            bool status = true;
            Syotetiedot syotteet = new Syotetiedot(); // tiedot nollataan
            Tulokset tulokset = new Tulokset();
            int tulos;

            // hakee syötetyt tekstit ja tehdyt valinnat, ei virhetarkastusta
            HaeSyotteetLomakkeelta(syotteet);  

            // Virhetarkastus ja laskenta erillisessä luokassa SelolaskuriOperations,
            // jotta niitä voidaan kutsua myös yksikkötestauksesta
            if ((tulos = so.TarkistaSyote(syotteet)) != Vakiot.SYOTE_STATUS_OK) {
                NaytaVirheilmoitus(tulos);
                status = false;
            } else {
                so.SuoritaLaskenta(syotteet, ref tulokset);  // tarvitaan ref
                NaytaTulokset(syotteet, tulokset);
            }

            return status;
        }


        // Lasketut tulokset lomakkeelle
        //   Uusi vahvuusluku ja sen muutos +/- NN pistettä
        //   uusi pelimäärä tai tyhjä
        //   piste-ero
        private void NaytaTulokset(Syotetiedot syotteet, Tulokset tulokset)
        {
            //   Uusi vahvuusluku ja sen muutos +/- NN pistettä
            uusiSelo_out.Text = tulokset.laskettuSelo.ToString();
            selomuutos_out.Text = (tulokset.laskettuSelo - syotteet.alkuperainenSelo).ToString("+#;-#;0");

            //   uusi pelimäärä tai tyhjä
            if (syotteet.alkuperainenPelimaara != Vakiot.PELIMAARA_TYHJA)
                uusiPelimaara_out.Text = tulokset.laskettuPelimaara.ToString();
            else
                uusiPelimaara_out.Text = "";

            // Yksi vastustaja: 
            //     piste-ero vastustajaan ja odotustulos
            if (tulokset.vastustajienLkm == 1)
                pisteEro_out.Text = Math.Abs(syotteet.alkuperainenSelo - tulokset.turnauksenKeskivahvuus).ToString();
            else
                pisteEro_out.Text = "";

            // Odotustulosta tai sen summaa ei uudelle pelaajalle
            if (syotteet.alkuperainenPelimaara == Vakiot.PELIMAARA_TYHJA || syotteet.alkuperainenPelimaara > Vakiot.MAX_UUSI_PELAAJA)
                odotustulos_out.Text = (tulokset.odotustulos / 100F).ToString("0.00");
            else
                odotustulos_out.Text = "";

            // Valintanapit tyhjiksi, jos oli muu kuin yksittäisen vastustajan tapaus
            if (syotteet.vastustajanSeloYksittainen == 0) {
                // nappeja on voinut jäädä valituksi, jos on lopulta syötetty yhden ottelun tulos muodossa "1.0 1434" tai "+1434"
                tulosTappio_btn.Checked   = false;
                tulosTasapeli_btn.Checked = false;
                tulosVoitto_btn.Checked   = false;
            }

            // Vahvuusluku on voinut vaihdella laskennan aikana vain, jos tulokset ovat olleet formaatissa "+1622 -1880 =1633"
            // Vaihteluväliä ei ole, jos laskenta on tehty yhdellä lausekkeella tai on ollut vain yksi vastustaja
            if (tulokset.minSelo < tulokset.maxSelo)
                vaihteluvali_out.Text = tulokset.minSelo.ToString() + " - " + tulokset.maxSelo.ToString();
            else
                vaihteluvali_out.Text = "";

            // kerroin on laskettu alkuperäisestä omasta selosta
            kerroin_out.Text = tulokset.kerroin.ToString();

            keskivahvuus_out.Text = tulokset.turnauksenKeskivahvuus.ToString();
            // Turnauksen loppupisteet / ottelujen lkm, esim.  2.5 / 6
            turnauksenTulos_out.Text =
                (tulokset.laskettuTurnauksenTulos / 2F).ToString("0.0") + " / " + tulokset.vastustajienLkm;
        }


        // --------------------------------------------------------------------------------
        // Ottelun tulos-buttonit
        // --------------------------------------------------------------------------------
        // Suorita laskenta aina kun siirrytään tulos-painikkeeseen.
        // Ennen laskentaa asetetaan nykyinen painike valituksi, koska sitä ei
        // muutoin vielä oltu valittu kenttään siirryttäessä.
        //
        // Jos tässä vaiheessa ei ole vielä annettu SELOja, niin tulee virheilmoitus
        // sekä siirrytään SELO-kenttään.
        // 
        private void tulosTappio_Button_Enter(object sender, EventArgs e)
        {
            tulosTappio_btn.Checked = true;
            LaskeOttelunTulosLomakkeelta();
        }

        private void tulosTasapeli_Button_Enter(object sender, EventArgs e)
        {
            tulosTasapeli_btn.Checked = true;
            LaskeOttelunTulosLomakkeelta();
        }

        private void tulosVoitto_Button_Enter(object sender, EventArgs e)
        {
            tulosVoitto_btn.Checked = true;
            LaskeOttelunTulosLomakkeelta();
        }

        // --------------------------------------------------------------------------------
        // Kun painettu Enter vastustajan SELO-kentässä, suoritetaan laskenta
        // --------------------------------------------------------------------------------
        private void vastustajanSelo_combobox_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter painettu vastustajan selojen tai useammankin syöttämisen jälkeen?
            if (e.KeyCode == Keys.Enter) {
                // Kun painettu Enter, niin lasketaan
                // siirrytäänkö myös kentän loppuun? Nyt jäädään samaan paikkaan, mikä myös OK.
                if (LaskeOttelunTulosLomakkeelta()) {

                    // Jos syöte (ja siten laskenta) OK, niin tallenna kentän syötä -> Drop-down combobox
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
        private void ohjeitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Oletuksena tulee OK-painike
            MessageBox.Show("Ohjeita: "
                + "\r\n" + "Syötä oma vahvuusluku. Jos olet uusi pelaaja, niin anna oma pelimäärä 0-10, jolloin käytetään uuden pelaajan laskentakaavaa."
                + "\r\n" + "Syötä lisäksi joko "
                + "\r\n" + "  1) Vastustajan vahvuusluku ja valitse ottelun tulos 0/0,5/1 nuolinäppäimillä tai hiirellä "
                + "\r\n" + "  2) Vahvuusluvut tuloksineen, esim. +1525 =1600 -1611 +1558, jossa + voitto, = tasan ja - tappio"
                + "\r\n" + "  3) Turnauksen pistemäärä ja vastustajien vahvuusluvut, esim. 2.5 1525 1600 1611 1558"
                + "\r\n" + "Lisäksi valitaan miettimisaika yläreunan valintapainikkeilla."
                + "\r\n" + "Ohjelma sisältää sekä SELO:n (pitkä peli) että PELO:n (pikashakki) laskennat.",
                "Ohjeikkuna");
        }

        private void laskentakaavatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Oletuksena tulee OK-painike
            MessageBox.Show("Shakin vahvuusluvun laskentakaavat: http://skore.users.paivola.fi/selo.html"
                + " \r\n" + "Lisätietoa: http://www.shakkiliitto.fi/ ja http://www.shakki.net/cgi-bin/selo",
                "Laskentakaavat");
        }

        private void tietojaOhjelmastaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Oletuksena tulee OK-painike
            MessageBox.Show("Shakin vahvuusluvun laskenta, ohjelmointikieli: C#/.NET/WinForms, https://github.com/isuihko/selolaskuri",
                "Tietoa Selolaskurista");
        }

        private void suljeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Application.Exit() kutsuu ikkunan FormClosing() -rutiinia, jossa varmistus.
            System.Windows.Forms.Application.Exit();
        }

        // Lopetuksen varmistaminen
        //      Suljettu ikkuna
        //      Valittu Menu->Sulje ohjelma (-> Application.Exit())
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult vastaus = MessageBox.Show("Haluatko poistua ohjelmasta?", "Exit/Close window", MessageBoxButtons.YesNo);
            if (vastaus == DialogResult.No) {
                e.Cancel = true;    // Ei poistutakaan
            }
        }


        // --------------------------------------------------------------------------------
        // Miettimisajan valinnan mukaan tekstit: SELO (pidempi peli) vai PELO (pikashakki)
        // --------------------------------------------------------------------------------
        private void vaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum suunta)
        {
            string alkup, uusi;

            if (suunta == Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI) {
                alkup = "PELO";
                uusi = "SELO";
                TuloksetPistemaaranKanssa_teksti.Font = new Font(TuloksetPistemaaranKanssa_teksti.Font, FontStyle.Regular);
            } else {
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
    }
}
