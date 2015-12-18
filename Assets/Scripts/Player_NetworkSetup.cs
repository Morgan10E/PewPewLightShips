using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {

	[SerializeField] Camera PlayerCam;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			//GameObject.Find("Main Camera").SetActive(true);
			GetComponent<ShipControl> ().enabled = true;
			GetComponent<Player_Fire> ().enabled = true;
			GetComponent<PlayerGui> ().enabled = true;
			PlayerCam.enabled = true;
		} else {
			// enable health bars
			GetComponent<HealthBarPositionScript>().enabled = true;
		}
		// enable the trail renderer
		this.gameObject.GetComponent<Player_Trail>().AddTrail();
	}

	void OnDestroy() {
		if (GetComponent<HealthBarPositionScript> ( )) {
			GetComponent<HealthBarPositionScript>().DestroyHealthBar();
		}
		Debug.Log ("Player Disconnected");
	}

}
