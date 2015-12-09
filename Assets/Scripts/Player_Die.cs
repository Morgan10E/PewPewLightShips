using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Die : NetworkBehaviour {

	TeamManager teamManager;
	bool spawning = false;

	// Use this for initialization
	void Start () {
		teamManager = GameObject.Find ("TeamManager").GetComponent<TeamManager> ();
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
			if (gameObject.tag == "Ship" && !spawning) {
				spawning = true;
				StartCoroutine(RespawnAfterTime(3));
			}
		}
	}

	IEnumerator RespawnAfterTime(float time)
	{
		yield return new WaitForSeconds(time);

		// Code to execute after the delay
		RespawnPlayer ();
	}

	void RespawnPlayer() {
		Debug.Log ("Spawning player");
		RpcRespawnPlayer ();
	}

	void SetSpriteRendering(bool visible) {
		SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer sr in spriteRenderers) {
			sr.enabled = visible;
		}
	}

	[ClientRpc]
	void RpcKillPlayer()
	{
		// disable the renderer
		//GetComponent<SpriteRenderer> ().enabled = false;
		SetSpriteRendering (false);

		// disable the trail renderer
		GetComponent<Player_Trail>().DisableTrail();

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

		// move to the respawn point
		teamManager.SetRespawnPoint (this.gameObject);

	}

	[ClientRpc]
	void RpcRespawnPlayer()
	{

		// reset the health value, TODO: have this called when the respawn occurs
		GetComponent<Player_Health> ().ResetHealth ();

		// enable the renderer
		SetSpriteRendering (true);

		// enable the trail renderer
		GetComponent<Player_Trail>().EnableTrail();
		
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
		spawning = false;
	}
}
