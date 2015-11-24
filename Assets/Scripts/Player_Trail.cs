using UnityEngine;
using System.Collections;

public class Player_Trail : MonoBehaviour {

	[SerializeField] Transform left;
	[SerializeField] Transform right;
	[SerializeField] float yOffset = 0.5f;
	[SerializeField] float xOffset = 0.5f;

	private Transform shipTransform;

	// Use this for initialization
	void Start () {
		shipTransform = transform;
		Vector3 leftOffset = new Vector3 (-xOffset,-yOffset,0);
		Vector3 rightOffset = new Vector3 (xOffset, -yOffset, 0);
		left.position = shipTransform.position + leftOffset;
		right.position = shipTransform.position + rightOffset;
	}
	
	// Update is called once per frame
//	void Update () {
//
//	}
}
