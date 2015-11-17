﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_ID : NetworkBehaviour {

	[SyncVar] string playerUniqueIdentity;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;

	public override void OnStartLocalPlayer() {
		GetNetIdentity ();
		SetIdentity ();
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
}
