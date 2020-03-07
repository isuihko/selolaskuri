using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SelolaskuriLibrary; // SelolaskuriOperations
using Selolaskuri.Razor.Models;
using System; // Exception
using System.Collections.Generic;


//
//
// *********************   UNDER DEVELOPMENT   *********************
//
// Created 29.2.2020
// Razor code was originally based on https://www.twilio.com/blog/validating-phone-numbers-in-razor-pages
//


namespace Selolaskuri.Razor
{


    // *********************   UNDER DEVELOPMENT   *********************


    public class SelolaskuriModel : PageModel
    {

        private readonly SelolaskuriOperations so = new SelolaskuriOperations();                //  Check the input data, calculate the results
        private readonly FormOperations fo = new FormOperations(Vakiot.Selolaskuri_enum.RAZOR); // information and instruction windows etc.

        [BindProperty]
        public int miettimisaika_radiobutton { get; set; }

        [BindProperty(SupportsGet = true)]
        public SelolaskuriRazorModel SelolaskuriRazorModel { get; set; }

        static private int laskettu_uusi_selo = 1525;
        static private int uusi_pelimaara = 0;

        public SelolaskuriModel()
        {
        }

        public IActionResult OnGet()
        {
            // Aseta oletusarvo (ks. Vakiot.Miettimisaika_enum)
            if (miettimisaika_radiobutton < 10 || miettimisaika_radiobutton > 90)
                miettimisaika_radiobutton = 90;

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

            if (miettimisaika_radiobutton <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN)
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN;
            else if (miettimisaika_radiobutton <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN)
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_11_59MIN;
            else if (miettimisaika_radiobutton <= (int)Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN)
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_60_89MIN;
            else
                aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;

            string selo = SelolaskuriRazorModel.selo_in;
            string pelimaara = SelolaskuriRazorModel.pelimaara_in;
            string vastustajat = SelolaskuriRazorModel.vastustajanSelo_in;

            // Virhetilanne, jos vastustajat on tyhjä (jatketaan silti vielä)
            if (selo == null)
                selo = "";
            if (pelimaara == null)
                pelimaara = "";
            if (vastustajat == null)
                vastustajat = "";

            //
            // TARKISTA SYÖTE
            //

            // process opponents field and check if CSV format was used
            //
            vastustajat = vastustajat.Trim();
            if (string.IsNullOrWhiteSpace(vastustajat) == false)
            {
                // poista ylimääräiset välilyönnit, korvaa yhdellä
                // poista myös välilyönnit pilkun molemmilta puolilta, jos on CSV-formaatti
                vastustajat = so.SiistiVastustajatKentta(vastustajat); // .Trim jo tehty

                // näytölle siistitty versio
                // XXX: ehkä voi muuttaa kentän arvoa muutenkin?
                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.vastustajanSelo_in)}").Value.RawValue = vastustajat;
            }

            Syotetiedot syotetiedot = new Syotetiedot(aika, selo, pelimaara, vastustajat, Vakiot.OttelunTulos_enum.TULOS_EI_ANNETTU);

            int status;
            Selopelaaja tulokset = null;

            if ((status = so.TarkistaSyote(syotetiedot)) == Vakiot.SYOTE_STATUS_OK)
            {
                //ViewData["virhe"] = "                                                                ";

                //
                // SUORITA LASKENTA
                //
                tulokset = so.SuoritaLaskenta(syotetiedot);

                laskettu_uusi_selo = tulokset.UusiSelo;
                uusi_pelimaara = tulokset.UusiPelimaara;

                //
                // NÄYTÄ TULOKSET
                //
                // XXX: järjestä tulokset näytölle paremmin!
                // 
                ViewData["TULOKSET"] = "TULOKSET";

                ViewData["uusi_selo"] = "Uusi vahvuusluku " + tulokset.UusiSelo;
                ViewData["selomuutos"] = "    Muutos : " + (tulokset.UusiSelo - tulokset.AlkuperainenSelo).ToString("+#;-#;0")
                                       + "    Vaihteluväli " + ((tulokset.MinSelo < tulokset.MaxSelo) ? tulokset.MinSelo.ToString() + " - " + tulokset.MaxSelo.ToString() : "");

                if (tulokset.UusiPelimaara > 0)
                    ViewData["uusi_pelimaara"] = "    Uusi pelimäärä: " + tulokset.UusiPelimaara;

                if (tulokset.UudenPelaajanLaskenta || tulokset.UudenPelaajanPelitLKM > 0)
                {
                    ViewData["odotustulos"] = "";
                    ViewData["uudenpelaajanlaskenta"] = (tulokset.UudenPelaajanPelitLKM > 0) ? " uuden pelaajan laskentaa " + tulokset.UudenPelaajanPelitLKM + " peliä" : " (uuden pelaajan laskenta)          ";
                }
                else
                {
                    ViewData["odotustulos"] = "Odotustulos: " + (tulokset.Odotustulos / 100F).ToString("0.00");
                    ViewData["uudenpelaajanlaskenta"] = "                                 ";
                }

                ViewData["turnauksentulos"] = "Turnauksen tulos: " + (tulokset.TurnauksenTulos2x / 2F).ToString("0.0") + " / " + tulokset.VastustajienLkm;
                ViewData["keskivahvuus"] = "Keskivahvuus: " + (tulokset.TurnauksenKeskivahvuus10x / 10F).ToString("0.0") +   "  Piste-ero: " + Math.Abs(tulokset.AlkuperainenSelo - tulokset.TurnauksenKeskivahvuus).ToString();            
                ViewData["suoritusluku"] = "Suoritusluku: " + tulokset.Suoritusluku + "   FIDE: " + tulokset.SuorituslukuFIDE + "   Lineaarinen: " + tulokset.SuorituslukuLineaarinen;
            }
            else
            {              
                if (status <= Vakiot.SYOTE_STATUS_OK && status >= Vakiot.SYOTE_VIRHE_MAX) {
                    ViewData["virhe"] = Vakiot.SYOTE_VIRHEET_text[Math.Abs(status)]; 
                }
            }

            return Page();
        }


        // kopioi edellisen laskennan tulos ja pelimäärä
        // jos ei vielä ollut laskentaa, kopioi 1525 ja 0
        public IActionResult OnPostKaytaTulostaJatkolaskennassa()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // XXX: Muuta syötekenttiä! Ehkä voi tehdä muutenkin??
            // ei toimi SelolaskuriRazorModel.selo_in = "1234";

            ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.selo_in)}").Value.RawValue = laskettu_uusi_selo.ToString();
            if (uusi_pelimaara >= 0)
                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.pelimaara_in)}").Value.RawValue = uusi_pelimaara.ToString();

            return Page();
        }
    }
}
        
