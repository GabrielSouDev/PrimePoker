namespace PrimePoker.Domain.Session;

public class Deck
{
    public Stack<Card> Cards { get; set; } = new Stack<Card>(52);
}
