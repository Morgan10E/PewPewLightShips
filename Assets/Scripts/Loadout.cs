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
		abilities = new Ability[3];
		abilities [0] = (DemoAbility)ScriptableObject.CreateInstance("DemoAbility");
		SetLoadout (defaultIndex);
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void SetLoadout(int index) {
		GetComponent<Abilities> ().SetAbilities (abilities [first [index]], abilities [second [index]], abilities [third [index]]);
		GetComponent<ShipControl> ().speed = speed[index];
		GetComponent<Player_Boost> ().boostSpeed = boostSpeed[index];
		GetComponent<Player_Boost> ().boostDuration = boostDuration[index];
	}
}
