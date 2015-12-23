using UnityEngine;
using System.Collections;

public class EvadeBehavior : BaseBehavior {

	public Rigidbody2D target;
	public float maxSteer;

	public EvadeBehavior(Rigidbody2D _body, float _maxSpeed, 
		Rigidbody2D _target, float _maxSteer  = 0.1f) : base(_body, _maxSpeed) {
		// store a reference to our target point
		target = _target;
		maxSteer = _maxSteer;
	}

	public override Vector2 GetVector() {
		Vector2 dist = body.position - target.position;
		float T = dist.magnitude / maxVelocity;
		Vector2 currVel = body.velocity;
		Vector2 desiredVel = body.position - (target.position + target.velocity * T);
		desiredVel = desiredVel.normalized * maxVelocity;
		Vector2 steering = desiredVel - body.velocity;
		steering = Vector2.ClampMagnitude (steering, maxSteer);
		steering = steering / body.mass;
		// body.velocity = Vector2.ClampMagnitude (currVel + steering, maxVelocity);
		return steering;
	}
}
