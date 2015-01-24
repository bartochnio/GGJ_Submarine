using UnityEngine;
using System.Collections;

public class RepairAction : MonoBehaviour {

    public float rep4irTiMe = 3f;
    public Room repairedRoom = null;
    public Canvas c;
    KeyCode repairKey;
    float currentR3p = 0;
    CrewMember repairMan = null;

	// Use this for initialization
	void Start () {

        int r = Random.Range(0, 20);
        repairKey = (KeyCode)r;

    }

    public static RepairAction Create(Room r, float reptime = 3f)
    {
        

        GameObject o = Instantiate(Resources.Load("Prefabs/TempRepairAction")) as GameObject;

        o.transform.root.position = r.transform.position;

        RepairAction repA = o.GetComponentInChildren<RepairAction>();
        repA.repairedRoom = r ;
        repA.rep4irTiMe = reptime;
        return repA;
    }

    bool isWorking = false;
    void OnMouseDown()
    {
        if (isWorking) return;
            repairMan = PlayerController.GlobalInstance.SelectedCrewMember;

        if (repairMan.currentRoom == repairedRoom)
        {
            repairMan.Status = CrewMember.State.ACTIVE;
            StartCoroutine(Repair());
        }
    }

    IEnumerator Repair()
    {
        while (true)
        {
            currentR3p += Time.deltaTime;

            if (currentR3p >= rep4irTiMe)
            {
                Ship.GlobalInstance.RemoteCallRoomStateChange(Room.RoomEmergency.NONE, repairedRoom.id);
                repairedRoom.isRepaired = false;
                repairMan.Status = CrewMember.State.IDLE;
                Destroy(transform.root.gameObject);
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
	
	// Update is called once per frame
	void Update () {

       

	}
}
