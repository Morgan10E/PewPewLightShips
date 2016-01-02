using UnityEngine;
using System.Collections;

public class Abilities : MonoBehaviour {

	public Ability[] abilities;

	// Use this for initialization
	public Abilities() {
		abilities = new Ability[3];
	}

//	public void SetAbilities(Ability[] abilities) {
//		this.abilities = abilities;
//	}
//
//	public void SetAbility(int index, Ability ability) {
//		abilities [index] = ability;
//	}

	public void Activate(int index) {
		abilities [index].Activate ();
	}
}
