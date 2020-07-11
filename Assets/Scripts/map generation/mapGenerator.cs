using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //type of tile that will be put in a specific position
    public enum TileType
    {
        Wall, Floor
    }

    public int columns = 100;                                   //how wide the board will be
    public int rows = 100;                                      //how tall the board will be
    public IntRange numRooms = new IntRange(15, 20);            //random number of rooms between
    public IntRange roomHeight = new IntRange(3, 10);           //random value for the height of a room
    public IntRange roomWidth = new IntRange(3, 10);            //random value for the width of a room
    public IntRange corridorLength = new IntRange(6, 10);       //random value for the length of a corridor
    public GameObject[] floorTiles;                             //array of tiles
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;

    private TileType[][] tiles;                                 //a jagged arrau of tile types that represents the board
    private Room[] rooms;                                       //array of generated rooms
    private Corridor[] corridors;                               //array of generated corridors
    private GameObject mapHolder;                               //keeps all of the generated tiles in a container



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

    //create the full board
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

        // creates the array of corridores with one less corrdior than there is rooms otherwise there will be a corridor leading to no room
        corridors = new Corridor[rooms.Length - 1];

        //creates the first room
        rooms[0] = new Room();

        //creates the first corrido
        corridors[0] = new Corridor();

        // there is no previous corridor so we use the first setuproom function.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        //create the first corridor leaving the first room
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

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
        // go through all the rooms
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            //then for each room, go through its width
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                //currentRoom.xPos and currentRoom.yPos is the bottom most left corner
                int xCoord = currentRoom.xPos + j;

                //for each horizontal tile, go up vertically through the room's height
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    tiles[xCoord][yCoord] = TileType.Floor;

                }
            }
        }

    }


    void SetTilesValuesForCorridors()
    {
        //go through all the corridors
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            //go through the corridor length
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {

                //start the coordinates at the start of the corridor
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                //depending on the fdirection, add or subtract based on how far through the length of the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;

                    case Direction.East:
                        xCoord += j;
                        break;

                    case Direction.South:
                        yCoord -= j;
                        break;

                    case Direction.West:
                        xCoord -= j;
                        break;

                }

                //set the tiles at the coordinates to floor
                tiles[xCoord][yCoord] = TileType.Floor;

            }
        }

    }

    //takes the tile coordinates then instantiate the prefabs
    void InstantiateTiles()
    {

        //go through the tiles in the jagged array
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                //instantiate a floor tile for each tile in the jagged array
                InstantiateFromArray(floorTiles, i, j);

                //if the tiletype is a wall
                if (tiles[i][j] == TileType.Wall)
                {
                    //instantiate a wall over the top
                    InstantiateFromArray(wallTiles, i, j);
                }
            }
        }
    }


    void InstantiateOuterWalls()
    {
        // the outer walls are 1 unit left, top, right and bottom from the board.
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        //instantiate both vertical outer walls
        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        //instantiate both horizontal walls
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);

    }

    void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
    {

        float currentY = startingY;

        //while the currentY is less than the end of the board, 
        while (currentY <= endingY)
        {
            //instantiate a wall at the current x and y
            InstantiateFromArray(outerWallTiles, xCoord, currentY);
            currentY++;
        }
    }

    void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
    {
        float currentX = startingX;

        while (currentX <= endingX)
        {
            InstantiateFromArray(outerWallTiles, currentX, yCoord);
            currentX++;
        }
    }

    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {

        int randomIndex = Random.Range(0, prefabs.Length);
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        
        //takes an array of prefabs amd instantiates then at the xCoord and yCoord
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        //sets the tiles parent to the map holder
        tileInstance.transform.parent = mapHolder.transform;
    }


}
