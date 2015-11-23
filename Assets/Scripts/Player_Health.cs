using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Health : NetworkBehaviour {

	[SyncVar] int health = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[ClientRpc]
	void RpcDamage(int amount)
	{
		Debug.Log("Took damage:" + amount);
		GetComponent<HealthBarPositionScript> ().setCurrentHealth (health);
	}
	
	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;
		
		health -= amount;
		RpcDamage(amount);
	}
}
