using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class validatedRoom : Room
{
    public bool hasLeftExit;
    public bool hasRightExit;
    public bool hasUpExit;
    public bool hasDownExit;


    public bool hasPathLeftToRight;
    public bool hasPathLeftToUp;
    public bool hasPathLeftToDown;
    public bool hasPathRightToUp;
    public bool hasPathRightToDown;
    public bool hasPathUpToDown;


    void ValidateRoom() {
        //ROOM_HEIGHT and ROOM_WIDTH were not working for some reason
        Vector2Int leftEntrance = new Vector2Int(0, 8 - 4);
        Vector2Int rightEntrance = new Vector2Int(10 - 1, 8 - 4);
        Vector2Int upEntrance = new Vector2Int(3, 8 - 1);
        Vector2Int downEntrance = new Vector2Int(3, 0);


        hasPathLeftToRight = Search(leftEntrance, rightEntrance);
        hasPathLeftToUp = Search(leftEntrance, upEntrance);
        hasPathLeftToDown = Search(leftEntrance, downEntrance);
        hasPathRightToUp = Search(rightEntrance, upEntrance);
        hasPathRightToDown = Search(rightEntrance, downEntrance);
        hasPathUpToDown = Search(upEntrance, downEntrance);
    }

    List<Vector2Int> GetNeighbors(Vector2Int currentNode) {
        List<Vector2Int> neighborList = new List<Vector2Int>();

        //Left
        if (currentNode.x > 0) {
            neighborList.Add(new Vector2Int(currentNode.x - 1, currentNode.y));
        }
        //Right
        if (currentNode.x + 1 < LevelGenerator.ROOM_WIDTH) {
            neighborList.Add(new Vector2Int(currentNode.x + 1, currentNode.y));
        }
        //Down
        if (currentNode.y > 0) {
            neighborList.Add(new Vector2Int(currentNode.x, currentNode.y - 1));
        }
        //Up
        if (currentNode.y + 1 < LevelGenerator.ROOM_HEIGHT) {
            neighborList.Add(new Vector2Int(currentNode.x, currentNode.y + 1));
        }

        return neighborList;
    }

    public bool Search(Vector2Int startPoint, Vector2Int endPoint) {
        //Nodes visited already
        List<Vector2Int> closedSet = new List<Vector2Int>();
        //Nodes to check in next step check (frontier)
        List<Vector2Int> openSet = new List<Vector2Int>();

        Vector2Int currentNode;
        List<Vector2Int> neighbors;

        closedSet.Add(startPoint);
        openSet.Add(startPoint);

        while(openSet.Count > 0) {
            currentNode = openSet[0];
            openSet.RemoveAt(0);

            if (currentNode.x == endPoint.x && currentNode.y == endPoint.y) {
                return true;
            }

            //TODO: Check grid to ensure this space doesn't contain a wall
            bool isTraversable = true;
            if (isTraversable == false) {
                continue;
            }

            neighbors = GetNeighbors(currentNode);

            foreach (Vector2Int neighbor in neighbors) {
                if (closedSet.Contains(neighbor)) {
                    continue;
                }
                else {
                    openSet.Add(neighbor);
                    closedSet.Add(neighbor);
                }
            }
        }

        return false;
    }

    public bool MeetsConstraints(ExitConstraint requiredExits)
    {
        if (requiredExits.leftExitRequired && hasLeftExit == false)
            return false;

        if (requiredExits.rightExitRequired && hasRightExit == false)
            return false;

        if (requiredExits.upExitRequired && hasUpExit == false)
            return false;

        if (requiredExits.downExitRequired && hasDownExit == false)
            return false;

        if (requiredExits.leftExitRequired && requiredExits.rightExitRequired)
        {
            return hasPathLeftToRight;
        }

        if (requiredExits.leftExitRequired && requiredExits.upExitRequired)
        {
            return hasPathLeftToUp;
        }

        if (requiredExits.leftExitRequired && requiredExits.downExitRequired)
        {
            return hasPathLeftToDown;
        }

        if (requiredExits.rightExitRequired && requiredExits.upExitRequired)
        {
            return hasPathRightToUp;
        }

        if (requiredExits.rightExitRequired && requiredExits.downExitRequired)
        {
            return hasPathRightToDown;
        }

        if (requiredExits.upExitRequired && requiredExits.downExitRequired)
        {
            return hasPathUpToDown;
        }
        return true;
    }

    private static bool isTraversable(int[,] indexGrid, Vector2Int currentNode) {
        bool isTraversable;
        if (indexGrid[currentNode.x, currentNode.y] == 1) {
            isTraversable = false;
        }
        else {
            isTraversable = true;
        }
        return isTraversable;
    }

    /*
    public virtual void fillRoom(LevelGenerator ourGenerator, ExitConstraint requiredExits) {

            string initialGridString = designedRoomFile.text;
            string[] rows = initialGridString.Trim().Split('\n');
            int width = rows[0].Trim().Split(',').Length;
            int height = rows.Length;
            if (height != LevelGenerator.ROOM_HEIGHT) {
                throw new UnityException(string.Format("Error in room by {0}. Wrong height, Expected: {1}, Got: {2}", roomAuthor, LevelGenerator.ROOM_HEIGHT, height));
            }
            if (width != LevelGenerator.ROOM_WIDTH) {
                throw new UnityException(string.Format("Error in room by {0}. Wrong width, Expected: {1}, Got: {2}", roomAuthor, LevelGenerator.ROOM_WIDTH, width));
            }
            int[,] indexGrid = new int[width, height];
            for (int r = 0; r < height; r++) {
                string row = rows[height-r-1];
                string[] cols = row.Trim().Split(',');
                for (int c = 0; c < width; c++) {
                    indexGrid[c, r] = int.Parse(cols[c]);
                }
            }
            
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    int tileIndex = indexGrid[i, j];
                    if (tileIndex == 0) {
                        continue; // 0 is nothing.
                    }
                    GameObject tileToSpawn;
                    if (tileIndex < LevelGenerator.LOCAL_START_INDEX) {
                        tileToSpawn = ourGenerator.globalTilePrefabs[tileIndex-1];
                    }
                    else {
                        tileToSpawn = localTilePrefabs[tileIndex-LevelGenerator.LOCAL_START_INDEX];
                    }
                    Tile.spawnTile(tileToSpawn, transform, i, j);
                }
            }
	}
    */
  
 }
