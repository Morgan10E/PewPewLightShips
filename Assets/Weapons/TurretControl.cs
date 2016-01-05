using UnityEngine;
using System.Collections;

public class TurretControl : MonoBehaviour {

	Camera shipCamera;
	public float radius = 50f;

	// Use this for initialization
	void Start () {
		shipCamera = GameObject.FindWithTag ("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePosition ();
	}

	public Vector3 getDirection() {
		Vector3 point = shipCamera.ScreenToWorldPoint(Input.mousePosition);
		float xDir = point.x - transform.parent.position.x;
		float yDir = point.y - transform.parent.position.y;
		float norm = Mathf.Sqrt (Mathf.Pow(xDir, 2) + Mathf.Pow(yDir,2));
		Vector3 direction = new Vector3 (xDir / norm, yDir / norm, 0);
		return direction;
	}

	void UpdatePosition() {
		Vector3 dir = getDirection ();
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg-90;
		dir = new Vector3 (dir.x * radius + transform.position.x, dir.y * radius+ transform.position.y, 0);
		transform.position = dir;
		//Debug.Log (dir);
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
