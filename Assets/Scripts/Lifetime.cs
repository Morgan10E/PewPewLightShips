using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Lifetime : NetworkBehaviour {

	public float lifetime = 1.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake() {
		StartCoroutine (ExecuteAfterTime (lifetime));
	}

	IEnumerator ExecuteAfterTime(float time)
	{
		Debug.Log ("yielding for " + time);
		yield return new WaitForSeconds(time);
		
		// Code to execute after the delay
		if (isServer) {
			NetworkServer.Destroy(this.gameObject);
		}
	}
}
