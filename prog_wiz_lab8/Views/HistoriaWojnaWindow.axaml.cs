using Avalonia.Controls;
using System.IO;

namespace prog_wiz_lab8.Views;

public partial class HistoriaWojnaWindow : Window
{
    public HistoriaWojnaWindow()
    {
        InitializeComponent();
        LoadHistory();
    }

    private void LoadHistory()
    {
        string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "historia_wojna.txt");

        if (File.Exists(path))
        {
            HistoryTextBlock.Text = File.ReadAllText(path);
        }
        else
        {
            HistoryTextBlock.Text = "Brak zarejestrowanych rozgrywek w Wojnę. Zagraj i dojdź do końca gry, by zobaczyć wynik!";
        }
    }
}