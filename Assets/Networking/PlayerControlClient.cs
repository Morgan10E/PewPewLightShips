using UnityEngine;
using System.Collections;

public class PlayerControlClient : MonoBehaviour {

	public float positionErrorThreshold = 0.5f;
	public Vector3 serverPosition;
	public Quaternion serverRotation;
	public float speed = 10f;

	public Vector3 positionToAim;

	public void lerpToTarget() {
		float distance = Vector3.Distance(transform.position, serverPosition);

		// fix position if we are too far off
		if (distance >= positionErrorThreshold) {

			if (transform.position.x > serverPosition.x) Debug.Log ("We are ahead of server");
			else Debug.Log ("We are behind server");

			float lerp = ((1 / distance) * speed) / 100;
			transform.position = Vector3.Lerp (transform.position, serverPosition, lerp);
			transform.rotation = Quaternion.Slerp (transform.rotation, serverRotation, lerp);
			Debug.Log("lerping to target, distance: " + distance);

		}
	}

	private NetworkPlayer owner;

	private float lastH, lastV;

	[RPC] public void SetOwner(NetworkPlayer player) {
		Debug.Log("Setting owner");
		owner = player;
		if (player == Network.player) {
			enabled = true;
			GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
			Camera2DFollow followScript = camera.GetComponent<Camera2DFollow>();
			followScript.target = this.transform;
			followScript.enabled = true;
		} else {
			//if (GetComponent(Camera)) {
			//	GetComponent(Camera).enabled = false;
			//}

			//etc.
		}
	}

	[RPC] public NetworkPlayer GetOwner() {
		return owner;
	}

	void Awake() {
		if (Network.isClient) {
			enabled = false; // default, assume that we are not the owner
		}
	}

	// Use this for initialization
	void Start () {
		positionToAim = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Network.isServer) return;

		if ((owner != null) && (Network.player == owner)) {
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			if (h != lastH || v != lastV) {
				GetComponent<NetworkView>().RPC("UpdateClientMotion", RPCMode.Server, h, v);
			}

			lastH = h;
			lastV = v;

			//Vector3 newPosition = new Vector3(positionToAim.x + h * speed * Time.deltaTime, positionToAim.y + v * speed * Time.deltaTime, transform.position.z);
			//positionToAim = newPosition;
			//MoveToPosition();
			UpdateMovement(h, v);
		}
	}

	public float velocityLerp = 0.5f;
	public float catchup = 5f;

	void MoveToPosition() {
		float distance = Vector3.Distance(transform.position, positionToAim);
		float catchupSpeed = catchup * distance;
		if (catchupSpeed > 12) catchupSpeed = 12;
		Debug.Log ("Catchup Speed: " + catchupSpeed);
		if (distance > 1) {
			Vector3 direction = positionToAim - transform.position;
			direction = direction.normalized;
			Vector2 newSpeed = new Vector2(direction.x * catchupSpeed, direction.y * catchupSpeed);
			GetComponent<Rigidbody2D>().velocity = Vector2.Lerp (GetComponent<Rigidbody2D>().velocity, newSpeed, velocityLerp);

			if (lastH != 0 || lastV != 0) {
				float angle = Mathf.Atan2 (newSpeed.y, newSpeed.x) * Mathf.Rad2Deg-90;
				Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, q, 300*Time.deltaTime);
			}
		} else if (distance > 0.05) {
			Debug.Log ("Slowing down...");
			GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(GetComponent<Rigidbody2D>().velocity, Vector2.zero, 0.75f);
		}
	}

	void UpdateMovement(float h, float v) {
		Vector2 newSpeed = new Vector2(h*speed, v*speed);
		//rigidbody2D.velocity = newSpeed;
		//Vector3 newPosition = new Vector3(transform.position.x + h * speed * Time.deltaTime, transform.position.y + v * speed * Time.deltaTime, transform.position.z);
		transform.position = new Vector3(transform.position.x + h * speed * Time.deltaTime, transform.position.y + v * speed * Time.deltaTime, transform.position.z);



		if (h != 0 || v != 0) {
			float angle = Mathf.Atan2 (newSpeed.y, newSpeed.x) * Mathf.Rad2Deg-90;
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, q, 300*Time.deltaTime);
		}
	}
}
