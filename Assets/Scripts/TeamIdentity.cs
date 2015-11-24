using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TeamIdentity : NetworkBehaviour {

	[SyncVar]
	public int teamNum = -1;

	public void SetTeam(int num) {
		teamNum = num;
	}

	public int GetTeam() {
		return teamNum;
	}
}
