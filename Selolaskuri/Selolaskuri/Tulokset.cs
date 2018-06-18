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
//
namespace Selolaskuri {

    public struct Tulokset {
        public int uusiSelo;
        public int uusiPelimaara;           // jos ei annettu, palautuu 0
        public int minSelo;                 // laskennan aikainen vaihtelu
        public int maxSelo;

        public int odotustulos;             // myös odotustuloksien summa, jos useita vastustajia
        public int kerroin;                 // laskennan aputieto

        public int vastustajienLkm;
        public int turnauksenKeskivahvuus;
        public int laskettuTurnauksenTulos;
    }
}
