using UnityEngine;
using System.Collections;

public class WasserMotor : MonoBehaviour {

    public float tankSize = 10f;
    public float burn = 0.2f;
    public float currentAmount;
    public float wungielFuelAmount = 5f;

    IEnumerator BurnFuel()
    {
        while (true)
        {
            currentAmount -= burn * Time.deltaTime;
            if (currentAmount <= 0)
            {
                currentAmount = 0;
                Debug.Log("END OF FUEL");
            }
            Debug.Log("Fuel Left " + currentAmount);
            yield return new WaitForEndOfFrame();
        }
    }

	// Use this for initialization
	void Start () {
        currentAmount = tankSize;
        StartCoroutine(BurnFuel());
	}

    public Room myRoom;

    void OnMouseDown()
    {
        CrewMember c = PlayerController.GlobalInstance.SelectedCrewMember;

        if (c.currentRoom == myRoom && c.Item == INVENTORY_ITEM.COAL){
            currentAmount += wungielFuelAmount; 
            if (currentAmount > tankSize )currentAmount = tankSize;
            c.DropInventoryItem();
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
