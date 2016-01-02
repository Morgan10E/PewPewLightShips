using UnityEngine;
using System.Collections;

public class Ability : ScriptableObject {
	private GameObject ship;

	public Ability() {
		//nothing
	}

	public Ability(GameObject ship) {
		this.ship = ship;
	}

	public virtual void Activate() {
		// Implement this
		Debug.Log("CALLING THE WRONG ACTIVATE");
	}
}
