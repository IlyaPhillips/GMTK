using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour
{
    public enum TileType
    {
        Wall, Floor
    }

    public int columns = 100;
    public int rows = 100;
    public IntRange numRooms = new IntRange(15, 20);
    public IntRange roomHeight = new IntRange(3, 10);
    public IntRange roomWidth = new IntRange(3, 10);
    public IntRange corridorLength = new IntRange(6, 10);
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;

    private TileType[][] tiles;
    private Room[] rooms;
    private Corridor[] corridors;
    private GameObject mapHolder;



    // Start is called before the first frame update
    void Start()
    {
        mapHolder = new GameObject("MapHolder");
        SetupTilesArray();
        CreateRoomsAndCorridors();
        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
        InstantiateOuterWalls();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void SetupTilesArray()
    {
        //sets the tiles jagged array to the correct width.
        tiles = new TileType[columns][];
        //go through all the tiles
        for (int i = 0; i < tiles.Length; i++)
        {
            //and and sets each tile array to the correct height
            tiles[i] = new TileType[rows];
        }
    }

    void CreateRoomsAndCorridors()
    {
        //creates the room array with a random number of rooms
        rooms = new Room[numRooms.Random];
        // creates the array of corridores with one less corrdior than there is rooms
        corridors = new Corridor[rooms.Length - 1];
        //creates the first room
        rooms[0] = new Room();
        corridors[0] = new Corridor();
        //creates the first corridor
        // there is no previous corridor so we use the first setuproom function.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        //creates all the rooms.
        for (int i = 1; i < rooms.Length; i++)
        {
            //creates a new room
            rooms[i] = new Room();

            //setup the room based on the previous corrdior
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);
            //if we havent reached the end of the corridor array then create a corridor;
            if (i < corridors.Length)
            {
                corridors[i] = new Corridor();
                //setupp the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
        }

    }


    void SetTilesValuesForRooms()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                for (int k = 0; k < currentRoom.roomHeight; k++)
                {


                }
            }
        }


    }
    void SetTilesValuesForCorridors()
    {

    }

    void InstantiateTiles()
    {

    }
    void InstantiateOuterWalls()
    {

    }



}
