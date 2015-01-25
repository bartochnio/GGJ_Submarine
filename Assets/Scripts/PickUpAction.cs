using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PickUpAction : MonoBehaviour {


    public Room myRoom;

    public INVENTORY_ITEM item = INVENTORY_ITEM.NONE;

   

    public void PickUp()
    {
        CrewMember m = PlayerController.GlobalInstance.SelectedCrewMember;
        if (m.currentRoom == myRoom )
        {
            m.GetInventoryItem(item);
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Click");
        PickUp();
    }

    

	// Use this for initialization
	void Start () {

       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
public enum INVENTORY_ITEM
{
    NONE,
    COAL,
    PATCH
}