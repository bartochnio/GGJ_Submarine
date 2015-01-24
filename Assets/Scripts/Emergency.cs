using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Emergency : MonoBehaviour {

    List<AssemblyPoint> assemblyPoints = new List<AssemblyPoint>();

	// Use this for initialization
	void Start () {
        assemblyPoints.AddRange(GetComponentsInChildren<AssemblyPoint>());

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (assemblyPoints.FindAll(x => x.Done).Count == assemblyPoints.Count) EmergencyMgr.GlobalInstance.OnEmergencyFinished();

	}
}
