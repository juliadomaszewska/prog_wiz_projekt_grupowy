using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MemoryGame.Models;
using MemoryGame.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryGame.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private string _playerName = "Player 1";
    private string _statusText = "Click a card to begin";
    private int _moves;
    private bool _isBusy;
    private CardViewModel? _firstCard;

    public MainWindowViewModel()
    {
        Cards = new ObservableCollection<CardViewModel>();
        HistoryEntries = new ObservableCollection<GameHistoryEntry>();
        NewGameCommand = new RelayCommand(StartNewGame);
        ShowHistoryCommand = new RelayCommand(OpenHistory);
        StartNewGame();
    }

    public string Title { get; } = "Memory Game";
    public string PlayerName
    {
        get => _playerName;
        set => SetProperty(ref _playerName, value);
    }

    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }

    public string MovesText => $"Moves: {_moves}";
    public ObservableCollection<CardViewModel> Cards { get; }
    public ObservableCollection<GameHistoryEntry> HistoryEntries { get; }
    public IRelayCommand NewGameCommand { get; }
    public IRelayCommand ShowHistoryCommand { get; }

    private void StartNewGame()
    {
        _moves = 0;
        StatusText = "Find matching pairs.";
        _firstCard = null;
        _isBusy = false;
        Cards.Clear();

        var symbols = new[] { "A", "B", "C", "D", "E", "F", "G", "H" }
            .SelectMany(x => new[] { x, x })
            .OrderBy(_ => Guid.NewGuid())
            .ToArray();

        foreach (var symbol in symbols)
        {
            Cards.Add(new CardViewModel(symbol, OnCardClicked));
        }

        OnPropertyChanged(nameof(MovesText));
    }

    private async void OnCardClicked(CardViewModel card)
    {
        if (_isBusy || card.IsFaceUp || card.IsMatched)
        {
            return;
        }

        card.IsFaceUp = true;

        if (_firstCard == null)
        {
            _firstCard = card;
            StatusText = "Choose the second card.";
            return;
        }

        _moves++;
        OnPropertyChanged(nameof(MovesText));

        if (_firstCard.Symbol == card.Symbol)
        {
            _firstCard.IsMatched = true;
            card.IsMatched = true;
            StatusText = "Great! You found a pair.";
            _firstCard = null;

            if (Cards.All(c => c.IsMatched))
            {
                StatusText = $"You won in {_moves} moves!";
                AddHistoryEntry("Win");
            }

            return;
        }

        StatusText = "Not a match.";
        _isBusy = true;
        await Task.Delay(800);

        card.IsFaceUp = false;
        _firstCard.IsFaceUp = false;
        _firstCard = null;
        _isBusy = false;
        StatusText = "Try again.";
    }

    private void OpenHistory()
    {
        var historyWindow = new HistoryWindow(new HistoryWindowViewModel(HistoryEntries));
        historyWindow.Show();
    }

    private void AddHistoryEntry(string result)
    {
        HistoryEntries.Add(new GameHistoryEntry
        {
            PlayerName = PlayerName,
            GameName = "Memory",
            Result = result,
            Moves = _moves,
            PlayedAt = DateTime.Now,
        });
    }
}
