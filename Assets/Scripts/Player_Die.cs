using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Die : NetworkBehaviour {

	TeamManager teamManager;
	bool spawning = false;
	public bool isDead = false;

	void Awake() {
		teamManager = GameObject.Find ("TeamManager").GetComponent<TeamManager> ();
	}

	// Use this for initialization
	void Start () {
		//teamManager.SetRespawnPoint (this.gameObject);
		transform.rotation = Quaternion.identity;
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
			isDead = true;

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
		isDead = false;
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


		//GetComponent<HealthBarPositionScript> ().enabled = false;

		if (isLocalPlayer) {
			// disable the ship control
			GetComponent<ShipControl> ().enabled = false;
			// disable bullets
			GetComponent<GunManager> ().enabled = false;
			// disable tracking shot
			GetComponent<TrackingShot>().enabled = false;

		} else {
			// disable the health bar
			GetComponent<HealthBarPositionScript> ().RemoveHealthBarSprite ();
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
		
		if (isLocalPlayer) {
			// enable the ship control
			GetComponent<ShipControl> ().enabled = true;
			// enable bullets
			GetComponent<GunManager> ().enabled = true;
			// re-enable tracking shot
			GetComponent<TrackingShot>().enabled = true;
		} else {
			// enable the health bar
			GetComponent<HealthBarPositionScript> ().EnableHealthBarSprite ();
		}
		spawning = false;
	}
}
