﻿using Microsoft.AspNetCore.Http; // HttpContext.Session
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelolaskuriLibrary; // SelolaskuriOperations
// using Selolaskuri.Razor.Models;
using System; // Exception


//
//
// *********************   UNDER DEVELOPMENT   *********************
//
// Created 29.2.2020
// Modified 16.3.2020
//
// Razor code was originally based on https://www.twilio.com/blog/validating-phone-numbers-in-razor-pages
// but then started to use BindProperty, ViewData["fieldname"], TextBoxFor() etc.
//
// Still some work to do at least with user interface. Create mobile version etc. Use Ajax?
//


namespace Selolaskuri.Razor
{
    public class SelolaskuriModel : PageModel
    {
        private readonly SelolaskuriOperations so = new SelolaskuriOperations(); //  Check the input data, calculate the results

        // SYÖTEKENTÄT xxx_in

        [BindProperty]
        public string selo_in { get; set; }

        [BindProperty]
        public string pelimaara_in { get; set; }

        [BindProperty]
        public int miettimisaika_radiobutton_in { get; set; }

        [BindProperty]
        public string vastustajat_in { get; set; }

        private readonly string Newline = "\n"; // Environment.NewLine would create two line breaks in Microsoft Edge

        public SelolaskuriModel()
        {
            Vastustajat_ohjeteksti = "Täytä oma vahvuusluku-kenttä ja pelimäärä (jos enintään 10)" + Newline
                + "Anna vastustaja tai vastustajat tuloksineen, esim. +1600 1785 -1882" + Newline
                + "Tai koko turnauksen tulos ja vastustajat: 1.5 1600 1785 1882 "
                + "jolla tavalla tulokset yleensä annetaan pikashakin laskentaa varten. Nyt pikashakin laskenta toimii molemmilla em. tavoilla syötettynä." + Newline
                + Newline
                + "Ottelu tai koko turnaus voidaan antaa myös csv-formaatissa, jossa annetut tiedot korvaavat "
                + "lomakkeen kentissä mahdollisesti olevat tiedot: miettimisaika, oma vahvuusluku ja pelimäärä." + Newline
                + "Esim. 'oma selo,tulokset' 1805,+1600 1785 1882 " + Newline
                + "tai 1820,1.5 1600 1785 1882" + Newline
                + "tai 'miettimisaika(minuutit),oma selo,pelimäärä(tai tyhjä),tulokset'" + Newline
                + "esim. pikashakin laskenta 5,1680,,1.5 1600 1822 1882" + Newline
                + Newline
                + "Normaalisti SELO lasketaan joko uuden tai vanhan pelaajan laskentakaavalla." + Newline
                + "Mutta voidaan myös käyttää molempia aloittaen uuden pelaajan laskennalla, kun laskennan vaihtokohtaan laitetaan '/'. "
                + "Tällöin uuden pelaajan pelimäärän on oltava aluksi enintään 10 ja on tultava täyteen vähintään 11 peliä ennen vaihtoa." + Newline
                + "Esim. selo 1700, pelimäärä 8, vastustajat " + Newline
                + " 1612 +1505 1850 -2102 / -1611 +1558" + Newline
                + "tai csv: 1700,8,1612 +1505 1850 -2102 / -1611 +1558";
        }

        [BindProperty]
        public string Vastustajat_ohjeteksti { get; set; }

