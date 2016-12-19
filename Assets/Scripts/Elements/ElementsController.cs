using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementsController {    
    private GameObject[] elementPrefabs;    // TODO: init dynamically; add challenge mode: limited set of element types
    private List<GameObject> staticCubes;   // Cubes that are static, participating in line formation
    private AbstractElementModel currentElement;
    private Transform targetParent;

    private GameField field;
    private CubeController cubeController;

    public ElementsController(Transform targetParent, CubeController cubeController)
    {
        this.targetParent = targetParent;
        this.cubeController = cubeController;
        Init();
    }

    // Use this for initialization
    void Init ()
    {
        InitEventListeners();
        field = new GameField(cubeController);
        InitElementPrefabs();
        CreateNewElement();
    }

    public void Reset()
    {
        if (elementPrefabs != null)
        {
            foreach (GameObject go in elementPrefabs)
            {
                GameObject.Destroy(go);
            }
        }

        if (staticCubes != null)
        {
            foreach (GameObject go in staticCubes)
            {
                GameObject.Destroy(go);
            }
        }

        if (currentElement != null)
        {
            currentElement.Destroy();
        }

        RemoveEventListeners();
    }

    private void InitEventListeners()
    {
        GameEvent.onElementTouchedField += OnElementTouchedField;
    }

    private void RemoveEventListeners()
    {
        GameEvent.onElementTouchedField -= OnElementTouchedField;
    }

    private void OnElementTouchedField(AbstractElementModel element, bool lose = false)
    {
        if (!lose)
        {
            CreateNewElement();
        }
    }

    private void InitElementPrefabs()
    {
        elementPrefabs = Resources.LoadAll<GameObject>("GameElements");
    }

    private void CreateNewElement()
    {
        if (currentElement != null)
        {
            currentElement.Destroy();
        }

        // Get a random element pattern;
        int[][] elementPatterns = ElementPatterns.patterns[Random.Range(0, ElementPatterns.patterns.Length)];

        // Get element rotation index or patternIndex
        int patternIndex = Random.Range(0, 3);

        currentElement = new AbstractElementModel(targetParent, field, elementPatterns, patternIndex, cubeController);
    }

    // Update is called once per frame
    public void Update ()
    {
	    if(GameStateModel.GameState == GameStateModel.GAME_STATE_PLAYING)
        {
            if (currentElement != null)
            {
                currentElement.Update();
            }
        }
	}

    public void Destroy()
    {
        Reset();
    }
}
