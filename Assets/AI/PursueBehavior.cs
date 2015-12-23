using UnityEngine;
using System.Collections;

public class PursueBehavior : BaseBehavior {

	public Rigidbody2D target;
	public float maxSteer;

	public PursueBehavior(Rigidbody2D _body, float _maxSpeed, 
		Rigidbody2D _target, float _maxSteer  = 0.1f) : base(_body, _maxSpeed) {
		// store a reference to our target point
		target = _target;
		maxSteer = _maxSteer;
	}

	public override Vector2 GetVector() {
		Vector2 dist = body.position - target.position;
		float T = dist.magnitude / maxVelocity;
		Vector2 currVel = body.velocity;
		Vector2 desiredVel = target.position + target.velocity * T - body.position;
		desiredVel = desiredVel.normalized * maxVelocity;
		Vector2 steering = desiredVel - body.velocity;
		steering = Vector2.ClampMagnitude (steering, maxSteer);
		steering = steering / body.mass;
		// body.velocity = Vector2.ClampMagnitude (currVel + steering, maxVelocity);
		return steering;
	}
}
