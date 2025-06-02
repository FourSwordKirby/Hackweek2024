using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] CardManager cardManager;
    [SerializeField] TextMeshProUGUI currentCard;
    [SerializeField] TextMeshProUGUI[] piles;


    void Update()
    {
        piles[0].SetText(cardManager.UserPiles[0].ToString());
        piles[0].color = cardManager.currentSelectedPileIndex == 0 ? Color.yellow : Color.white;
        
        piles[1].SetText(cardManager.UserPiles[1].ToString());
        piles[1].color = cardManager.currentSelectedPileIndex == 1 ? Color.yellow : Color.white;

        piles[2].SetText(cardManager.UserPiles[2].ToString());
        piles[2].color = cardManager.currentSelectedPileIndex == 2 ? Color.yellow : Color.white;

        currentCard.SetText(cardManager.MainDeck.IsEmpty() ? "No cards left" : cardManager.MainDeck.PeekTopCard().ToString());
    }
}
