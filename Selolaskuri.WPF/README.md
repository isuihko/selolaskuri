### isuihko.github.io

Version history:
- 21.-22.8.2018 Created a WPF/XAML version of Selolaskuri desktop application which I've created earlier using WinForms
- 26.8.2018 Moved Selolaskuri.WPF source code under solution Selolaskuri https://github.com/isuihko/selolaskuri where all Selolaskuri's versions use the same SelolaskuriLibrary for input checking and calculation. Also they have same unit tests from project Selolaskuri.Tests (76 tests), which tests SelolaskuriLibrary routines. User interfaces are similar but created differently.

What and why
- Because WPF is newer, more flexible and more used nowadays: https://www.wpf-tutorial.com/about-wpf/wpf-vs-winforms/
- Window/Form definitions are in the file MainWindow.xaml
- Routines accessing the form are in a new module MainWindow.operations.cs (compare with Selolaskuri's SelolaskuriForm.cs)
- App.xaml is not in use, because this application has only one window
- Reused the former code when adding operations i.e. checking of input, calculations and displaying the results (SelolaskuriLibary)

You find the window captures of this new version from .png files:
- alkunäyttö = startup window
- uusi pelaaja,turnaus = new player, calculation from a tournament of seven matches
- PELO-laskenta,turnaus,CSV = blitz calculation from a tournament of 12 matches, entered in CSV format (comma separated values)

Source code is under https://github.com/isuihko/selolaskuri see Selolaskuri.WPF. To run this program in Visual Studio, go to Solution Explorer, right click Selolaskuri.WPF and choose Set as StartUp Project and then click Start.

TODO:
- Use more WPF/XAML features
