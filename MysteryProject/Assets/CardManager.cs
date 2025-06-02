using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


public class CardManager : MonoBehaviour
{
    public CardPile MainDeck;

    public CardPile pile1;
    public CardPile pile2;
    public CardPile pile3;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Pile 1: " + pile1.PeekTopCard().ToString() + ": \n" +
                    "Pile 2: " + pile1.PeekTopCard().ToString() + ": \n" +
                    "Pile 3: " + pile1.PeekTopCard().ToString());
        }
    }
}

[Serializable]
public class CardPile
{
    public List<Card> Cards;
    
    public int ScorePile()
    {
        return Cards.Count * (Cards.Count + 1) / 2;
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
    public string officialName => suit + " " + numberValue;
    public int numberValue; // <0 thru 9>
    public CardSuit suit;

    public string ToString()
    {
        return officialName;
    }

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
