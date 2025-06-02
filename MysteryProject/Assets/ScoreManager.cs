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


