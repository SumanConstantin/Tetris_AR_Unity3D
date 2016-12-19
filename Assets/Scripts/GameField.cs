using UnityEngine;
using System.Collections;

public class GameField {

    public const int WIDTH = 10;
    public const int HEIGHT = 20;
    public const int EXTRA_HEIGHT = 24;

    private int[] fieldPattern;
    public int[] FieldPattern { get { return fieldPattern; } }
    private GameObject[] cubesInFieldPattern;
    public GameObject[] CubesInFieldPattern { get { return cubesInFieldPattern; } }

    private CubeController cubeController;

    public GameField(CubeController cubeController)
    {
        this.cubeController = cubeController;
        Init();
    }

    private void Init()
    {
        int len = WIDTH * EXTRA_HEIGHT;
        fieldPattern = new int[len];
        cubesInFieldPattern = new GameObject[len];
        for (int i=0; i< len; i++)
        {
            fieldPattern[i] = 0;
        }
    }

    public int GetDropAddDistance(Vector2 elementPosition, int[] elementPattern)
    {
        int result = 0;

        int elementPatternSize = AbstractElementModel.patternSize;
        int initI = elementPatternSize-1;
        int finI = 0;
        int initJ = 0;
        int finJ = elementPatternSize;

        ArrayList lowestElementPoints = new ArrayList();
        int index = 0;

        // Populate lowestElementPoints
        int lowestElementY = 0;     // Used to drop element on bottom of field
        int leftMostLowestElementX = elementPatternSize - 1;
        for (int j = initJ; j < finJ; j++)
        {
            for (int i = initI; i >= finI; i--)
            {
                index = i * finJ + j;
                if (elementPattern[index] != 0)
                {
                    lowestElementPoints.Add(new Vector2(j, i));
                    
                    // Set lowestElementY
                    if (i > lowestElementY)
                    {
                        lowestElementY = i;
                    }

                    // Set leftMostLowestElementX
                    if (j < leftMostLowestElementX)
                    {
                        leftMostLowestElementX = j;
                    }

                    break;
                }
            }
        }

        // Find shortest distance to fieldPattern (not to bottom border)
        initI = HEIGHT - (int)elementPosition.y;// - elementPatternSize;
        finI = HEIGHT;
        initJ = (int)elementPosition.x + leftMostLowestElementX;
        finJ = initJ + lowestElementPoints.Count;
        // Check pattern for contact points only under the element
        int shortestDistance = EXTRA_HEIGHT;
        int fieldIndexI = 0;
        int fieldIndexJ = 0;
        int fieldIndex = 0;
        foreach (Vector2 point in lowestElementPoints)
        {
            fieldIndexJ = (int)elementPosition.x + (int)point.x;
            for (int i = initI; i < finI; i++)
            {

                fieldIndexI = i - elementPatternSize + (int)point.y + 1 + 1;
                fieldIndex = fieldIndexI * WIDTH + fieldIndexJ;

                if (fieldPattern[fieldIndex] != 0)
                {
                    int distance = -(HEIGHT - fieldIndexI - (int)elementPosition.y - elementPatternSize + (int)point.y + 1);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                    }

                    break;
                }
            }
        }

        // Return the shortest distance
        if(shortestDistance < EXTRA_HEIGHT)
        {
            return -shortestDistance;
        }

        // Element should drop on bottom border
        result = -((int)elementPosition.y + (elementPatternSize - 1 - lowestElementY));

