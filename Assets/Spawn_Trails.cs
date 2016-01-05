using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawn_Trails : MonoBehaviour {

	List<GameObject> trailList;
	public int numObjects = 20;
	public GameObject trailPrefab;

	// Use this for initialization
	void Start () {
		trailList = new List<GameObject> ();
		for (int i = 0; i < numObjects; i++) {
			GameObject trail = Instantiate (trailPrefab);
			trail.SetActive (false);
			trail.transform.parent = this.transform;
			trailList.Add (trail);
		}
	}

	public int AttachTrail(GameObject obj) {
		for (int i = 0; i < numObjects; i++) {
			if (!trailList [i].activeInHierarchy) {
				trailList [i].transform.position = obj.transform.position;
				trailList [i].transform.rotation = obj.transform.rotation;
				trailList [i].SetActive (true);
				trailList [i].transform.parent = obj.transform;

				// set the particle system rotation
				trailList[i].GetComponent<ParticleSystem>().startRotation = ((obj.transform.eulerAngles.z - 180) % 360) * Mathf.Deg2Rad;
				return i;
			}
		}
		return -1;
	}

	public void ReturnTrail(int num) {
		if (num >= 0) {
			trailList [num].SetActive (false);
			trailList [num].transform.parent = this.transform;
		}
		
	}

}
