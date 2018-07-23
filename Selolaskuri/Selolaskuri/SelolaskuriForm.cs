//
// Shakin vahvuuslukulaskennan lomakkeen käsittely, tarkistuksen ja laskennan kutsuminen, sekä tuloksien näyttäminen
//
// Luotu 10.6.2018 Ismo Suihko (aiemmin 17.11.2017 "Selolaskuri.cs", alkuvaiheen muutoshistoria poistettu)
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

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Selolaskuri
{
    public partial class SelolaskuriForm : Form
    {
        SelolaskuriOperations so = new SelolaskuriOperations();   //  Syötteen tarkastus ja laskennan kutsuminen. Esim. so.TarkistaSyote

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
        // saadaan pelaajaa luodessa käytetyt alkuarvot SELO 1525 ja pelimääärä 0.
        private void KaytaTulosta_btn_Click(object sender, EventArgs e)
        {
            var lasketutTulokset = so.HaeViimeksiLasketutTulokset();  // -> selo ja pelimaara

            selo_in.Text = lasketutTulokset.Item1.ToString();

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
            int tulos;

            // hakee syötetyt tekstit ja tehdyt valinnat, ei virhetarkastusta
            Syotetiedot syotteet = HaeSyotteetLomakkeelta();

            // Virhetarkastus ja laskenta erillisessä luokassa SelolaskuriOperations,
            // jotta niitä voidaan kutsua myös yksikkötestauksesta
            if ((tulos = so.TarkistaSyote(syotteet)) != Vakiot.SYOTE_STATUS_OK) {
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
            if (tulokset.UudenPelaajanLaskenta)
                odotustulos_out.Text = "";
            else
                odotustulos_out.Text = (tulokset.Odotustulos / 100F).ToString("0.00");

            // kerroin on laskettu alkuperäisestä omasta selosta (laskennan aputieto)
            kerroin_out.Text = tulokset.Kerroin.ToString();

            // Jos ei käytetty tulospainikkeita, niin tuloksen valintanapit varmuuden vuoksi pois päältä
            // Tulospainikkeita käytettäessä yksi vastustajan selo, eikä tulosta annettu muodossa "1.0 1434" tai "+1434"
            if (!tulokset.KaytettiinkoTulospainikkeita) {
                tulosTappio_btn.Checked   = false;
                tulosTasapeli_btn.Checked = false;
                tulosVoitto_btn.Checked   = false;
            }
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
                + "\r\n" + "   1) Yhden vastustajan vahvuusluku (esim. 1922) ja lisäksi ottelun tulos 0/0,5/1 nuolinäppäimillä tai hiirellä. Laskennan tulos päivittyy valinnan mukaan."
                + "\r\n" + "   2) Vahvuusluvut tuloksineen, esim. +1525 =1600 -1611 +1558, jossa + voitto, = tasan ja - tappio"
                + "\r\n" + "   3) Turnauksen pistemäärä ja vastustajien vahvuusluvut, esim. 2.5 1525 1600 1611 1558"
                + "\r\n"
                + "\r\n" + "Laskenta suoritetaan klikkaamalla laskenta-painiketta tai painamalla Enter vastustajan SELO-kentässä sekä (jos yksi vastustaja) tuloksen valinta -painikkeilla."
                + "\r\n"
                + "\r\n" + "Jos haluat jatkaa laskentaa uudella vahvuusluvulla, klikkaa Käytä uutta SELOa jatkolaskennassa. Jos ei ole vielä ollut laskentaa, saadaan uuden pelaajan oletusarvot SELO 1525 ja pelimäärä 0.",

                "Ohjeita");
        }

        private void laskentakaavatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Shakin vahvuusluvun laskentakaavat: http://skore.users.paivola.fi/selo.html"
                + "\r\n" +"Lisätietoa: http://www.shakkiliitto.fi/ ja http://www.shakki.net/cgi-bin/selo",
                "Laskentakaavat");
        }

        private void tietojaOhjelmastaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Shakin vahvuusluvun laskenta, ohjelmointikieli C#/.NET/WinForms"
                + "\r\n" + "Lähdekoodit ja asennusohjelma https://github.com/isuihko/selolaskuri",
                "Tietoa Selolaskurista");
        }

        private void suljeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Application.Exit() kutsuu ikkunan FormClosing() -rutiinia, jossa varmistus ja voidaan perua poistuminen.
            System.Windows.Forms.Application.Exit();
        }

        // Lopetuksen varmistaminen
        //      Suljettu ikkuna
        //      Valittu Menu->Sulje ohjelma (-> Application.Exit())
        private void SelolaskuriForm_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
