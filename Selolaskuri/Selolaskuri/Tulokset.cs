//
// Tietorakenne lasketuille tuloksille
//
// Luotu 10.6.2018 Ismo Suihko
//
// Tietorakenteella välitetään tulokset laskentaluokasta (SelolaskuriOperations.SuoritaLaskenta)
//       1. käyttöliittymälle (SelolaskuriForm) näytettäväksi
//       2. yksikkötestaukseen (Selolaskuri.Tests) tarkistusta varten
//
// Muutoksia:
//  15.-17.6.2018 Poistettu useita turhia kenttiä
//  18.7.2018     Nyt Selopelaaja-luokka käyttää tätä pohjana eikä esittele samoja kenttiä uudestaan laskentaa varten.
//  20.7.2018     Renaming of class members to follow naming conventions
//
namespace Selolaskuri {

    public class Tulokset {
        //// --------------------------------------------------------------------------------
        //// Laskennan aikana päivitettävät tiedot, jotka kopioidaan tuloksiin
        //// ks. SeloLaskuriOperations.SuoritaLaskenta sekä struct Tulokset
        //// --------------------------------------------------------------------------------

        public int UusiSelo { get; set; }
        public int UusiPelimaara { get; set; }

        // Lasketun selon vaihteluväli, jos vastustajien selot ja tulokset formaatissa: +1622 -1880 =1633
        public int MinSelo { get; set; }
        public int MaxSelo { get; set; }

        // laskennan aputiedot
        public int Odotustulos { get;  set; }
        public int Kerroin { get;  set; }

        // Turnauksen tulos
        //
        // Syötteistä laskettu tulos
        // Selvitetään tulos, jos ottelut formaatissa "+1525 =1600 -1611 +1558", josta esim. saadaan
        // tulokseksi 2,5 (2 voittoa ja 1 tasapeli). Tallennetaan kokonaislukuna tuplana (int)(2*2,5) eli 5.
        public int TurnauksenTulos { get; set; }

        public int VastustajienLkm { get; set; }
        public int TurnauksenKeskivahvuus { get; set; }
    }
}

