using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

    public List<Room> m_rooms = new List<Room>();

    float eventTimer;
    public float minTimeToEmergency = 2.5f;
    public float maxTimeToEmergency = 5f;

    IEnumerator EmergencyPolling()
    {

        while (true)
        {
            List<Room> nonEmergencyRooms = m_rooms.FindAll(x => x.Status == Room.RoomEmergency.NONE && !x.m_containsPlayer);

            if (nonEmergencyRooms.Count > 0)
            {
                Room r = nonEmergencyRooms[Random.Range(0, nonEmergencyRooms.Count - 1)];
                RemoteCallRoomStateChange(Room.RoomEmergency.DESTRoYED, r.id);
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

    public static Ship GlobalInstance
    {
        get { return FindObjectOfType<Ship>(); }
    }

	// Use this for initialization
	void Start () {
        m_rooms.AddRange(GetComponentsInChildren<Room>());
	}

    public void Init()
    {
        if (Network.isServer)
            StartCoroutine(EmergencyPolling());
    }
	
	// Update is called once per frame
	void Update () 
    {
	}
}
