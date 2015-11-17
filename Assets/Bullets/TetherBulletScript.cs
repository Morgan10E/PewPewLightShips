using UnityEngine;
using System.Collections;

public class TetherBulletScript : MonoBehaviour {

	public int spinSpeed = 1;
	public int speed = 1000;
	private GameObject player;

	void Start () {
		Vector2 mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		Vector2 direction = mousePosition - new Vector2(this.transform.position.x, this.transform.position.y);
		direction = direction.normalized;
		this.GetComponent<Rigidbody2D>().AddForce(direction * speed);
		this.GetComponent<Rigidbody2D>().AddTorque(spinSpeed);
		
		GameObject playerObject = GameObject.FindGameObjectWithTag("Ship");
		Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), playerObject.GetComponent<Collider2D>());
	}

	public void SetPlayer(GameObject player) {
		this.player = player;
	}

	public void TetherPlayer(GameObject tetherObject) {
		ShipControl playerControl = player.GetComponent<ShipControl>();
//		playerControl.TetherToObject(tetherObject);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Grabable") {
			other.GetComponent<Rigidbody2D>().isKinematic = false;
			TetherPlayer(other.gameObject);
			Destroy(this.gameObject); // remove bullet when done
			return;
		}

		if (other.gameObject.tag != "Ship") Destroy(this.gameObject);

	}
}
