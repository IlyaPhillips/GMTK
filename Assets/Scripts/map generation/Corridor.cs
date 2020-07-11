using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North, East, South, West
}
public class Corridor : MonoBehaviour
{

    public int startXPos;
    public int startYPos;
    public int corridorLength;
    public Direction direction;


    public int EndPositionX
    {
        get
        {
            if (direction == Direction.North || direction == Direction.South)
            {
                return startXPos;
            }
            if (direction == Direction.East)
            {
                return startXPos + corridorLength - 1;
            }
            return startXPos - corridorLength + 1;
        }
    }

    public int EndPositionY
    {
        get
        {
            if (direction == Direction.East || direction == Direction.West)
            {
                return startYPos;
            }
            if (direction == Direction.North)
            {
                return startYPos + corridorLength - 1;
            }
            return startYPos - corridorLength + 1;
        }

    }

    public void SetupCorridor(Room room, IntRange length, IntRange roomWidth, IntRange roomHeight, int columns, int rows, bool firstCorridor)
    {
        //set a random direction
        //casts a random number between 1 and 4 to Direction
        direction = (Direction)Random.Range(0, 4);

        //finds the direction opposite to the one entering the room this corridor is leaving from
        //this is so that the new corridor does not go back on itself.
        Direction oppositeDirection = (Direction)(((int)room.enteringCorridor + 2) % 4);

        if (!firstCorridor && direction == oppositeDirection)
        {
            int directionInt = (int)direction;
            direction++;
            directionInt = directionInt % 4;
            direction = (Direction)directionInt;
        }

        //sets a random length for the corridor
        corridorLength = length.Random;

        //create a cap for the length of the corridor
        int maxLength = length.mMax;

        switch (direction)
        {
            //if the chosen position is north
            case Direction.North:
                //the start position of x can be random vbut within the width of the room
                startXPos = Random.Range(room.xPos, room.xPos + room.roomWidth - 1);
                //start position of y must be at the top of the room
                startYPos = room.yPos + room.roomHeight;

                //the max length the corridor can be is the height of the board. but from the top of the room
                maxLength = rows - startYPos - roomHeight.mMin;
                break;

            case Direction.East:
                startXPos = room.xPos + room.roomWidth;
                startYPos = Random.Range(room.yPos, room.yPos + room.roomHeight);
                maxLength = columns - startXPos - roomWidth.mMin;
                break;

            case Direction.South:
                startXPos = Random.Range(room.xPos, room.xPos + room.roomWidth);
                startYPos = room.yPos;
                maxLength = startYPos - roomHeight.mMin;
                break;
            case Direction.West:
                startXPos = room.xPos;
                startYPos = Random.Range(room.yPos, room.yPos + room.roomHeight);
                maxLength = startXPos - roomWidth.mMin;
                break;



        }
        //make sure the corrdor doesnt go off the board
        corridorLength = Mathf.Clamp(corridorLength, 1, maxLength);

    }


}
