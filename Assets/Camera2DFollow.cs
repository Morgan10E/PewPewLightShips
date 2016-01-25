using UnityEngine;
using System.Collections;

public class Camera2DFollow : MonoBehaviour {
	
	public Transform target;
	public float damping = 1;
	public float lookAheadFactor = 3;
	//public float lookAheadReturnSpeed = 0.5f;
	//public float lookAheadMoveThreshold = 0.1f;
	public float thresh = 0.5f;
	public float offsetDist = 4f;
	
	float offsetZ;
	Vector3 lastTargetPosition;
	Vector3 currentVelocity;
	Vector3 lookAheadPos;
	Vector2 offset;
	
	// Use this for initialization
	void Start () {
		if (target == null)
			return;
		lastTargetPosition = target.position;
		offsetZ = (transform.position - target.position).z;
		transform.parent = null;
		offset = Vector2.zero;
	}

	// Update is called once per frame
	void Update () {
		if (target == null)
			return;
		//transform.rotation = Quaternion.Euler(Vector3.zero);
		//transform.rotation = Quaternion.Euler(new Vector3(0, 0, -target.rotation.z));
		
		// only update lookahead pos if accelerating or changed direction
		float xMoveDelta = (target.position - lastTargetPosition).x;
		float yMoveDelta = (target.position - lastTargetPosition).y;
		Vector2 lookTarget = new Vector2 (xMoveDelta, yMoveDelta) * lookAheadFactor;
		if (lookTarget.magnitude > thresh) {
			offset = Vector2.Lerp (offset, lookTarget.normalized * offsetDist, 0.1f);
		} else {
			offset = Vector2.Lerp (offset, Vector2.zero, 0.1f);
		}
		if (lookTarget.magnitude > lookAheadFactor) {
			lookTarget = lookTarget.normalized * lookAheadFactor;
		}
		lookTarget = lookTarget + offset;	

		Vector3 aheadTargetPos = target.position + new Vector3(lookTarget.x, lookTarget.y, offsetZ);
		Vector3 newPos = Vector3.Slerp(transform.position, aheadTargetPos, damping);
		
		transform.position = newPos;
		
		lastTargetPosition = target.position;		
	}
}
