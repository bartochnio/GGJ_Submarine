using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FirstPersonMotor))]
public class TurretControl : MonoBehaviour {

	FirstPersonMotor motor;

	// Use this for initialization
	void Start () {
		motor = GetComponent<FirstPersonMotor>();
	}
	
	// Update is called once per frame
	void Update () {
	
		float v = Input.GetAxis("Mouse Y");
		float h = Input.GetAxis("Mouse X");

		Vector2 n = new Vector2(h, v).normalized;

		motor.SetHorAxis(n.x);
		motor.SetVeAxis(n.y);

	}
}
