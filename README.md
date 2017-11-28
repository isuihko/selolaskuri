# selolaskuri
Shakin vahvuusluvun laskenta

Visual Studio Community 2015
Ohjelmointikieli: C# ja .NET

Lasketaan shakinpelaajalle uusi vahvuusluku SELO tai PELO, ks. http://www.shakki.net/cgi-bin/selo
SELO on Suomen kansallinen shakin vahvuusluku, esim. https://fi.wikipedia.org/wiki/Elo-luku#Suomen_Elo
PELO on vastaavasti pikashakin vahvuusluku.

Laskettu tulos on alustava ja voi poiketa virallisesta laskennasta noin pisteellä (kaavat vielä tarkistetaan).

Syötekenttiä:
* miettimisaika: vähintään 90 min, 60-89 minuuttia, 15-59 minuuttia tai alle 15 minuuttia
* oman vahvuusluku eli SELO tai jos miettimisaika on alle 15 minuuttia, niin PELO
* oma aiempi pelimäärä tai tyhjä. Jos pelimääräksi annetaan 0-10, käytetään uuden pelaajan vahvuusluvun laskentakaavaa.
* ottelun tiedot, joissa vaihtoehdot:
----- a) yhden vastustajan vahvuusluku, esim. 1720
----- b) usean vastustajan (esim. turnauksen kaikki ottelut) vahvuusluvut tuloksineen, esim. +1622 -1880 =1633 tai +1622 -1880 1633, jossa + tarkoittaa voittoa, - tappiota ja = tai tyhjä tasapeliä
----- c) oma esim. turnauksessa saatu kokonaispistemäärä ja vastustajien vahvuusluvut ilman tuloksia, esim. 1.5 1622 1880 1683
* yhden ottelun tulos, käytettävissä vain vaihtoehdossa a. Tulos valitaan valintapainikkeista: 0 = tappio, 1/2 = tasapeli ja 1 = voitto

Painonapit:
* Laske uusi SELO tai Laske uusi PELO
----- eli vahvuusluvun laskenta
* Käytä uutta SELOa jatkolaskennassa tai Käytä uutta PELOa jatkolaskennassa
----- kopioi lasketun vahvuusluvun ja pelimäärän syötekenttiin uutta laskentaa varten
----- jos vielä ei ollut laskettu, niin kopioi uuden pelaajan tiedot eli vahvuusluku 1525 ja pelimäärä 0
 
Laskentaa suoritetaan, kun
- valitaan Laske uusi SELO-painike tai pikashakin miettimisjalla Laske uusi PELO
- liikutaan yhtä ottelua syötettäessä tuloksen valintapainikkeissa
- painetaan Enter kentässä, jossa annetaan ottelun tiedot eli vastustajien vahvuusluvut

Kaikki tarvittava tieto on oltava annettu ennen kuin voidaan laskea
Virhetarkastukset:
- SELO-lukujen oltava välillä kokonaisluku 1000-2999
- jos on annettu oma pelimäärä, sen on oltava kokonaisluku 0-9999
- (a) jos on annettu yksi SELO, niin ottelun tulos on annettava valintapainikkeilla 0, 1/2 ja 1
- (b) onko annettu muita merkkejä kuin +, - ja = tuloksia syötettäessä
- (c) turnauksen pistemäärän oltava vähintään nolla ja enintään annettujen vastustajien lukumäärä
- (c) annetuissa vahvuusluvuissa ei saa antaa tulosta, koska pistemäärä oli jo annettu

Tulostiedot:
- uusi vahvuusluku eli SELO pidemmällä miettimisajalla tai PELO pikashakissa
- vahvuusluvun muutos, voi olla negatiivinen tai positiivinen tai nolla
- uusi pelimäärä, jos oma pelimäärä oli annettu numerona
- vahvuusluvun vaihteluväli, kun tulokset syötetty tavalla b
- vastutajien keskivahvuus, kun tulokset syötetty tavoilla b ja c
- turnauksen tulos, joka voi olla esim. yhden ottelun tapauksessa  0,5 / 1  ja muutoin 2 / 5
- SELO-ero (tai PELO-ero), kun laskettu yhden ottelun tulos (a)
- Odotustulos, kun laskettu yhden ottelun tulos (a) tai odotustuloksien summa (b ja c), mutta jos laskettiin uuden pelaajan vahvuuslukua, niin tämä on tyhjä
- kerroin, jota käytetään normaalissa laskennassa (ei uusi pelaaja) ja saadaan pelaajan omasta alkuperäisestä selosta