        public IActionResult OnGet()
        {
            // Aseta oletusarvo
            // Lomaketta päivitettäessä tämä palautuu 90:een
            if (miettimisaika_radiobutton_in < (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN || miettimisaika_radiobutton_in > (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN)
                miettimisaika_radiobutton_in = (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;

            //// sivu ladattu uudestaan, tiedot pitää asettaa uudestaan
            //// Muista viimeisimmät arvot lomakkeen piilotetuilla kentillä
            //if (HttpContext.Session.GetInt32("laskettu selo") != null)
            //    ViewData["viimeksi_laskettu_selo"] = HttpContext.Session.GetInt32("laskettu selo").ToString();
            //if (HttpContext.Session.GetInt32("laskettu uusi pelimaara") != null)
            //    ViewData["viimeksi_laskettu_pelimaara"] = HttpContext.Session.GetInt32("laskettu uusi pelimaara").ToString();

            // XXX: Voiko käyttää Session["variable"]?
            if (HttpContext.Session.GetInt32("kayta tulosta") > 0)
            {
                HttpContext.Session.SetInt32("kayta tulosta", 0);

                // XXX: this is used in desktop applications
                // XXX: Check this, why it does not work (values stored in a library?)
                //int selo1 = so.HaeViimeksiLaskettuSelo();
                //int pelimaara1 = so.HaeViimeksiLaskettuPelimaara();
                //selo_in = selo1.ToString();
                //if (pelimaara1 != (int)Vakiot.PELIMAARA_TYHJA)
                //    pelimaara_in = pelimaara1.ToString();

                selo_in = HttpContext.Session.GetInt32("laskettu selo").ToString();
                // jos ei vielä ollut laskentaa, laitetaan uuden pelaajan arvot
                if (string.IsNullOrEmpty(selo_in))
                {
                    selo_in = "1525";
                    pelimaara_in = "0";
                }
                else if (HttpContext.Session.GetInt32("laskettu uusi pelimaara") > 0)
                    pelimaara_in = HttpContext.Session.GetInt32("laskettu uusi pelimaara").ToString();

                if (HttpContext.Session.GetInt32("miettimisaika") > 0)
                    miettimisaika_radiobutton_in = (int)HttpContext.Session.GetInt32("miettimisaika");

                //// Muista viimeisimmät arvot lomakkeen piilotetuilla kentillä
                //ViewData["viimeksi_laskettu_selo"] = selo_in;
                //ViewData["viimeksi_laskettu_pelimaara"] = pelimaara_in;
            }

            return Page();
        }

        public IActionResult OnPostLaskeVahvuusluku()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //
            // HAE SYÖTTEET LOMAKKEELTA
            //
            Vakiot.Miettimisaika_enum aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;

            if (miettimisaika_radiobutton_in <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN)
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN;
            else if (miettimisaika_radiobutton_in <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN)
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN;
            else if (miettimisaika_radiobutton_in <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN)
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN;
            else
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;

            string selo = selo_in;
            string pelimaara = pelimaara_in;
            string vastustajat = vastustajat_in;

            // Virhetilanne, jos vastustajat on tyhjä (jatketaan silti vielä)
            if (selo == null)
                selo = "";
            if (pelimaara == null)
                pelimaara = "";
            if (vastustajat == null)
                vastustajat = "";

            //
            // KÄSITTELE JA TARKISTA SYÖTE
            //
            vastustajat = vastustajat.Trim();
            if (string.IsNullOrWhiteSpace(vastustajat) == false)
            {
                // poista ylimääräiset välilyönnit, korvaa yhdellä
                // poista myös välilyönnit pilkun molemmilta puolilta, jos on CSV-formaatti
                vastustajat = so.SiistiVastustajatKentta(vastustajat); // .Trim jo tehty

                // näytölle siistitty versio (tämä ei toimikaan näin helposti, Ajax?)
                vastustajat_in = vastustajat;
            }

            Syotetiedot syotetiedot = new Syotetiedot(aika, selo, pelimaara, vastustajat, Vakiot.OttelunTulos_enum.TULOS_EI_ANNETTU);

            int status;

            if ((status = so.TarkistaSyote(syotetiedot)) == Vakiot.SYOTE_STATUS_OK)
            {
                //
                // LASKE
                //
                Selopelaaja tulokset = so.SuoritaLaskenta(syotetiedot);

                //// jos on CSV, voidaan ottaa sieltä oikeasti käytetty miettimisaika
                //// tosin se ei päivity näytölle näin
                //if (aika != tulokset.Miettimisaika)
                //{
                //    miettimisaika_radiobutton_in = (int)tulokset.Miettimisaika;
                //}

                // Viimeksi lasketut tulokset talteen
                HttpContext.Session.SetInt32("laskettu selo", tulokset.UusiSelo);
                if (tulokset.UusiPelimaara > 0)
                    HttpContext.Session.SetInt32("laskettu uusi pelimaara", tulokset.UusiPelimaara);
                else
                    HttpContext.Session.SetInt32("laskettu uusi pelimaara", 0);
                HttpContext.Session.SetInt32("miettimisaika", miettimisaika_radiobutton_in);


                //
                // NÄYTÄ TULOKSET
                //
                // XXX: järjestä tulokset näytölle paremmin!
                // 
                ViewData["TULOKSET"] = "Tulokset";

                ViewData["uusi_selo_nimi"] = "Uusi vahvuusluku (" + (tulokset.Miettimisaika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN ? "PELO" : "SELO") + "): ";
                ViewData["uusi_selo"] = tulokset.UusiSelo;

                //// Muista viimeisimmät arvot lomakkeen piilotetuilla kentillä
                //ViewData["viimeksi_laskettu_selo"] = tulokset.UusiSelo.ToString();
                //if (tulokset.UusiPelimaara > 0)
                //    ViewData["viimeksi_laskettu_pelimaara"] = tulokset.UusiPelimaara.ToString();
                //else
                //    ViewData["viimeksi_laskettu_pelimaara"] = "";

                ViewData["selomuutos"] = "    Muutos: " + (tulokset.UusiSelo - tulokset.AlkuperainenSelo).ToString("+#;-#;0")
                                       + ((tulokset.MinSelo < tulokset.MaxSelo) ? "    Vaihteluväli: " + tulokset.MinSelo.ToString() + " - " + tulokset.MaxSelo.ToString() : "");

                if (tulokset.UusiPelimaara > 0)
                {
                    ViewData["uusi_pelimaara_nimi"] = "Uusi pelimäärä: ";
                    ViewData["uusi_pelimaara"] = tulokset.UusiPelimaara;
                    
                }

                if (tulokset.UudenPelaajanLaskenta || tulokset.UudenPelaajanPelitLKM > 0)
                {
                    ViewData["odotustulos"] = "";
                    ViewData["uudenpelaajanlaskenta"] = (tulokset.UudenPelaajanPelitLKM > 0) ? " (uuden pelaajan laskentaa " + tulokset.AlkuperainenPelimaara + "+" + tulokset.UudenPelaajanPelitLKM + " peliä)" : " (uuden pelaajan laskenta) ";
                }
                else
                {
                    ViewData["odotustulos"] = "Odotustulos: " + (tulokset.Odotustulos / 100F).ToString("0.00");
                    ViewData["uudenpelaajanlaskenta"] = "";
                }

                ViewData["turnauksentulos"] = "Turnauksen tulos: " + (tulokset.TurnauksenTulos2x / 2F).ToString("0.0") + " / " + tulokset.VastustajienLkm;

                // Keskivahvuus ja piste-ero
                // Vastustajien vahvuuslukujen keskiarvo. Kumpi, ei desimaaleja vai yksi desimaali?
                // oman vahvuusluvun piste-ero turnauksen keskivahvuuteen nähden
                // näytetään etumerkki miinus, jos turnaus on heikompi
                //ViewData["keskivahvuus"] = "Keskivahvuus: " + (tulokset.TurnauksenKeskivahvuus10x / 10F).ToString("0.0") + " Piste-ero: " + (Math.Abs(10*tulokset.AlkuperainenSelo - tulokset.TurnauksenKeskivahvuus10x)/10F).ToString("0.0");
                ViewData["keskivahvuus"] = "Keskivahvuus: " + tulokset.TurnauksenKeskivahvuus.ToString() + " Piste-ero: " + /*Math.Abs*/(tulokset.TurnauksenKeskivahvuus - tulokset.AlkuperainenSelo).ToString("+#;-#;0");

                if (tulokset.VastustajienLkm > 1)
                    ViewData["vastustajat_alue"] = "Vastustajat (min-max): " + tulokset.VastustajaMin + " - " + tulokset.VastustajaMax;

                ViewData["suoritusluku"] = "Suoritusluku: " + tulokset.Suoritusluku;
                ViewData["suorituslukuFIDE"] = " Suoritusluku FIDE: " + tulokset.SuorituslukuFIDE;
                ViewData["suorituslukuLineaarinen"] = "Suoritusluku lineaarinen: " + tulokset.SuorituslukuLineaarinen;
            }
            else
            {
                //// Muista viimeisimmät arvot lomakkeen piilotetuilla kentillä
                //if (HttpContext.Session.GetInt32("laskettu selo") != null)
                //    ViewData["viimeksi_laskettu_selo"] = HttpContext.Session.GetInt32("laskettu selo").ToString();
                //if (HttpContext.Session.GetInt32("laskettu uusi pelimaara") != null)
                //    ViewData["viimeksi_laskettu_pelimaara"] = HttpContext.Session.GetInt32("laskettu uusi pelimaara").ToString();

                // virhestatus on negatiivinen luku, hae virheteksti
                if (Vakiot.SYOTE_STATUS_OK >= status && status >= Vakiot.SYOTE_VIRHE_MAX) {
                    // requires <div asp-validation-summary... >
                    ModelState.AddModelError(string.Empty, Vakiot.SYOTE_VIRHEET_text[Math.Abs(status)]);
                }
            }

            return Page();
        }


        // kopioi lomakkeelle edellisen laskennan tulos ja pelimäärä
        // jos ei vielä ollut laskentaa, kopioi 1525 ja 0
        public IActionResult OnPostKaytaTulostaJatkolaskennassa()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            HttpContext.Session.SetInt32("kayta tulosta", 1); // tulokset Session-muuttujista

            return RedirectToPage("Selolaskuri"); // --> OnGet() tyhjentää kentät sekä laittaa uuden vahvuusluvun ja pelimäärän näytölle
        }
    }
}
        
