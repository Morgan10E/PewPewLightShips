using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class OnHitBullet : NetworkBehaviour {

	private float damage = 10f;
	BulletPayload payload;
	ScoreController scoreController;

	void Awake() {
		payload = GetComponent<BulletPayload> ();
		scoreController = GameObject.Find ("Scoreboard").GetComponent<ScoreController> ();
		if (scoreController != null)
			Debug.Log ("score controller found");
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
		

	void OnTriggerEnter2D (Collider2D col)
	{
		if (!isServer)
			return;
		TeamIdentity myTeam = GetComponent<TeamIdentity> ();
		TeamIdentity colTeam = col.gameObject.GetComponent<TeamIdentity> ();
		if (myTeam != null && colTeam != null && myTeam.GetTeam() == colTeam.GetTeam()) {
			return;
		}
		Player_Health playerHealth = col.gameObject.GetComponent<Player_Health> ();
		if (playerHealth != null) {
			bool isDead = col.gameObject.GetComponent<Player_Die> ().isDead;
			playerHealth.TakeDamage(damage);
			if (playerHealth.GetHealth () <= 0f && !isDead) {
				
				// add to the score of the team of the bullet
				int teamNum = myTeam.GetTeam();
				scoreController.teamScored (teamNum);
				RpcUpdateScore (teamNum);
			}
		}
		DestroyOnCollision (this.gameObject);
	}

	[Command]
	void CmdDestroyBullet(GameObject obj) {
		NetworkServer.Destroy (obj);
	}

	[ClientRpc]
	void RpcUpdateScore(int teamNum) {
		scoreController.teamScored(teamNum);
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
