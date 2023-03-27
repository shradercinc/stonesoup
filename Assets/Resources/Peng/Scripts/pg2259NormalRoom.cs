using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class pg2259NormalRoom : Room
{
    int roomType = 0;
    int ROOM_TYPE_ONE = 0;
    int  ROOM_TYPE_TWO = 1;
    int ROOM_TYPE_THREE = 2;
    List<Vector2Int> filledSpots = new List<Vector2Int>();

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public override void fillRoom(LevelGenerator ourGenerator, ExitConstraint requiredExits)
    {
        filledSpots = new List<Vector2Int>();
        roomType = Random.Range(0, 3);
        print("room type is: " + roomType);

        for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
        {
            for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
            {
                if(x==0 || x == LevelGenerator.ROOM_WIDTH - 1 || y == 0 || y == LevelGenerator.ROOM_HEIGHT - 1)
                {
                    //filledSpots.Add(new Vector2Int(x, y));
                }
                bool fill = false;
                if (x == 0 && requiredExits.leftExitRequired == false)
                {
                    fill = true;
                }
                else if (x == LevelGenerator.ROOM_WIDTH - 1 && requiredExits.rightExitRequired == false)
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
                    //filledSpots.Add(new Vector2Int(x, y));
                    filledSpots.Add(new Vector2Int(x, y));
                }
            }
        }

        generateCluster(getEmptySpot(filledSpots), Random.Range(2, 6), ourGenerator.normalWallPrefab, filledSpots);
        generateCluster(getEmptySpot(filledSpots), Random.Range(2, 6), ourGenerator.normalWallPrefab, filledSpots);

        
        if (roomType == ROOM_TYPE_ONE)
        {
            print("making room type 1"); //shield
            Vector2Int spot = getEmptySpot(filledSpots);
            Tile.spawnTile(localTilePrefabs[7 - LevelGenerator.LOCAL_START_INDEX], transform, spot.x, spot.y);
            spot = getEmptySpot(filledSpots);
            Tile.spawnTile(localTilePrefabs[9 - LevelGenerator.LOCAL_START_INDEX], transform, spot.x, spot.y);
            spot = getEmptySpot(filledSpots);
            Tile.spawnTile(localTilePrefabs[9 - LevelGenerator.LOCAL_START_INDEX], transform, spot.x, spot.y);
        }
        else if (roomType == ROOM_TYPE_TWO) //mutant pigs
        {
            Vector2Int spot = getEmptySpot(filledSpots);
            spot = getEmptySpot(filledSpots);
            Tile.spawnTile(localTilePrefabs[9 - LevelGenerator.LOCAL_START_INDEX], transform, spot.x, spot.y);
            print("making room type 2");
            fillAllWithChance(getAllEmptySpots(filledSpots), 4, 0.04f, ourGenerator);
        }
        else if(roomType == ROOM_TYPE_THREE) //axe
        {
            print("making room type 3");
            Vector2Int spot = getEmptySpot(filledSpots);
            Tile.spawnTile(localTilePrefabs[5 - LevelGenerator.LOCAL_START_INDEX], transform, spot.x, spot.y);
            spot = getEmptySpot(filledSpots);
            Tile.spawnTile(localTilePrefabs[9 - LevelGenerator.LOCAL_START_INDEX], transform, spot.x, spot.y);
            spot = getEmptySpot(filledSpots);
            Tile.spawnTile(localTilePrefabs[9 - LevelGenerator.LOCAL_START_INDEX], transform, spot.x, spot.y);
        }

    }

    private void fillAllWithChance(List<Vector2Int> spaceToFill, int tileIndex, float chance, LevelGenerator ourGenerator)
    {
        for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
        {
            for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
            {
                if(Random.Range(0f,1f) < chance)
                {

                    GameObject tileToSpawn;
                    if (tileIndex < LevelGenerator.LOCAL_START_INDEX)
                    {
                        tileToSpawn = ourGenerator.globalTilePrefabs[tileIndex - 1];
                    }
                    else
                    {
                        tileToSpawn = localTilePrefabs[tileIndex - LevelGenerator.LOCAL_START_INDEX];
                    }
                    Tile.spawnTile(tileToSpawn, transform, x, y);

                    filledSpots.Add(new Vector2Int(x,y));
                }
            }
        }
    }

    private List<Vector2Int> getAllEmptySpots(List<Vector2Int> excludeList)
    {
        List<Vector2Int> output = new List<Vector2Int>();
        for (int x = 1; x < LevelGenerator.ROOM_WIDTH-1; x++)
        {
            for (int y = 1; y < LevelGenerator.ROOM_HEIGHT-1; y++)
            {
                if(!listContainsVec(excludeList, new Vector2Int(x, y)) )
                {
                    output.Add(new Vector2Int(x, y));
                }
            }
        }
        return output;
    }

    private Vector2Int getEmptySpot(List<Vector2Int> excludeList) 
    {
        /*
        if(LevelGenerator.ROOM_WIDTH * LevelGenerator.ROOM_HEIGHT == excludeList.Count)
        {
            Debug.LogError("No empty space to choose from.");
            return new Vector2Int(0, 0);
        }
        Vector2Int output = new Vector2Int(Random.Range(0, LevelGenerator.ROOM_WIDTH), Random.Range(0, LevelGenerator.ROOM_HEIGHT));
        while (excludeList.Contains(output))
        {
            output = new Vector2Int(Random.Range(0, LevelGenerator.ROOM_WIDTH), Random.Range(0, LevelGenerator.ROOM_HEIGHT));
        }*/
        List<Vector2Int> allEmpty = getAllEmptySpots(excludeList);
        return allEmpty[Random.Range(0, allEmpty.Count)];//output;
    }

    private List<Vector2Int> generateCluster(Vector2Int startPosition, int amount, GameObject prefab,List<Vector2Int> excludeList)
    {
        List<Vector2Int> chosenSpots = new List<Vector2Int>();
        Vector2Int lastSpot = startPosition;
        chosenSpots.Add(lastSpot);
        for (int i = 0; i < amount - 1; i++)
        {
            List<Vector2Int> potential = new List<Vector2Int> { lastSpot + new Vector2Int(1, 0), lastSpot + new Vector2Int(-1, 0), lastSpot + new Vector2Int(0, 1), lastSpot + new Vector2Int(0, -1) };
            for (int j = 3; j >= 0; j--)
            {
                if (listContainsVec(chosenSpots, potential[j]) || listContainsVec(excludeList, potential[j]) || potential[j].x<0 || potential[j].y < 0 || potential[j].x >= LevelGenerator.ROOM_WIDTH || potential[j].y >= LevelGenerator.ROOM_HEIGHT) potential.Remove(potential[j]);
            }
            if (potential.Count == 0) break;
            lastSpot = potential[Random.Range(0, potential.Count)];
            chosenSpots.Add(lastSpot);
        }
        foreach (Vector2Int spot in chosenSpots)
        {
            Tile.spawnTile(prefab, transform, spot.x, spot.y);
            excludeList.Add(spot);
        }
        return chosenSpots;
    }

    private bool listContainsVec(List<Vector2Int> theList, Vector2Int theVec)
    {
        foreach(Vector2Int v in theList)
        {
            if (v.x == theVec.x && v.y == theVec.y) return true;
        }
        return false;
    }
}
