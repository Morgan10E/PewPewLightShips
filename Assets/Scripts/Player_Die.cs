using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Die : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!isServer)
			return;

		CheckPlayerHealth ();
	}

	void CheckPlayerHealth() {
		if (GetComponent<Player_Health> ().GetHealth () <= 0) {
			//Debug.Log ("killing player");
			RpcKillPlayer();

			// if it was a player character, respawn them after a timer
			// TODO: make this more flexible?
			if (gameObject.tag == "Ship") {
				Invoke ("RespawnPlayer", 3f);
			}
		}
	}

	void RespawnPlayer() {
		RpcRespawnPlayer ();
	}


	[ClientRpc]
	void RpcKillPlayer()
	{
		// disable the renderer
		GetComponent<SpriteRenderer> ().enabled = false;

		// disable the colliders
		foreach(Collider2D c in GetComponents<Collider2D> ()) {
			c.enabled = false;
		}

		// disable the health bar
		GetComponent<HealthBarPositionScript> ().RemoveHealthBarSprite ();
		//GetComponent<HealthBarPositionScript> ().enabled = false;

		if (isLocalPlayer) {
			// disable the ship control
			GetComponent<ShipControl>().enabled = false;
			// disable bullets
			GetComponent<Player_Fire>().enabled = false;
		}
	}

	[ClientRpc]
	void RpcRespawnPlayer()
	{
		// reset the health value, TODO: have this called when the respawn occurs
		GetComponent<Player_Health> ().ResetHealth ();

		// enable the renderer
		GetComponent<SpriteRenderer> ().enabled = true;
		
		// enable the colliders
		foreach(Collider2D c in GetComponents<Collider2D> ()) {
			c.enabled = true;
		}


		
		// enable the health bar
		//GetComponent<HealthBarPositionScript> ().enabled = true;
		GetComponent<HealthBarPositionScript> ().EnableHealthBarSprite ();


		
		if (isLocalPlayer) {
			// enable the ship control
			GetComponent<ShipControl>().enabled = true;
			// enable bullets
			GetComponent<Player_Fire>().enabled = true;
		}
	}
}
