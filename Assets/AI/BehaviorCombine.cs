using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviorCombine : MonoBehaviour {

	private List<BaseBehavior> behaviors;
	Rigidbody2D body;
	public float maxSpeed = 5f;
	public float rotationRate = 400f;
	public bool enableRotation = true;

	void Awake() {
		behaviors = new List<BaseBehavior> ();
		body = GetComponent<Rigidbody2D> ();
		if (body == null)
			Debug.LogError ("AI has no rigidbody attached");
	}

	// Use this for initialization
	void Start () {
	}

	public bool behaviorsPresent() {
		return (behaviors.Count > 0);
	}

	public void AddBehavior(BaseBehavior behavior) {
		behaviors.Add (behavior);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 newVel = body.velocity;
		foreach (BaseBehavior behavior in behaviors) {
			newVel += behavior.GetVector ();
		}
		// TODO: make this cleaner so that all of this isn't shared
		body.velocity = Vector2.ClampMagnitude(newVel, maxSpeed);
		if (enableRotation && body.velocity != Vector2.zero) {
			float angle = Mathf.Atan2 (body.velocity.y, body.velocity.x) * Mathf.Rad2Deg - 90;
			body.rotation = Mathf.Lerp(body.rotation, angle, 0.3f);
			//Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
			//transform.rotation = Quaternion.RotateTowards (transform.rotation, q, rotationRate * Time.deltaTime);
		}
	}
}
