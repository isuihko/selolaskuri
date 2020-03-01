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
// Razor code is based on https://www.twilio.com/blog/validating-phone-numbers-in-razor-pages
//


namespace Selolaskuri.Razor
{


    // *********************   UNDER DEVELOPMENT   *********************


    public class SelolaskuriModel : PageModel
    {

        private readonly SelolaskuriOperations so = new SelolaskuriOperations();                //  Check the input data, calculate the results
        private readonly FormOperations fo = new FormOperations(Vakiot.Selolaskuri_enum.RAZOR); // information and instruction windows etc.

        [BindProperty(SupportsGet = true)]
        public SelolaskuriRazorModel SelolaskuriRazorModel { get; set; }

        public SelolaskuriModel()
        {
            
        }

        public IActionResult OnGet()
        {
            SelolaskuriRazorModel.miettimisaika_in = "90";
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //
            // HAE SYÖTTEET LOMAKKEELTA
            //
            Vakiot.Miettimisaika_enum aika = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;

         
            int temp; // define here instead of "out int temp" for Visual Studio 2015 compatibility
            if (int.TryParse(SelolaskuriRazorModel.miettimisaika_in, out /*int*/ temp) == true)
            {
                if (temp < 1)
                {
                    // ei voida pelata ilman miettimisaikaa
                    // käytetään oletusta = Vakiot.Miettimisaika_enum.MIETTIMISAIKA_VAH_90MIN;
                    ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.miettimisaika_in)}").Value.RawValue = "90";
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

            string selo = SelolaskuriRazorModel.selo_in;
            string pelimaara = SelolaskuriRazorModel.pelimaara_in;
            string vastustajat = SelolaskuriRazorModel.vastustajanSelo_in;
            Vakiot.OttelunTulos_enum tulos = Vakiot.OttelunTulos_enum.TULOS_EI_ANNETTU;

            if (selo == null)
                selo = "";
            if (pelimaara == null)
                pelimaara = "";
            if (vastustajat == null)
                vastustajat = "";

            //Syotetiedot syotetiedot = null; // XXX: can't be null

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
                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.vastustajanSelo_in)}").Value.RawValue = vastustajat;
            }

            Syotetiedot syotetiedot = new Syotetiedot(aika, selo, pelimaara, vastustajat, tulos);

            int status;
            Selopelaaja tulokset = null;

            if ((status = so.TarkistaSyote(syotetiedot)) == Vakiot.SYOTE_STATUS_OK)
            {
                //
                // SUORITA LASKENTA
                //
                tulokset = so.SuoritaLaskenta(syotetiedot);

                //
                // NÄYTÄ TULOKSET
                // 
                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.uusiSelo_out)}").Value.RawValue = tulokset.UusiSelo;
                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.uusiPelimaara_out)}").Value.RawValue = tulokset.UusiPelimaara;
                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.selomuutos_out)}").Value.RawValue =
                    (tulokset.UusiSelo - tulokset.AlkuperainenSelo).ToString("+#;-#;0"); ;

                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.pisteEro_out)}").Value.RawValue =
                    Math.Abs(tulokset.AlkuperainenSelo - tulokset.TurnauksenKeskivahvuus).ToString();

                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.keskivahvuus_out)}").Value.RawValue =
                    (tulokset.TurnauksenKeskivahvuus10x / 10F).ToString("0.0");

                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.turnauksenTulos_out)}").Value.RawValue =
                    (tulokset.TurnauksenTulos2x / 2F).ToString("0.0") + " / " + tulokset.VastustajienLkm;

                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.vaihteluvali_out)}").Value.RawValue =
                        (tulokset.MinSelo < tulokset.MaxSelo) ? tulokset.MinSelo.ToString() + " - " + tulokset.MaxSelo.ToString() : "";


                if (tulokset.UudenPelaajanLaskenta || tulokset.UudenPelaajanPelitLKM > 0)
                {
                    ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.odotustulos_out)}").Value.RawValue = "";

                    ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.UudenPelaajanLaskenta_out)}").Value.RawValue =
                        (tulokset.UudenPelaajanPelitLKM > 0) ? "Uuden pelaajan laskenta " + tulokset.UudenPelaajanPelitLKM + " peliä" : "Uuden pelaajan laskenta          ";
                    //UudenPelaajanLaskenta_txt.Visible = true;
                }
                else
                {
                    ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.odotustulos_out)}").Value.RawValue = (tulokset.Odotustulos / 100F).ToString("0.00");
                    //UudenPelaajanLaskenta_txt.Visible = false;
                    ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.UudenPelaajanLaskenta_out)}").Value.RawValue = "                                 ";
                }

                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.suoritusluku_out)}").Value.RawValue = tulokset.Suoritusluku;
                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.suorituslukuFIDE_out)}").Value.RawValue = tulokset.SuorituslukuFIDE;
                ModelState.FirstOrDefault(x => x.Key == $"{nameof(SelolaskuriRazorModel)}.{nameof(SelolaskuriRazorModel.suorituslukuLineraarinen_out)}").Value.RawValue = tulokset.SuorituslukuLineaarinen;

            }

            return Page();
       }
    }
}
        
