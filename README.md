# selolaskuri

Selolaskuri - Shakin vahvuusluvun laskenta - Calculation of Finnish chess ratings - My hobby project (I'm also chess player)

Visual Studio Community 2019/2017/2015, C#/.NET, Windows 7, Windows 8 and Windows 10

Now all Selolaskuri's versions are under same Visual Studio's solution:
- **Selolaskuri** which is WinForms version and desktop application, can be installed into Windows computer.
- **Selolaskuri.WPF**, user interface created with WPF/XAML (WPF is newer than WinForms) and is also a desktop application, can be installed into Windows computer. Resizable window. More in folder Selolaskuri.WPF.
- **Selolaskuri.Razor**, ASP.NET Razor Pages version (, user interface under development **NEW!!** You can try the program in Azure https://selolaskurirazor20200308095542.azurewebsites.net/Selolaskuri (at this moment, may not be there permanently) Works with any browser and mobile phones.
- **Selolaskuri.XBAP**, user interface created with WPF/XAML and is a web application with the limit that it runs only in IE browser. More in https://github.com/isuihko/isuihko.github.io including executable for Internet Explorer.

If you've downloaded all source codes, you can choose, which of the application to run in Visual Studio: in Solution Explorer right click Project's name, choose Set as StartUp Project and then click Start. To compile Razor pages sources Visual Studio 2019 is required.

------

The following text about the usage of application is only in Finnish.

## OHJEET

Lasketaan shakinpelaajalle uusi vahvuusluku SELO tai PELO, ks. http://www.shakki.net/cgi-bin/selo
- SELO on Suomen kansallinen shakin vahvuusluku, esim. https://fi.wikipedia.org/wiki/Elo-luku#Suomen_Elo
- PELO on vastaavasti pikashakin vahvuusluku, jota käytetään kun miettimisaika on enintään 10 minuuttia. Eri miettimisajoille on omat laskentakaavansa.

Laskettu tulos voi poiketa virallisesta laskennasta hieman, ehkä pisteellä pyöristyksistä johtuen.
Vahvuusluvun laskentakaavat ovat peräisin sivulta http://skore.users.paivola.fi/selo.html ja suoritusluvun laskenta on sivulta http://shakki.kivij.info/performance_calculator.shtml 

Selolaskentaa netissä: 
- http://www.shakki.net/kerhot/salsk/ohjelmat/selo.html  SalSK - selolaskentaohjelma
- http://shakki.kivij.info/performance_calculator.shtml  Selo- ja suorituslukulaskuri, myös uuden pelaajan laskenta
- https://ratings.fide.com/calculators.phtml             FIDE's calculators


Syötekenttiä:
* miettimisaika: vähintään 90 min, 60-89 minuuttia, 11-59 minuuttia tai enintään 10 minuuttia
* oma vahvuusluku eli SELO tai jos miettimisaika on enintään 10 minuuttia, niin pikashakin vahvuusluku PELO
* aiempi oma pelimäärä tai tyhjä. Jos pelimääräksi annetaan 0-10, käytetään uuden pelaajan vahvuusluvun laskentakaavaa.
* ottelun tiedot eli Vastustajan SELO-kenttä, joissa vaihtoehdot:
 - a) yhden vastustajan vahvuusluku, esim. 1720 ja ottelun tuloksen valitseminen radiobuttoneista 1  1/2  0.
 - b) usean vastustajan (esim. turnauksen kaikki ottelut) vahvuusluvut tuloksineen, esim. +1622 -1880 =1633 tai +1622 -1880 1633, jossa + tarkoittaa voittoa, - tappiota ja = tai tyhjä tasapeliä
 - c) oma esim. turnauksessa saatu kokonaispistemäärä ja vastustajien vahvuusluvut ilman tuloksia, esim. 1.5 1622 1880 1683
 - d) csv-formaatti (comma-separated values), jossa voidaan antaa kaikki syöte pilkulla erotettuna eikä silloin lomakkeen muiden kenttien arvoilla ole merkitystä. Esim. miettimisaika,selo,pelimaara,ottelut: 90,1525,0,+1621 -1812 =1710 tai selo,ottelut: 2191,+1622 -1880 =1633. Huom! Käytä desimaalipistettä, jos tuloksessa on tasapeli mukana, esim. 2.5.
 
