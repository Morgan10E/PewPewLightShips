using UnityEngine;
using System.Collections;

public class ArriveBehavior : BaseBehavior {

	public GameObject target;
	public float maxSteer;
	public float arriveDist;

	public ArriveBehavior(Rigidbody2D _body, float _maxSpeed, 
		GameObject _target, float _maxSteer  = 0.1f, float _arriveDist = 5f) : base(_body, _maxSpeed) {
		// store a reference to our target point
		target = _target;
		maxSteer = _maxSteer;
		arriveDist = _arriveDist;
	}

	public override Vector2 GetVector() {
		Vector2 currVel = body.velocity;
		Vector2 desiredVel = new Vector2 (target.transform.position.x, 
			target.transform.position.y) - body.position;
		float dist = desiredVel.magnitude;
		if (dist > arriveDist) {
			desiredVel = desiredVel.normalized * maxVelocity;
		} else {
			desiredVel = desiredVel.normalized * maxVelocity * (dist / arriveDist);
		}
		Vector2 steering = desiredVel - body.velocity;
		steering = Vector2.ClampMagnitude (steering, maxSteer);
		steering = steering / body.mass;
		// body.velocity = Vector2.ClampMagnitude (currVel + steering, maxVelocity);
		return steering;
	}
}
