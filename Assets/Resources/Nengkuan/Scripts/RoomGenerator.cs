using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Nengkuan.Scripts
{
    public class RoomGenerator : Room
    {
        [SerializeField]
        private List<RoomConfig> roomConfigs;

        public override Room createRoom(ExitConstraint requiredExits)
        {
            return Instantiate(GetRandomRoomConfig().RoomPrefab);
        }
        
        private RoomConfig GetRandomRoomConfig()
        {
            var totalWeight = 0f;
            foreach (var roomConfig in roomConfigs)
            {
                totalWeight += roomConfig.ProbabilityWeight;
            }

            var randomValue = UnityEngine.Random.Range(0, totalWeight);
            foreach (var roomConfig in roomConfigs)
            {
                if (randomValue < roomConfig.ProbabilityWeight)
                {
                    return roomConfig;
                }

                randomValue -= roomConfig.ProbabilityWeight;
            }

            throw new Exception("Should not reach here");
        }
    }

    [Serializable]
    public class RoomConfig
    {
        [SerializeField]
        private RoomType roomType;
        public RoomType RoomType => roomType;
        
        [SerializeField]
        private float probabilityWeight = 1;
        public float ProbabilityWeight => probabilityWeight;
        
        [SerializeField]
        private Room roomPrefab;
        public Room RoomPrefab => roomPrefab;
    }

    public enum RoomType
    {
        TrapRoom,
        MirrorRoom,
    }
}