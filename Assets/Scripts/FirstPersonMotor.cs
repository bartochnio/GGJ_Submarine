using UnityEngine;
using System.Collections;

public class FirstPersonMotor : MonoBehaviour {

	public float maxVertAcc;
	public float maxVertVelo;

	public float maxHorAcc;
	public float maxHorVelo;

	public float drag;

	float currentHorAcc;
	float currentVertAcc;

	float currentHorVelo;
	float currentVertVelo;



	public void SetVeAxis(float a){
		a = Mathf.Clamp (a, -1, 1);

		currentVertAcc = a * maxVertAcc;
	}

	public void SetHorAxis(float a) {

		 a = Mathf.Clamp (a, -1, 1);
		
		currentHorAcc = a * maxVertAcc;
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	// Update is called once per frame
	void Update () {
	



		currentHorVelo += currentHorAcc * Time.deltaTime;

		currentHorVelo = Mathf.Clamp (currentHorVelo, -maxHorVelo, maxHorVelo);

		currentVertVelo += currentVertAcc * Time.deltaTime;
		currentVertVelo = Mathf.Clamp (currentVertVelo, -maxVertVelo, maxVertVelo);

		// Drag
		currentHorVelo = Mathf.MoveTowards (currentHorVelo, 0, drag * Time.deltaTime);
		currentVertVelo = Mathf.MoveTowards (currentVertVelo, 0, drag * Time.deltaTime);

		transform.Rotate(new Vector3(currentVertVelo * Time.deltaTime, currentHorVelo * Time.deltaTime, 0));
		Vector3 e = transform.rotation.eulerAngles;
		e.z = 0;
		transform.rotation =Quaternion.Euler( e );


	}
}
