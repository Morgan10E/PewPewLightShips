using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TestSeek : NetworkBehaviour {

	GameObject target;
	BehaviorCombine behave;
	bool lockedOn = false;

	void Awake() {
		behave = GetComponent<BehaviorCombine> ();
		int environmentLayer = LayerMask.NameToLayer ("Environment");
		int currentLayer = LayerMask.NameToLayer ("Little Enemies");
		//Physics2D.IgnoreLayerCollision (environmentLayer, currentLayer);
	}

	// Use this for initialization
	void Start () {
	
	}

//	void OnTriggerEnter2D(Collider2D other) {
//		if (!lockedOn && other.tag == "Ship") {
//			Debug.Log ("locked on to ship");
//			lockedOn = true;
//			target = other.gameObject;
//		}
//	}
//
//	void OnTriggerExit2D(Collider2D other) {
//		// wait N seconds, then de-lock from the character
//	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!isServer)
			return;
		
		if (target != null && !behave.behaviorsPresent()) {
			//behave.AddBehavior (new ArriveBehavior (GetComponent<Rigidbody2D> (), 5f, target, 0.1f, 5f));
			behave.AddBehavior (new SeekBehavior (GetComponent<Rigidbody2D> (), 5f, target));
			//behave.AddBehavior(new WanderBehavior(GetComponent<Rigidbody2D>(), 5f));
			behave.AddBehavior (new AvoidBehavior (GetComponent<Rigidbody2D> (), 5f, 0.5f, target.GetComponent<Rigidbody2D>()));
		} else {
			target = GameObject.FindWithTag ("Ship");
		}
	}
}
