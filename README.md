# selolaskuri
Shakin vahvuusluvun laskenta


2.4.2018 (edelliset isommat muutokset 7.1.2018) Visual Studio Community 2015, C# ja .NET/WinForms, Windows 7/10

Muutokset: Koodin järjestämistä. Ei eroa toiminnassa verrattuna 7.1.2018 versioon muutoin kuin, että tulostuu uudempi versionumero ja päivämäärä. Ja virhetilanteissa (virheellinen syöte) voi tulostua tarkempi virheilmoitus.

--

Tein tästä myös Java-version, jolla tein oman repositoryn jSelolaskuri. Ohjelma on muuten samanlainen, paitsi vastustajien vahvuuslukukenttään syötettyjä tietoja ei tallenneta listaan, eikä ole menuja.

Lasketaan shakinpelaajalle uusi vahvuusluku SELO tai PELO, ks. http://www.shakki.net/cgi-bin/selo
- SELO on Suomen kansallinen shakin vahvuusluku, esim. https://fi.wikipedia.org/wiki/Elo-luku#Suomen_Elo
- PELO on vastaavasti pikashakin vahvuusluku, jota käytetään kun miettimisaika on alle 15 minuuttia. Eri miettimisajoille on omat kaavansa.

Laskettu tulos on alustava ja voi poiketa virallisesta laskennasta hieman, ehkä pisteellä tai parilla (tarkistan vielä kaavat).
Laskentaohjeet sivulta: http://skore.users.paivola.fi/selo.html

Selolaskentaa netissä: 
- http://www.shakki.net/kerhot/salsk/ohjelmat/selo.html  SalSK - selolaskentaohjelma
- http://shakki.kivij.info/performance_calculator.shtml  Selo- ja suorituslukulaskuri, myös uuden pelaajan laskenta


Syötekenttiä:
* miettimisaika: vähintään 90 min, 60-89 minuuttia, 11-59 minuuttia tai enintään 10 minuuttia
* oman vahvuusluku eli oma SELO tai jos miettimisaika on enintään 10 minuuttia, niin PELO
* aiempi oma pelimäärä tai tyhjä. Jos pelimääräksi annetaan 0-10, käytetään uuden pelaajan vahvuusluvun laskentakaavaa, vain SELO. Vielä ei ole toteutettu PELO:n laskentaa uudelle pelaajalle.
* ottelun tiedot eli Vastustajan SELO-kenttä, joissa vaihtoehdot:
 - a) yhden vastustajan vahvuusluku, esim. 1720
 - b) usean vastustajan (esim. turnauksen kaikki ottelut) vahvuusluvut tuloksineen, esim. +1622 -1880 =1633 tai +1622 -1880 1633, jossa + tarkoittaa voittoa, - tappiota ja = tai tyhjä tasapeliä
 - c) oma esim. turnauksessa saatu kokonaispistemäärä ja vastustajien vahvuusluvut ilman tuloksia, esim. 1.5 1622 1880 1683
* yhden ottelun tulos, käytettävissä vain vaihtoehdossa a. Tulos valitaan valintapainikkeista: 0 = tappio, 1/2 = tasapeli ja 1 = voitto ja laskelmat päivittyvät sitä mukaa.

Pikashakin tulokset on paras syöttää formaatissa c eli pistemäärä pelo pelo pelo pelo ... jolloin laskenta tehdään yhdellä kertaa kaikkien otteluiden odotustuloksien summaa käyttäen.

Vastustajan SELO-kenttä muistaa laskennassa käytetyt syötteet, jos siinä kentässä on painettu Enter tai on klikattu Laske uusi SELO/PELO -painiketta. Listasta on helppo valita uudestaan aiemmin käytetyt tiedot, jonka jälkeen niitä voidaan tarvittaessa muokata ennen uutta laskentaa.

Painonapit:
* "Laske uusi SELO" tai "Laske uusi PELO" eli vahvuusluvun laskenta
* "Käytä uutta SELOa jatkolaskennassa" tai "Käytä uutta PELOa jatkolaskennassa", joka kopioi lasketun vahvuusluvun ja pelimäärän syötekenttiin uutta laskentaa varten tai jos vielä ei ollut laskettu, niin kopioi uuden pelaajan tiedot laskentaa varten: vahvuusluku 1525 ja pelimäärä 0.

Menut:
* Ohjeita
* Laskentakaavat
* Tietoja ohjelmasta
* Sulje ohjelma
 
Laskentaa suoritetaan, kun
- valitaan painike "Laske uusi SELO" tai pikashakin miettimisajalla "Laske uusi PELO"
- liikutaan yhtä ottelua syötettäessä tuloksen valintapainikkeissa
- painetaan Enter vastustajien vahvuuslukujen syöttökentässä

Kaikki tarvittava tieto on annettava ennen kuin voidaan laskea. Yksittäisen ottelun tulosta varten varattuja valintapainikkeita ei kuitenkaan käytetä silloin kun, syötetään useamman matsin tulos kerralla.

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

Jos syötetään yhden ottelun tulosta, niin se voidaan tehdä kolmella tavalla:
- a) annetaan vahvuusluku numerona, esim. 1720 ja valitaan tulos valintapainikkeista 0, 1/2 ja 1.
- b) Annetaan tulos vahvuusluvun kanssa, esim. +1720, =1720 tai -1720, eikä tuloksen valintapainikkeita käytetä.
- c) Annetaan tulos numerona ennen vahvuuslukua, esim. 1 1720, 0.5 1720 tai 0 1720, eikä valintapainikkeita käytetä.

Kahden tai useamman ottelun tulos voidaan antaa kahdella eri tavalla
- b) Tulokset vahvuuslukujen yhteydessä, esim. +1622 -1880 =1633 +1717
- c) Pistemäärä ensin ja sitten vahvuusluvut, esim. 2.5 1622 1880 1633 1717

Ohjelman asennus:
Lataa publish-hakemisto alihakemistoineen ja suorita setup.exe. Vaatii .NET Frameworkin 4.6:n.
Tai lataa koko Visual Studio -projekti ja käännä.

TODO:
- Koodin jako moduuleihin/tiedostohin/luokkiin ei ehkä ole oikea ja paras mahdollinen, tarkista!
- properties (aloitettu, mutta vielä lisää)
- out pois muutaman aliohjelman parametreista, turha
- lisää valmiiden kirjastojen käyttöä
- optimointia (jo tehty 1.4. ja 2.4., mutta vielä lisää)
- jaa yksi pitkä aliohjelma pienempiin osiin (vastustajien vahvuuslukukentän käsittely)
- lisälaskentaa
- tee myös uusi versio käyttäen WPF:ää ja XAML:ia
- automaattinen testaus, vaatii ehkä muutoksia koodiinkin ennen kuin testausrutiineja voi kirjoittaa
