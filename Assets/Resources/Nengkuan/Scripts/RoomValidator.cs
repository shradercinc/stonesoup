using System;
using UnityEngine;

namespace Resources.Nengkuan.Scripts
{
    public static class RoomValidator
    {
        // public static bool IsRoomValid(Vector2Int[][] roomMap, ExitRequirement constraint)
        // {
        //     
        // }
    }

    
    [Flags]
    public enum ExitRequirement
    {
        Up,
        Down,
        Left,
        Right,
    }
}