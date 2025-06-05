using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum CriteriaType
{
    Increment,
    Decrement,
    SameColor,
    DifferentColor,
    Fibonacci,
    Prime,
    MersennePrime,
    Polite
};

public class CardManager : MonoBehaviour
{
    public bool hardMode = false;
    public CardPile MainDeck;

    public List<Bin> UserBins = new List<Bin>();
    public List<GradedCardPile> StashedPiles = new List<GradedCardPile>();

    public CardPile currentSelectedPile => UserBins[currentSelectedPileIndex].pile;
    public CardPileCriteria currentSelectedCriteria => UserBins[currentSelectedPileIndex].criteria;
    public int currentSelectedPileIndex = 0;

    public UnityEvent<int> OnPileStashed = new UnityEvent<int>();
    public UnityEvent<int> OnCardAddedToPile = new UnityEvent<int>();
    public UnityEvent<int, CriteriaType> OnPileCreated = new UnityEvent<int, CriteriaType>();
    public UnityEvent OnFailedToAddCard = new ();

    public static CardManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }


    public void Start()
    {
        MainDeck = new CardPile(true, true);

        UserBins.Add(new Bin(new IncrementWithDuplicatesCriteria()));
        UserBins.Add(new Bin(new DecrementWithDuplicatesCriteria()));
        UserBins.Add(new Bin(new IncrementWithDuplicatesCriteria()));

        for (int index = 0; index < UserBins.Count; index++)
        {
            OnPileCreated?.Invoke(index, CriteriaTypeTable[UserBins[index].criteria.GetType()]);
        }
    }

    public void TryAddCardToCurrentPile()
    {
        Card peekedCard = MainDeck.PeekTopCard();
        if (!currentSelectedPile.IsEmpty())
        {
            if (!currentSelectedCriteria.IsCardValid(currentSelectedPile, peekedCard))
            {
                Card peekedPileCard = currentSelectedPile.PeekTopCard();
                Debug.LogError($"Can't add {peekedCard.numberValue} to pile. ({peekedCard.numberValue} < {peekedPileCard.numberValue})");
                OnFailedToAddCard?.Invoke();
                return;
            }
        }

        Card drawnCard = MainDeck.DrawTopCard();
        currentSelectedPile.PlaceCardOnTop(drawnCard);

        Debug.Log($"Added {drawnCard.officialName} to pile {currentSelectedPileIndex}.");
        OnCardAddedToPile?.Invoke(currentSelectedPileIndex);
    }

    public void SelectNextPile()
    {
        currentSelectedPileIndex = (currentSelectedPileIndex + 1) % UserBins.Count;

        Debug.Log($"Selected user pile {currentSelectedPileIndex} with {currentSelectedPile.Count} cards.");
        Debug.Log(currentSelectedPile.ToString());
    }

    public void SelectPreviousPile()
    {
        currentSelectedPileIndex = (currentSelectedPileIndex - 1 + UserBins.Count) % UserBins.Count;

        Debug.Log($"Selected user pile {currentSelectedPileIndex} with {currentSelectedPile.Count} cards.");
        Debug.Log(currentSelectedPile.ToString());
    }

    public void StashAndCreateNewUserPile()
    {
        if (UserBins[currentSelectedPileIndex].pile.Count <= 0)
            return;

        GradedCardPile cardPileToStash = UserBins[currentSelectedPileIndex].ProcessBin();
        StashedPiles.Add(cardPileToStash);

        Debug.Log($"Stashed pile {currentSelectedPileIndex} with {cardPileToStash.pile.Count} cards.");
        OnPileStashed?.Invoke(currentSelectedPileIndex);

        CardPileCriteria newCriteria = GenerateNewCriteria();
        UserBins[currentSelectedPileIndex].criteria = newCriteria;
        // New event for creating a new bin.
        OnPileCreated?.Invoke(currentSelectedPileIndex, CriteriaTypeTable[newCriteria.GetType()]);
    }

    

    public static Dictionary<System.Type, CriteriaType> CriteriaTypeTable = new Dictionary<Type, CriteriaType>()
    {
        { typeof(IncrementWithDuplicatesCriteria), CriteriaType.Increment },
        { typeof(DecrementWithDuplicatesCriteria), CriteriaType.Decrement },
        { typeof(SameColorAnyNumberOrderCriteria), CriteriaType.SameColor },
        { typeof(DiffierentColorCriteria), CriteriaType.DifferentColor },
        { typeof(FibbonacciCriteria), CriteriaType.Fibonacci },
        { typeof(PrimeCriteria), CriteriaType.Prime },
        { typeof(MersennePrimeCriteria), CriteriaType.MersennePrime },
        { typeof(PoliteCriteria), CriteriaType.Polite },
    };

    private static CardPileCriteria[] EasyCriteriaList = new CardPileCriteria[]
    {
        new IncrementWithDuplicatesCriteria(),
        new DecrementWithDuplicatesCriteria(),
        new SameColorAnyNumberOrderCriteria(),
        new DiffierentColorCriteria(),
    };

    private static CardPileCriteria[] HardCriteriaList = new CardPileCriteria[]
    {
        new IncrementWithDuplicatesCriteria(),
        new DecrementWithDuplicatesCriteria(),
        new SameColorAnyNumberOrderCriteria(),
        new DiffierentColorCriteria(),
        new FibbonacciCriteria(),
        new PrimeCriteria(),
        new MersennePrimeCriteria(),
        new PoliteCriteria(),
    };

    private CardPileCriteria GenerateNewCriteria()
    {
        if (hardMode)
            return HardCriteriaList[UnityEngine.Random.Range(0, HardCriteriaList.Length)];

        return EasyCriteriaList[UnityEngine.Random.Range(0, EasyCriteriaList.Length)];
    }

    public void StashAllPiles()
    {
        for (int i = 0; i < UserBins.Count; i++)
        {
            CardPile cardPileToStash = UserBins[i].pile;
            if (cardPileToStash.Count != 0)
            {
                StashedPiles.Add(UserBins[i].ProcessBin());
            }
        }
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

    public GradedCardPile ProcessBin()
    {
        GradedCardPile gradedPile = new GradedCardPile(criteria, pile);
        pile = new CardPile();
        return gradedPile;
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
    private List<Card> Cards;

    public int Count => Cards.Count;

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

    /// <summary>
    /// Returns a copy of the current pile of cards in order for the user to inspect
    /// </summary>
    public Card[] PeekAllCards()
    {
        return Cards.ToArray();
    }

    public Card DrawTopCard()
    {
        Card DrawnCard = Cards[0];
        Cards.RemoveAt(0);
        return DrawnCard;
    }

    public Card PeekTopCard()
    {
        if (Cards.Count <= 0)
            return null;

        Card DrawnCard = Cards[0];
        return DrawnCard;
    }

    public Card DrawBottomCard()
    {
        Card DrawnCard = Cards[Cards.Count - 1];
        Cards.RemoveAt(Cards.Count - 1);
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

    public void PlaceCardOnTop(Card card)
    {
        Cards.Insert(0, card);
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
