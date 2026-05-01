using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;

namespace MemoryGame.ViewModels;

public class CardViewModel : ObservableObject
{
    private bool _isFaceUp;
    private bool _isMatched;

    public CardViewModel(string symbol, Action<CardViewModel> onClick)
    {
        Symbol = symbol;
        ClickCommand = new RelayCommand(() => onClick(this));
    }

    public string Symbol { get; }
    public ICommand ClickCommand { get; }

    public bool IsFaceUp
    {
        get => _isFaceUp;
        set
        {
            if (SetProperty(ref _isFaceUp, value))
            {
                OnPropertyChanged(nameof(DisplayText));
            }
        }
    }

    public bool IsMatched
    {
        get => _isMatched;
        set
        {
            if (SetProperty(ref _isMatched, value))
            {
                OnPropertyChanged(nameof(DisplayText));
            }
        }
    }

    public string DisplayText => IsFaceUp || IsMatched ? Symbol : "?";
}
