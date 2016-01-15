using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Attach_Trail : NetworkBehaviour {

	Spawn_Trails trailSpawner;
	private int trailNum;
	TeamIdentity team;

	// Use this for initialization
	void Awake () {
		// Store a reference to the TrailSpawner
		trailSpawner = GameObject.Find("TrailPool").GetComponent<Spawn_Trails>();
		team = GetComponent<TeamIdentity> ();
		//Debug.Log("Bullet created");
	}

	void Start() {
		// Instantiate a trail
		if (isServer) {
		//	trailNum = trailSpawner.AttachTrail (this.gameObject);
			RpcAttachTrail();
		}
	}

	void OnDestroy() {
		foreach (Transform child in this.transform) {
			child.parent = null;
		}
		trailSpawner.ReturnTrail (trailNum);
	}

	[ClientRpc]
	void RpcAttachTrail() {
		trailNum = trailSpawner.AttachTrail (this.gameObject);
		int teamNum = team.GetTeam ();
		if (trailNum >= 0 && teamNum >= 0) {
			trailSpawner.trailList [trailNum].GetComponent<ParticleSystem> ().startColor = team.teamColors [teamNum];
		}
	}

}
