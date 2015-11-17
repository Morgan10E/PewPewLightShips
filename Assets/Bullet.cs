using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public GameObject block;
	public GameObject target;

	// Use this for initialization
	void Start () {
		Vector2 mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		Vector2 direction = mousePosition - new Vector2(this.transform.position.x, this.transform.position.y);
		direction = direction.normalized;
		this.GetComponent<Rigidbody2D>().AddForce(direction * 1000);

		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
		Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), playerObject.GetComponent<Collider2D>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
