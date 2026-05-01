namespace prog_wiz_lab8.Models;

public enum CardSuit
{
    Kier,
    Karo,
    Trefl,
    Pik
}

public enum CardRank
{
    Dwa = 2, Trzy, Cztery, Pięć, Sześć, Siedem, Osiem, Dziewięć, Dziesięć,
    Walet, Dama, Król, As
}

public class Card
{
    public CardSuit Suit { get; set; }
    public CardRank Rank { get; set; }
    
    public int Value => (int)Rank;

    public string DisplayName 
    {
        get 
        {
            string rankName = Rank switch
            {
                CardRank.Walet => "Walet",
                CardRank.Dama => "Dama",
                CardRank.Król => "Król",
                CardRank.As => "As",
                _ => ((int)Rank).ToString() 
            };

            string suitSymbol = Suit switch
            {
                CardSuit.Kier => "♥",
                CardSuit.Karo => "♦",
                CardSuit.Trefl => "♣",
                CardSuit.Pik => "♠",
                _ => ""
            };

            return $"{rankName} {suitSymbol}";
        }
    }

    public Card(CardSuit suit, CardRank rank)
    {
        Suit = suit;
        Rank = rank;
    }
}
