using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicolegeneratedRoom : Room
{

    public override void fillRoom(LevelGenerator ourGenerator, ExitConstraint requiredExits)
    {


        /*
        for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
        {
            for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
            {
                /*
                bool isLeftOrRightEdge = false;
                if (x == 0 || x == LevelGenerator.ROOM_WIDTH - 1)
                {
                    isLeftOrRightEdge = true;
                }
                
                bool fill = false;
                if (x == 0 && requiredExits.leftExitRequired == false)
                {
                    fill = true;
                }
                else if (x == LevelGenerator.ROOM_WIDTH -1 && requiredExits.rightExitRequired == false)
                {
                    fill = true;
                }
                else if (y == LevelGenerator.ROOM_HEIGHT - 1 && requiredExits.upExitRequired == false)
                {
                    fill = true;
                }
                else if (y == 0 && requiredExits.downExitRequired == false)
                {
                    fill = true;
                }

                if (fill)
                {
                    Tile.spawnTile(ourGenerator.normalWallPrefab, transform, x, y);
                }
            }
        }
        */
    }
   
 }
