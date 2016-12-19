using UnityEngine;
using System.Collections;

public class GameStateModel {
	public const string GAME_STATE_INIT = "gameStateInit";
	public const string GAME_STATE_PLAYING = "gameStatePlaying";
	public const string GAME_STATE_STOPPED = "gameStateStopped";

    private static string gameState = GAME_STATE_INIT;
	public static string GameState
	{
		get { return gameState; }
	}

	private int playerCharCount = 1;

	public GameStateModel()
	{
		Init();
	}

	private void Init()
	{
		InitEventListeners();
    }

    public void Reset()
    {
        RemoveEventListeners();
        gameState = GAME_STATE_INIT;
    }

    private void InitEventListeners()
	{
		GameEvent.onGameInitialized += OnGameInitialized;
        GameEvent.onElementTouchedField += OnElementTouchedField;
    }

	private void RemoveEventListeners()
	{
		GameEvent.onGameInitialized -= OnGameInitialized;
        GameEvent.onElementTouchedField -= OnElementTouchedField;
    }

    private void OnElementTouchedField(AbstractElementModel element, bool isGameLost = false)
    {
        if (isGameLost)
        {
            gameState = GAME_STATE_STOPPED;
            GameEvent.GameStateChanged(gameState, false);
        }
    }

	private void OnGameInitialized()
	{
		gameState = GAME_STATE_PLAYING;
	}

	public void Destroy()
	{
        Reset();
	}
}
