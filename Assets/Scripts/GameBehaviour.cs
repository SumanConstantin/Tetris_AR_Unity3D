using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameBehaviour : MonoBehaviour {

    private ElementsController elementsController;
    private PlayerModel playerModel;
    private GameStateModel gameStateModel;
    private InputController inputController;
    private CubeController cubeController; 

    [SerializeField]
    private Transform targetParent;

    private bool testingInEditor = false;

    void Awake() {
        if (testingInEditor)
        {
            Init();
        }
        else
        {
            OnAwakeInitListeners();
        }
    }

    private void Init()
    {
        cubeController = new CubeController();
        elementsController = new ElementsController(targetParent, cubeController);
        playerModel = new PlayerModel();
        gameStateModel = new GameStateModel();
        inputController = new InputController();

        InitEventListeners();
        //OnAwakeInitListeners();

        GameEvent.GameInitialized();
    }

    private void Reset()
    {
        if(cubeController != null)
        {
            cubeController.Reset();
            cubeController = null;
        }
        if (elementsController != null)
        {
            elementsController.Reset();
            elementsController = null;
        }
        if (playerModel != null)
        {
            playerModel.Reset();
            playerModel = null;
        }
        if (gameStateModel != null)
        {
            gameStateModel.Reset();
            gameStateModel = null;
        }
        if (gameStateModel != null)
        {
            gameStateModel.Reset();
            gameStateModel = null;
        }

        RemoveEventListeners();
    }

    private void OnAwakeInitListeners()
    {
        GameEvent.onImageTargetInitialized += OnImageTargetInitialized;
    }

    private void OnAwakeRemoveListeners()
    {
        GameEvent.onImageTargetInitialized -= OnImageTargetInitialized;
    }

    private void InitEventListeners()
    {
        GameEvent.onGameStateChanged += OnGameStateChanged;
    }

    private void RemoveEventListeners()
    {
        GameEvent.onGameStateChanged -= OnGameStateChanged;
    }

    private void OnImageTargetInitialized(GameObject imageTarget)
    {
        Debug.Log("OnImageTargetInitialized():" + imageTarget);

        if (imageTarget != null)
        {
            Reset();
            Init();
        }
        else
        {
            Reset();
        }
    }

    private void OnGameStateChanged(string gameState, bool isWin)
    {
        if (gameState == GameStateModel.GAME_STATE_STOPPED)
        {
            if (isWin)
            {
                OnWin();
            }
            else
            {
                OnLose();
            }
        }
    }

    private void OnWin()
    {
        ShowWinMessage();
    }

    private void OnLose()
    {
        ShowLoseMessage();
    }

    private void ShowWinMessage()
    {

    }

    private void ShowLoseMessage()
    {
        Debug.Log("GAME LOST");
    }

    private void OnRestartClick()
    {
        //Reset();
    }

    private void Update()
    {
        if (GameStateModel.GameState == GameStateModel.GAME_STATE_PLAYING)
        {
            inputController.Update();
            elementsController.Update();
        }
    }
	
	private void OnDestroy()
    {
        Debug.Log("GameBehaviour.OnDestroy()");

        Reset();
        OnAwakeRemoveListeners();
    }
}
