using UnityEngine;
using System.Collections;

public class PlayerControlServer : MonoBehaviour {

	private float networkH, networkV;
	public float speed = 10f;

	// Use this for initialization
	void Start () {
		networkH = 0;
		networkV = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.isClient) return;
	}

	void FixedUpdate() {
		if (Network.isClient) return;
		UpdateMovement(networkH, networkV);
	}

	void UpdateMovement(float h, float v) {
		Vector2 newSpeed = new Vector2(h, v);
		//newSpeed = newSpeed.normalized * speed;
		//rigidbody2D.velocity = newSpeed;
		//rigidbody2D.MovePosition(new Vector2(h * speed * Time.deltaTime, v * speed * Time.deltaTime));
		transform.position = new Vector3(transform.position.x + h * speed * Time.deltaTime, transform.position.y + v * speed * Time.deltaTime, transform.position.z);

		if (h != 0 || v != 0) {
			float angle = Mathf.Atan2 (newSpeed.y, newSpeed.x) * Mathf.Rad2Deg-90;
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, q, 300*Time.deltaTime);
		}
	}

	[RPC] public void UpdateClientMotion(float h, float v) {
		networkH = h;
		networkV = v;
	}
}
