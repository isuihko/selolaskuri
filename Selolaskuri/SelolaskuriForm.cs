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
// 11.8.2018        - Lisätty valikko Edit, jossa cut tyhjentää vastustaja-historian, copy kopioi vastustajat leikekirjaan
//                    ja paste kopioi tekstin leikekirjasta vastustaja-historiaan.
//                    Pastessa vain tarkistukset, että pituus on vähintään seloluvun pituus (eli 4), eikä tule kahta samaa riviä.
//                    Lisätään enintään 100 riviä.
//                  - Lisätty virheilmoitus virheelliselle miettimisajalla, joka on mahdollinen (vain) CSV-formaatissa,
//                    kun voidaan antaa miettimisaika (minuutit) numerona
//
// Publish --> Versio 2.1.0.0, myös github
//
// 12.8.2018        - Pastessa nyt rajoitettu myös rivin maksipituus sekä rivin pitää alkaa sallitulla merkillä (numero tai tulos +-=).
//                  - Muokattu Paste-toiminnon ilmoitusikkunaa.
//                  - Vakiot leikekirjan käytön rajoituksille (rivin pituus 1000 ja rivien lkm 100)
//                  - Edit-menun tekstejä muokattu sekä &Edit (eli Alt-E toimii)
//
// Publish --> Versio 2.1.0.1, myös github
//
// 15.8.2018        - Yksikkötestausta muutettu, testitapaukset jaettu moduuleihin testattavan asian mukaan
//                  - CSV-formaatin testaukseen muutos: merkkijonon alkukäsittely tehdään uudella yleisellä rutiinilla,
//                    joka on SelolaskuriOperations-luokassa ja jota nyt myös käytetään tietoja haettaessa lomakkeelta.
//                    Nyt merkkijonon jakaminen osiin saadaan testaukseen.
//
// 19.8.2018        - CSV-formaatin testit jaettu kahteen moduuliin:
//                       UnitTest4_TarkistaCSV:  virheellinen data
//                       UnitTest5_Laskenta:     kelvollinen data, josta lasketaan
//                  - CSV-formaatin tarkistuksia lisätty. Poistetaan ylimääräiset välilyönnit alusta ja lopusta, sekä pilkkujen ympäriltä:
//                        "   90 , 1525 ,  20  ,    2.5 1505 1600    1611 1558   "  -> "90,1525,20,2.5 1505 1600 1611 1558"
//                    Siistitty versio tallennetaan vastustajat-historiaan. Tällöin samasisältöinen, mutta eri määrät välilyöntejä, ei tallennu
//                    historiaan kuin kerran.
//                    Aiemmin laskenta epäonnistui, jos formaatissa oli vastustajia edeltävän pilkun jälkeen välilyönti.
//                  - Välilyöntien poisto (using Regex) luokassa SelolaskuriOperation (aiemmin oli lomake ja unit test)
//                  - Paste: Tehdään ylimääräisten välilyöntien poisto ennen tallennusta.
//                  - Lisätty testitapauksia laskentaan em. korjauksien varmistamiseen ja
//                    nyt yhteensä 65 testiä.
//
// Publish --> Versio 2.1.0.2, myös github
//
// 20.8.2018        - Pikashakin laskentaan Math.Round, jolloin saadaan täsmälleen samat tulokset kuin kolmessa
//                    Joukkupikashakin SM-kisoista otetusta esimerkistä. Pyöristyksissä myös +0.001, koska muutoin
//                    esim. Math.Round(2.5) olisi 2. Nyt Math.Round(2.5+0.001) on oikein 3.
//                  - ½ (alt - numeerisesta näppäimistöstä 171) on sallittu yksittäisen ottelun tuloksessa CSV-formaatissa
//                  - Myös turnauksen tuloksessa voidaan käyttää puolikasta, esim. 10½
//                  - Lomakkeeseen vaihdettu tasapelibuttonin teksti 1/2 = tasapeli  ->  ½ = tasapeli
//                  - Lisää testausta, mm. ½:n käyttö. Nyt 76 testiä.
//
// Publish --> Versio 2.1.0.3, myös github
//
//  21.8.2018       Muutoksia tehty WPF-versiota tehdessä, ks. GitHub SelolaskuriWPF
//                  - Painikkeen teksti "Käytä uutta SELOa jatkolaskennassa" -> "Käytä tulosta jatkolaskennassa"
//                  - Päivetty myös ohjeikkunaan sekä poistettu "SELO" - "PELO"-muutos tekstikentältä
//
//  22.8.2018       - Pientä järjestelyä, muutoksia kommentteihin
//
// Publish --> Versio 2.1.0.4, myös github
//
// 25.8.2018        - Lomakkeen tulos-buttonien käsittely, käytä CheckedChanged
//                  - vähän leveämpi ikkuna, versionumero (v2.1.0.4) poistettu otsikosta, versiopvm riittää
//                  - "test"-komennolla lisätään kolme esimerkkiä Joukkuepikashakin SM-kisoista vastustajat-historiaan
//                  - kommentteja
//
// Publish --> Versio 2.1.0.5, myös github
//
// 26.8.2018        - Aseta versionumero 1.0.0.0:ksi, kuten on myös ensimmäisillä WPF/XAML- ja XBAP/web-versioilla
//
// Publish --> Versio 1.0.0.0, myös github
//
// 4.9.2018         - Siirretty ohje- ja tietoikkunat kirjastoon SelolaskuriLibrary.
//                  - Siirretty myös Vastustajat-kenttään testausta varten laitettavat merkkijonot kirjastoon
//
// 5.9.2018         - FormOperations.cs: Ohje- ja tietoikkunoiden tekstit taulukoissa, joista niitä on helpompi muokata
//
// 16.-17.2.2020    - vastustajien keskivahvuus näytetään yhdellä desimaalilla, aiemmin kokonaisluku
//                  - virheilmoitukset uuden pelaajan laskentaan, jossa jatketaan normaalilla vanhan pelaajan laskennalla
//                  - virheilmoituksien tekstit nyt Vakiot.cs:ssä, joten niitä on helpompi ylläpitää
//
// TODO: F1 = ohjeikkuna
// TODO: web-versio
// TODO: Menujen käsittelyrutiinit ovat samat kaikissa versioissa, saakohan kirjastoon?
//

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using SelolaskuriLibrary;

