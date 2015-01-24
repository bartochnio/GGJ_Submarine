using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class AssemblyPoint : MonoBehaviour {

	void Start() {
		collider2D.isTrigger = true;

	}
		
}

