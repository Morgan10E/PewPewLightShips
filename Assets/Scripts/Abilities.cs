using UnityEngine;
using System.Collections;

public class Abilities : MonoBehaviour {

	public Ability[] abilities;

	// Use this for initialization
	void Start () {
		abilities = new Ability[3];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		bool QPressed = Input.GetKeyDown (KeyCode.Q);
		bool EPressed = Input.GetKeyDown (KeyCode.E);
		bool RPressed = Input.GetKeyDown (KeyCode.R);

		if (QPressed) {
			Debug.Log ("Q Pressed");
			Activate (0);
		} else if (EPressed) {
			Debug.Log ("E Pressed");
			Activate (1);
		} else if (RPressed) {
			Debug.Log ("R Pressed");
			Activate (2);
		}
	}

	public void Activate(int index) {
		abilities [index].Activate ();
	}

	public void SetAbilities(Ability first, Ability second, Ability third) {
		abilities [0] = first;
		abilities [1] = second;
		abilities [2] = third;
	}
}
