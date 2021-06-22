using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    [SerializeField]GameObject[] Levels = new GameObject[4];
    float[,] levelArray;
    int[,] roomGrid;
    Vector3Int startTile;
    Vector3Int currentTile;
    [SerializeField]int roomSize = 10;
    bool resetButton,logButton;
    GameObject parentObject;

    float[,] RandomizeArray()
    {
        float[,] tempArray = new float[10, 10];
        for (int x = 0; x < 10; x++)
        {
            if (x != 9)
            {
                for (int y = 0; y < 10; y++)
                {
                        if (tempArray[x, y] == 0)
                        {
                            float tileFloat = Random.Range(1, 100);
                            if(tileFloat >= 75)
                            {
                                tempArray[x+1, y] = 1000;
                            }
                            tempArray[x, y] = tileFloat;
                        }
                }
            }
            else
            {
                for (int y = 0; y < 10; y++)
                {
                        if (tempArray[x, y] == 0)
                        {
                            float tileFloat = Random.Range(1, 80);
                            tempArray[x, y] = tileFloat;
                        }
                }    
            }
        }
        return tempArray;
    }

    void GenerateLevel(float[,] array)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                currentTile = Vector3Int.up * y * roomSize + Vector3Int.right * x * roomSize + startTile;

                if(array[x, y] < 30)
                {
                    //no room
                }
                else if (array[x,y] < 60)
                {
                    // Room type 1
                    CreateRoom(0, currentTile, parentObject.transform);
                }
                else if(array[x, y] < 80)
                {
                    // Room type 2
                    CreateRoom(1, currentTile, parentObject.transform);
                }
                else if(array[x, y] < 100)
                {
                    // Room type 3 
                    CreateRoom(2, currentTile, parentObject.transform);
                }
                else if(array[x, y] == 1000)
                {
                    // Room type 4 
                    CreateRoom(3, currentTile, parentObject.transform);
                }
            }
        }
    }

    void GenerateConnectors(float[,] array)
    {
        for(int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if(array[x,y] > 30 && array[x + 1, y] > 30 && array[x + 1, y] != 1000)
                {
                        currentTile = Vector3Int.up * y * roomSize + Vector3Int.right * x * roomSize + startTile;
                        Instantiate(Levels[4], currentTile, Quaternion.identity, parentObject.transform);
                }
            }
        }

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (array[x, y] > 30 && array[x, y+1] > 30 && array[x, y+1] != 1000)
                {
                    currentTile = Vector3Int.up * y * roomSize + Vector3Int.right * x * roomSize + startTile;
                    Instantiate(Levels[5], currentTile, Quaternion.identity, parentObject.transform);
                }
            }
        }
    }

    void CreateRoom(int type,Vector3Int position, Transform parent)
    {
        switch(type)
        {
            case 0:
                {
                    Instantiate(Levels[0], position, Quaternion.identity, parent);
                    break;
                }

            case 1:
                {
                    Instantiate(Levels[1], position, Quaternion.identity, parent);
                    break;
                }
            case 2:
                {
                    Instantiate(Levels[2], position, Quaternion.identity, parent);
                    break;
                }
            case 3:
                {
                    Instantiate(Levels[3], position, Quaternion.identity, parent);
                    break;
                }
        }
    }

    void Start()
    {
        levelArray = new float[10, 10];
        roomGrid = new int[10, 10];
        startTile = (Vector3Int.left * 10 * roomSize+ Vector3Int.down * 10 * roomSize) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        resetButton = Input.GetKeyDown("a");
        logButton = Input.GetKeyDown("d");

        if (resetButton)
        {
            Destroy(parentObject);
            parentObject = new GameObject();
            levelArray = RandomizeArray();
            GenerateLevel(levelArray);
            GenerateConnectors(levelArray);
        }


    }
}
