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

    public InputAction PauseButton;
    public InputAction ResetButton;// = Keyboard.current?.rKey;

    public InputAction previousPile;// = Keyboard.current?.leftArrowKey;
    public InputAction nextPile;// = Keyboard.current?.rightArrowKey;

    public InputAction stashAndCreateNewUserPile;// = Keyboard.current?.enterKey;
    public InputAction addCardToCurrentPile;// = Keyboard.current?.spaceKey;

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

        PauseButton.Enable();
        ResetButton.Enable();
        previousPile.Enable();
        nextPile.Enable();
        stashAndCreateNewUserPile.Enable();
        addCardToCurrentPile.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentState != previousState)
        {
            OnGameStateChanged?.Invoke(CurrentState);
            previousState = CurrentState;
        }

        if (Keyboard.current == null)
            return;

        switch (CurrentState)
        {
            case GameManagerState.NotStarted:
                if (PauseButton.WasPerformedThisFrame())
                {
                    CurrentState = GameManagerState.Running;
                    OnGameStarted?.Invoke();
                }
                return;
            case GameManagerState.Running:
                if (PauseButton.WasPerformedThisFrame())
                {
                    CurrentState = GameManagerState.Paused;
                    OnGamePaused?.Invoke();
                }
                break;
            case GameManagerState.Paused:
                if (PauseButton.WasPerformedThisFrame())
                {
                    CurrentState = GameManagerState.Running;
                    OnGameResumed?.Invoke();
                }
                else if (ResetButton.WasPerformedThisFrame())
                {
                    ResetGame();
                }
                return;
            case GameManagerState.Ended:
                if (ResetButton.WasPerformedThisFrame())
                    ResetGame();
                return;
        }

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0.0f || CardManager.instance.MainDeck.IsEmpty())
        {
            EndGame();
            return;
        }

        if (previousPile.WasPerformedThisFrame())
            CardManager.instance.SelectPreviousPile();
        else if (nextPile.WasPerformedThisFrame())
            CardManager.instance.SelectNextPile();
        else if (addCardToCurrentPile.WasPerformedThisFrame())
            CardManager.instance.TryAddCardToCurrentPile();
        else if (stashAndCreateNewUserPile.WasPerformedThisFrame())
            CardManager.instance.StashAndCreateNewUserPile();
    }

    void ResetGame()
    {
        Debug.Log("Game reset");
        TimeRemaining = InitialTime;
        CardManager.instance.ResetCards();
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
