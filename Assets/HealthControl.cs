using UnityEngine;
using System.Collections;

public class HealthControl : MonoBehaviour {

	private Transform parent;

	// Use this for initialization
	void Start () {
		parent = GetComponent<Transform> ();
		Debug.Log (parent);
	}
	
	// Update is called once per frame
	void Update () {
		// position the health bar
	}
}
