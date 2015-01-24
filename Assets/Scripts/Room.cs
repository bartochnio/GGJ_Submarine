﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {


    private RoomEmergency m_status;
    static private int globalID = 0;

    private Flood flood;
    private float floodCounter = 0.0f;

    public int id;
    public float timerToFlood = 10.0f;
    public CrewMember curPlayer;

    public RoomEmergency Status
    {
        get { return m_status; }
        set {
            switch (value)
            {
                case RoomEmergency.FLOODING:
                    gameObject.SetColor("Red");
                    flood.StartFlood();
                    break;
                case RoomEmergency.BROKEN:
                    gameObject.SetColor("Yellow");
                    break;
                default:
                    gameObject.SetColor("Green");
                    break;
            }
            m_status = value; 
        }
    }
    public RoomType m_type;
    public Vector3 m_center;
    public bool m_containsPlayer = false;

    public List<Room> m_neighbours = new List<Room>();

    public Door top;
    public Door bottom;
    public Door left;
    public Door right;

    public List<Room> GetOpenNeighbours()
    {
        List<Room> result = new List<Room>();

        if (top != null && top.open)
        {
            Room r = top.GetOther(this);
            if (r.IsRoomAvailable())
                result.Add(r);
        }

        return result;
    }

    public enum RoomEmergency
    {
        NONE,
        FLOODING,
        BROKEN,
        DESTRoYED
    }

    public bool isRepaired = false;

    public enum RoomType
    {
        HELM,
        ENGINE,
        PING,
        TURRET,
        EMPTY
    }

    bool IsRoomAvailable()
    {
        return m_status != RoomEmergency.DESTRoYED;
    }

	void Start () 
    {
        id = ++globalID;

        m_center = transform.position + m_center;
        Status = RoomEmergency.NONE;

        flood = GetComponentInChildren<Flood>();
	}

    public bool IsRoomCleared()
    {
        return flood.IsCleared();
    }

    public void PumpWater()
    {
        flood.Pump();
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        //Debug.Log("Hit from room");

        if (col.tag == "Player")
        {
            CrewMember mem = col.GetComponent<CrewMember>();
            if (mem.currentRoom != null)
                    mem.currentRoom.m_containsPlayer = false;
            mem.currentRoom = this;
            curPlayer = mem;
            m_containsPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        curPlayer = null;
    }

    void Update () 
    {   
        switch(m_status)
        {
            case RoomEmergency.BROKEN:
                floodCounter += Time.deltaTime;
                if (floodCounter >= timerToFlood)
                {
                    if (!m_containsPlayer)
                        Ship.GlobalInstance.RemoteCallRoomStateChange(RoomEmergency.FLOODING, id);
                    else
                        floodCounter = 0.0f;
                }
                break;

            default:
                floodCounter = 0.0f;
                break;
        }
	}

    void OnMouseDown()
    {
        CrewMember crewMember = PlayerController.GlobalInstance.SelectedCrewMember;

        if (!PlayerController.GlobalInstance.isBusy) crewMember.SetMoving(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + m_center, 0.25f);
    }

}
