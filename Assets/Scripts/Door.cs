using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    public Room room1;
    public Room room2;
    public bool open;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {

        Gizmos.color = Color.black;
        Gizmos.DrawLine(room1.transform.position, room2.transform.position);
    }


    public Room GetOther(Room r)
    {
        return r == room1 ? room2 : room1;
    }

    void OnMouseDown()
    {
        CrewMember crewMember = PlayerController.GlobalInstance.SelectedCrewMember;

        if (room1.curPlayer == crewMember || room2.curPlayer == crewMember)
        {
            open = !open;
        }
    }
}
