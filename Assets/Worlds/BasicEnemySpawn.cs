using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BasicEnemySpawn : NetworkBehaviour {

	public GameObject enemyPrefab;
	MapGenerator mapGen;

	void Awake() {
		mapGen = GameObject.Find ("MapGenerator").GetComponent<MapGenerator> ();
	}

	public override void OnStartServer () {
		List<Vector2> spawns = mapGen.GetEnemySpawnLocation ();
		for (int i = 0; i < spawns.Count; i++) {
			SpawnEnemy (spawns [i]);
		}
	}

	void SpawnEnemy(Vector2 location) {
		GameObject enemy = GameObject.Instantiate (enemyPrefab, new Vector3 (location.x, location.y), Quaternion.identity) as GameObject;
		NetworkServer.Spawn (enemy);
	}

}
