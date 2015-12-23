using UnityEngine;
using System.Collections;

public class FleeBehavior : BaseBehavior {

	public GameObject target;
	public float maxSteer;

	public FleeBehavior(Rigidbody2D _body, float _maxSpeed, 
		GameObject _target, float _maxSteer  = 0.1f) : base(_body, _maxSpeed) {
		// store a reference to our target point
		target = _target;
		maxSteer = _maxSteer;
	}

	public override Vector2 GetVector() {
		Vector2 currVel = body.velocity;
		Vector2 desiredVel = body.position - new Vector2 (target.transform.position.x, 
			                     target.transform.position.y);
		desiredVel = desiredVel.normalized * maxVelocity;
		Vector2 steering = desiredVel - body.velocity;
		steering = Vector2.ClampMagnitude (steering, maxSteer);
		steering = steering / body.mass;
		// body.velocity = Vector2.ClampMagnitude (currVel + steering, maxVelocity);
		return steering;
	}
}
