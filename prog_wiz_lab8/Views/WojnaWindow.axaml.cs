using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace prog_wiz_lab8.Views;

public partial class WojnaWindow : Window
{
    public WojnaWindow()
    {
        InitializeComponent();
    }

    private void StartGame_Click(object? sender, RoutedEventArgs e)
    {
        // Tutaj czytamy co gracz wpisał w pole TextBox
        string playerName = PlayerNameTextBox.Text ?? "Anonim";
        
        // Z tym pobranym 'playerName' docelowo otworzysz stąd okienko główne
        // tasowania talii / gry (lub zmienisz po prostu UI tego okna na właściwą rozgrywkę).
    }
}