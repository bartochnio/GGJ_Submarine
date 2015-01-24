using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {


    private RoomEmergency m_status;

    public RoomEmergency Status
    {
        get { return m_status; }
        set {
            switch (value)
            {
                case RoomEmergency.NONE:
                    gameObject.SetColor("Green");
                    break;
                default:
                    gameObject.SetColor("Red");
                    break;
            }
            m_status = value; 
        }
    }
    public RoomType m_type;
    public Vector3 m_center;
    public bool m_containsPlayer = false;

    public List<Room> m_neighbours = new List<Room>();

    public enum RoomEmergency
    {
        NONE,
        FLOODING,
        DESTRoYED,
        BROKEN
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

	void Start () 
    {
        m_center = transform.position + m_center;
        Status = RoomEmergency.NONE;
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
            m_containsPlayer = true;
        }
    }

	
    void Update () 
    {   
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
