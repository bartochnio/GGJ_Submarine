﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Part : MonoBehaviour {

	public int partID = -1;

	bool used = false;

	enum PartState{
		IDLE,
		DRAGED,
		READY,
		FIXED
	}

	int allBolts = 0;
	int currentBalts = 0;

    Plane castPlane;

	IEnumerator GoStraight() {
		while (transform.rotation != Quaternion.identity) {
			transform.rotation =  Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis( 180f, Vector3.up), 90f * Time.deltaTime);

			yield return new WaitForEndOfFrame();

		}
	}

	PartState state = PartState.IDLE;

	public void BoltScrewd () {
		currentBalts++;
		if (allBolts == currentBalts) {
			rPoint.Done = true;
		}
	}

	// Use this for initialization
	void Start () {

		transform.Rotate(transform.forward, 45f);

		rigidbody2D.isKinematic = true;
		collider2D.isTrigger = true;

		allBolts = GetComponentsInChildren<Bolt>().Length;

		StartCoroutine(GoStraight());

        castPlane = new Plane(-Vector3.forward, Vector3.zero);


	}

    Vector3 GetPositionOnPlane()
    {
        Camera c = GameObject.FindGameObjectWithTag("AssemblyCam").camera;
        if (c == null) c = Camera.main;

        Ray r = c.ScreenPointToRay(Input.mousePosition);

        float d = 0;

        castPlane.Raycast(r, out d);

        return r.GetPoint(d);
        
    }

	void OnMouseDown() {

		switch(state) {
		case PartState.IDLE:
			state = PartState.DRAGED;
			break;
		case PartState.DRAGED:
			state = PartState.IDLE;
			break;
		case PartState.READY:
			state = PartState.FIXED;

			foreach( var b in GetComponentsInChildren<Bolt>()){
				b.Activate();
			}
			//Destroy(this);
			// enable bolts

			break;

		}

	}

	AssemblyPoint rPoint = null;

	void OnTriggerEnter2D(Collider2D col) {

		AssemblyPoint p = col.GetComponent<AssemblyPoint>();

		if (p == null || state == PartState.FIXED) return;

		else if (p.partID == this.partID) {
			transform.position = p.transform.position;
			state = PartState.READY;
			rPoint = p;
			rPoint.collider2D.enabled = false;
		}


	}

    
    


	void Update() {

        //if (state == PartState.FIXED)
        //{
        //    EmergencyMgr.GlobalInstance.OnEmergencyFinished();
        //    return;
        //}

        //else 
            if (state == PartState.DRAGED)
        {
            Vector3 point = GetPositionOnPlane();
            
            transform.position = point;
        }
        else if (state == PartState.READY)
        {

            if (Vector3.Distance(transform.position, rPoint.transform.position) > 0.25f)
            {
                Vector3 point = GetPositionOnPlane();
               
                transform.position = point;
                rPoint = null;
                state = PartState.DRAGED;
            }
        }


	}


}
