using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGen : MonoBehaviour
{

    [SerializeField] GameObject[] Levels = new GameObject[3];
    float[,] roomType;
    [SerializeField] int sizeX, sizeY;
    [SerializeField] int roomAmount;
    [SerializeField] int startingRoomX, startingRoomY;
    [SerializeField] int currentRoomX, currentRoomY;
    int roomSize = 10;
    bool[,] isDeadEnd;
    int connectedRooms;
    Vector3Int nextRoomDir;
    GameObject levelParent;
    int dirInt;

    bool resetButton;

    void Start()
    {
        roomType = new float[sizeX, sizeY];
        isDeadEnd = new bool[sizeX, sizeY];
        currentRoomX = startingRoomX;
        currentRoomY = startingRoomY;
    }

    int CheckConnectedAmount(int posX, int posY)
    {
        int connectors = 0;

        if (posX > 0)
        {
            if (roomType[posX - 1, posY] > 0) connectors++;
        }

        if (posX < sizeX)
        {
            if (roomType[posX + 1, posY] > 0) connectors++;
        }

        if (posY > 0)
        {
            if (roomType[posX, posY - 1] > 0) connectors++;
        }

        if (posY < sizeY)
        {
            if (roomType[posX, posY + 1] > 0) connectors++;
        }

        return connectors;
    }

    Vector3Int GetRandomDir(int posX, int posY)
    {
        Vector3Int dir = Vector3Int.zero;
        bool done = true;
        while (done)
        {
            dirInt = Random.Range(0, 4);
            switch (dirInt)
            {

                //left
                case 0:
                    if (posX == 0) break;
                    if (roomType[posX - 1, posY] > 0) break;
                    else dir = Vector3Int.left;
                    done = false;
                    break;


                //right
                case 1:
                    if (posX == sizeX - 1) break;
                    if (roomType[posX + 1, posY] > 0) break;
                    else dir = Vector3Int.right;
                    done = false;
                    break;

                //top
                case 2:
                    if (posY == sizeY - 1) break;
                    if (roomType[posX, posY + 1] > 0) break;
                    else dir = Vector3Int.up;
                    done = false;
                    break;

                //down
                case 3:
                    if (posY == 0) break;
                    if (roomType[posX, posY - 1] > 0) break;
                    else dir = Vector3Int.down;
                    done = false;
                    break;

                default:
                    break;

            }
        }
        return dir;
    }

    void GenerateRoom(int posX, int posY,float seed, GameObject parent)
    {
        Debug.Log("X: " + posX + "Y: " + posY);
        Vector3Int vectorPos = Vector3Int.right * posX * roomSize + Vector3Int.up * posY * roomSize;
        if (seed < 30) { Instantiate(Levels[0],vectorPos,Quaternion.identity, parent.transform); }
        else if(seed < 60) { Instantiate(Levels[1], vectorPos, Quaternion.identity, parent.transform); }
        else if(seed < 100) { Instantiate(Levels[2], vectorPos, Quaternion.identity, parent.transform); }
    }

    void GenerateDungeon(int amount)
    {
        levelParent = new GameObject();
        for(int i = 0; i < amount;i++)
        {
            connectedRooms = CheckConnectedAmount(currentRoomX, currentRoomY);
                
         //   if(connectedRooms <2)
            {
                nextRoomDir = GetRandomDir(currentRoomX, currentRoomY);
                roomType[currentRoomX,currentRoomY] = Random.Range(0, 100);
                GenerateRoom(currentRoomX, currentRoomY, roomType[currentRoomX, currentRoomY], levelParent);


                //Check 30 times for a direction with no connectors at destination
                for (int g = 0; g < 30; g++)
                {
                    switch (dirInt)
                    {
                        case 0: //left
                            if (CheckConnectedAmount(currentRoomX - 1, currentRoomY) < 2)
                            {
                                Debug.Log("Going left it has 1");
                                currentRoomX -= 1;
                                goto loopbreak;
                            }
                            else
                            {
                                dirInt = Random.Range(0, 4);
                            }
                            break;

                        case 1: //right
                            if (CheckConnectedAmount(currentRoomX + 1, currentRoomY) < 2)
                            {
                                Debug.Log("Going right it has 1");
                                currentRoomX += 1;
                                goto loopbreak;
                            }
                            else
                            {
                                dirInt = Random.Range(0, 4);
                            }
                            break;

                        case 2: //up
                            if (CheckConnectedAmount(currentRoomX, currentRoomY + 1) < 2)
                            {
                                Debug.Log("Going up it has 1");
                                currentRoomY += 1;
                                goto loopbreak;
                            }
                            else
                            {
                                dirInt = Random.Range(0, 4);
                            }
                            break;

                        case 3: //down
                            if (CheckConnectedAmount(currentRoomX, currentRoomY - 1) < 2)
                            {
                                Debug.Log("Going down it has 1");
                                currentRoomY -= 1;
                                goto loopbreak;
                            }
                            else
                            {
                                dirInt = Random.Range(0, 4);
                            }
                            break;
                    }
                }


                //Check 30 times again, this time more forgiving
                for (int g = 0; g < 30; g++)
                {
                    switch (dirInt)
                    {
                        case 0: //left
                            if (CheckConnectedAmount(currentRoomX - 1, currentRoomY) < 3)
                            {
                                Debug.Log("Going left it has 2");
                                currentRoomX -= 1;
                                goto loopbreak;
                            }
                            else
                            {
                                dirInt = Random.Range(0, 4);
                            }
                            break;

                        case 1: //right
                            if (CheckConnectedAmount(currentRoomX + 1, currentRoomY) < 3)
                            {
                                Debug.Log("Going right it has 2");
                                currentRoomX += 1;
                                goto loopbreak;
                            }
                            else
                            {
                                dirInt = Random.Range(0, 4);
                            }
                            break;

                        case 2: //up
                            if (CheckConnectedAmount(currentRoomX, currentRoomY + 1) < 3)
                            {
                                Debug.Log("Going up it has 2");
                                currentRoomY += 1;
                                goto loopbreak;
                            }
                            else
                            {
                                dirInt = Random.Range(0, 4);
                            }
                            break;

                        case 3: //down
                            if (CheckConnectedAmount(currentRoomX, currentRoomY - 1) < 3)
                            {
                                Debug.Log("Going down it has 2");
                                currentRoomY -= 1;
                                goto loopbreak;
                            }
                            else
                            {
                                dirInt = Random.Range(0, 4);
                            }
                            break;
                    }
                }

                //there is no fucking way to place a room
                //getting the fuck away from level creation for now
                break;


            loopbreak:;
            }
          /*  else
            {
                break;
            }*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        resetButton = Input.GetKeyDown("a");

        if (resetButton)
        {
            roomType = new float[sizeX, sizeY];
            currentRoomX = startingRoomX;
            currentRoomY = startingRoomY;
            Debug.Log("OK");
            Destroy(levelParent);
            GenerateDungeon(roomAmount);
        }
    }
}