CSV:ssä annetut arvot ohittavat muut (miettimisaika, vahvuusluku, pelimäärä). Jos miettimisaika on antamatta, käytetään lomakkeelta valittua ja pelimäärän puuttuessa käytetään tyhjää.
 
* yhden ottelun tulos, käytettävissä vain vaihtoehdossa a. Tulos valitaan valintapainikkeista: 0 = tappio, 1/2 = tasapeli ja 1 = voitto ja laskelmat päivittyvät sitä mukaa kun kentissä liikutaan.

Pikashakin tulokset on helpointa syöttää formaatissa pistemäärä pelo pelo pelo pelo ... jolloin laskenta tehdään yhdellä kertaa kaikkien otteluiden odotustuloksien summaa käyttäen.

Vastustajan SELO-kenttä muistaa laskennassa käytetyt syötteet, jos siinä kentässä on painettu Enter tai on klikattu Laske vahvuusluku -painiketta. Listasta on helppo valita uudestaan aiemmin käytetyt tiedot, jonka jälkeen niitä voidaan tarvittaessa muokata ennen uutta laskentaa. Kentässä voi myös käyttää komentoja test ja clear. Komento test lisää testimateriaalia Vastustajat-listaan ja clear nollaa kaiken syötteen ja näytön kentät.

Painonapit:
* "Laske vahvuusluku" eli vahvuusluvun laskenta
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

Kaikki tarvittava tieto on annettava laskentaa. Yksittäisen ottelun tulosta varten varattuja valintapainikkeita ei käytetä silloin kun syötetään useamman matsin tulos kerralla.

Virhetarkastukset:
- SELO-lukujen oltava välillä kokonaisluku 1000-2999
- jos on annettu oma pelimäärä, sen on oltava kokonaisluku 0-9999 (laskennan jälkeen uusi pelimäärä saa olla isompikin kuin 9999)
- (a) jos on annettu yksi SELO eli on vain yksi vastustaja, niin ottelun tulos on annettava valintapainikkeilla 0, 1/2 ja 1
- (b) onko annettu muita merkkejä kuin +, - ja = tuloksia syötettäessä
- (c) turnauksen pistemäärän oltava vähintään nolla ja enintään annettujen vastustajien lukumäärä
- (c) annetuissa vahvuusluvuissa ei saa antaa tulosta, jos turnauksen pistemäärä oli annettu
- (d) CSV-formaatissa ei saa olla liikaa pilkkuja ja vaadittavat tiedot on annettava (vähintään oma selo ja vastustajat tuloksineen)

Tulostiedot:
- uusi vahvuusluku
- vahvuusluvun muutos verrattuna alkuperäiseen, voi olla negatiivinen tai positiivinen tai nolla
- uusi pelimäärä, jos pelimäärä oli annettu
- vahvuusluvun vaihteluväli, kun tulokset syötetty tavalla b
- vastustajien keskivahvuus
- turnauksen tulos, joka voi olla esim. yhden ottelun tapauksessa  0,5 / 1  ja muutoin vaikkapa 2 / 5
- piste-ero, kun laskettu yhden ottelun tuloksella (a) tai piste-ero turnauksen keskivahvuuteen
- odotustulos, kun laskettu yhden ottelun tulos (a) tai odotustuloksien summa (b ja c) paitsi jos laskettiin uuden pelaajan vahvuuslukua, jolloin summaa ei näytetä
- suoritusluvut, kolme erilaista: vahvuusluku, jolla odotustulos vastaa saatua pistemäärä sekä FIDE ja lineaarinen.

