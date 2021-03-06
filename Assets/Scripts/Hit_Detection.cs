﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Hit_Detection : NetworkBehaviour {

	//[SyncVar] int health = 100;
	private float damage = 10f;
	BulletPayload payload;

	void Awake() {
		payload = GetComponent<BulletPayload> ();
	}

	// Use this for initialization
	void Start () {
		if (payload != null)
			damage = payload.GetDamage ();
	}
	
	// Update is called once per frame
	void Update () {
		/* Empty */
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		TeamIdentity myTeam = GetComponent<TeamIdentity> ();
		TeamIdentity colTeam = col.gameObject.GetComponent<TeamIdentity> ();
		if (myTeam != null && colTeam != null && myTeam.GetTeam() == colTeam.GetTeam()) {
			return;
		}
		Player_Health playerHealth = col.gameObject.GetComponent<Player_Health> ();
		if (playerHealth != null) {
			playerHealth.TakeDamage(damage);
			if (playerHealth.GetHealth () <= 0f) {
				// add to the score of the team of the bullet
			}
		}
		DestroyOnCollision (this.gameObject);
	}

	[Command]
	void CmdDestroyBullet(GameObject obj) {
		NetworkServer.Destroy (obj);
	}

	[ClientCallback]
	void DestroyOnCollision(GameObject obj) {
		if (isServer) {
			CmdDestroyBullet (obj);
		} else {
			// destroy it locally?
			Destroy (obj);
		}
	}
	
}
