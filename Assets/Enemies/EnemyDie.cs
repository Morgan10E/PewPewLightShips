using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyDie : NetworkBehaviour {

	// Update is called once per frame
	void FixedUpdate () {
		if (!isServer)
			return;
		CheckPlayerHealth ();
	}

	void CheckPlayerHealth() {
		if (GetComponent<Player_Health> ().GetHealth () <= 0) {
			//Debug.Log ("killing player");
			//RpcKillEnemy();
			GetComponent<HealthBarPositionScript> ().RemoveHealthBarSprite ();
			Destroy(this.gameObject);
		}
	}

}
