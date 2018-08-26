# selolaskuri

Selolaskuri - Shakin vahvuusluvun laskenta - Calculation of Finnish chess ratings - My hobby project (I'm also chess player)

Visual Studio Community 2017 (also 2015), C# and .NET/WinForms, Windows 7 and Windows 10

New refactored version of Selolaskuri! Now all Selolaskuri's versions are under same Visual Studio's solution:
- Selolaskuri which is WinForms version and desktop application
- Selolaskuri.WPF, user interface created with WPF/XAML (WPF is newer than WinForms) and is a desktop application. More in https://github.com/isuihko/SelolaskuriWPF
- Selolaskuri.XBAP, user interface created with WPF/XAML and is a web application with the limit that it runs only in IE browser. More in https://github.com/isuihko/isuihko.github.io including executable.

To choose, which of the application to run in Visual Studio, go to Solution Explorer, right click Project's name, Set as StartUp Project and then click Start.

Version history highlights:
- 10.-19.6.2018 A lot of code refactoring to make automatic unit testing possible. Separated checking of input and calculations (i.e. business logic) from the form. Unit tests are now in Selolaskuri.Tests and it makes testing of the  input checking and calculations easy. Checked the calculations and usage of temporary variables and cleaned and documented the code a lot.
- 4.8.2018: Added support for CSV format (comma-separated values). Also Java version is up-to-date.
- 11.8.2018: Uses clipboard. Edit-menu with cut, copy and paste for handing opponents list. Handy with CSV format.
- 20.8.2018: Adds usage of half character "1/2" in chess results. Added new unit tests for that.
- 26.8.2018: Moved common checking and calculation into SelolaskuriLibrary. Added Selolaskuri's WFP/XAML version as Selolaskuri.WFP and XBAP version as Selolaskuri.XBAP under this same Visual Studio's solution.

All Selolaskuri versions use the same SelolaskuriLibrary for input checking and calculation. Also all of them have same unit tests from project Selolaskuri.Tests (now 76 tests) which tests SelolaskuriLibrary routines. User interfaces are similar but created differently.

--

The following text is only in Finnish. There is about the usage of the Finland's SELO calculation application.

Lasketaan shakinpelaajalle uusi vahvuusluku SELO tai PELO, ks. http://www.shakki.net/cgi-bin/selo
- SELO on Suomen kansallinen shakin vahvuusluku, esim. https://fi.wikipedia.org/wiki/Elo-luku#Suomen_Elo
- PELO on vastaavasti pikashakin vahvuusluku, jota käytetään kun miettimisaika on enintään 10 minuuttia. Eri miettimisajoille on omat laskentakaavansa.

Laskettu tulos voi poiketa virallisesta laskennasta hieman, ehkä pisteellä pyöristyksistä johtuen.
Laskentaohjeet sivulta: http://skore.users.paivola.fi/selo.html

Selolaskentaa netissä: 
- http://www.shakki.net/kerhot/salsk/ohjelmat/selo.html  SalSK - selolaskentaohjelma
- http://shakki.kivij.info/performance_calculator.shtml  Selo- ja suorituslukulaskuri, myös uuden pelaajan laskenta


Syötekenttiä:
* miettimisaika: vähintään 90 min, 60-89 minuuttia, 11-59 minuuttia tai enintään 10 minuuttia
* oman vahvuusluku eli oma SELO tai jos miettimisaika on enintään 10 minuuttia, niin PELO
* aiempi oma pelimäärä tai tyhjä. Jos pelimääräksi annetaan 0-10, käytetään uuden pelaajan vahvuusluvun laskentakaavaa.
* ottelun tiedot eli Vastustajan SELO-kenttä, joissa vaihtoehdot:
 - a) yhden vastustajan vahvuusluku, esim. 1720
 - b) usean vastustajan (esim. turnauksen kaikki ottelut) vahvuusluvut tuloksineen, esim. +1622 -1880 =1633 tai +1622 -1880 1633, jossa + tarkoittaa voittoa, - tappiota ja = tai tyhjä tasapeliä
 - c) oma esim. turnauksessa saatu kokonaispistemäärä ja vastustajien vahvuusluvut ilman tuloksia, esim. 1.5 1622 1880 1683
 - d) csv-formaatti (comma-separated values), jossa voidaan antaa kaikki syöte pilkulla erotettuna eikä silloin muiden lomakkeen kenttien arvoilla ole merkitystä. Esim. miettimisaika,selo,pelimaara,ottelut: 90,1525,0,+1621 -1812 =1710 tai selo,ottelut: 2191,+1622 -1880 =1633. Huom! Käytä desimaalipistettä, jos tuloksessa on tasapeli mukana, esim. 2.5.
 
Huom! CSV:ssä annetut arvot ohittavat muut (miettimisaika, vahvuusluku, pelimäärä). Jos miettimisaika on antamatta, käytetään lomakkeelta valittua ja pelimäärän puuttuessa käytetään tyhjää.
 
