using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {

	[SerializeField] Camera PlayerCam;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			//GameObject.Find("Main Camera").SetActive(true);
			GetComponent<ShipControl>().enabled = true;
			GetComponent<Player_Fire>().enabled = true;
			PlayerCam.enabled = true;
		}
	}

}
