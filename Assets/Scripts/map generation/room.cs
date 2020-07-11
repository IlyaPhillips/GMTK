using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomWidth;
    public int roomHeight;
    public int xPos;
    public int yPos;
    public Direction enteringCorridor;

    //sets up the first room created 
    public void SetupRoom(IntRange widthRange, IntRange heightRange, int columns, int rows)
    {
        //set a random wifth and height for the room
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
        yPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);


    }

    //overload of the setuproom function with a corridor parameter
    public void SetupRoom(IntRange widthRange, IntRange heightRange, int columns, int rows, Corridor corridor)
    {
        enteringCorridor = corridor.direction;
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;
        switch (corridor.direction)
        {
            //if the corridor of this room is going north
            case Direction.North:
                // the height of the room should not go beyond the board. 
                // so it must be clamped based on the height of the board.
                roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.EndPositionY);

                //the y coordinate of the room must be at the end of the corrdfor (bottom of the room)
                yPos = corridor.EndPositionY;

                //the x coordinate can be randomised but can be no further than the width of the board
                //the corrdidor must end at the end of the room or before.
                xPos = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
                xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);
                break;

            case Direction.East:
                roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndPositionX);
                xPos = corridor.EndPositionX;

                yPos = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPos = Mathf.Clamp(yPos, 0, rows - roomHeight);
                break;


            case Direction.South:
                roomHeight = Mathf.Clamp(roomHeight, 1, corridor.EndPositionY);
                yPos = corridor.EndPositionY - roomHeight + 1;
                xPos = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
                xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);
                break;

            case Direction.West:
                roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndPositionX);
                xPos = corridor.EndPositionX - roomWidth + 1;

                yPos = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPos = Mathf.Clamp(yPos, 0, rows - roomHeight);
                break;

        }

    }


}
