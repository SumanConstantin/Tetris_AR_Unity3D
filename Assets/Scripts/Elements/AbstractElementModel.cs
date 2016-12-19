using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbstractElementModel {

    private const int framesPerMove = 12;
    private int framesPerMoveCount = 0;
    public const int verticalMoveDistance = 1;
    private Vector2 centerPosition = new Vector2();
    
    private List<GameObject> cubes;
    private float positionShift = 0.5f;

    private ActionQueue actionQueue;
    private AbstractAction currentAction;

    private List<Vector2> points;
    public List<Vector2> Points
    {
        get { return points; }
    }

    private int[] pattern;
    private GameObject[] cubesInPattern;
    private int[][] patterns;

    // Index of the current pattern array from the patterns array
    // Represents the rotation of the element
    private int patternIndex = 0;

    private int PatternIndex
    {
        get { return patternIndex; }
        set { patternIndex = value >= patterns.Length ? value - patterns.Length : value < 0 ? patterns.Length - 1 : value ; }
    }

    public const int patternSize = 4;

    private Vector3 spawnPoint;
    private Vector3 currentPoint;

    private Transform targetParent;
    private GameField field;
    private CubeController cubeController;

    public AbstractElementModel(Transform targetParent, GameField field, int[][] patterns, int patternIndex, CubeController cubeController)
    {
        this.targetParent = targetParent;
        this.field = field;
        this.patterns = patterns;
        PatternIndex = patternIndex;
        this.cubeController = cubeController;

        Init();
    }

    private void Init()
    {
        spawnPoint = new Vector3((int)((GameField.WIDTH - patternSize)/2), GameField.HEIGHT, 0);
        actionQueue = new ActionQueue();

        InitCubes();
        InitListeners();
    }

    private void InitCubes()
    {
        pattern = patterns[PatternIndex];

        points = new List<Vector2>();
        cubesInPattern = new GameObject[patternSize*patternSize];

        if (cubes == null)
        {
            cubes = new List<GameObject>();
        }

        currentPoint = new Vector3(spawnPoint.x, spawnPoint.y);

        for (int i = 0; i < patternSize; i++)
        {
            for (int j = 0; j < patternSize; j++)
            {
                int index = i * patternSize + j;

                if (pattern[index] != 0)
                {
                    GameObject cube = cubeController.GetCube(targetParent);
                    cube.transform.position = new Vector3(spawnPoint.x + j + positionShift, spawnPoint.y + (patternSize - 1 - i) + positionShift);
                    cubes.Add(cube);

                    points.Add(new Vector2(spawnPoint.x + j, spawnPoint.y + (patternSize - 1 - i)));

                    cubesInPattern[index] = cube;
                }
                else
                {
                    cubesInPattern[index] = null;
                }
            }
        }
    }

    private void ReInitCubesWithCurrentPosition()
    {
        pattern = patterns[PatternIndex];

        points = new List<Vector2>();
        cubesInPattern = new GameObject[patternSize * patternSize];

        int cubeIndex = 0;

        for (int i = 0; i < patternSize; i++)
        {
            for (int j = 0; j < patternSize; j++)
            {
                int index = i * patternSize + j;

                if (pattern[index] != 0)
                {
                    cubes[cubeIndex].transform.position = new Vector3(currentPoint.x + j + positionShift, currentPoint.y + (patternSize - 1 - i) + positionShift);
                    points.Add(new Vector2(currentPoint.x + j, currentPoint.y + (patternSize - 1 - i)));
                    cubesInPattern[index] = cubes[cubeIndex];
                    cubeIndex++;
                }
                else
                {
                    cubesInPattern[index] = null;
                }
            }
        }
    }

    private void InitListeners()
    {
        GameEvent.onHorizontalMove += OnHorizontalMove;
        GameEvent.onRotateElement += OnRotateElement;
        GameEvent.onDropElement += OnDropElement;
    }

    private void RemoveListeners()
    {
        GameEvent.onHorizontalMove -= OnHorizontalMove;
        GameEvent.onRotateElement -= OnRotateElement;
        GameEvent.onDropElement -= OnDropElement;
    }

    private void OnHorizontalMove(int x)
    {
        actionQueue.Add(new MoveAction(new Vector2(x, 0)));
    }

    private void OnRotateElement(int rotationDirection)
    {
        actionQueue.Add(new RotateAction(rotationDirection));
    }

    private void OnDropElement()
    {
        Vector2 addVector = new Vector2( 0, field.GetDropAddDistance(currentPoint, pattern));
        actionQueue.Add(new DropAction(addVector));
    }

    private void Drop(Vector2 addVector)
    {
        Move((currentAction as DropAction).AddVector);

        //Debug.Log("Drop().OnTouchField()");
        OnTouchField();
    }

    private bool CanMoveHorizontally(int addX)
    {
        foreach (Vector2 point in points)
        {
            int i = (GameField.HEIGHT - 1) - (int)point.y;
            int j = (int)point.x;
            int index = (i) * GameField.WIDTH + (j + addX);

            // Field
            if (index < field.FieldPattern.Length && index >= 0 && field.FieldPattern[index] != 0)
            {
                return false;
            }

            // Left/Right borders
            if(j+addX < 0 || j+addX > GameField.WIDTH-1)
            {
                return false;
            }
        }

        return true;
    }

    private bool CanMoveVertically(int addY)
    {
        foreach (Vector2 point in points)
        {
            int i = (GameField.HEIGHT - 1) - (int)point.y;
            int j = (int)point.x;
            int index = (i - addY) * GameField.WIDTH + j;

            // Field
            if (i == GameField.HEIGHT - 1 ||
                (index < field.FieldPattern.Length && index >= 0 && field.FieldPattern[index] != 0))
            {
                OnTouchField();
                return false;
            }
        }

        return true;
    }

    private void CheckTouchField()
    {
        foreach (Vector2 point in points)
        {
            int i = (GameField.HEIGHT - 1) - (int)point.y;
            int j = (int)point.x;
            int index = i * GameField.WIDTH + j;

            // Field
            if ( i == GameField.HEIGHT - 1 ||
                (index < field.FieldPattern.Length && index >= 0 && field.FieldPattern[index] != 0))
            {
                OnTouchField();
                return;
            }
        }
    }

    private void OnTouchField()
    {
        field.AddPatternToField(currentPoint, pattern, cubesInPattern);
        actionQueue.Clear();
        GameEvent.FieldTouched(this, field.IsGameLost());
    }

    private void Move(Vector3 moveValue)
    {
        // If can't move horizontally -> return
        if (moveValue.x != 0)
        {
            if (!CanMoveHorizontally((int)moveValue.x))
            {
                return;
            }
        }

        // If can't move vertically -> return
        if (moveValue.y != 0)
        {
            if (!CanMoveVertically((int)moveValue.y))
            {
                return;
            }
        }

        // Move
        foreach (GameObject cube in cubes)
        {
            cube.transform.position = new Vector3(cube.transform.position.x + moveValue.x, cube.transform.position.y + moveValue.y);
        }

        for (int i = 0; i < points.Count; i++)// Vector2 point in points)
        {
            points[i] = new Vector2(points[i].x + moveValue.x, points[i].y + moveValue.y);
        }

        currentPoint.x += moveValue.x;
        currentPoint.y += moveValue.y;
    }

    private void UpdateVerticalMove()
    { 
        framesPerMoveCount++;
        if (framesPerMoveCount == framesPerMove)
        {
            if(CanMoveVertically(-verticalMoveDistance))
            {
                actionQueue.Add(new MoveAction(new Vector3(0, -verticalMoveDistance)));
            }

            framesPerMoveCount = 0;
        }
    }

    private void UpdateActions()
    {
        if(actionQueue.Count > 0)
        {
            currentAction = actionQueue.PopFirst();
        }

        if(currentAction != null)
        {
            if(currentAction.Type == ActionType.MOVE)
            {
                Move((currentAction as MoveAction).AddVector);

                // The move is instant, so currentAction is not needed any more
                currentAction = null;
            }
            else
            if (currentAction.Type == ActionType.ROTATE)
            {
                Rotate((currentAction as RotateAction).RotateValue);

                // The move is instant, so currentAction is not needed any more
                currentAction = null;
            }
            else
            if (currentAction.Type == ActionType.DROP)
            {
                Drop((currentAction as DropAction).AddVector);

                // The move is instant, so currentAction is not needed any more
                currentAction = null;
            }

            //CheckTouchField();
        }
    }

    private void Rotate(int rotationDirection)
    {
        if (CanRotate(rotationDirection))
        {
            PatternIndex += rotationDirection;
            ReInitCubesWithCurrentPosition();
        }
    }

    private bool CanRotate(int rotationDirection)
    {
        int value = PatternIndex + rotationDirection;
        int rotatedPatternIndex = value >= patterns.Length ? value - patterns.Length : value < 0 ? patterns.Length - 1 : value;

        int[] rotatedPattern = patterns[rotatedPatternIndex];

        if (field.IsIntersecting(currentPoint, rotatedPattern))
        {
            return false;
        }

        return true;
    }

    public void Update()
    {
        UpdateVerticalMove();
        UpdateActions();
    }

    public void Destroy()
    {
        RemoveListeners();
        cubes = null;
    }

}
