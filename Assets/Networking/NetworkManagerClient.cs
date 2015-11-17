using UnityEngine;
using System.Collections;

public class NetworkManagerClient : MonoBehaviour {

	private const string typeName = "PewPewLaserShips";

	void OnConnectedToServer() {
		Debug.Log ("Connected To Server");
		Network.isMessageQueueRunning = false; // disables message queue
		// load the level
		GameObject loaderObject = GameObject.FindGameObjectWithTag("Loader");
		Loader loader = loaderObject.GetComponent<Loader>();
		loader.LoadMap();
		
		if (Network.isClient) {
			Network.isMessageQueueRunning = true; // re-enable message queue
			// do this so level is loaded first
			GetComponent<NetworkView>().RPC ("RequestSpawn", RPCMode.Server, Network.player);
		}
	}
	
	void OnGUI() {
		if (!Network.isClient && !Network.isServer) {
			MasterServer.ipAddress = "127.0.0.1"; // if you host the master server manually
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null) {
				for (int i = 0; i < hostList.Length; i++) {
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	private HostData[] hostList;
	
	private void RefreshHostList() {
		//MasterServer.ipAddress = "127.0.0.1"; // if you host the master server manually
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent) {
		if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList();
		}
	}
	
	private void JoinServer(HostData hostData) {
		Network.Connect(hostData);
	}
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
