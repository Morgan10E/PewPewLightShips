using UnityEngine;
using System.Collections;

public class WanderBehavior : BaseBehavior {

	public float circleDistance;
	public float circleRadius;
	private float wanderAngle = 0f;
	private float angleChange = 0.1f;
	System.Random prng;

	public WanderBehavior(Rigidbody2D _body, float _maxSpeed, 
		float _dist = 1f, float _radius  = 2f) : base(_body, _maxSpeed) {
		// store a reference to our target point
		circleRadius = _radius;
		circleDistance = _dist;
		prng = new System.Random (Time.time.ToString().GetHashCode());
	}

	public override Vector2 GetVector() {
		Vector2 circlePos = body.velocity.normalized * circleDistance;
		Vector2 displace = Vector2FromAngle (wanderAngle).normalized * circleRadius;
		wanderAngle += (float) prng.NextDouble () *  prng.Next(-1, 2) * angleChange;
		Vector2 wander = circlePos + displace;
		return wander;
	}

	public Vector2 Vector2FromAngle(float a) {
		//a *= Mathf.Deg2Rad;
		return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
	}
}
