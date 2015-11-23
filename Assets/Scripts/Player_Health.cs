using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Health : NetworkBehaviour {

	public int maxHealth = 100;
	[SyncVar] int health = 100;

	// Use this for initialization
	void Start () {
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[ClientRpc]
	void RpcDamage(int amount)
	{
		//Debug.Log("Took damage:" + amount);
		GetComponent<HealthBarPositionScript> ().setCurrentHealth (health);
	}
	
	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;
		
		health -= amount;
		RpcDamage(amount);
	}

	public int GetHealth() {
		return health;
	}

	public void ResetHealth() {
		if (!isServer)
			return;
		health = maxHealth;
	}
}
