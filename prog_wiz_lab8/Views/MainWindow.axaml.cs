using Avalonia.Controls;
using Avalonia.Interactivity;

namespace prog_wiz_lab8.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnGame1_Click(object? sender, RoutedEventArgs e)
    {
        // Utworzenie i otwarcie Twojego okna po kliknięciu 1 gry
        var wojnaWindow = new WojnaWindow();
        wojnaWindow.Show();
    }

    private void BtnGame2_Click(object? sender, RoutedEventArgs e)
    {
        // Tutaj otwierać się będzie okno Gry 2
    }

    private void BtnGame3_Click(object? sender, RoutedEventArgs e)
    {
        var makaoWindow = new MakaoView();
        makaoWindow.Show();
    }
}