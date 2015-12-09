using UnityEngine;
using System.Collections;

public class rotate_sprite : MonoBehaviour {

	public float rotationSpeed = 2.5f;
	TeamManager teamManager;

	// Use this for initialization
	void Awake () {
		teamManager = GameObject.Find ("TeamManager").GetComponent<TeamManager> ();
	}

	void Start() {
		teamManager.RegisterSpawn (this.gameObject);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate (0, 0, rotationSpeed);
	}
		
}

