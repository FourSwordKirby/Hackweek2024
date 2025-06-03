using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public const float InitialTime = 30.0f;//99.9f;
    public float TimeRemaining = 30.0f;//99.9f;

    public enum GameManagerState { NotStarted, Running, Paused, Ended }
    public GameManagerState CurrentState = GameManagerState.NotStarted;
    GameManagerState previousState = GameManagerState.NotStarted;

    public UnityEngine.InputSystem.Controls.KeyControl PauseButton = Keyboard.current?.pKey;
    public UnityEngine.InputSystem.Controls.KeyControl ResetButton = Keyboard.current?.rKey;

    public UnityEngine.InputSystem.Controls.KeyControl previousPile = Keyboard.current?.leftArrowKey;
    public UnityEngine.InputSystem.Controls.KeyControl nextPile = Keyboard.current?.rightArrowKey;

    public UnityEngine.InputSystem.Controls.KeyControl stashAndCreateNewUserPile = Keyboard.current?.enterKey;
    public UnityEngine.InputSystem.Controls.KeyControl addCardToCurrentPile = Keyboard.current?.spaceKey;

    public UnityEvent OnGameStarted = new UnityEvent();
    public UnityEvent OnGamePaused = new UnityEvent();
    public UnityEvent OnGameResumed = new UnityEvent();
    public UnityEvent OnGameReset = new UnityEvent();
    public UnityEvent<GameState> OnGameEnded = new UnityEvent<GameState>();

    public UnityEvent<GameManagerState> OnGameStateChanged = new UnityEvent<GameManagerState>();

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
        if (CurrentState != previousState)
        {
            OnGameStateChanged?.Invoke(CurrentState);
            previousState = CurrentState;
        }

        switch (CurrentState)
        {
            case GameManagerState.NotStarted:
                if (PauseButton.wasPressedThisFrame)
                {
                    CurrentState = GameManagerState.Running;
                    OnGameStarted?.Invoke();
                }
                return;
            case GameManagerState.Running:
                if (PauseButton.wasPressedThisFrame)
                {
                    CurrentState = GameManagerState.Paused;
                    OnGamePaused?.Invoke();
                }
                break;
            case GameManagerState.Paused:
                if (PauseButton.wasPressedThisFrame)
                {
                    CurrentState = GameManagerState.Running;
                    OnGameResumed?.Invoke();
                }
                else if (ResetButton.wasPressedThisFrame)
                {
                    ResetGame();
                }
                return;
            case GameManagerState.Ended:
                if (ResetButton.wasPressedThisFrame)
                    ResetGame();
                return;
        }

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0.0f || CardManager.instance.MainDeck.IsEmpty())
        {
            EndGame();
            return;
        }

        if (previousPile.wasPressedThisFrame)
            CardManager.instance.SelectPreviousPile();
        else if (nextPile.wasPressedThisFrame)
            CardManager.instance.SelectNextPile();
        else if (addCardToCurrentPile.wasPressedThisFrame)
            CardManager.instance.TryAddCardToCurrentPile();
        else if (stashAndCreateNewUserPile.wasPressedThisFrame)
            CardManager.instance.StashAndCreateNewUserPile();
    }

    void ResetGame()
    {
        Debug.Log("Game reset");
        TimeRemaining = InitialTime;
        CardManager.instance.MainDeck = new CardPile(true, true);
        CurrentState = GameManagerState.NotStarted;

        OnGameReset?.Invoke();
    }

    void EndGame()
    {
        if (CurrentState == GameManagerState.Ended)
            return;

        Debug.Log("Game ended");
        GameState finalGameState = new GameState();
        finalGameState.TimeRemaining = (int)TimeRemaining;

        CardManager.instance.StashAllPiles();
        finalGameState.StashedPiles = CardManager.instance.StashedPiles;

        CurrentState = GameManagerState.Ended;

        OnGameEnded?.Invoke(finalGameState);
    }
}
