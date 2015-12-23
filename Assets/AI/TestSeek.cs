using UnityEngine;
using System.Collections;

public class TestSeek : MonoBehaviour {

	GameObject player;
	BehaviorCombine behave;

	void Awake() {
		player = GameObject.FindWithTag ("Ship");
		behave = GetComponent<BehaviorCombine> ();
		if (player != null) {
			behave.AddBehavior(new SeekBehavior(GetComponent<Rigidbody2D>(), 5f, player));
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (player == null) {
			player = GameObject.FindWithTag ("Ship");
			if (player != null) {
				behave.AddBehavior(new ArriveBehavior(GetComponent<Rigidbody2D>(), 5f, player, 0.1f, 5f));
				//behave.AddBehavior(new WanderBehavior(GetComponent<Rigidbody2D>(), 5f));
				behave.AddBehavior(new AvoidBehavior(GetComponent<Rigidbody2D>(), 5f));
			}
		}
	}
}
