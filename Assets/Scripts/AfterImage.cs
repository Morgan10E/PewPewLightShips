using UnityEngine;
using System.Collections;

public class AfterImage : MonoBehaviour {
	public GameObject particlePrefab;
	GameObject afterImage;
	ParticleSystem pSys;

	// Use this for initialization
	void Start () {
		AttachParticleSystem ();
	}

	void AttachParticleSystem() {
		GameObject afterImage = Instantiate(particlePrefab);
		afterImage.transform.parent = gameObject.transform;
		pSys = afterImage.GetComponent<ParticleSystem> ();
		pSys.enableEmission = false;
	}

	public void EnableAfterImage() {
		pSys.startRotation = ((gameObject.transform.eulerAngles.z - 180) % 360) * Mathf.Deg2Rad;
		pSys.enableEmission = true;
	}

	public void DisableAfterImage() {
		pSys.enableEmission = false;
	}
}
