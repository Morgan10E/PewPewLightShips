using UnityEngine;
using System.Collections;

public class DemoAbility : Ability {
	private GameObject ship;

	public DemoAbility() {
		this.ship = null;
	}

	public DemoAbility(GameObject ship){
		this.ship = ship;
	}

	public override void Activate() {
		Debug.Log ("ABILITY ACTIVATED");
	}

}
