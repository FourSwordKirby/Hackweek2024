using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int ScoreGame(GameState gameState)
    {
        return ScoreAllPiles(gameState.PileToCriteria) + gameState.TimeRemaining;
    }

    public int ScoreAllPiles(Dictionary<CardPile, CardPileCriteria> pileToCriteria)
    {
        int total = 0;
        foreach (var item in pileToCriteria)
        {
            total += item.Value.ScorePile(item.Key);
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
        return pile.Cards.Count * (pile.Cards.Count + 1) / 2;
    }
}

public class IncrementWithDuplicatesCriteria : CardPileCriteria
{
    public override bool IsCardValid(CardPile pile, Card card)
    {
        // First get the last card in the pile.
        // If no cards, card is auto valid.
        if (pile.Cards.Count == 0)
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
        if (pile.Cards.Count == 0)
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
        if (pile.Cards.Count == 0)
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
    public Dictionary<CardPile, CardPileCriteria> PileToCriteria;
    public int TimeRemaining;
}


