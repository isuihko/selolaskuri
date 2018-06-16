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
//  16.-17.6.2018 Poistettu turhana mm. kasitellytOttelut, seloMuutos
//
namespace Selolaskuri {

    public struct Tulokset {
        public int vastustajienLkm;
        public int turnauksenKeskivahvuus;
        public int laskettuTurnauksenTulos;  // tai yksittäisen ottelun tulos
        public int odotustulos;       // tai odotustuloksien summa, jos useita vastustajia
        public int kerroin;
        public int laskettuSelo;
        public int laskettuPelimaara; // 0, jos ei ollut pelimäärää
        public int minSelo;
        public int maxSelo;     
        public int pisteero;          // jos oli yksi vastustaja, niin piste-ero
    }
}
