using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class rotate_sprite : MonoBehaviour {

	public float rotationSpeed = 2.5f;
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate (0, 0, rotationSpeed);
	}
		
}

