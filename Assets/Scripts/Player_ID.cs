using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_ID : NetworkBehaviour {

	[SyncVar] string playerUniqueIdentity;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;

	public override void OnStartLocalPlayer() {
		GetNetIdentity ();
		SetIdentity ();
		AssignTeam ();

	}

	// Use this for initialization
	void Awake () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (myTransform.name == "" || myTransform.name == "Ship(Clone)") {
			SetIdentity();
		}
	}

	[Client]
	void AssignTeam() {
		CmdAssignPlayerTeam ();
	}

	[Client]
	void GetNetIdentity() {
		playerNetID = GetComponent<NetworkIdentity> ().netId;
		CmdTellServerMyIdentity (MakeUniqueIdentity());
	}
	
	void SetIdentity() {
		if (!isLocalPlayer) {
			myTransform.name = playerUniqueIdentity;
		} else {
			myTransform.name = MakeUniqueIdentity();
		}
	}

	string MakeUniqueIdentity() {
		string uniqueIdentity = "Player " + playerNetID.ToString ();
		return uniqueIdentity;
	}

	[Command]
	void CmdTellServerMyIdentity(string identity) {
		playerUniqueIdentity = identity;
	}

	[Command]
	void CmdAssignPlayerTeam() {
		if (GetComponent<TeamIdentity>().GetTeam() == -1 && GameObject.Find("TeamManager") != null)
			GameObject.Find("TeamManager").GetComponent<TeamManager>().AssignTeam(gameObject);
	}
}
