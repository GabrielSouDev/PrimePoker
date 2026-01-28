using PrimePoker.Domain.Session;
using PrimePoker.Domain.Session.Enums;

namespace PrimePoker.Application.Game.Extensions;

public static class DeckExtensions
{
    public static Card[] NewDeck()
    {
        Card[] cards = new Card[52];
        int index = 0;

        foreach (Rank rank in Enum.GetValues<Rank>())
        {
            foreach (Suit suit in Enum.GetValues<Suit>())
            {
                cards[index++] = new Card
                {
                    Rank = rank,
                    Suit = suit
                };
            }
        }

        return cards;
    }

    public static void Shuffle(this Card[] cards)
    {
        Random rng = new();
        int n = cards.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (cards[n], cards[k]) = (cards[k], cards[n]);
        }
    }
}
