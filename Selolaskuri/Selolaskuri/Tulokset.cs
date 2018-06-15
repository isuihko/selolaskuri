//
// Tietorakenne lasketuille tuloksille
//
// Luotu 10.6.2018 Ismo Suihko
//
// Tietorakenteella välitetään tulokset laskentaluokasta (SelolaskuriOperations)
//       1. käyttöliittymälle (SelolaskuriForm) näytettäväksi
//       2. yksikkötestaukseen (Selolaskuri.Tests) tarkistusta varten
//
// Muutoksia:
//  16.6.2018 Poistettu turhana kasitellytOttelut
//
namespace Selolaskuri {

    public struct Tulokset {
        public int turnauksenKeskivahvuus;
        public int vastustajienLkm;
        public int alkuperainenSelo;
        public int alkuperainenPelimaara;

        public int annettuTurnauksenTulos;
        public int laskettuTurnauksenTulos;

        public int pisteero;
        public int odotustulos;
        public int odotustuloksienSumma;
        public int kerroin;
        public int vaihteluvali;

        public int minSelo;
        public int maxSelo;

        public int laskettuSelo;
        public int selomuutos;
        public int laskettuPelimaara;
    }
}
