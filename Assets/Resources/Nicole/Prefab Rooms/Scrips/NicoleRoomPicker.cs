using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicoleRoomPicker : Room
{
    public GameObject[] validatedRooms;

    public override Room createRoom(ExitConstraint requiredExits)
    {
        List<Room> roomsThatMeetConstraints = new List<Room>();

        foreach (GameObject roomPrefab in validatedRooms)
        {
            NicoleValidatedRoom validatedRoom = roomPrefab.GetComponent<NicoleValidatedRoom>();
            validatedRoom.ValidateRoom();
            if (validatedRoom.MeetsConstraints(requiredExits))
            {
                roomsThatMeetConstraints.Add(validatedRoom);
            }
        }

        return Instantiate(roomsThatMeetConstraints[Random.Range(0, roomsThatMeetConstraints.Count)]).GetComponent<Room>();
    }
    
    // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
