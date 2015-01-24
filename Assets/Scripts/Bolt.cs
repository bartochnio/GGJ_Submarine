using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour {

	public bool _active = false;
	bool _fixed = false;
	public bool Fixed {
		get {return _fixed;}
	}

	public void Activate() {
		_active = true;
	
		gameObject.SetColor("Green");

	}


	void OnMouseDown() {
		if (_active && !Fixed) {
			_fixed = true;
			gameObject.SetColor("Red");
		}
	}

}
