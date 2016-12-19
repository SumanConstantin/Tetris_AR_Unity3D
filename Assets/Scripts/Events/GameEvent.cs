using UnityEngine;
using System.Collections;

public class GameEvent : MonoBehaviour {
	
	// Bomb Event
	public delegate void ElementDelegate (AbstractElementModel element, bool lose = false);
	public static event ElementDelegate onElementTouchedField;

	public static void FieldTouched(AbstractElementModel element, bool lose = false)
	{
		if(onElementTouchedField != null)
		{
            onElementTouchedField(element, lose);
		}
	}

	// Player Event
	public delegate void PlayerMadeInputDelegate ();
	public static event PlayerMadeInputDelegate onPlayerMadeInput;

	public static void PlayerMadeInput()
	{
		if(onPlayerMadeInput != null)
		{
			onPlayerMadeInput();
		}
	}

	// Game State Event
	public delegate void GameStateChangeDelegate ( string gameState, bool isWin = false );
	public static event GameStateChangeDelegate onGameStateChanged;

	public static void GameStateChanged( string gameState, bool isWin = false)
	{
		if(onGameStateChanged != null)
		{
			onGameStateChanged( gameState, isWin );
		}
	}

	// Game Initialized Event
	public delegate void GameInitializedDelegate ();
	public static event GameInitializedDelegate onGameInitialized;

	public static void GameInitialized()
	{
		if(onGameInitialized != null)
		{
			onGameInitialized();
		}
    }

    // Input Event
    public delegate void HorizontalMoveDelegate(int x);
    public static event HorizontalMoveDelegate onHorizontalMove;

    public static void HorizontalMove(int x)
    {
        if (onHorizontalMove != null)
        {
            onHorizontalMove(x);
        }
    }

    public delegate void RotateElementDelegate(int rotationDirection);
    public static event RotateElementDelegate onRotateElement;

    public static void RotateElement(int rotationDirection)
    {
        if (onRotateElement != null)
        {
            onRotateElement(rotationDirection);
        }
    }

    public delegate void DropElementDelegate();
    public static event DropElementDelegate onDropElement;

    public static void DropElement()
    {
        if (onDropElement != null)
        {
            onDropElement();
        }
    }

    // Image Target Initialized Event
    public delegate void ImageTargetInitializedDelegate(GameObject imageTarget);
    public static event ImageTargetInitializedDelegate onImageTargetInitialized;

    public static void ImageTargetInitialized(GameObject imageTarget)
    {
        if (onImageTargetInitialized != null)
        {
            onImageTargetInitialized(imageTarget);
        }
    }
}