namespace Selolaskuri
{
    public partial class SelolaskuriForm : Form
    {
        private readonly SelolaskuriOperations so = new SelolaskuriOperations();                   //  Check the input data, calculate the results
        private readonly FormOperations fo = new FormOperations(Vakiot.Selolaskuri_enum.WINFORMS); // information and instruction windows etc.

        public SelolaskuriForm()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------------------
        // LOMAKKEEN KENTTIEN ARVOJEN HAKEMINEN LASKENTAA ALOITETTAESSA
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
                valinta = Vakiot.OttelunTulos_enum.TULOS_MAARITTELEMATON; // ei mahdollinen

            return valinta;
        }


        // --------------------------------------------------------------------------------
        // Painikkeiden toiminta
        // --------------------------------------------------------------------------------
        //    Laske vahvuusluku
        //    Käytä tulosta jatkolaskennassa

        // Suoritetaan laskenta -button
        private void Laske_btn_Click(object sender, EventArgs e)
        {
            if (LaskeOttelunTulosLomakkeelta()) { 
                // Annettu teksti talteen (jos ei ennestään ollut) -> Drop-down Combo box
                // Tallennus kun klikattu Laske vahvuusluku tai painettu enter vastustajan selo-kentässä
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
            string message = "VIRHETEKSTI ALUSTAMATTA";
            if (virhestatus <= Vakiot.SYOTE_STATUS_OK && virhestatus >= Vakiot.SYOTE_VIRHE_MAX)
                message = Vakiot.SYOTE_VIRHEET_text[Math.Abs(virhestatus)];
            //MessageBox.Show(message);

            // Erikoiskäsittely muutamalla virheelle, siirrytään tiettyyn kenttään
            // Tai kentän värjääminen punaiseksi virheilmoituksen ajaksi
            // Näytetään virheilmoitus
            switch (virhestatus) {
                case Vakiot.SYOTE_VIRHE_OMA_SELO:
                    selo_in.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    selo_in.ForeColor = Color.Black;

                    // Tyhjennä liian täysi kenttä? Tyhjennä
                    if (selo_in.Text.Length > Vakiot.MAX_PITUUS)
                        selo_in.Text = "";
                    selo_in.Select();
                    break;

                case Vakiot.SYOTE_VIRHE_PELIMAARA:
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
                    MessageBox.Show(message);
                    tulosVoitto_btn.Select();  // ensimmäinen tulos-painikkeista
                    break;

                case Vakiot.SYOTE_VIRHE_MIETTIMISAIKA_CSV:
                case Vakiot.SYOTE_VIRHE_VASTUSTAJAN_SELO:
                case Vakiot.SYOTE_VIRHE_YKSITTAINEN_TULOS:
                case Vakiot.SYOTE_VIRHE_TURNAUKSEN_TULOS:
                case Vakiot.SYOTE_VIRHE_CSV_FORMAT:
                case Vakiot.SYOTE_VIRHE_UUDEN_PELAAJAN_OTTELUT_ENINT_10:
                case Vakiot.SYOTE_VIRHE_UUDEN_PELAAJAN_OTTELUT_VAHINT_11:
                    vastustajanSelo_comboBox.ForeColor = Color.Red;
                    MessageBox.Show(message);
                    vastustajanSelo_comboBox.ForeColor = Color.Black;
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
            //keskivahvuus_out.Text = tulokset.TurnauksenKeskivahvuus.ToString();
            keskivahvuus_out.Text = (tulokset.TurnauksenKeskivahvuusDesim / 10F).ToString("0.0");

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
            if (tulokset.Miettimisaika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN)
                VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_PELOKSI);
            else
                VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        // --------------------------------------------------------------------------------
        // Miettimisajan valinnan mukaan tekstit: SELO (pidempi peli) vai PELO (pikashakki)
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
        private void Miettimisaika_vah90_btn_CheckedChanged(object sender, EventArgs e)
        {
            VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void Miettimisaika_60_89_btn_CheckedChanged(object sender, EventArgs e)
        {
            VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void Miettimisaika_11_59_btn_CheckedChanged(object sender, EventArgs e)
        {
            VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
        }

        private void Miettimisaika_enint10_btn_CheckedChanged(object sender, EventArgs e)
        {
            VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_PELOKSI);
        }


        // --------------------------------------------------------------------------------
        // Ottelun tulos-buttonit
        // --------------------------------------------------------------------------------
        // Suorita laskenta aina kun tulos-painike on valittu.
        //
        // Jos tässä vaiheessa ei ole vielä annettu SELOja (oma ja yksi vastustaja),
        // tulee virheilmoitus sekä siirrytään kenttään, josta puuttuu tieto.
        // 
        private void TulosVoitto_btn_CheckedChanged(object sender, EventArgs e)
        {
            LaskeOttelunTulosLomakkeelta();
        }

        private void TulosTasapeli_btn_CheckedChanged(object sender, EventArgs e)
        {
            LaskeOttelunTulosLomakkeelta();
        }

        private void TulosTappio_btn_CheckedChanged(object sender, EventArgs e)
        {
            LaskeOttelunTulosLomakkeelta();
        }

        // Tyhjennä talteen otetut vastustajien/otteluiden tiedot
        private void TyhjennaVastustajat()
        {
            // This still leaves empty lines into comboBox...
            vastustajanSelo_comboBox.Items.Clear();
            vastustajanSelo_comboBox.ResetText();
            // ...but they disappear after adding new item
            vastustajanSelo_comboBox.Items.Add("");
            vastustajanSelo_comboBox.Items.Clear();
        }

        // Tyhjentää lomakkeen syötteet ja tuloskentät ja palauttaa alkuarvot, miettimisaika vähintään 90 min, ei tulospainikkeita valittuna
        // Suoritetaan kun kirjoitetaan sana "clear" (ilman lainausmerkkejä) vastustajien kenttään ja painetaan Enter
        private void TyhjennaSyotteet()
        {
            selo_in.Text = "";
            pelimaara_in.Text = "";
            miettimisaika_vah90_btn.Select();

            tulosVoitto_btn.Checked = false;
            tulosTasapeli_btn.Checked = false;
            tulosTappio_btn.Checked = false;

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
            if (e.KeyCode == Keys.Enter) {

                // Prevent beep sound, because Enter is accepted in this form field
                e.Handled = true;

                // Tarkista erikoistapaukset eli mahdolliset komennot
                //   clear  tyhjentää kaikki syötekentät, myös vastustajanSelo_comboBox:n
                //   test   tyhjentää kaikki syötekentät ja laittaa vastustajanSelo_comboBox:iin testausta varten aineistoa
                if (vastustajanSelo_comboBox.Text.Equals("clear")) {
                    // Huom! Jättää muistiin aiemmin lasketut vahvuusluvun ja pelimäärän, jolloin
                    // painike Käytä tulosta jatkolaskennassa voi hakea ne (ei siis palauta 1525,0)
                    TyhjennaSyotteet();
                    TyhjennaTuloskentat();

                    // palauta tekstit
                    VaihdaSeloPeloTekstit(Vakiot.VaihdaMiettimisaika_enum.VAIHDA_SELOKSI);
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
                    // Tallennus myös, kun klikattu Laske vahvuusluku
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
        private void OhjeitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fo.NaytaOhjeita();
        }
        
        //
        private void LaskentakaavatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fo.NaytaLaskentakaavat();
        }

        //
        private void TietoaOhjelmastaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fo.NaytaTietoaOhjelmasta(Program.GetPublishVersion());
        }

        // Lopetuksen varmistaminen
        //      Valittu Menu->Sulje ohjelma
        private void SuljeToolStripMenuItem_Click(object sender, EventArgs e)
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
                        !vastustajanSelo_comboBox.Items.Contains(rivi2))
                    {
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
