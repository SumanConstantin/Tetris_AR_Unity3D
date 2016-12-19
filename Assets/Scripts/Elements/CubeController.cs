using UnityEngine;
using System.Collections;

public class CubeController {
    private ArrayList cubePool;
    private ArrayList allCubes;

    private GameObject cubePrefab;
    private Transform targetParent;

    public CubeController()
    {
        Init();
    }

    private void Init()
    {
        cubePool = new ArrayList();
        allCubes = new ArrayList();
    }

    public void Reset()
    {
        if (allCubes != null)
        {
            foreach( GameObject cube in allCubes)
            {
                GameObject.Destroy(cube);
            }
            allCubes = null;
        }
        if (cubePool != null)
        {
            cubePool = null;
        }
        if (cubePrefab != null)
        {
            //GameObject.Destroy(cubePrefab);
            cubePrefab = null;
        }
        if (targetParent != null)
        {
            targetParent = null;
        }
    }

    public GameObject GetCube(Transform targetParent)
    {
        // TODO: later on, when there will be different prefabs,
        //add different pools for different prefabs. Place the pools in a dictionary.

        this.targetParent = targetParent;
        
        if (cubePrefab == null)
        {
            cubePrefab = Resources.Load<GameObject>("Prefabs/GameElements/Cube");
        }

        if(cubePool.Count > 0)
        {
            return GetCubeFromPool();
        }
        else
        {
            return GetCubeNew();
        }
    }

    // Pop cube from arrayList
    private GameObject GetCubeFromPool()
    {
        GameObject result;

        int index = cubePool.Count - 1;
        result = (GameObject)cubePool[index];
        cubePool.RemoveAt(index);
        result.SetActive(true);

        return result;
    }

    // Create new cube object
    private GameObject GetCubeNew()
    {
        //Debug.Log("GetCubeNew()");
        GameObject result;

        result = MonoBehaviour.Instantiate(cubePrefab, targetParent) as GameObject;
        allCubes.Add(result);

        return result;
    }

    public void ReturnCubeToPool(GameObject cube)
    {
        cubePool.Add(cube);
        cube.SetActive(false);
    }

    public void Destroy()
    {
        Reset();
    }
}