Jos syötetään yhden ottelun tulosta, niin se voidaan tehdä:
- a) annetaan vahvuusluku numerona, esim. 1720 ja valitaan tulos valintapainikkeista 0, 1/2 ja 1.
- b) Annetaan tulos vahvuusluvun kanssa, esim. +1720, =1720 tai -1720, eikä tuloksen valintapainikkeita ole käytetty.
- c) Annetaan tulos numerona ennen vahvuuslukua, esim. 1 1720, 0.5 1720 tai 0 1720, eikä valintapainikkeita ole käytetty.
- d) CSV-formaatissa, jossa oma selo myös mukana, esim. 1521,1 1720 tai 1521,+1720

Kahden tai useamman ottelun tulos voidaan antaa eri tavoilla
- a) Tulokset vahvuuslukujen yhteydessä, esim. +1622 -1880 =1633 +1717
- b) Pistemäärä ensin ja sitten vahvuusluvut, esim. 2.5 1622 1880 1633 1717
- c) CSV-formaatissa, jossa tulokset tavalla a tai b, esim. jos oma selo on 1521, niin 1521,2.5 1622 1880 1633 1717

UUSI LASKENTA:
Laskenta voidaan aloittaa uuden pelaajan laskennalla ja jatkaa sitten normaalilla laskennalla. Erota Vastustajat-kentässä vahvuusluvut oikeassa kohdassa '/' -merkillä. Alkuperäisen pelimäärän on oltava enintään 10 ja pelimäärän pitää vaihdossa olla vähintään 11.
Kunkin ottelun tulos on annettava vahvuusluvun yhteydessä, esim. jos alkup. pelimäärä on enintään 10, niin tässä 3 peliä lisää vaikkapa turnauksesta, ja sitten vaihtuu laskenta: "+1321 -1678 -1864 / -1995 +1695". Tai csv-formaatissa "90,1525,0,+1525 +1441 -1973 +1718 -1784 -1660 -1966 +1321 -1678 -1864 -1944 / -1995 +1695 -1930 1901", jossa lasketaan 11 ottelua uuden pelaajan kaavalla ja sitten loput neljä normaalilla kaavalla.

Ohjelman asennus:
Windows 7/10: Lataa publish-hakemisto alihakemistoineen ja suorita setup.exe ja hyväksy Selolaskuri.exe:n asennus. Tai lataa koko Visual Studio -projekti ja käännä projekti uudestaan Visual Studiossa.
Koko projektin voi ladata komennolla: git clone https://github.com/isuihko/selolaskuri

------

Version history highlights:
- 10.-19.6.2018 Code refactoring to make automatic unit testing possible (Selolaskuri.Tests). Separated checking of input and calculations (i.e. business logic) from the form.
- 4.8.2018: Added support for CSV format (comma-separated values). Also Java version is up-to-date.
- 11.8.2018: Uses clipboard. Edit-menu with cut, copy and paste for handing opponents list. Handy with CSV format.
- 20.8.2018: Adds usage of half character "1/2" in chess results. Added new unit tests for that.
- 26.8. & 4.9.2018: Moved common checking and calculation into SelolaskuriLibrary. Added Selolaskuri's WFP/XAML version as Selolaskuri.WFP and XBAP version as Selolaskuri.XBAP under this same Visual Studio's solution. Moved common information/instruction windows into SelolaskuriLibrary FormOperations.cs
- 16.-17.2.2020 Better instructions in Menu->Ohjeet. More checking and code refactoring. Now allows entering new player calculation up to certain game and then after '/' continue with normal calculations, like "+1321 -1678 -1864 / -1995 +1695". Note that this requires that players game count is not > 10 when starting and is at least 11 when changing calculation from new player formula to normal.
- 19.2.2020  Rating performance calculations and new output fields for them.
- 8.-9.3.2020 Razor pages versions, published in Azure and anyone can use

All Selolaskuri versions use the same SelolaskuriLibrary for input checking and calculation. The parameter checking and calculations are tested in project Selolaskuri.Tests (now 86 tests) which tests SelolaskuriLibrary routines. User interfaces are similar but created differently.

TODO
- user interface in Razor pages versions, separate mobile user interface
- refactoring, more Object-Oriented Programming (OOP)
- use database as an exercise e.g. to store calculations
- check what else could be moved into SelolaskuriLibrary
- update also Java version (suorituslukulaskenta is missing)

