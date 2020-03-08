using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Selolaskuri.Razor.Models
{

    // XXX:  SelolaskuriRazorModel EI KÄYTÖSSÄ TÄLLÄ HETKELLÄ

    public class SelolaskuriRazorModel
    {

        // Syötetiedot
        //   selo
        //   pelimäärä, otetaan huomioon jos enintään 10 = uuden pelaajan laskenta
        //   miettimisaika (min)
        //   vastustajat, esim. +1600 -1822 =1785 tai 1.5 1600 1822 1785
        //   ottelun tulos (ei tätä kenttää, anna tulos vastustajan selon yhteydessä)

        ////[Required]
        //[Display(Name = "vahvuusluku")]
        //[MaxLength(5)]
        //public string selo_in { get; set; }

        //[Display(Name = "pelimäärä (jos ≤ 10) ")]
        //[MaxLength(5)]
        //public string pelimaara_in { get; set; }

        ////[Required]
        //[Display(Name = "vastustajat/SELO/SELOt tuloksineen")]
        //[MaxLength(200)]
        //public string vastustajanSelo_in { get; set; }

        //// VIRHEILMOITUS
        //[Display(Name = "")]
        //public string virhe { get; set; }

        // XXX: yksittäisen ottelun tulosta ei nyt käytetä
        //[Display(Name = "Yksittäisen ottelun tulos")]
        //[MaxLength(3)]
        //public string ottelunTulos_in { get; set; }

        // Tulokset
        //   uusi selo
        //   uusi pelimäärä
        //   piste-ero
        //   keskivahvuus
        //   turnauksen tulos
        //   tuloksen vaihteluväli
        //   onko uuden pelaajan laskenta
        //   odotustulos
        //   suoritusluku
        //   suoritusluku FIDE
        //   suoritusluku lineaarinen

        //[Display(Name = "uusi vahvuusluku")]
        //public string uusiSelo_out { get; set; }
        
        //[Display(Name = "muutos")]
        //public string selomuutos_out { get; set; }

        //[Display(Name = "uusi pelimäärä")]
        //public string uusiPelimaara_out { get; set; }

        //[Display(Name = "piste-ero")]
        //public string pisteEro_out { get; set; }

        //[Display(Name = "keskivahvuus")]
        //public string keskivahvuus_out { get; set; }

        //[Display(Name = "turnauksen tulos")]
        //public string turnauksenTulos_out { get; set; }

        //[Display(Name = "vaihteluväli")]
        //public string vaihteluvali_out { get; set; }

        //[Display(Name = "uuden pelaajan laskenta")]
        //public string UudenPelaajanLaskenta_out { get; set; }

        //[Display(Name = "odotustulos")]
        //public string odotustulos_out { get; set; }

        //[Display(Name = "suoritusluku")]
        //public string suoritusluku_out { get; set; }
        //[Display(Name = "FIDE")]
        //public string suorituslukuFIDE_out { get; set; }

        //[Display(Name = "lineaarinen")]
        //public string suorituslukuLineraarinen_out { get; set; }
    }
}
