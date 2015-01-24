using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Part : MonoBehaviour {

	public int idGroup = -1;

	bool used = false;

	enum PartState{
		IDLE,
		DRAGED,
		READY,
		FIXED
	}

	IEnumerator GoStraight() {
		while (transform.rotation != Quaternion.identity) {
			transform.rotation =  Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 360 * Time.deltaTime);

			yield return new WaitForEndOfFrame();

		}
	}

	PartState state = PartState.IDLE;

	// Use this for initialization
	void Start () {

		transform.Rotate(transform.forward, 45f);

		rigidbody2D.isKinematic = true;
		collider2D.isTrigger = true;

		StartCoroutine(GoStraight());

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
			Destroy(this);
			// enable bolts

			break;

		}

	}

	AssemblyPoint rPoint = null;

	void OnTriggerEnter2D(Collider2D col) {

		AssemblyPoint p = col.GetComponent<AssemblyPoint>();

		if (p == null || state == PartState.FIXED) return;

		else {
			transform.position = p.transform.position;
			state = PartState.READY;
			rPoint = p;
			rPoint.collider2D.enabled = false;
		}


	}

	void Update() {

		if (state == PartState.FIXED) return;

		else if (state == PartState.DRAGED) {
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			point.z = -1;
			transform.position = point;
		}else if (state == PartState.READY) {

			if (Vector3.Distance(transform.position, rPoint.transform.position) > 0.25f) {
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			point.z = -1;
			transform.position = point;
			rPoint = null;
			state = PartState.DRAGED;
			}
		}


	}


}
