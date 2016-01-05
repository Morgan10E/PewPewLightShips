using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkSpawner : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[Command]
	public void CmdSpawnObject(GameObject obj) {
		NetworkServer.Spawn (obj);
	}

	[ClientCallback]
	public void registerSpawn(GameObject obj) {
		CmdSpawnObject (obj);
	}
		
}
