using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using prog_wiz_lab8.Models;

namespace prog_wiz_lab8.ViewModels;

public partial class MakaoViewModel : ViewModelBase
{
    [ObservableProperty] private string _playerName = string.Empty;
    [ObservableProperty] private bool _isGameStarted = false;
    [ObservableProperty] private Card _topCard;
    [ObservableProperty] private bool _isPlayerTurn = true;
    [ObservableProperty] private string _gameMessage = "Witaj w Makao!";

    private Deck _deck;
    private int _drawPenalty = 0; // Kary za 2, 3, Króle
    private CardSuit? _requestedSuit = null; // Żądanie Asa
    private CardRank? _requestedRank = null; // Żądanie Waleta

    public ObservableCollection<Card> PlayerCards { get; } = new ObservableCollection<Card>();
    public ObservableCollection<Card> ComputerCards { get; } = new ObservableCollection<Card>();

    [RelayCommand]
    private void StartGame()
    {
        if (!string.IsNullOrWhiteSpace(PlayerName))
        {
            IsGameStarted = true;
            _deck = new Deck();
            _deck.Shuffle();
            PlayerCards.Clear();
            ComputerCards.Clear();
            _drawPenalty = 0; _requestedSuit = null; _requestedRank = null;

            for (int i = 0; i < 5; i++)
            {
                PlayerCards.Add(_deck.DrawCard());
                ComputerCards.Add(_deck.DrawCard());
            }

            TopCard = _deck.DrawCard();
            UpdateGameMessageForPlayer();
        }
    }

    [RelayCommand]
    private void EndGame()
    {
        // Jeśli kończymy w trakcie gry
        if (IsGameStarted)
        {
            ZapiszWynikDoHistorii("Przerwana przez gracza (Poddanie)");
        }
        IsGameStarted = false; 
    }

    private void ZapiszWynikDoHistorii(string wynik)
    {
        string wpis = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Gra: Makao | Gracz: {PlayerName} | Wynik: {wynik}\n";
        File.AppendAllText("historia_makao.txt", wpis);
    }

    [RelayCommand]
    private async Task DrawOneCard()
    {
        // Dobieranie karty / branie kart karnych
        if (!IsGameStarted || _deck == null || !IsPlayerTurn) return;

        int cardsToTake = _drawPenalty > 0 ? _drawPenalty : 1;
        for (int i = 0; i < cardsToTake; i++)
        {
            var c = _deck.DrawCard();
            if (c != null) PlayerCards.Add(c);
        }
        
        _drawPenalty = 0; // Wyczyszczenie kary
        await ComputerTurn(); // Oddajemy turę
    }

    [RelayCommand]
    private async Task PlayCard(Card playedCard)
    {
        if (!IsGameStarted || playedCard == null || !IsPlayerTurn) return;

        if (IsMoveValid(playedCard))
        {
            PlayerCards.Remove(playedCard);
            TopCard = playedCard;
            ApplySpecialRules(playedCard, PlayerCards);
            
            if (PlayerCards.Count == 0) 
            { 
                GameMessage = "WYGRAŁEŚ!"; 
                IsPlayerTurn = false; 
                ZapiszWynikDoHistorii("ZWYCIĘSTWO"); 
                return; 
            }
            await ComputerTurn();
        }
    }

    private bool IsMoveValid(Card c)
    {
        // Jeśli jest kara, można użyć tylko 2, 3, i bojowych Króli (Czarne/Czerwone Serce -> Pik/Kier)
        if (_drawPenalty > 0)
        {
            return c.Rank == CardRank.Dwa || c.Rank == CardRank.Trzy || 
                   (c.Rank == CardRank.Król && (c.Suit == CardSuit.Kier || c.Suit == CardSuit.Pik));
        }
        
        // Jeśli as zażądał koloru
        if (_requestedSuit != null) return c.Suit == _requestedSuit || c.Rank == CardRank.As;
        
        // Jeśli walet zażądał figury
        if (_requestedRank != null) return c.Rank == _requestedRank || c.Rank == CardRank.Walet;

        // Normalna zasada
        return c.Suit == TopCard.Suit || c.Rank == TopCard.Rank || c.Rank == CardRank.As || c.Rank == CardRank.Walet;
    }

    private void ApplySpecialRules(Card c, ObservableCollection<Card> hand)
    {
        _requestedSuit = null; _requestedRank = null; // Kasuj stare żądania

        if (c.Rank == CardRank.Dwa) _drawPenalty += 2;
        else if (c.Rank == CardRank.Trzy) _drawPenalty += 3;
        else if (c.Rank == CardRank.Król && (c.Suit == CardSuit.Kier || c.Suit == CardSuit.Pik)) _drawPenalty += 5;
        else if (c.Rank == CardRank.As) 
        {
            // PRO UX: Auto-żądanie koloru którego masz najwięcej ;)
            _requestedSuit = hand.Any() ? hand.GroupBy(x => x.Suit).OrderByDescending(g => g.Count()).First().Key : CardSuit.Pik;
        }
        else if (c.Rank == CardRank.Walet)
        {
            _requestedRank = hand.FirstOrDefault(x => x.Rank != CardRank.Walet && x.Rank != CardRank.As)?.Rank ?? CardRank.Pięć;
        }
    }

    private async Task ComputerTurn()
    {
        IsPlayerTurn = false;
        
        await Task.Delay(1500); // 1.5 sekundy dramatyzmu i realizmu 

        var cardToPlay = ComputerCards.FirstOrDefault(c => IsMoveValid(c));

        if (cardToPlay != null)
        {
            ComputerCards.Remove(cardToPlay);
            TopCard = cardToPlay;
            ApplySpecialRules(cardToPlay, ComputerCards);
        }
        else
        {
            // Komputer bierze karę lub dobiera
            int cardsToTake = _drawPenalty > 0 ? _drawPenalty : 1;
            for(int i = 0; i < cardsToTake; i++) 
            {
               var c = _deck.DrawCard();
               if(c != null) ComputerCards.Add(c);
            }
            _drawPenalty = 0;
        }

        if (ComputerCards.Count == 0) 
        { 
            GameMessage = "PRZECIWNIK WYGRAŁ!"; 
            ZapiszWynikDoHistorii("PORAŻKA"); 
            return; 
        }

        await Task.Delay(1500);
        IsPlayerTurn = true;
        UpdateGameMessageForPlayer();
    }

    private void UpdateGameMessageForPlayer()
    {
        string msg = "Twoja tura. ";
        if (_drawPenalty > 0) msg += $"KARA: +{_drawPenalty} kart (broń się albo dobierz)!";
        else if (_requestedSuit != null) msg += $"Żądanie Koloru: {_requestedSuit}!";
        else if (_requestedRank != null) msg += $"Żądanie Figury: {_requestedRank}!";
        GameMessage = msg;
    }
}
