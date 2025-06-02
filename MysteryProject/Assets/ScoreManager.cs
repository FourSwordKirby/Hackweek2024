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

    public int ScoreGame(List<CardPile> pileList)
    {
        int total = 0;
        foreach (CardPile pile in pileList)
        {
            total += pile.ScorePile();
        }
        
        return total;
    }
    
}

// This should definitely live somewhere else (like a GameManager), but 
// we'll define how it should hook in for now (to get a rough idea)
public class CardPile
{
    public List<Card> Cards;
    
    public virtual int ScorePile()
    {
        return Cards.Count * (Cards.Count + 1) / 2;
    }
}

public class IncrementWithDuplicatesPile : CardPile
{
}

public class DecrementWithDuplicatesPile : CardPile
{
}


