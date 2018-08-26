### Selolaskuri.WPF

Version history:
- 21.-22.8.2018 Created a WPF/XAML version of Selolaskuri desktop application which I had created earlier using WinForms. User interfaces between WinForms and WPF/XAML versions are similar but they are created differently.
- 26.8.2018 Moved Selolaskuri.WPF source code here under solution Selolaskuri https://github.com/isuihko/selolaskuri where all Selolaskuri's versions use the same *SelolaskuriLibrary* for input checking and calculation. Also there is common project *Selolaskuri.Tests* (76 tests), which are unit tests for SelolaskuriLibrary routines.

What and why
- Because WPF is newer, more flexible and more used nowadays: https://www.wpf-tutorial.com/about-wpf/wpf-vs-winforms/
- Window/Form definitions are in the file *MainWindow.xaml*
- Routines accessing the form are in a new module *MainWindow.operations.cs* (you can compare with Selolaskuri's *SelolaskuriForm.cs*)
- App.xaml is not in use, because this application has only one window
- Reused the former code when adding operations i.e. checking of input, calculations and displaying the results (SelolaskuriLibary)

You find the window captures of this new version from .png files:
- alkunäyttö = startup window
- uusi pelaaja,turnaus = new player, calculation from a tournament of seven matches
- PELO-laskenta,turnaus,CSV = blitz calculation from a tournament of 12 matches, entered in CSV format (comma separated values)

To run this program in Visual Studio, load the whole project (git clone https://github.com/isuihko/selolaskuri), go to Solution Explorer, right click Selolaskuri.WPF and choose Set as StartUp Project and then click Start.

TODO:
- Use more WPF/XAML features
