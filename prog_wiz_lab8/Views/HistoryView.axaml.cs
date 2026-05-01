using Avalonia.Controls;

namespace prog_wiz_lab8.Views;

public partial class HistoryView : Window
{
    public HistoryView()
    {
        InitializeComponent();
        DataContext = new prog_wiz_lab8.ViewModels.HistoryViewModel();
    }
}