using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	private GameObject target;
	public float moveSpeed;
	
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Ship");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dir = target.transform.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
	}

	void SetTarget(GameObject newTarget) {
		target = newTarget;
	}
}
