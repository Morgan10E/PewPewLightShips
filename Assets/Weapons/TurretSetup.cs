using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TurretSetup : NetworkBehaviour {

	TurretControl control;
	Weapon weapon;
	NetworkIdentity parentIdentity;
	bool local;

	// store references to everything we need
	void Awake() {
		control = GetComponent<TurretControl> ();
		weapon = GetComponent<Weapon> ();
		parentIdentity = transform.parent.GetComponent<NetworkIdentity> ();
	}

	// Use this for initialization
	void Start () {
		local = parentIdentity.isLocalPlayer;

		if (local) {
			control.enabled = true;
			weapon.enabled = true;
		} else {
			/* Do nothing */
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
