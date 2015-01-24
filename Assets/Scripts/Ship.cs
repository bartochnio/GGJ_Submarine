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
                r.Status = Room.RoomEmergency.DESTRoYED;
            }

            if (Input.GetKeyDown(KeyCode.F12)) yield break;

            yield return new WaitForSeconds(Random.Range(minTimeToEmergency, maxTimeToEmergency));
        }
    }

	// Use this for initialization
	void Start () {
        eventTimer = Random.Range(1.0f, 5.0f);

        StartCoroutine(EmergencyPolling());
        m_rooms.AddRange(GetComponentsInChildren<Room>());

	}
	
	// Update is called once per frame
	void Update () 
    {
	}
}
