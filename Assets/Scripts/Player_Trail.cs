using UnityEngine;
using System.Collections;

public class Player_Trail : MonoBehaviour {

	[SerializeField] GameObject trail;
	[SerializeField] float yOffset = 0.5f;
	[SerializeField] float xOffset = 0.5f;

	private Transform shipTransform;
	GameObject leftTrail;
	GameObject rightTrail;
	bool created = false;

	// Use this for initialization
	void Start () {
		//this.AddTrail ();
	}

	public void AddTrail() {
		shipTransform = this.transform;
		Vector3 leftOffset = new Vector3 (-xOffset,-yOffset,0);
		Vector3 rightOffset = new Vector3 (xOffset, -yOffset, 0);
		leftTrail = Instantiate(trail, shipTransform.position+leftOffset, Quaternion.identity) as GameObject;
		rightTrail = Instantiate(trail, shipTransform.position+rightOffset, Quaternion.identity) as GameObject;
		leftTrail.transform.parent = shipTransform;
		rightTrail.transform.parent = shipTransform;
	}

	public void DisableTrail() {
		if (!created)
			return;
		leftTrail.GetComponent<TrailRenderer> ().enabled = false;
		rightTrail.GetComponent<TrailRenderer> ().enabled = false;
	}

	public void EnableTrail() {
		if (!created)
			return;
		leftTrail.GetComponent<TrailRenderer> ().enabled = true;
		rightTrail.GetComponent<TrailRenderer> ().enabled = true;
	}
	
	// Update is called once per frame
//	void Update () {
//
//	}
}
