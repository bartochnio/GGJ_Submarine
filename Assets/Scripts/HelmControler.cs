using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(FirstPersonMotor))]
public class HelmControler : MonoBehaviour {

	FirstPersonMotor motor = null;
	Canvas helmCanvas = null;

	Slider vSlide;
	Slider hSlide;


	void Start() {

		motor = GetComponent<FirstPersonMotor> ();
		helmCanvas = GameObject.Find("HelmCanvas").GetComponent<Canvas> ();
		//helmCanvas.enabled = false;

		vSlide = GameObject.Find ("VertSlider").GetComponent<Slider> ();
		hSlide = GameObject.Find ("HorSlider").GetComponent<Slider> ();

		vSlide.value = 0.5f;
		hSlide.value = 0.5f;
		}

	// Update is called once per frame
	void Update () {
	
		float v = -1 + (vSlide.value * 2);
		float h = -1 + (hSlide.value * 2);

		motor.SetHorAxis (h);
		motor.SetVeAxis (v);

	}
}
