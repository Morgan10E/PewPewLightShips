using UnityEngine;
using System.Collections;

public class SpawnBebes : MonoBehaviour {

	public int maxBebes = 5;
	public GameObject bebeType;
	public float spawnTimer = 10;
	private float timer;
	private int numBebes = 0;
	// Use this for initialization
	void Start () {
		timer = spawnTimer;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer -= Time.fixedDeltaTime;
		if (timer <= 0 && numBebes < maxBebes) {
			timer = spawnTimer;
			numBebes++;
			spawnEnemy();
		}
	}

	void spawnEnemy() {
		GameObject bebe = Instantiate(bebeType, new Vector2(transform.position.x - transform.up.x, transform.position.y - transform.up.y), Quaternion.identity) as GameObject;
	}
}