* yhden ottelun tulos, käytettävissä vain vaihtoehdossa a. Tulos valitaan valintapainikkeista: 0 = tappio, 1/2 = tasapeli ja 1 = voitto ja laskelmat päivittyvät sitä mukaa kun kentissä liikutaan.

Pikashakin tulokset on helpointa syöttää formaatissa c eli pistemäärä pelo pelo pelo pelo ... jolloin laskenta tehdään yhdellä kertaa kaikkien otteluiden odotustuloksien summaa käyttäen.

Vastustajan SELO-kenttä muistaa laskennassa käytetyt syötteet, jos siinä kentässä on painettu Enter tai on klikattu Laske uusi SELO/PELO -painiketta. Listasta on helppo valita uudestaan aiemmin käytetyt tiedot, jonka jälkeen niitä voidaan tarvittaessa muokata ennen uutta laskentaa. Kentässä voi myös käyttää komentoja test ja clear. Komento test lisää testimateriaalia Vastustajat-listaan ja clear nollaa kaiken syötteen ja näytön kentät.

Painonapit:
* "Laske uusi SELO" tai "Laske vahvuusluku" eli vahvuusluvun laskenta
* "Käytä tulosta jatkolaskennassa" kopioi lasketun vahvuusluvun ja pelimäärän syötekenttiin uutta laskentaa varten tai jos vielä ei ollut laskettu, niin kopioi uuden pelaajan tiedot laskentaa varten: vahvuusluku 1525 ja pelimäärä 0.

Menut:
* Ohjeita
* Laskentakaavat
* Tietoja ohjelmasta
* Sulje ohjelma

Edit:
* Cut (kopioi ja tyhjentää Vastustajat-historian)
* Copy (kopioi Vastustajat-historian)
* Paste (täyttää Vastustajat-historian, ei tarkistusta)
 
Laskentaa suoritetaan, kun
- valitaan painike "Laske vahvuusluku"
- yhtä ottelua syötettäessä klikataan jotain tuloksen valintapainiketta (0, 1/2 tai 1)
- painetaan Enter vastustajien vahvuuslukujen syöttökentässä

Kaikki tarvittava tieto on annettava ennen kuin voidaan laskea. Yksittäisen ottelun tulosta varten varattuja valintapainikkeita ei kuitenkaan käytetä silloin kun, syötetään useamman matsin tulos kerralla.

Virhetarkastukset:
- SELO-lukujen oltava välillä kokonaisluku 1000-2999
- jos on annettu oma pelimäärä, sen on oltava kokonaisluku 0-9999 (laskennan jälkeen uusi pelimäärä saa olla isompikin kuin 9999)
- (a) jos on annettu yksi SELO eli on vain yksi vastustaja, niin ottelun tulos on annettava valintapainikkeilla 0, 1/2 ja 1
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

Jos syötetään yhden ottelun tulosta, niin se voidaan tehdä:
- a) annetaan vahvuusluku numerona, esim. 1720 ja valitaan tulos valintapainikkeista 0, 1/2 ja 1.
- b) Annetaan tulos vahvuusluvun kanssa, esim. +1720, =1720 tai -1720, eikä tuloksen valintapainikkeita ole käytetty.
- c) Annetaan tulos numerona ennen vahvuuslukua, esim. 1 1720, 0.5 1720 tai 0 1720, eikä valintapainikkeita ole käytetty.
- d) CSV-formaatissa, jossa oma selo myös mukana, esim. 1521,1 1720 tai 1521,+1720

Kahden tai useamman ottelun tulos voidaan antaa eri tavoilla
- a) Tulokset vahvuuslukujen yhteydessä, esim. +1622 -1880 =1633 +1717
- b) Pistemäärä ensin ja sitten vahvuusluvut, esim. 2.5 1622 1880 1633 1717
- c) CSV-formaatissa, jossa tulokset tavalla a tai b, esim. jos oma selo on 1521, niin 1521,2.5 1622 1880 1633 1717


Ohjelman asennus:
Windows 7/10: Lataa publish-hakemisto alihakemistoineen ja suorita setup.exe ja hyväksy Selolaskuri.exe:n asennus. Tai lataa koko Visual Studio -projekti ja suorita setup.exe publish-hakemistossa. Tai käännä projekti uudestaan Visual Studiossa.
Koko projektin voi ladata komennolla (kunhan git on asennettu): git clone https://github.com/isuihko/selolaskuri

TODO: (listalla pitkään ollut automaattinen testaus on toteutettu 10.6.2018)
- vielä hieman lisälaskentaa (suorituslukulaskenta) sekä tarkista laskennan apumuuttujien käyttöä
- järjestä ohjelmaa vielä enemmän objektiläheisemmäksi
- tee myös uusi versio käyttäen WPF:ää ja XAML:ia
- käytä SQL-tietokantaa jollain tavalla, vaikka tallentamaan laskentoja niin, että niitä voi hakea (harjoituksen vuoksi)
- Tee myös web-versio (ASP.NET Core?)
