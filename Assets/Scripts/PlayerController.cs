using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    List<CrewMember> crew;
    CrewMember m_currentMember;


    public CrewMember SelectedCrewMember
    {
        get { return m_currentMember; }
        
    }

    public static PlayerController GlobalInstance
    {
        get { return FindObjectOfType<PlayerController>(); }
    }

	// Use this for initialization
	void Start () 
    {
        crew = new List<CrewMember>(FindObjectsOfType<CrewMember>());
        m_currentMember = crew[0];
	}

    public void Repair(CrewMember m,  Room r)
    {
        if (r.Status != Room.RoomEmergency.NONE && !r.isRepaired)
        {
            r.isRepaired = true;
            RepairAction.Create(r);
        }
    }
	
    //ToDo: Later check if controller is busy no member
    public bool isBusy
    {
        get { return SelectedCrewMember.Status == CrewMember.State.ACTIVE; }
    }


	// Update is called once per frame
	void Update () 
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_currentMember.gameObject.SetColor("Gray");
            m_currentMember = crew[0];
            m_currentMember.gameObject.SetColor("Red");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_currentMember.gameObject.SetColor("Gray");
            m_currentMember = crew[1];
            m_currentMember.gameObject.SetColor("Red");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_currentMember.gameObject.SetColor("Gray");
            m_currentMember = crew[2];
            m_currentMember.gameObject.SetColor("Red");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_currentMember.gameObject.SetColor("Gray");
            m_currentMember = crew[3];
            m_currentMember.gameObject.SetColor("Red");
        }
	}
}
