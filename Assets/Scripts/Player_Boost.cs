using UnityEngine;
using System.Collections;

public class Player_Boost : MonoBehaviour {

	public float boostSpeed = 20f;
	public float boostDuration = 0.5f;

	private bool isBoosting = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		bool shiftPressed = Input.GetKeyDown ("left shift");

		if (shiftPressed) {
			if (!isBoosting) {
				// start boosting
				GetComponent<ShipControl>().enableControl = false;
				isBoosting = true;
				float angle = transform.rotation.eulerAngles.z;
				GetComponent<Rigidbody2D> ().velocity = new Vector2(-transform.right.y * boostSpeed, transform.right.x * boostSpeed);
				Invoke("DoneBoosting", boostDuration);

				// if we can, enable the after image
				if (GetComponent<AfterImage>() != null) {
					// TODO: make this happen over the network
					GetComponent<AfterImage>().EnableAfterImage();
				}
			}
		}
	}


	void DoneBoosting() {
		// reset the velocity
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
//		Vector2 newSpeed = new Vector2 (h * speed, v * speed);
//		GetComponent<Rigidbody2D> ().velocity = newSpeed;
		if (GetComponent<AfterImage>() != null) {
			// TODO: make this happen over the network
			GetComponent<AfterImage>().DisableAfterImage();
		}
		isBoosting = false;
		GetComponent<ShipControl>().enableControl = true;
	}
}
