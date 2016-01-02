using UnityEngine;
using System.Collections;

public class Loadout : MonoBehaviour {

	public Ability[] abilities;
//	public Sprite[] sprite;
	public float[] speed;
	public float[] boostSpeed;
	public float[] boostDuration;
	public int defaultIndex = 0;
	public int[] first;
	public int[] second;
	public int[] third;
	public int[][] abs;

//	public float[] boostDelay;

	// Use this for initialization
	void Start () {
		SetLoadout (defaultIndex);
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void SetLoadout(int index) {
		Ability[] newAbilities = new Ability[3];
		newAbilities [0] = abilities [first [index]];
		newAbilities [1] = abilities [second [index]];
		newAbilities [2] = abilities [third [index]];

		GetComponent<Abilities>().abilities = newAbilities;
		GetComponent<ShipControl> ().speed = speed[index];
		GetComponent<Player_Boost> ().boostSpeed = boostSpeed[index];
		GetComponent<Player_Boost> ().boostDuration = boostDuration[index];
	}
}
