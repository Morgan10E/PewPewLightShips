using UnityEngine;
using System.Collections;

public class NetworkManagerServer : MonoBehaviour {

	public GameObject playerPrefab;

	private const string typeName = "PewPewLaserShips";
	private const string gameName = "defaultRoom";

	public const int PLAYER_GROUP = 0;

	private bool processSpawnRequests = false;

	private ArrayList scheduledSpawns;
	private ArrayList playerTracker;

	private void StartServer() {
		MasterServer.ipAddress = "127.0.0.1"; // if you host the master server manually
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		Debug.Log("Server started");
	}

	void OnGUI() {
		if (!Network.isClient && !Network.isServer) {
			if (GUI.Button (new Rect(100, 100, 250, 100), "Start Server")) StartServer();
		}
	}

	// Use this for initialization
	void Start () {
		if (Network.isClient) return;
		scheduledSpawns = new ArrayList();
		playerTracker = new ArrayList();

		//StartServer();
	}

	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log ("Spawning prefab for new client");
		scheduledSpawns.Add(player);
		processSpawnRequests = true;
	}

	[RPC]
	void RequestSpawn(NetworkPlayer requester) {
		if (Network.isClient) {
			return;
		}

		Debug.Log ("Preparing to spawn player");

		if (!processSpawnRequests) return;

		foreach (NetworkPlayer spawn in scheduledSpawns) {
			int number = int.Parse(spawn + "");
			GameObject handle = Network.Instantiate(playerPrefab, transform.position, Quaternion.identity, PLAYER_GROUP) as GameObject;
			PlayerControlClient clientHandle = handle.GetComponent<PlayerControlClient>();
			playerTracker.Add(clientHandle);
			NetworkView netView = handle.GetComponent<NetworkView>();
			netView.RPC("SetOwner", RPCMode.AllBuffered, spawn);
		}

		scheduledSpawns.Remove(requester);
		if (scheduledSpawns.Count == 0) {
			processSpawnRequests = false;
		}
	}
}
