using UnityEngine;
using System.Collections;

public class BaseBehavior {

	public Rigidbody2D body;
	public float maxVelocity;

	public BaseBehavior(Rigidbody2D _body, float maxV) {
		body = _body;
		maxVelocity = maxV;
	}

	public virtual Vector2 GetVector () {
		// Return the x,y position of the controlled body
		return new Vector2(body.position.x, body.position.y);
	}
}
