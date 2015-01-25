using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewMember : MonoBehaviour {

    public Room destination;
    public Room currentRoom;
    public float speed;

    public enum State
    {
        IDLE,
        MOVING,
        ACTIVE
    }

    public Renderer wungielRender;
    Renderer patchRendere;

    INVENTORY_ITEM item = INVENTORY_ITEM.NONE;

    public INVENTORY_ITEM Item
    {
        get { return item; }
        
    }

    public void DropInventoryItem()
    {
        item = INVENTORY_ITEM.NONE;
        wungielRender.enabled = false;
        //Visualisation
    }
    public void GetInventoryItem(INVENTORY_ITEM item)
    {
        DropInventoryItem();
        switch (item)
        {
            case INVENTORY_ITEM.NONE:
                wungielRender.enabled = false;
                break;
            case INVENTORY_ITEM.COAL:
                wungielRender.enabled = true;
                break;
        }
        this.item = item;
    }
    public bool UseInventoryItem(INVENTORY_ITEM item)
    {
        if (this.item == item)
        {
            DropInventoryItem();
            return true;
        }
        else
        {
            return false;
        }
    }

    State m_state;

    public State Status
    {
        get { return m_state; }
        set { m_state = value; }
    }

    Stack<Room> m_path = new Stack<Room>();
    Room m_curWaypoint;

	// Use this for initialization
	void Start () 
    {
        m_state = State.IDLE;
        
	}

    public void SetMoving(Room destination)
    {
        if (destination != currentRoom)
        {
            m_path.Clear();
            m_path = PathFinder.FindPath(currentRoom, destination);
            if (m_path.Count > 0)
            {
                m_state = State.MOVING;
                this.destination = destination;

                m_curWaypoint = m_path.Pop();
            }
        }
    }

	// Update is called once per frame
	void Update () 
    {
	    if (m_state == State.MOVING)
        {
            Move();
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Hit from crew");
    }

    

    void Move()
    {
      transform.position = Vector3.MoveTowards(transform.position, m_curWaypoint.m_center, speed * Time.deltaTime);

      if (Vector3.Distance(transform.position, m_curWaypoint.m_center) < 0.0001f)
      {

          if (m_path.Count == 0)
          {
              currentRoom.m_containsPlayer = false;
              if (destination.Status != Room.RoomEmergency.NONE)
              {
                  PlayerController.GlobalInstance.Repair(this, destination);
                  m_state = State.ACTIVE;
              }
              
              m_state = State.IDLE;
          }
          else
          {
              m_curWaypoint = m_path.Pop();
          }
      }
    }
}
