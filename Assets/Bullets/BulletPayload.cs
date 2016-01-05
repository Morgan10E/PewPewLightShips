using UnityEngine;
using System.Collections;

public class BulletPayload : MonoBehaviour {

	public float damage = 10f;

	// Use this for initialization
	void Start () {
	
	}

	public float GetDamage() {
		return damage;
	}

	public void SetDamage(float val) {
		damage = val;
	}
}
