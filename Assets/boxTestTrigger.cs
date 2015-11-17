using UnityEngine;
using System.Collections;

public class boxTestTrigger : MonoBehaviour {

	private bool dragging = false;
	private LineRenderer lineRenderer;
	Rigidbody2D lineDestination;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Tractor") {
			GameObject player = GameObject.FindGameObjectWithTag("Ship");

			GetComponent<Rigidbody2D>().isKinematic = false;
			//print("collision detected with: " + other.ToString());
			SpringJoint2D spring = gameObject.AddComponent<SpringJoint2D>();
			spring.connectedBody = player.gameObject.GetComponent<Rigidbody2D>();
			spring.distance = 5f;
			spring.dampingRatio = 5f;
			//hinge.anchor = new Vector2(2f, 2f);
			lineDestination = player.gameObject.GetComponent<Rigidbody2D>();
			this.lineRenderer = gameObject.AddComponent<LineRenderer>();
			//lineRenderer.SetWidth(0.1, 0.1);
			lineRenderer.SetVertexCount(2);
			lineRenderer.enabled = true;
			lineRenderer.GetComponent<Renderer>().enabled = true;
			dragging = true;
		}
	}

	void Update() {
		if (dragging) {
			//Debug.Log ("LineRenderer: " + lineRenderer);
			Debug.Log ("position: " + transform.position);
			//lineRenderer.SetPosition(0, transform.position);
			this.lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
			this.lineRenderer.SetPosition(1, lineDestination.transform.position);
		}
	}
}

