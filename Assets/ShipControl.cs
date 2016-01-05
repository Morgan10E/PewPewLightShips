using UnityEngine;
using System.Collections;

public class ShipControl : MonoBehaviour { 

	public Transform turret;
	public float radius = 50f;
	public Camera shipCamera;
	public float rotationRate = 400f;
	public float speed = 10f;
	public bool enableControl = true;

//	public GameObject projectile;
	private bool isTethered = false;
	private bool isBoosting = false;

	// Use this for initialization
	void Start () {
		transform.localEulerAngles = Vector3.up;
//		tetherTarget = null;
//		lineRenderer = null;
	}

	public Vector3 getDirection() {
		Vector3 point = shipCamera.ScreenToWorldPoint(Input.mousePosition);
		float xDir = point.x - transform.position.x;
		float yDir = point.y - transform.position.y;
		float norm = Mathf.Sqrt (Mathf.Pow(xDir, 2) + Mathf.Pow(yDir,2));
		Vector3 direction = new Vector3 (xDir / norm, yDir / norm, 0);
		//Debug.Log (direction);
		return direction;
	}

	void Update() {
		MoveTurret ();
	}

	void MoveTurret (){
		Vector3 dir = getDirection ();
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg-90;
		dir = new Vector3 (dir.x * radius + transform.position.x, dir.y * radius+ transform.position.y, 0);
		turret.position = dir;
		//Debug.Log (dir);
		turret.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		//turret.rotation.y = transform.position.y + dir.y * radius;
	}
		
	void FixedUpdate () {
		if (enableControl) {
			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");

			Vector2 newSpeed = new Vector2 (h * speed, v * speed);
			if (!isBoosting) {
				GetComponent<Rigidbody2D> ().velocity = newSpeed;
			}

			if (h != 0 || v != 0) {
				float angle = Mathf.Atan2 (newSpeed.y, newSpeed.x) * Mathf.Rad2Deg - 90;
				Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, q, rotationRate * Time.deltaTime);
			}
		}
	}
}
