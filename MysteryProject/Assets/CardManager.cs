using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CardManager : MonoBehaviour
{
    public CardPile MainDeck;

    public List<Bin> UserBins = new List<Bin>();
    public List<GradedCardPile> StashedPiles = new List<GradedCardPile>();

    public CardPile currentSelectedPile => UserBins[currentSelectedPileIndex].pile;
    public CardPileCriteria currentSelectedCriteria => UserBins[currentSelectedPileIndex].criteria;
    public int currentSelectedPileIndex = 0;

    public void Start()
    {
        MainDeck = new CardPile(true, true);

        UserBins.Add(new Bin(new IncrementWithDuplicatesCriteria()));
        UserBins.Add(new Bin(new DecrementWithDuplicatesCriteria()));
        UserBins.Add(new Bin(new IncrementWithDuplicatesCriteria()));
    }

    public void Update()
    {
        if (MainDeck.IsEmpty())
        {
            Debug.LogError("Main deck is empty. Can't draw cards.");
            return;
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            SelectPreviousPile();
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            SelectNextPile();
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            StashAndCreateNewUserPile();

        if (Keyboard.current.enterKey.wasPressedThisFrame)
            TryAddCardToCurrentPile();
    }

    public void TryAddCardToCurrentPile()
    {
        Card peekedCard = MainDeck.PeekTopCard();
        if (!currentSelectedPile.IsEmpty())
        {
            Card peekedPileCard = currentSelectedPile.PeekBottomCard();
            if (!currentSelectedCriteria.IsCardValid(currentSelectedPile, peekedCard))
            {
                Debug.LogError($"Can't add {peekedCard.numberValue} to pile. ({peekedCard.numberValue} < {peekedPileCard.numberValue})");
                return;
            }
        }

        Card drawnCard = MainDeck.DrawTopCard();
        currentSelectedPile.Cards.Add(drawnCard);

        Debug.Log($"Added {drawnCard.officialName} to pile {currentSelectedPileIndex}.");
    }

    public void SelectNextPile()
    {
        currentSelectedPileIndex = (currentSelectedPileIndex + 1) % UserBins.Count;

        Debug.Log($"Selected user pile {currentSelectedPileIndex} with {currentSelectedPile.Cards.Count} cards.");
        Debug.Log(currentSelectedPile.ToString());
    }

    public void SelectPreviousPile()
    {
        currentSelectedPileIndex = (currentSelectedPileIndex - 1 + UserBins.Count) % UserBins.Count;

        Debug.Log($"Selected user pile {currentSelectedPileIndex} with {currentSelectedPile.Cards.Count} cards.");
        Debug.Log(currentSelectedPile.ToString());
    }

    public void StashAndCreateNewUserPile()
    {
        CardPile cardPileToStash = currentSelectedPile;
        StashedPiles.Add(new GradedCardPile(currentSelectedCriteria, cardPileToStash));
        UserBins[currentSelectedPileIndex].pile = new CardPile();

        Debug.Log($"Stashed pile {currentSelectedPileIndex} with {cardPileToStash.Cards.Count} cards.");
    }
}

[Serializable]
public class Bin
{
    public CardPileCriteria criteria;
    public CardPile pile;

    public Bin(CardPileCriteria criteria)
    {
        this.criteria = criteria;
        this.pile = new CardPile();
    }
}

[Serializable]
public class GradedCardPile
{
    public CardPileCriteria criteria;
    public CardPile pile;

    public GradedCardPile(CardPileCriteria criteria, CardPile pile)
    {
        this.criteria = criteria;
        this.pile = pile;
    }
}

[Serializable]
public class CardPile
{
    public List<Card> Cards;

    public CardPile(bool initialize = false, bool shuffle = false)
    {
        Cards = new List<Card>();

        if (initialize)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < Enum.GetValues(typeof(CardSuit)).Length; j++)
                {
                    Card newCard = new Card();
                    newCard.numberValue = i;
                    newCard.suit = (CardSuit)j;
                    Cards.Add(newCard);
                }
            }
        }

        if (shuffle)
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Card temp = Cards[i];
                int randomIndex = UnityEngine.Random.Range(0, Cards.Count);
                Cards[i] = Cards[randomIndex];
                Cards[randomIndex] = temp;
            }
        }
    }

    public Card DrawTopCard()
    {
        Card DrawnCard = Cards[0];
        Cards.RemoveAt(0);
        return DrawnCard;
    }

    public Card PeekTopCard()
    {
        Card DrawnCard = Cards[0];
        return DrawnCard;
    }

    public Card PeekBottomCard()
    {
        Card DrawnCard = Cards[Cards.Count - 1];
        return DrawnCard;
    }

    public Card MulliganTopCard()
    {
        Card DrawnCard = Cards[0];
        Cards.RemoveAt(0);
        Cards.Add(DrawnCard);
        return DrawnCard;
    }

    public bool IsEmpty() => Cards.Count == 0;

    public override string ToString()
    {
        string output = "Cards in pile:\n";
        foreach (Card card in Cards)
            output += card.ToString() + "\n";
        output.Remove(output.Length - 1);

        return output;
    }
}

public class Card : IComparable<Card>
{
    public string officialName => suit + " " + numberValue;
    public int numberValue; // <0 thru 9>
    public CardSuit suit;

    public override string ToString()
    {
        return officialName;
    }

    public int CompareTo(Card other)
    {
        return numberValue - other.numberValue;
    }

    public bool LargerThan(Card other) => numberValue > other.numberValue;

    public bool SmallerThan(Card other) => numberValue < other.numberValue;
}

public enum CardSuit
{
    Red,
    Yellow,
    Green,
    Blue
}
