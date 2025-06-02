using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] CardManager cardManager;
    [SerializeField] TextMeshProUGUI currentCard;
    [SerializeField] TextMeshProUGUI[] piles;


    void Update()
    {
        piles[0].SetText($"{cardManager.UserBins[0].criteria}\n{cardManager.UserBins[0].pile}");
        piles[0].color = cardManager.currentSelectedPileIndex == 0 ? Color.yellow : Color.white;

        piles[1].SetText($"{cardManager.UserBins[1].criteria}\n{cardManager.UserBins[1].pile}");
        piles[1].color = cardManager.currentSelectedPileIndex == 1 ? Color.yellow : Color.white;

        piles[2].SetText($"{cardManager.UserBins[2].criteria}\n{cardManager.UserBins[2].pile}");
        piles[2].color = cardManager.currentSelectedPileIndex == 2 ? Color.yellow : Color.white;

        currentCard.SetText(cardManager.MainDeck.IsEmpty() ? "No cards left" : $"<color={cardManager.MainDeck.PeekTopCard().suit}>{cardManager.MainDeck.PeekTopCard()}</color>");
    }
}
