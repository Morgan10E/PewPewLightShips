using UnityEngine;
using System.Collections;

public class PlayerNetworkScript : MonoBehaviour {

	private int playerID = -1;
	private int playerCount = 0;

	public Rigidbody2D projectile;
	private float networkH, networkV;

	// Use this for initialization
	void Start () {
		transform.localEulerAngles = Vector3.up;
		networkH = 0;
		networkV = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.isServer/*networkView.isMine*/) {
			UpdateMovement(networkH, networkV);
			//InputMovement();

			/*if (Input.GetMouseButtonDown(1)) { // right click
				Rigidbody2D bullet = Instantiate(projectile, new Vector2(transform.position.x + transform.up.x, transform.position.y + transform.up.y), Quaternion.identity) as Rigidbody2D;
				
			}*/
		} 
	}

	[RPC] void SendInput(float h, float v) {
		//Debug.Log
		if (Network.isClient) {
			GetComponent<NetworkView>().RPC("SendInput", RPCMode.Server, h, v);
		} else {
			// server should update movement
			networkH = h;
			networkV = v;
		}
	}

	//[RPC] void PlayerID(

	void UpdateMovement(float h, float v) {
		Vector2 newSpeed = new Vector2(h*10, v*10);
		GetComponent<Rigidbody2D>().velocity = newSpeed;
		
		if (h != 0 || v != 0) {
			float angle = Mathf.Atan2 (newSpeed.y, newSpeed.x) * Mathf.Rad2Deg-90;
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, q, 300*Time.deltaTime);
		}
	}


	void InputMovement() {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		Vector2 newSpeed = new Vector2(h*10, v*10);
		GetComponent<Rigidbody2D>().velocity = newSpeed;
		
		if (h != 0 || v != 0) {
			float angle = Mathf.Atan2 (newSpeed.y, newSpeed.x) * Mathf.Rad2Deg-90;
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, q, 300*Time.deltaTime);
		}
	}
}
