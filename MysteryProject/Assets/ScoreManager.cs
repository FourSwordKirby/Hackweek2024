using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    public static int ScoreGame(GameState gameState)
    {
        return ScoreAllPiles(gameState.StashedPiles) + gameState.TimeRemaining;
    }

    public static int ScoreAllPiles(List<GradedCardPile> pileToCriteria)
    {
        int total = 0;
        foreach (var item in pileToCriteria)
        {
            total += item.criteria.ScorePile(item.pile);
        }
        
        return total;
    }
}

// Each CardPile is simply a list of cards
// Each CardPile will be associated with a CardPileCriteria
// The Criteria will be used to determine:
// - Whether a new card can go on top of the pile
// - The score of the pile.
public abstract class CardPileCriteria
{
    // Returns true if the new card can go on top of the pile.
    public abstract bool IsCardValid(CardPile pile, Card card);

    public abstract string CriteriaDescriptor();

    // Given a card pile, score a current pile.
    public virtual int ScorePile(CardPile pile)
    {
        return pile.Count * (pile.Count + 1) / 2;
    }
}

public class IncrementWithDuplicatesCriteria : CardPileCriteria
{
    public override bool IsCardValid(CardPile pile, Card card)
    {
        // First get the last card in the pile.
        // If no cards, card is auto valid.
        if (pile.Count == 0)
        {
            return true;
        }

        Card topCard = pile.PeekTopCard();

        // True if the card being added is greater than or equal to the top card of the deck.
        return card.CompareTo(topCard) >= 0;
    }

    public override string CriteriaDescriptor()
    {
        return "Increment";
    }
}

public class DecrementWithDuplicatesCriteria : CardPileCriteria
{
    public override bool IsCardValid(CardPile pile, Card card)
    {
        // First get the last card in the pile.
        // If no cards, card is auto valid.
        if (pile.Count == 0)
        {
            return true;
        }

        Card topCard = pile.PeekTopCard();

        // True if the card being added is greater than or equal to the top card of the deck.
        return card.CompareTo(topCard) <= 0;
    }
    public override string CriteriaDescriptor()
    {
        return "Decrement";
    }
}

public class SameColorAnyNumberOrderCriteria : CardPileCriteria
{
    public override string ToString() => "Same Color";
    
    public override bool IsCardValid(CardPile pile, Card card)
    {
        // First get the last card in the pile.
        // If no cards, card is auto valid.
        if (pile.Count == 0)
        {
            return true;
        }

        Card topCard = pile.PeekTopCard();
        
        // True if the card being added is the same suit as the top card of the deck
        return card.suit == topCard.suit;
    }
    public override string CriteriaDescriptor()
    {
        return "Same Color";
    }
}

public class DiffierentColorCriteria : CardPileCriteria
{
    public override bool IsCardValid(CardPile pile, Card card)
    {
        // First get the last card in the pile.
        // If no cards, card is auto valid.
        if (pile.Count == 0)
        {
            return true;
        }

        Card topCard = pile.PeekTopCard();

        // True if the card being added is NOT the same suit as the top card of the deck
        return card.suit != topCard.suit;
    }

    public override string CriteriaDescriptor()
    {
        return "Different Color";
    }
}

public class FibbonacciCriteria : CardPileCriteria
{
    public override bool IsCardValid(CardPile pile, Card card)
    {
        Card[] currentPile = pile.PeekAllCards();
        if (currentPile.Length < 2)
        {
            return true;
        }
        else
        {
            return (currentPile[currentPile.Length - 1].numberValue + currentPile[currentPile.Length - 2].numberValue) == card.numberValue;
        }
    }

    public override string CriteriaDescriptor()
    {
        return "Fibonacci Sequence";
    }
}

public class PrimeCriteria : CardPileCriteria
{
    public override bool IsCardValid(CardPile pile, Card card)
    {
        return card.numberValue == 2 ||
            card.numberValue == 3 ||
            card.numberValue == 5 ||
            card.numberValue == 7 ||
            card.numberValue == 11 ||
            card.numberValue == 13 ||
            card.numberValue == 17 ||
            card.numberValue == 19 ||
            card.numberValue == 23 ||
            card.numberValue == 29 ||
            card.numberValue == 31 ||
            card.numberValue == 37;
        // And so forth
    }

    public override string CriteriaDescriptor()
    {
        return "Primes";
    }
}

public class MersennePrimeCriteria : CardPileCriteria
{
    public override bool IsCardValid(CardPile pile, Card card)
    {
        return card.numberValue == 2 ||
            card.numberValue == 3 ||
            card.numberValue == 5 ||
            card.numberValue == 7 ||
            card.numberValue == 13 ||
            card.numberValue == 17 ||
            card.numberValue == 19 ||
            card.numberValue == 31;
        // And so forth
    }

    public override string CriteriaDescriptor()
    {
        return "Mersenne Primes";
    }
}

/// <summary>
/// https://en.wikipedia.org/wiki/Polite_number
/// </summary>
public class PoliteCriteria : CardPileCriteria
{
        public override bool IsCardValid(CardPile pile, Card card)
        {
            return card.numberValue == 3 ||
                card.numberValue == 5 ||
                card.numberValue == 6 ||
                card.numberValue == 7 ||
                card.numberValue == 9 ||
                card.numberValue == 10 ||
                card.numberValue == 11 ||
                card.numberValue == 12 ||
                card.numberValue == 13 ||
                card.numberValue == 14 ||
                card.numberValue == 15 ||
                card.numberValue == 17;
            // And so forth
    }

    public override string CriteriaDescriptor()
    {
        return "Polite Numbers";
    }
}


public class GameState
{
    public List<GradedCardPile> StashedPiles;
    public int TimeRemaining;
}


