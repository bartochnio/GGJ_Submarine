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
    public Renderer patchRendere;

    INVENTORY_ITEM item = INVENTORY_ITEM.NONE;

    public INVENTORY_ITEM Item
    {
        get { return item; }
        
    }

    public void DropInventoryItem()
    {
        item = INVENTORY_ITEM.NONE;
        wungielRender.enabled = false;
        patchRendere.enabled = false;
        //Visualisation
    }
    public void GetInventoryItem(INVENTORY_ITEM item)
    {
        DropInventoryItem();
        switch (item)
        {
            case INVENTORY_ITEM.NONE:
                wungielRender.enabled = false;
                patchRendere.enabled = false;
                break;
            case INVENTORY_ITEM.COAL:
                wungielRender.enabled = true;
                patchRendere.enabled = false;
                break;
            case INVENTORY_ITEM.PATCH:
                patchRendere.enabled = true;
                wungielRender.enabled = false;
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

    public void SetMoving(Room dest)
    {
        if (dest != currentRoom && dest != destination)
        {
            m_path.Clear();
            m_path = PathFinder.FindPath(currentRoom, dest);
            if (m_path.Count > 0)
            {
                m_state = State.MOVING;
                this.destination = dest;

                m_curWaypoint = m_path.Pop();
            }
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (!networkView.isMine)
        {
            syncTime += Time.deltaTime;
            transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        }

	    if (m_state == State.MOVING)
        {
            Move();
        }

        if (currentRoom != null && currentRoom.Status != Room.RoomEmergency.NONE)
        {
            PlayerController.GlobalInstance.Repair(this, currentRoom);
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Hit from crew");
    }

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        int syncItem = (int)INVENTORY_ITEM.NONE;
        if (stream.isWriting)
        {
            syncPosition = transform.position;
            syncItem = (int)item;
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncItem);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncItem);

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncStartPosition = transform.position;
            syncEndPosition = syncPosition;
            GetInventoryItem((INVENTORY_ITEM)syncItem);
        }
    }

    void Move()
    {
      transform.position = Vector3.MoveTowards(transform.position, m_curWaypoint.m_center, speed * Time.deltaTime);

      if (Vector3.Distance(transform.position, m_curWaypoint.m_center) < 0.0001f)
      {

          if (m_path.Count == 0)
          {
              currentRoom.m_containsPlayer = false;
              if (currentRoom.Status != Room.RoomEmergency.NONE)
              {
                  //PlayerController.GlobalInstance.Repair(this, destination);
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
