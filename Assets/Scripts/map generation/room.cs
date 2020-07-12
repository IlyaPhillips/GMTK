using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{


    public int roomWidth;                       //the width and height of each room
    public int roomHeight;

    public int xPos;                            //xPos and yPos is the bottom left of the room
    public int yPos;
    public Direction enteringCorridor;          //the direction of the corridor entering the room




    //sets up the first room created 
    //columns and rows are used to make sure generated rooms do not go beyond the edge of the board
    public void SetupRoom(IntRange widthRange, IntRange heightRange, int columns, int rows)
    {
        //InitialiseList();

        //set a random wifth and height for the room
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;
        //creates the first room in the miuddle of the board
        xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
        yPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);


    }


    //overload of the setuproom function with a corridor parameter
    public void SetupRoom(IntRange widthRange, IntRange heightRange, int columns, int rows, Corridor corridor)
    {

        //sets the entering corridor direction
        enteringCorridor = corridor.direction;

        //sets random values for width and height
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        //if the entering corrdior direction is this, then create a room in this way
        switch (corridor.direction)
        {
            //if the corridor of this room is going north
            case Direction.North:

                // the height of the room should not go beyond the board using the y coordinate of the end of the corridor entering the room
                // so it must be clamped based on the height of the board.
                roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.EndPositionY);

                //the y coordinate of the room must be at the end of the corrdfor (bottom of the room)
                yPos = corridor.EndPositionY;

                //the x coordinate can be randomised but can be no further than the width of the board
                //the corrdidor must end at the end of the room or before.
                xPos = UnityEngine.Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);

                xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);
                break;

            case Direction.East:
                roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndPositionX);
                xPos = corridor.EndPositionX;

                yPos = UnityEngine.Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPos = Mathf.Clamp(yPos, 0, rows - roomHeight);
                break;


            case Direction.South:
                roomHeight = Mathf.Clamp(roomHeight, 1, corridor.EndPositionY);
                yPos = corridor.EndPositionY - roomHeight + 1;
                xPos = UnityEngine.Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
                xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);
                break;

            case Direction.West:
                roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndPositionX);
                xPos = corridor.EndPositionX - roomWidth + 1;
                xPos = Mathf.Clamp(xPos, 0,roomWidth);

                yPos = UnityEngine.Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPos = Mathf.Clamp(yPos, 0, rows - roomHeight);
                break;

        }

        Debug.Log("Setting y position: " + yPos + ", Direction: " + corridor.direction);
    }


}
