using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using prog_wiz_lab8.Models;

namespace prog_wiz_lab8.Views;

public partial class WojnaWindow : Window
{
    private string _playerName = "Gracz";
    private Queue<Card> _playerCards = new();
    private Queue<Card> _computerCards = new();
    private List<Card> _tableCards = new();

    public WojnaWindow()
    {
        InitializeComponent();
    }

    private void StartGame_Click(object? sender, RoutedEventArgs e)
    {
        string input = PlayerNameTextBox.Text?.Trim() ?? "";
        if (!string.IsNullOrEmpty(input))
        {
            _playerName = input;
        }
        
        LoginPanel.IsVisible = false;
        GamePanel.IsVisible = true;
        this.Title = $"Wojna - {_playerName} vs BOT";

        var deck = new Deck();
        deck.Shuffle();
        
        var dealt = deck.DealToTwoPlayers();
        _playerCards = dealt.player1Deck;
        _computerCards = dealt.player2Deck;

        UpdateLabels();
    }

    private void History_Click(object? sender, RoutedEventArgs e)
    {
        var historia = new HistoriaWojnaWindow();
        historia.Show();
    }

    private void Fight_Click(object? sender, RoutedEventArgs e)
    {
        if (_playerCards.Count == 0 || _computerCards.Count == 0)
        {
            EndGame();
            return;
        }

        Card playerCard = _playerCards.Dequeue();
        Card computerCard = _computerCards.Dequeue();

        _tableCards.Add(playerCard);
        _tableCards.Add(computerCard);

        PlayerCardText.Text = playerCard.DisplayName;
        ComputerCardText.Text = computerCard.DisplayName;

        if (playerCard.Value > computerCard.Value)
        {
            FightResultText.Text = $"Wygrałeś rundę! Zgarniasz {_tableCards.Count} kart.";
            FightResultText.Foreground = Avalonia.Media.Brushes.LightGreen;
            GiveTableCardsToWinner(_playerCards);
        }
        else if (computerCard.Value > playerCard.Value)
        {
            FightResultText.Text = $"Pech... Bot odbiera Ci: {_tableCards.Count} kart.";
            FightResultText.Foreground = Avalonia.Media.Brushes.IndianRed;
            GiveTableCardsToWinner(_computerCards);
        }
        else
        {
            FightResultText.Text = "REMIS! Szykuj się do wojny!";
            FightResultText.Foreground = Avalonia.Media.Brushes.Yellow;

            if (_playerCards.Count > 0) _tableCards.Add(_playerCards.Dequeue());
            if (_computerCards.Count > 0) _tableCards.Add(_computerCards.Dequeue());
        }

        UpdateLabels();
        
        if (_playerCards.Count == 0 || _computerCards.Count == 0) EndGame();
    }

    private void GiveTableCardsToWinner(Queue<Card> winnerDeck)
    {
        foreach (var card in _tableCards)
        {
            winnerDeck.Enqueue(card);
        }
        _tableCards.Clear();
    }

    private void UpdateLabels()
    {
        PlayerCardsCount.Text = $"{_playerName}: (Ilość kart: {_playerCards.Count})";
        ComputerCardsCount.Text = $"Przeciwnik BOT: (Ilość kart: {_computerCards.Count})";
    }

    private void GiveUp_Click(object? sender, RoutedEventArgs e)
    {
        EndGame("Poddana");
    }

    private void EndGame(string? wymuszonyWynik = null)
    {
        string wynik;
        
        if (wymuszonyWynik == "Poddana")
        {
            FightResultText.Text = "Poddano rozgrywkę...";
            wynik = "Poddana";
        }
        else if (_playerCards.Count > _computerCards.Count)
        {
            FightResultText.Text = "🏆 WYGRAŁEŚ CAŁĄ GRĘ! 🏆";
            wynik = "Wygrana";
        }
        else
        {
            FightResultText.Text = "Niestety, przegrałeś ten mecz.";
            wynik = "Przegrana";
        }
        
        FightResultText.Foreground = Avalonia.Media.Brushes.Goldenrod;

        string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "historia_wojna.txt");
        string linijkaHistorii = $"{System.DateTime.Now:yyyy-MM-dd HH:mm} | Gracz: {_playerName} | Wynik: {wynik}\n";
        System.IO.File.AppendAllText(path, linijkaHistorii);

        GamePanel.IsVisible = false;
        LoginPanel.IsVisible = true;
        this.Title = "Wojna - Logowanie Gracza";
        PlayerCardText.Text = "?";
        ComputerCardText.Text = "?";
        FightResultText.Text = "Wyłóż kartę, aby walczyć!";
        FightResultText.Foreground = Avalonia.Media.Brushes.LightBlue;
    }
}