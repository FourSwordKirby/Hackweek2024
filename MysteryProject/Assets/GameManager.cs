using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public const float InitialTime = 30.0f;//99.9f;
    public float TimeRemaining = 30.0f;//99.9f;

    public bool paused;

    public UnityEngine.InputSystem.Controls.KeyControl PauseButton = Keyboard.current?.pKey;
    public UnityEngine.InputSystem.Controls.KeyControl ResetButton = Keyboard.current?.rKey;

    public static GameManager instance;

    public static event System.Action OnEndGame;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            if (ResetButton.wasPressedThisFrame)
            {
                ResetGame();
            }
        }

        if (PauseButton.wasPressedThisFrame)
        {
            paused = !paused;
        }

        if (!paused)
        {
            TimeRemaining -= Time.deltaTime;
        }

        if(TimeRemaining <= 0.0f)
        {
            EndGame();
        }
    }

    void ResetGame()
    {
        Debug.Log("Game reset");
        TimeRemaining = InitialTime;
        CardManager.instance.MainDeck = new CardPile(true, true);
        paused = true;
    }

    void EndGame()
    {
        Debug.Log("Game ended");
        GameState finalGameState = new GameState();
        finalGameState.TimeRemaining = (int)TimeRemaining;

        CardManager.instance.StashAllPiles();
        finalGameState.StashedPiles = CardManager.instance.StashedPiles;

        if(!paused)
        {
            int finalScore = ScoreManager.ScoreGame(finalGameState);
            Debug.Log("Final Score: " + finalScore);
            paused = true;

            OnEndGame?.Invoke();
        }
    }
}
