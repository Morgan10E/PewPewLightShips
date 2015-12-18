using UnityEngine;
using System.Collections;

public class SetupSpawn : MonoBehaviour {

	TeamManager teamManager;

	// Use this for initialization
	void Awake () {
		teamManager = GameObject.Find ("TeamManager").GetComponent<TeamManager> ();
	}

	void Start() {
		teamManager.RegisterSpawn (this.gameObject);
	}
}
