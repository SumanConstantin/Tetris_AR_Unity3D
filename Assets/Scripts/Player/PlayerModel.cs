using UnityEngine;
using System.Collections;

public class PlayerModel {

	private const int playerInputLimit = 1;		// Number of moves per level
	private static int playerInputRemainCount = playerInputLimit;

	public static bool PlayerMayMakeInput
	{
		get { return playerInputRemainCount > 0; }
	}

	public PlayerModel()
	{
		Init();
    }

    public void Reset()
    {
        RemoveEventListeners();
    }

    private void Init()
	{
		playerInputRemainCount = playerInputLimit;
		InitEventListeners();
	}

	private void InitEventListeners()
	{
		GameEvent.onPlayerMadeInput += onPlayerMadeAnInput;
	}

	private void RemoveEventListeners()
	{
		GameEvent.onPlayerMadeInput -= onPlayerMadeAnInput;
	}

	private void onPlayerMadeAnInput()
	{
		playerInputRemainCount--;
	}

	public void Destroy()
	{
        Reset();
    }
}
