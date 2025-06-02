using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public const float InitialTime = 99.9f;
    public float TimeRemaining = 99.9f;

    public bool paused;

    public UnityEngine.InputSystem.Controls.KeyControl PauseButton = Keyboard.current.pKey;
    public UnityEngine.InputSystem.Controls.KeyControl ResetButton = Keyboard.current.rKey;

    public static GameManager instance;

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
        if (ResetButton.wasPressedThisFrame)
        {
            ResetGame();
        }

        if(PauseButton.wasPressedThisFrame)
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
        paused = true;
    }

    void EndGame()
    {
        Debug.Log("Game ended");
        GameState finalGameState = new GameState();
        finalGameState.TimeRemaining = (int)TimeRemaining;

        CardManager.instance.StashAllPiles();
        finalGameState.StashedPiles = CardManager.instance.StashedPiles;

        int finalScore = ScoreManager.ScoreGame(finalGameState);
        Debug.Log("Final Score: " + finalScore);
        paused = true;
    }
}
