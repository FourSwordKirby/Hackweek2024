using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


public class CardManager : MonoBehaviour
{
    public Deck deck;

}

[Serializable]
public class Deck
{
    public List<Card> Cards;

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

    public Card MulliganTopCard()
    {
        Card DrawnCard = Cards[0];
        Cards.RemoveAt(0);
        Cards.Add(DrawnCard);
        return DrawnCard;
    }
}

public class Card : IComparable<Card>
{
    public string officialName;
    public int numberValue;
    public CardSuit suit;

    public int CompareTo(Card other)
    {
        return numberValue - other.numberValue;
    }
}

public enum CardSuit
{
    Red,
    Blue,
    Green,
    Yellow
}
