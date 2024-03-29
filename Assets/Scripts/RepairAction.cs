﻿using UnityEngine;
using System.Collections;

public class RepairAction : MonoBehaviour {

    public float rep4irTiMe = 3f;
    public Room repairedRoom = null;
    public Canvas c;
    KeyCode repairKey;
  
    CrewMember repairMan = null;

	// Use this for initialization
	void Start () {

        int r = Random.Range(0, 20);
       

    }

    public static RepairAction Create(Room r, float reptime)
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

        if (repairMan.currentRoom == repairedRoom )
        {
            //TODO: Dojebac obsluge stanu pokoju, dany stan = inna korutyna
            if (repairedRoom.Status == Room.RoomEmergency.FLOODING)
            {
                repairMan.Status = CrewMember.State.ACTIVE;
                StartCoroutine(Pump());
            }
            else if (repairMan.Item == INVENTORY_ITEM.PATCH)
            {
                repairMan.Status = CrewMember.State.ACTIVE;
                repairMan.DropInventoryItem();
                EmergencyMgr.GlobalInstance.StartRepair();
                StartCoroutine(Repair());
            }
        }
    }

    IEnumerator Pump()
    {
        while (true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (Network.isServer)
                    repairedRoom.PumpWater();
                else
                    Ship.GlobalInstance.RemoteCallPumpWater(repairedRoom.id);
            }

            if (repairedRoom.IsRoomCleared())
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

    IEnumerator Repair()
    {
        while (true)
        {
            if (!EmergencyMgr.GlobalInstance.isWorking)
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
