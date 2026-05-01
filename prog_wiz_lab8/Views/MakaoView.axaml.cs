using Avalonia.Controls;
using Avalonia.Interactivity;

namespace prog_wiz_lab8.Views;

public partial class MakaoView : Window
{
    public MakaoView()
    {
        InitializeComponent();
        DataContext = new prog_wiz_lab8.ViewModels.MakaoViewModel();
    }

    private void BtnHistory_Click(object? sender, RoutedEventArgs e)
    {
        var historyWindow = new HistoryView();
        historyWindow.Show();
    }
}
