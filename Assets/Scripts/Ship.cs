﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

    public List<Room> m_rooms = new List<Room>();
    public List<Door> m_doors = new List<Door>();

    float eventTimer;
    public float minTimeToEmergency = 2.5f;
    public float maxTimeToEmergency = 5f;

    float maxFlood;
    // Between 0 - 1;
    public float gameOverRequirement = 0.5f;
    
    IEnumerator EmergencyPolling()
    {

        while (true)
        {
            List<Room> nonEmergencyRooms = m_rooms.FindAll(x => x.Status == Room.RoomEmergency.NONE && !x.m_containsPlayer);

            if (nonEmergencyRooms.Count > 0)
            {
                Room r = nonEmergencyRooms[Random.Range(0, nonEmergencyRooms.Count - 1)];

                Room.RoomEmergency emergency = Room.RoomEmergency.BROKEN;
                RemoteCallRoomStateChange(emergency, r.id);
            }

            if (Input.GetKeyDown(KeyCode.F12)) yield break;

            yield return new WaitForSeconds(Random.Range(minTimeToEmergency, maxTimeToEmergency));
        }
    }

    public void RemoteCallPumpWater(int roomID)
    {
        networkView.RPC("RemotePumpWater", RPCMode.Server, roomID);
    }

    public void RemoteCallRoomStateChange(Room.RoomEmergency state, int roomID)
    {
        networkView.RPC("RemoteChangeRoomState", RPCMode.AllBuffered, (int)state, roomID);
    }

    public void RemoteCallDoorStateChange(bool state, int doorID)
    {
        networkView.RPC("RemoteDoorChanged", RPCMode.AllBuffered, state, doorID);
    }

	[RPC]
    void RemoteChangeRoomState(int status, int roomID) {
        Room room = m_rooms.Find(x => x.id == roomID);
        room.Status = (Room.RoomEmergency)status;
    }

    [RPC]
    void RemotePumpWater(int roomID)
    {
        Room room = m_rooms.Find(x => x.id == roomID);
        room.PumpWater();
    }

    [RPC]
    void RemoteDoorChanged(bool state, int doorID)
    {
        Door door = m_doors.Find(x => x.id == doorID);
        door.Open = state;

        CrewMember player = PlayerController.GlobalInstance.SelectedCrewMember;
        if (player.Status == CrewMember.State.MOVING)
            player.SetMoving(player.destination);
    }

    public static Ship GlobalInstance
    {
        get { return FindObjectOfType<Ship>(); }
    }

	// Use this for initialization
	void Start () {
        m_rooms.AddRange(GetComponentsInChildren<Room>());
        m_doors.AddRange(GetComponentsInChildren<Door>());

        gameOverRequirement = Mathf.Clamp(gameOverRequirement, 0, 1);

        maxFlood = m_rooms.Count * gameOverRequirement;

	}

    public Vector3 GetRandomRoomPos()
    {
        Room r = m_rooms[Random.Range(0, m_rooms.Count - 1)];
        return r.m_center;
    }

    float UpdateFloodLevel()
    {
        float f = 0;

        foreach (var d in m_rooms)
        {
            f += d.flood.Height;
        }

        return f;
    }



    public void Init()
    {
        if (Network.isServer)
            StartCoroutine(EmergencyPolling());
    }
	
    [RPC]
    void GameOver()
    {
        foreach (var o in FindObjectsOfType<GameObject>())
        {
            Destroy(o);
        }
    }


	// Update is called once per frame
	void Update () 
    {
        if (Network.isServer)
        {
            if (UpdateFloodLevel() > maxFlood)
            {
                networkView.RPC("GameOver", RPCMode.AllBuffered);
                //GameOver
            }
        }


	}
}
