using UnityEngine;
using System.Collections;

public class Gatling : MonoBehaviour {

	public Gun[] guns;
	public float fireRound = 1f;
	float sep;
	bool isFiring = false;


	public void OpenFire(bool f) {
		if (f) {
			if (!isFiring){
			StartCoroutine(WarmUp());
				isFiring = true;
			}
		}else {
			foreach(var g in guns ) {
				g.fireCannon = false;

			}
			isFiring = false;
		}
	}

	IEnumerator WarmUp() {

		foreach (var g in guns) {
			g.fireCannon = true;
			yield return new WaitForSeconds(sep/guns.Length);
		}

	}


	void OnGUI() {

		Rect r = new Rect(Screen.width/2 - 1f, Screen.height/2 - 1f, 2f,2f);
		GUI.Box(r, "");

	}

	// Use this for initialization
	void Start () {
	

		sep = fireRound / guns.Length;

		foreach (var g in guns) {
			g.reloadTime = sep;
		}



	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)){
			OpenFire(!isFiring);
		}
	
	}
}
