using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPicker : Room
{
    public GameObject[] validatedRooms;

    public override Room createRoom(ExitConstraint requiredExits)
    {
        List<Room> roomsThatMeetConstraints = new List<Room>();

        foreach (GameObject roomPrefab in validatedRooms)
        {
            validatedRoom validatedRoom = roomPrefab.GetComponent<validatedRoom>();
            if (validatedRoom.MeetsConstraints(requiredExits))
            {
                roomsThatMeetConstraints.Add(validatedRoom);
            }
        }

        return Instantiate(roomsThatMeetConstraints[Random.Range(0, roomsThatMeetConstraints.Count)]).GetComponent<Room>();
    }
}
