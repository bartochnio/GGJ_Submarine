using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class AssemblyPoint : MonoBehaviour {

	// Have to the same as the part to allow it to be fixed at this position
	public int partID = -1;

	public bool done = false;

	void Start() {
		collider2D.isTrigger = true;

	}
		
}

