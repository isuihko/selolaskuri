using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Selolaskuri.Razor.Models
{
    public class SelolaskuriRazorModel
    {
        
        // Syötetiedot
        //   selo
        //   pelimäärä
        //   miettimisaika
        //   vastustajat
        //   ottelun tulos

        [Required]
        [Display(Name = "SELO")]
        [MaxLength(5)]
        public string selo_in { get; set; }
        //public string selo_in { get => _selo_in; set => selo_in = value?.ToUpperInvariant(); }

        [Display(Name = "Pelimäärä")]
        [MaxLength(5)]
        public string pelimaara_in { get; set; }

        [Display(Name = "Miettimisaika")]
        [MaxLength(5)]
        public string miettimisaika_in {
            get; set;
        }

        [Required]
        [Display(Name = "Vastustajat/SELO/SELOt tuloksineen")]
        [MaxLength(100)]
        public string vastustajanSelo_in { get; set; }

        [Display(Name = "Yksittäisen ottelun tulos")]
        [MaxLength(3)]
        public string ottelunTulos_in { get; set; }

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

        [Display(Name = "Uusi SELO")]
        public string uusiSelo_out { get; set; }

        [Display(Name = "Uusi pelimäärä")]
        public string uusiPelimaara_out { get; set; }

        [Display(Name = "piste-ero")]
        public string pisteEro_out { get; set; }

        [Display(Name = "keskivahvuus")]
        public string keskivahvuus_out { get; set; }

        [Display(Name = "turnauksen tulos")]
        public string turnauksenTulos_out { get; set; }

        [Display(Name = "vaihteluväli")]
        public string vaihteluvali_out { get; set; }

        [Display(Name = "uuden pelaajan laskenta")]
        public string UudenPelaajanLaskenta_out { get; set; }

        [Display(Name = "odotustulos")]
        public string odotustulos_out { get; set; }

        [Display(Name = "suoritusluku")]
        public string suoritusluku_out { get; set; }
        [Display(Name = "suoritusluku FIDE")]
        public string suorituslukuFIDE_out { get; set; }

        [Display(Name = "suoritusluku lineaarinen")]
        public string suorituslukuLineraarinen_out { get; set; }

    }
}
