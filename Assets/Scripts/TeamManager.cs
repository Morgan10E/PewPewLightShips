using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamManager : MonoBehaviour {

	public int numPlayerTeams = 2;
	private int numCurrentPlayers = 0;
	List<GameObject> respawns;

	// Use this for initialization
	void Awake () {
		// TODO: is there any initialization that should happen here?
		respawns = new List<GameObject>();
	}

	public void AssignTeam(GameObject player) {
		//Debug.Log (player);
		if (player != null) {
			player.GetComponent<TeamIdentity> ().SetTeam (numCurrentPlayers % numPlayerTeams);
			numCurrentPlayers += 1;
		}
	}

	public void RegisterSpawn(GameObject spawn/*, int teamNum*/) {
		this.respawns.Add (spawn);
		Debug.Log ("Num Spawns: " + respawns.Count);
	}

	public void SetRespawnPoint(GameObject playerObj) {
		int teamNum = playerObj.GetComponent<TeamIdentity> ().GetTeam ();
		Vector3 spawnPosition = respawns [teamNum].transform.position;
		playerObj.GetComponent<Rigidbody2D>().MovePosition(spawnPosition);
	}
}
