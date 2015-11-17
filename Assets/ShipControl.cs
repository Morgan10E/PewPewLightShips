using UnityEngine;
using System.Collections;

public class ShipControl : MonoBehaviour { 

	[SerializeField] Transform turret;
	[SerializeField] float radius = 50f;
	[SerializeField] Camera shipCamera;

	public GameObject projectile;
	private bool isTethered = false;
	GameObject tetherTarget;
	LineRenderer lineRenderer;
	SpringJoint2D springJoint;

	// Use this for initialization
	void Start () {
		transform.localEulerAngles = Vector3.up;
		tetherTarget = null;
		lineRenderer = null;
	}

	Vector3 getDirection() {
		Vector3 point = shipCamera.ScreenToWorldPoint(Input.mousePosition);
		float xDir = point.x - transform.position.x;
		float yDir = point.y - transform.position.y;
		float norm = Mathf.Sqrt (Mathf.Pow(xDir, 2) + Mathf.Pow(yDir,2));
		Vector3 direction = new Vector3 (xDir / norm, yDir / norm, 0);
		//Debug.Log (direction);
		return direction;
	}

	// Update is called once per frame
	void MouseUpdate () {
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			GameObject bullet = Instantiate(projectile, new Vector2(turret.position.x + turret.up.x, turret.position.y + turret.up.y), Quaternion.identity) as GameObject;
			bullet.GetComponent<Rigidbody2D>().velocity = getDirection() * 10;
			Debug.Log(getDirection()*10);
		}

		if (Input.GetMouseButtonDown(1)) { // right click
			if (isTethered) {
				isTethered = false;
				this.springJoint.enabled = false;
				springJoint.connectedBody = null;
				lineRenderer.enabled = false;

			} else {
				GameObject bullet = Instantiate(projectile, new Vector2(transform.position.x + transform.up.x, transform.position.y + transform.up.y), Quaternion.identity) as GameObject;
				TetherBulletScript bulletControlScript = bullet.GetComponent<TetherBulletScript>();
				bulletControlScript.SetPlayer(this.gameObject);
			}
		}
	}

	public void TetherToObject(GameObject tetherTarget) {
		this.tetherTarget = tetherTarget;
		if (springJoint == null) springJoint = this.gameObject.AddComponent<SpringJoint2D>();
		else springJoint.enabled = true;
		springJoint.connectedBody = tetherTarget.gameObject.GetComponent<Rigidbody2D>();
		springJoint.distance = 5f;
		springJoint.dampingRatio = 5f;

		if (lineRenderer == null) lineRenderer = this.gameObject.GetComponent<LineRenderer>();
		lineRenderer.enabled = true;
		lineRenderer.SetVertexCount(2);
		lineRenderer.GetComponent<Renderer>().enabled = true;
		isTethered = true;
	}

	void Update() {
		MouseUpdate();
		if (isTethered) {
			lineRenderer.SetPosition(0, this.transform.position);
			lineRenderer.SetPosition(1, tetherTarget.transform.position);
		}
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
		//#if CROSS_PLATFORM_INPUT
		//float h = CrossPlatformInput.GetAxis("Horizontal");
		//float v = CrossPlatformInput.GetAxis("Vertical");
		//#else
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		//#endif

		Vector2 newSpeed = new Vector2(h*10, v*10);
		GetComponent<Rigidbody2D>().velocity = newSpeed;

		if (h != 0 || v != 0) {
			float angle = Mathf.Atan2 (newSpeed.y, newSpeed.x) * Mathf.Rad2Deg-90;
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, q, 300*Time.deltaTime);
		}
	}
}
