using System;
using System.Collections.Generic;
using System.Linq;

namespace prog_wiz_lab8.Models;

public class Deck
{
    private List<Card> _cards;
    private static readonly Random _random = new Random();

    public Deck()
    {
        _cards = new List<Card>();
        Initialize();
    }

    public void Initialize()
    {
        _cards.Clear();
        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            foreach (CardRank rank in Enum.GetValues(typeof(CardRank)))
            {
                _cards.Add(new Card(suit, rank));
            }
        }
    }

    public void Shuffle()
    {
        int n = _cards.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            Card value = _cards[k];
            _cards[k] = _cards[n];
            _cards[n] = value;
        }
    }

    public Card? DrawCard()
    {
        if (_cards.Count == 0) return null;
        
        Card card = _cards.Last();
        _cards.RemoveAt(_cards.Count - 1);
        return card;
    }

    public (Queue<Card> player1Deck, Queue<Card> player2Deck) DealToTwoPlayers()
    {
        var p1 = new Queue<Card>();
        var p2 = new Queue<Card>();
        
        for (int i = 0; i < _cards.Count; i++)
        {
            if (i % 2 == 0) p1.Enqueue(_cards[i]);
            else p2.Enqueue(_cards[i]);
        }
        
        return (p1, p2);
    }
}