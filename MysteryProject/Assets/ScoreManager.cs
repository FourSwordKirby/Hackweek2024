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
}

public class SameColorAnyNumberOrderCriteria : CardPileCriteria
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
        return card.suit == topCard.suit;
    }
}


public class GameState
{
    public List<GradedCardPile> StashedPiles;
    public int TimeRemaining;
}


