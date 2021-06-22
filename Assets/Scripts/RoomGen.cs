using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGen : MonoBehaviour
{

    GameObject gridObj;
    Grid grid;
    Tilemap tilemap;
    [SerializeField] int sizeX, sizeY;
    Vector3Int startTile;
    Vector3Int currentTile;
    [SerializeField] Tile baseTile;
    [SerializeField] Tile randomTile;
    [SerializeField] float randomTileChance;

    float[,] tileArray;

    bool resetButton;
    
    float[,] RandomizeArray()
    {
        float[,] tempArray = new float[sizeX, sizeY];
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                float tileFloat = Random.Range(0f, 100f);
                Debug.Log(x + " " + y + "= " + tileFloat);
                tempArray[x, y] = tileFloat;
            }
        }
        return tempArray;
    }

    void SetTilesToArray(float[,] array)
    {
        Tile tileToSet;
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                currentTile = Vector3Int.up * y + Vector3Int.right * x + startTile;
                if(array[x,y] < 90)
                {
                            tileToSet = baseTile;
                }
                else
                { 
                            tileToSet = randomTile;
                }
                tilemap.SetTile(currentTile,tileToSet);
            }
        }
    }

    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("TileGrid").GetComponent<Grid>();
        tilemap = GameObject.FindGameObjectWithTag("BaseTile").GetComponent<Tilemap>();
        float[,] tileArray = new float[sizeX, sizeY];
        startTile = (Vector3Int.left * sizeX + Vector3Int.down * sizeY)/2;
    }

    // Update is called once per frame
    void Update()
    {
        resetButton = Input.GetKeyDown("a");

        if(resetButton)
        {
            tileArray = RandomizeArray();
            SetTilesToArray(tileArray);
        }
    }
}
