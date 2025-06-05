using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] CardManager cardManager;
    [SerializeField] TextMeshProUGUI currentCard;
    [SerializeField] TextMeshProUGUI[] piles;

    [SerializeField] TextMeshProUGUI timeRemaining;

    public void OnGameEnded(GameState finalGameState)
    {
        timeRemaining.SetText($"Game Over!");
        //timeRemaining.SetText($"Game Over! \nScore: {ScoreManager.ScoreGame(finalGameState)} \nPress R to reset");
    }

    void Update()
    {
        if (GameManager.instance.CurrentState == GameManager.GameManagerState.NotStarted)
        {
            timeRemaining.SetText($"Press <b>{GameManager.instance.PauseButton.bindings[0].ToDisplayString()}</b> to start!");
            return;
        }
        else if (GameManager.instance.CurrentState == GameManager.GameManagerState.Paused)
        {
            timeRemaining.SetText($"Paused!\n"
                                + $"Time Remaining: {GameManager.instance.TimeRemaining:00.00}\n"
                                + $"Press <b>{GameManager.instance.PauseButton.bindings[0].ToDisplayString()}</b> to resume.\n"
                                + $"Press <b>{GameManager.instance.ResetButton.bindings[0].ToDisplayString()}</b> to reset.");
        }
        else if (GameManager.instance.CurrentState == GameManager.GameManagerState.Running)
        {
            timeRemaining.SetText(GameManager.instance.TimeRemaining.ToString("00.00"));
        }

        piles[0].SetText($"{cardManager.UserBins[0].criteria.CriteriaDescriptor()}\n{cardManager.UserBins[0].pile}");
        piles[0].color = cardManager.currentSelectedPileIndex == 0 ? Color.yellow : Color.white;

        piles[1].SetText($"{cardManager.UserBins[1].criteria.CriteriaDescriptor()}\n{cardManager.UserBins[1].pile}");
        piles[1].color = cardManager.currentSelectedPileIndex == 1 ? Color.yellow : Color.white;

        piles[2].SetText($"{cardManager.UserBins[2].criteria.CriteriaDescriptor()}\n{cardManager.UserBins[2].pile}");
        piles[2].color = cardManager.currentSelectedPileIndex == 2 ? Color.yellow : Color.white;

        currentCard.SetText(cardManager.MainDeck.IsEmpty() ? "No cards left" : $"<color={cardManager.MainDeck.PeekTopCard().suit}>{cardManager.MainDeck.PeekTopCard()}</color>");
    }
}
