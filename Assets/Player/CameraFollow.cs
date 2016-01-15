using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	//public Camera camera;
	public float lookAhead = 1f;
	public float lookSpeed = 0.2f;
	Rigidbody2D body;
	Transform target;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		target = transform.parent;
		body = target.gameObject.GetComponent<Rigidbody2D> ();
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 lookOffset = new Vector3 (body.velocity.x, body.velocity.y) * lookAhead;
		lookOffset.z = transform.position.z;
		Vector3 targetPos = target.position + lookOffset;
		transform.position = Vector3.Lerp (transform.position, targetPos, lookSpeed);
	}
}
