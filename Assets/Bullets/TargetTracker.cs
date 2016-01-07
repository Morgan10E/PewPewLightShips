using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TargetTracker : NetworkBehaviour {

	public Transform target;
	Vector2 dest;
	Rigidbody2D body;
	public float steeringForce = 5f;
	public float maxSpeed = 30f;
	bool located = false;

	public void setTarget(Transform target) {
		this.target = target;
	}

	public void setDest(Vector2 dest) {
		this.dest = dest;
	}

	void Awake() {
		body = GetComponent<Rigidbody2D> ();
	}

	// Use this for initialization
	void Start () {
		// get the initial speed based of direction
		if (dest == null)
			dest = new Vector2(0, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!isServer)
			return;
		if (target != null)
			dest = new Vector2 (target.position.x, target.position.y);
		Vector2 desired = dest - body.position;
		Vector2 steering = (desired - body.velocity).normalized * steeringForce;
		if (located) {
			body.velocity = body.velocity.normalized * (body.velocity.magnitude + 4f);
		} else {
			float alignment = Vector2.Dot (body.velocity.normalized, desired.normalized);
			if (alignment > 0.95f) {
				// we are close enough, only need to speed up
				located = true;
				Debug.Log ("located");
			} else {
				body.velocity = new Vector2 (body.velocity.x + steering.x, body.velocity.y + steering.y);
			}
		}

		// regulate the velocity
		if (body.velocity.magnitude > maxSpeed) {
			body.velocity = body.velocity.normalized * maxSpeed;
		}

		var angle = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;
		body.MoveRotation(angle);
	}
}
