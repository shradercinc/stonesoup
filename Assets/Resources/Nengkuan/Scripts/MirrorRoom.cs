using System.Collections.Generic;
using UnityEngine;

namespace Resources.Nengkuan.Scripts
{
    public class MirrorRoom : Room
    {
        [SerializeField]
        private float laserGunSpawnChance = 0.3f;
        
        [SerializeField]
        private float mirrorSpawnChance = 0.3f;
        
        
        public override void fillRoom(LevelGenerator ourGenerator, ExitConstraint requiredExits)
        {
            var height = LevelGenerator.ROOM_HEIGHT;
            var width = LevelGenerator.ROOM_WIDTH;
            bool spawnLaserGun = Random.value < laserGunSpawnChance;
            // List<Vector2Int> filledMirrorSpots = new List<Vector2Int>();
            Tile.spawnTile(localTilePrefabs[1].gameObject, transform, width/2, height/2);
            for (int i = 0; i < width; i++) 
            {
                for (int j = 0; j < height; j++) 
                {
                    if (Random.value < mirrorSpawnChance)
                    {
                        if (i != width / 2 && j != height / 2)
                        {
                            Tile.spawnTile(localTilePrefabs[0].gameObject, transform, i, j);
                        }
                    }
                }
            }
        }

        // public List<Vector2Int> GenerateMirrorMap(ExitConstraint exitConstraint)
        // {
        //     
        // }
    }
}