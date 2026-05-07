using Avalonia.Controls;
using Avalonia.Input;
using MemoryGame.ViewModels;

namespace MemoryGame.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Card_DoubleTapped(object? sender, TappedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is CardViewModel card)
        {
            // Wywołujemy metodę z ViewModelu
            if (DataContext is MainWindowViewModel vm)
            {
                vm.OnCardDoubleClicked(card);
            }
        }
    }
}
