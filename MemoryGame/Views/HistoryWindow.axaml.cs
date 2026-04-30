using Avalonia.Controls;
using Avalonia.Interactivity;
using MemoryGame.ViewModels;

namespace MemoryGame.Views;

public partial class HistoryWindow : Window
{
    public HistoryWindow()
    {
        InitializeComponent();
    }

    public HistoryWindow(HistoryWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
