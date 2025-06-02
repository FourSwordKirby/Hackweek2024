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
        return ScoreAllPiles(gameState.Piles) + gameState.TimeRemaining;
    }

    public int ScoreAllPiles(List<CardPile> pileList)
    {
        int total = 0;
        foreach (CardPile pile in pileList)
        {
            total += ScorePile(pile);
        }
        
        return total;
    }
    
    public int ScorePile(CardPile pile)
    {
        return pile.Cards.Count * (pile.Cards.Count + 1) / 2;
    }
    
}

public class GameState
{
    public List<CardPile> Piles;
    public int TimeRemaining;
}


