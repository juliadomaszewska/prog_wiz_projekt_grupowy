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
            // Jeśli karta nie jest matched → odwróć ją
            if (!card.IsMatched)
            {
                card.IsFaceUp = false;
            }
        }
    }
}
