using UnityEngine;
using System.Collections;

public class AvoidBehavior : BaseBehavior {

	public float maxAvoid;
	public float losDist = 1.0f;
	Vector2 stored = Vector2.zero;
	int pingCount = 0;
	public int refreshRate = 5;
	bool debugDraw = false;
	Rigidbody2D target;

	public AvoidBehavior(Rigidbody2D _body, float _maxSpeed,
		float _maxAvoid  = 0.5f, Rigidbody2D _target = null) : base(_body, _maxSpeed) {
		// store a reference to our target point
		maxAvoid = _maxAvoid;
		target = _target;
	}

	public override Vector2 GetVector() {
		Vector2 start = body.position;
		Vector2 targetPos = target.position;
		Vector2 dir = targetPos - start;
		Vector2 checkLos = start + dir.normalized * 1;
		RaycastHit2D playerHit = Physics2D.Raycast (checkLos, dir, dir.magnitude);
		if (playerHit.collider.tag == "Ship") {
			return Vector2.zero;
		}
		
		if (pingCount % refreshRate != 0) {
			pingCount++;
			return stored;
		}
		pingCount++;
		Vector2 result = Vector2.zero;

		float straight = body.rotation;
		Vector2 straightVector = Vector2FromAngle(straight).normalized * losDist;
		Vector2 straightStart = start + straightVector.normalized * 1;

		float left = straight + 60;
		Vector2 leftVector = Vector2FromAngle(left).normalized * losDist;
		Vector2 leftStart = start + leftVector.normalized * 1;

		float leftStraight = straight + 35;
		Vector2 leftStraightVector = Vector2FromAngle(leftStraight).normalized * losDist;
		Vector2 leftStraightStart = start + leftStraightVector.normalized * 1;

		float right = straight - 60;
		Vector2 rightVector = Vector2FromAngle(right).normalized * losDist;
		Vector2 rightStart = start + rightVector.normalized * 1;

		float rightStraight = straight - 30;
		Vector2 rightStraightVector = Vector2FromAngle(rightStraight).normalized * losDist;
		Vector2 rightStraightStart = start + rightStraightVector.normalized * 1;

		RaycastHit2D hit = Physics2D.Raycast (straightStart, straightVector);
		if (debugDraw) Debug.DrawRay(straightStart, straightVector, Color.green);
		if (hit.collider != null) {
			//Debug.Log ("straight hit");
		}


		hit = Physics2D.Raycast (leftStart, leftVector, losDist);
		if (debugDraw) Debug.DrawRay(leftStart, leftVector, Color.yellow);
		if (hit.collider != null) {
			float dist = hit.distance;
			result += rightVector.normalized * dist;
			//Debug.Log ("left hit");
		}

		hit = Physics2D.Raycast (leftStraightStart, leftStraightVector, losDist);
		if (debugDraw) Debug.DrawRay(leftStraightStart, leftStraightVector, Color.yellow);
		if (hit.collider != null) {
			result += rightStraightVector.normalized * hit.distance;
			//Debug.Log ("left straight hit");
		}
			
		hit = Physics2D.Raycast (rightStart, rightVector, losDist);
		if (debugDraw) Debug.DrawRay(rightStart, rightVector, Color.yellow);
		if (hit.collider != null) {
			result += leftVector.normalized * hit.distance;
			//Debug.Log ("right hit");
		}
			
		hit = Physics2D.Raycast (rightStraightStart, rightStraightVector, losDist);
		if (debugDraw) Debug.DrawRay(rightStraightStart, rightStraightVector, Color.yellow);
		if (hit.collider != null) {
			result += leftStraightVector.normalized * hit.distance;
			//Debug.Log ("right straight hit");
		}
		stored = result;
		return result.normalized * maxAvoid;
	}

	public Vector2 Vector2FromAngle(float a) {
		a *= Mathf.Deg2Rad;
		a += Mathf.PI / 2;
		return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
	}
}
