using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public float reloadTime = 0.75f;
	bool readyToFire = true;
	public Object buletPrefab;
	public float fireOffset = 1.5f;
	public float recoil = 0.5f;
	public bool fireCannon = false;

	public void Fire(){
		if (readyToFire) {

			GameObject b = Instantiate(buletPrefab, transform.position + transform.up * fireOffset, transform.rotation)as GameObject;
			b.rigidbody.AddForce(transform.up * 900f, ForceMode.Impulse);
			Destroy(b, 1f);
		}
	}

	IEnumerator FireRoutine() {

		while (true) {
			if (fireCannon) {
				Fire();
				yield return StartCoroutine(ReloadRoutine());

			}
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator ReloadRoutine() {



		Vector3 recoilPos = transform.localPosition +  (transform.root.InverseTransformDirection( -transform.up)  * recoil);
		Vector3 orgPos = transform.localPosition;
		readyToFire = false;

		transform.localPosition = recoilPos;

		float s = (recoilPos - orgPos).magnitude / reloadTime;

		while (transform.localPosition != orgPos) {
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, orgPos, s * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
			readyToFire = true;



	}

	void Start() {
		StartCoroutine(FireRoutine());

	}
}
