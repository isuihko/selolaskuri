# selolaskuri
Shakin vahvuusluvun laskenta

Visual Studio Community 2015, C# ja .NET.

Lasketaan shakinpelaajalle uusi vahvuusluku SELO tai PELO, ks. http://www.shakki.net/cgi-bin/selo
SELO on Suomen kansallinen shakin vahvuusluku, esim. https://fi.wikipedia.org/wiki/Elo-luku#Suomen_Elo
PELO on vastaavasti pikashakin vahvuusluku, jota käytetään kun miettimisaika on alle 15 minuuttia.

Laskettu tulos on alustava ja voi poiketa virallisesta laskennasta hieman, ehkä pisteellä tai parilla (tarkistan vielä kaavat).

Syötekenttiä:
* miettimisaika: vähintään 90 min, 60-89 minuuttia, 15-59 minuuttia tai alle 15 minuuttia
* oman vahvuusluku eli SELO tai jos miettimisaika on alle 15 minuuttia, niin PELO
* oma aiempi pelimäärä tai tyhjä. Jos pelimääräksi annetaan 0-10, käytetään uuden pelaajan vahvuusluvun laskentakaavaa.
* ottelun tiedot, joissa vaihtoehdot:
 a) yhden vastustajan vahvuusluku, esim. 1720
 b) usean vastustajan (esim. turnauksen kaikki ottelut) vahvuusluvut tuloksineen, esim. +1622 -1880 =1633 tai +1622 -1880 1633, jossa + tarkoittaa voittoa, - tappiota ja = tai tyhjä tasapeliä
 c) oma esim. turnauksessa saatu kokonaispistemäärä ja vastustajien vahvuusluvut ilman tuloksia, esim. 1.5 1622 1880 1683
* yhden ottelun tulos, käytettävissä vain vaihtoehdossa a. Tulos valitaan valintapainikkeista: 0 = tappio, 1/2 = tasapeli ja 1 = voitto.

Pikashakin tulokset on paras syöttää formaatissa c eli pistemäärä pelo pelo pelo pelo ... jolloin laskenta tehdään yhdellä kertaa kaikkien otteluiden odotustuloksien summaa käyttäen.

Painonapit:
* "Laske uusi SELO" tai "Laske uusi PELO" eli vahvuusluvun laskenta
* "Käytä uutta SELOa jatkolaskennassa" tai "Käytä uutta PELOa jatkolaskennassa"
- kopioi lasketun vahvuusluvun ja pelimäärän syötekenttiin uutta laskentaa varten tai jos vielä ei ollut laskettu, niin kopioi uuden pelaajan tiedot laskentaa varten: vahvuusluku 1525 ja pelimäärä 0.
 
Laskentaa suoritetaan, kun
- valitaan painike "Laske uusi SELO" tai pikashakin miettimisajalla "Laske uusi PELO"
- liikutaan yhtä ottelua syötettäessä tuloksen valintapainikkeissa
- painetaan Enter vastustajien vahvuuslukujen syöttökentässä

Kaikki tarvittava tieto on annettava ennen kuin voidaan laskea.

Virhetarkastukset:
- SELO-lukujen oltava välillä kokonaisluku 1000-2999
- jos on annettu oma pelimäärä, sen on oltava kokonaisluku 0-9999
- (a) jos on annettu yksi SELO, niin ottelun tulos on annettava valintapainikkeilla 0, 1/2 ja 1
- (b) onko annettu muita merkkejä kuin +, - ja = tuloksia syötettäessä
- (c) turnauksen pistemäärän oltava vähintään nolla ja enintään annettujen vastustajien lukumäärä
- (c) annetuissa vahvuusluvuissa ei saa antaa tulosta, koska pistemäärä oli jo annettu

Tulostiedot:
- uusi vahvuusluku
- vahvuusluvun muutos verrattuna alkuperäiseen, voi olla negatiivinen tai positiivinen tai nolla
- uusi pelimäärä, jos pelimäärä oli annettu
- vahvuusluvun vaihteluväli, kun tulokset syötetty tavalla b
- vastustajien keskivahvuus
- turnauksen tulos, joka voi olla esim. yhden ottelun tapauksessa  0,5 / 1  ja muutoin vaikkapa 2 / 5
- piste-ero, kun laskettu yhden ottelun tuloksella (a)
- odotustulos, kun laskettu yhden ottelun tulos (a) tai odotustuloksien summa (b ja c) paitsi jos laskettiin uuden pelaajan vahvuuslukua, jolloin summaa ei näytetä
- kerroin, jota käytetään normaalissa laskennassa (eli ei uusi pelaaja) ja selvitään pelaajan omasta alkuperäisestä selosta
