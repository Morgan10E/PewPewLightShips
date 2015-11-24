using UnityEngine;
using System.Collections;

public class TeamManager : MonoBehaviour {

	public int numPlayerTeams = 2;
	private int numCurrentPlayers = 0;

	// Use this for initialization
	void Start () {
		// TODO: is there any initialization that should happen here?
	}

	public void AssignTeam(GameObject player) {
		//Debug.Log (player);
		if (player != null) {
			player.GetComponent<TeamIdentity> ().SetTeam (numCurrentPlayers % numPlayerTeams);
			numCurrentPlayers += 1;
		}
	}
}
