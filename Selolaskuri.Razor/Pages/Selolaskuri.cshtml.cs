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
// Modified 8.3.2020
//
// Razor code was originally based on https://www.twilio.com/blog/validating-phone-numbers-in-razor-pages
// but then started to use BindProperty, ViewData["fieldname"], TextBoxFor() etc.
//
// Still some work to do.
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

        // APUKENTÄT
        static private int laskettu_uusi_selo = 1525;
        static private int uusi_pelimaara = 0;
        static private bool kayta_tulosta = false;

        public SelolaskuriModel()
        {
            Vastustajat_ohjeteksti = "Täytä oma vahvuusluku-kenttä ja pelimäärä (jos enintään 10)" + Environment.NewLine
                + "Anna vastustaja tai vastustajat tuloksineen, esim. +1600 1785 -1882" + Environment.NewLine
                + "Tai koko turnauksen tuloksesi ja vastustajat: 1.5 1600 1785 1882" + Environment.NewLine
                 + Environment.NewLine
                + "Ottelu tai koko turnaus voidaan antaa myös csv-formaatissa, jossa annetut tiedot korvaavat "
                + "lomakkeen kentissä olevat tiedot: miettimisaika, oma vahvuusluku ja pelimäärä." + Environment.NewLine
                + "Esim. 'oma selo,tulokset' 1805,+1600 1785 1882 " + Environment.NewLine
                + "tai 1820,1.5 1600 1785 1882" + Environment.NewLine
                + "tai 'miettimisaika,oma selo,pelimaara(tai tyhjä),tulokset'" + Environment.NewLine
                + "esim. 5,1680,,1.5 1600 1822 1882" + Environment.NewLine
                + Environment.NewLine
                + "Normaalisti lasketaan joko uuden tai vanhan pelaajan laskentakaavalla." + Environment.NewLine
                + "Mutta voidaan myös aloittaa uuden pelaajan laskennalla ja jatkaa loput ottelut vanhan pelaajan laskentakaavalla. "
                + "Tällöin pelimäärän on oltava aluksi enintään 10 ja on tultava täyteen vähintään 11 peliä ennen laskentakaavan vaihtamista." + Environment.NewLine
                + "Esim. selo 1700, pelimäärä 8, vastustajat " + Environment.NewLine
                + " 1612 +1505 1850 -2102 / -1611 +1558" + Environment.NewLine
                + "tai csv: 1700,8,1612 +1505 1850 -2102 / -1611 +1558";
        }

        [BindProperty]
        public string Vastustajat_ohjeteksti { get; set; }

        public IActionResult OnGet()
        {
            // Aseta oletusarvo (ks. Vakiot.Miettimisaika_enum)
            if (miettimisaika_radiobutton_in < 10 || miettimisaika_radiobutton_in > 90)
                miettimisaika_radiobutton_in = 90;

            if (kayta_tulosta)
            {
                kayta_tulosta = false;
                selo_in = laskettu_uusi_selo.ToString();
                if (uusi_pelimaara >= 0)
                    pelimaara_in = uusi_pelimaara.ToString();
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

                // näytölle siistitty versio (tämä ei toimikaan näin helposti)
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

                laskettu_uusi_selo = tulokset.UusiSelo;
                uusi_pelimaara = tulokset.UusiPelimaara;

                //
                // NÄYTÄ TULOKSET
                //
                // XXX: järjestä tulokset näytölle paremmin!
                // 
                ViewData["TULOKSET"] = "Tulokset";

                ViewData["uusi_selo"] = "Uusi vahvuusluku ("+ (tulokset.Miettimisaika == Vakiot.Miettimisaika_enum.MIETTIMISAIKA_ENINT_10MIN ? "PELO" : "SELO") +") : " + tulokset.UusiSelo;
                ViewData["selomuutos"] = "    Muutos : " + (tulokset.UusiSelo - tulokset.AlkuperainenSelo).ToString("+#;-#;0")
                                       + ((tulokset.MinSelo < tulokset.MaxSelo) ? "    Vaihteluväli " + tulokset.MinSelo.ToString() + " - " + tulokset.MaxSelo.ToString() : "");

                if (tulokset.UusiPelimaara > 0)
                    ViewData["uusi_pelimaara"] = "    Uusi pelimäärä: " + tulokset.UusiPelimaara;

                if (tulokset.UudenPelaajanLaskenta || tulokset.UudenPelaajanPelitLKM > 0)
                {
                    ViewData["odotustulos"] = "";
                    ViewData["uudenpelaajanlaskenta"] = (tulokset.UudenPelaajanPelitLKM > 0) ? " (uuden pelaajan laskentaan lisää " + tulokset.UudenPelaajanPelitLKM + " peliä)" : " (uuden pelaajan laskenta) ";
                }
                else
                {
                    ViewData["odotustulos"] = "Odotustulos: " + (tulokset.Odotustulos / 100F).ToString("0.00");
                    ViewData["uudenpelaajanlaskenta"] = "";
                }

                ViewData["turnauksentulos"] = "Turnauksen tulos: " + (tulokset.TurnauksenTulos2x / 2F).ToString("0.0") + " / " + tulokset.VastustajienLkm;
                ViewData["keskivahvuus"] = "Keskivahvuus: " + (tulokset.TurnauksenKeskivahvuus10x / 10F).ToString("0.0") +   "  Piste-ero: " + Math.Abs(tulokset.AlkuperainenSelo - tulokset.TurnauksenKeskivahvuus).ToString();

                ViewData["suoritusluku"] = "Suoritusluku: " + tulokset.Suoritusluku;
                ViewData["suorituslukuFIDE"] = " Suoritusluku FIDE: " + tulokset.SuorituslukuFIDE;
                ViewData["suorituslukuLineaarinen"] = "Suoritusluku lineaarinen: " + tulokset.SuorituslukuLineaarinen;
            }
            else
            {              
                // virhestatus on negatiivinen luku, virheteksti haetaan taulukosta
                if (Vakiot.SYOTE_STATUS_OK >= status && status >= Vakiot.SYOTE_VIRHE_MAX) {
                    ViewData["virheteksti"] = Vakiot.SYOTE_VIRHEET_text[Math.Abs(status)]; 
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

            kayta_tulosta = true;

            return RedirectToPage("Selolaskuri"); // OnGet() laittaa uuden vahvuusluvun ja pelimäärän näytölle
        }
    }
}
        
