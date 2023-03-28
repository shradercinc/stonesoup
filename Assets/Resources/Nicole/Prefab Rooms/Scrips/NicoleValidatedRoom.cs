using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicoleValidatedRoom : Room
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


    public void ValidateRoom()
    {
        Vector2Int leftEntrance = new Vector2Int(0, LevelGenerator.ROOM_HEIGHT-4);
        Vector2Int rightEntrance = new Vector2Int(LevelGenerator.ROOM_WIDTH - 1, LevelGenerator.ROOM_HEIGHT - 4);
        Vector2Int upEntrance = new Vector2Int(4, LevelGenerator.ROOM_HEIGHT - 1);
        Vector2Int downEntrance = new Vector2Int(4, 0);

        int[,] indexGrid = loadIndexGrid();

        hasLeftExit = IsTraversable(indexGrid, leftEntrance);
        hasRightExit = IsTraversable(indexGrid, rightEntrance);
        hasUpExit = IsTraversable(indexGrid, upEntrance);
        hasDownExit = IsTraversable(indexGrid, downEntrance);

        hasPathLeftToRight = Search(indexGrid, leftEntrance, rightEntrance);
        hasPathLeftToUp = Search(indexGrid, leftEntrance, upEntrance);
        hasPathLeftToDown = Search(indexGrid, leftEntrance, downEntrance);
        hasPathRightToUp = Search(indexGrid, rightEntrance, upEntrance);
        hasPathRightToDown = Search(indexGrid, rightEntrance, downEntrance);
        hasPathUpToDown = Search(indexGrid, upEntrance, downEntrance);
    }

    List<Vector2Int> GetNeighbors(Vector2Int currentNode)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        //left
        if (currentNode.x > 0)
            neighbors.Add(new Vector2Int(currentNode.x - 1, currentNode.y));

        //right
        if (currentNode.x + 1 < LevelGenerator.ROOM_WIDTH)
            neighbors.Add(new Vector2Int(currentNode.x + 1, currentNode.y));

        //down
        if (currentNode.y > 0)
            neighbors.Add(new Vector2Int(currentNode.x, currentNode.y - 1));

        //up
        if (currentNode.y + 1 < LevelGenerator.ROOM_HEIGHT)
            neighbors.Add(new Vector2Int(currentNode.x, currentNode.y + 1));

        return neighbors;
    }



    //returns true if startPoint can connect to endPoint
    public bool Search(int[,] indexGrid, Vector2Int startPoint, Vector2Int endPoint, bool depthFirst = false)
    {
        List<Vector2Int> closedSet = new List<Vector2Int>();
        List<Vector2Int> openSet = new List<Vector2Int>();

        Vector2Int currentNode;
        List<Vector2Int> neighbors;

        closedSet.Add(startPoint);
        openSet.Add(startPoint);

        while(openSet.Count > 0)
        {
            if (depthFirst)
            {
                currentNode = openSet[openSet.Count - 1];
                openSet.RemoveAt(openSet.Count - 1);
            }
            else
            {
                currentNode = openSet[0];
                openSet.RemoveAt(0);
            }

            if (currentNode == endPoint)
                return true;

            //TODO: Check grid to ensure this space doesnt contain a wall
            bool isTraversable;

            isTraversable = IsTraversable(indexGrid, currentNode);

            if (isTraversable == false)
                continue;

            neighbors = GetNeighbors(currentNode);

            foreach (Vector2Int neighbor in neighbors)
            {
                if (closedSet.Contains(neighbor))
                    continue;
                else
                {
                    openSet.Add(neighbor);
                    closedSet.Add(neighbor);
                }
            }
        }

        return false;
    }

    private static bool IsTraversable(int[,] indexGrid, Vector2Int currentNode)
    {
        bool isTraversable;
        if (indexGrid[currentNode.x, currentNode.y] == 1)
            isTraversable = false;
        else
            isTraversable = true;
        return isTraversable;
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
    public int[,] loadIndexGrid()
    {

        string initialGridString = designedRoomFile.text;
        string[] rows = initialGridString.Trim().Split('\n');
        int width = rows[0].Trim().Split(',').Length;
        int height = rows.Length;

        int[,] indexGrid = new int[width, height];
        for (int r = 0; r < height; r++)
        {
            string row = rows[height - r - 1];
            string[] cols = row.Trim().Split(',');
            for (int c = 0; c < width; c++)
            {
                indexGrid[c, r] = int.Parse(cols[c]);
            }
        }

        return indexGrid;
    }


}
