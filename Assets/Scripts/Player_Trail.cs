﻿using UnityEngine;
using System.Collections;

public class Player_Trail : MonoBehaviour {

	[SerializeField] GameObject trail;
	[SerializeField] float yOffset = 0.5f;
	[SerializeField] float xOffset = 0.5f;

	private Transform shipTransform;

	// Use this for initialization
	void Start () {
		shipTransform = transform;
		Vector3 leftOffset = new Vector3 (-xOffset,-yOffset,0);
		Vector3 rightOffset = new Vector3 (xOffset, -yOffset, 0);
		GameObject leftTrail = Instantiate(trail, shipTransform.position+leftOffset, Quaternion.identity) as GameObject;
		GameObject rightTrail = Instantiate(trail, shipTransform.position+rightOffset, Quaternion.identity) as GameObject;
		leftTrail.transform.parent = shipTransform;
		rightTrail.transform.parent = shipTransform;
	}


	
	// Update is called once per frame
//	void Update () {
//
//	}
}