        return result;
    }

    public bool IsGameLost()
    {
        for (int j = 0; j < WIDTH; j++)
        {
            if (fieldPattern[j] != 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsIntersecting(Vector2 elementPosition, int[] elementPattern)
    {
        int elementPatternSize = AbstractElementModel.patternSize;
        int initI = 0;
        int finI = elementPatternSize;
        int initJ = 0;
        int finJ = elementPatternSize;

        // Get leftmost/rightmost X of elementPattern
        int leftmostX = elementPatternSize;
        int rightmostX = 0;
        for (int i = initI; i < finI; i++)
        {
            for (int j = initJ; j < finJ; j++)
            {
                int elemPatternIndex = i * finJ + j;
                if(elementPattern[elemPatternIndex] != 0)
                {
                    if (j < leftmostX)
                    {
                        leftmostX = j;
                    }

                    if (j > rightmostX)
                    {
                        rightmostX = j;
                    }
                }
            }
        }

        // Left/Right borders
        if (((int)elementPosition.x + leftmostX) < 0 || ((int)elementPosition.x + rightmostX) > GameField.WIDTH - 1)
        {
            return true;
        }

        for (int i = initI; i < finI; i++)
        {
            for (int j = initJ; j < finJ; j++)
            {
                // Field pattern
                int elementPatternIndex = (i) * elementPatternSize + (j);
                int fieldIndex = (i + (HEIGHT - (int)elementPosition.y) - elementPatternSize) * WIDTH + (j + (int)elementPosition.x);

                if (elementPatternIndex >= 0 && fieldIndex >= 0
                    && elementPattern[elementPatternIndex] != 0
                    && fieldPattern[fieldIndex] != 0
                    )
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void AddPatternToField(Vector2 elementPosition, int[] elementPattern, GameObject[] elementCubesInPattern)
    {
        int elementPatternSize = AbstractElementModel.patternSize;
        int initI = 0;
        int finI = elementPatternSize;
        int initJ = 0;
        int finJ = elementPatternSize;

        for (int i = initI; i < finI; i++)
        {
            for(int j = initJ; j < finJ; j++)
            {
                int elementPatternIndex = (i) * elementPatternSize + (j);
                int fieldIndex = (i + (HEIGHT - (int)elementPosition.y) - elementPatternSize) * WIDTH + (j + (int)elementPosition.x);

                if (elementPatternIndex >= 0 && fieldIndex >= 0 && elementPattern[elementPatternIndex] != 0)
                {
                    this.fieldPattern[fieldIndex] = elementPattern[elementPatternIndex];
                    this.cubesInFieldPattern[fieldIndex] = elementCubesInPattern[elementPatternIndex];
                }
            }
        }

        CheckAndRemoveLinesInField();
    }

    private void CheckAndRemoveLinesInField()
    {
        ArrayList indicesOfRemovedLines = new ArrayList();
        bool doRemoveLine = true;
        int index = 0;
        for(int i = 0; i < HEIGHT; i++)
        {
            doRemoveLine = true;
            for (int j = 0; j < WIDTH; j++)
            {
                index = i * WIDTH + j;
                if(fieldPattern[index] == 0)
                {
                    doRemoveLine = false;
                    break;
                }
            }

            if(doRemoveLine)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    index = i * WIDTH + j;
                    cubeController.ReturnCubeToPool(cubesInFieldPattern[index]);
                    fieldPattern[index] = 0;
                }

                indicesOfRemovedLines.Add(i);
            }
        }

        if(indicesOfRemovedLines.Count > 0)
        {
            CompressFieldPattern(indicesOfRemovedLines);
        }
    }

    private void CompressFieldPattern(ArrayList indicesOfRemovedLines)
    {
        int len = indicesOfRemovedLines.Count;

        for( int idx = 0; idx < len; idx++)
        {
            int removedLineIndex = (int)indicesOfRemovedLines[idx];
            // Move fieldPattern one line down
            // Each line one by one from removedLineIndex to top
            for (int i = removedLineIndex; i >= 0; i--)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    int targetIndex = i * WIDTH + j;
                    int srcIndex = (i - 1) * WIDTH + j;
                    if (i == 0)
                    {
                        // Upper-most line
                        fieldPattern[targetIndex] = 0;
                        cubesInFieldPattern[targetIndex] = null;
                    }
                    else
                    {
                        fieldPattern[targetIndex] = fieldPattern[srcIndex];
                        cubesInFieldPattern[targetIndex] = cubesInFieldPattern[srcIndex];

                        GameObject cube = cubesInFieldPattern[srcIndex];

                        if (cube != null)
                        {
                            cube.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y - AbstractElementModel.verticalMoveDistance);
                        }
                    }
                }
            }
        }
    }

    public void LogField()
    {
        string allLines = "";
        for (int i = 0; i < HEIGHT; i++)
        {
            string line = "";
            for (int j = 0; j < WIDTH; j++)
            {
                line += (fieldPattern[i * WIDTH + j]).ToString();
            }
            allLines += line + "\n";
        }

        Debug.Log(allLines);
    }

    public void LogCubesInField()
    {
        string allLines = "";
        for (int i = 0; i < HEIGHT; i++)
        {
            string line = "";
            for (int j = 0; j < WIDTH; j++)
            {
                line += cubesInFieldPattern[i * WIDTH + j] == null ? "0" : "1";
            }
            allLines += line + "\n";
        }

        Debug.Log(allLines);
    }

    public void Destroy()
    {

    }
}
