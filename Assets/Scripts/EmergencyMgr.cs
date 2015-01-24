using UnityEngine;
using System.Collections;

public class EmergencyMgr : MonoBehaviour {

    public GameObject assemblyEm;
    public Camera shipCamera;
    public Camera assemblyCamera;

    public bool isWorking;
    public GameObject currEmergency;


    public static EmergencyMgr GlobalInstance
    {
        get { return FindObjectOfType<EmergencyMgr>(); }
    }

    public void StartRepair()
    {
        isWorking = true;
        currEmergency = Instantiate(assemblyEm) as GameObject;
        shipCamera.enabled = false;
    }

    public void OnEmergencyFinished()
    {
        isWorking = false;
        Destroy(currEmergency);
        shipCamera.enabled = true;
    }

	void Start () 
    {
        isWorking = false;
	}

	void Update () {
	
	}
}
