using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Health : NetworkBehaviour {

	public float maxHealth = 100;
	[SyncVar] public float health = 100;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		StartCoroutine (UpdateHealthLoop ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[ClientRpc]
	void RpcDamage(float amount)
	{
		//Debug.Log("Took damage:" + amount);
		if (GetComponent<HealthBarPositionScript> ().enabled) {
			GetComponent<HealthBarPositionScript> ().setCurrentHealth (health);
		} else {
			GetComponent<PlayerGui> ().setCurrentHealth (health);
		}
	}

	IEnumerator UpdateHealthLoop()
	{
		while(true)
		{
			if (GetComponent<HealthBarPositionScript> ().enabled) {
				GetComponent<HealthBarPositionScript> ().setCurrentHealth (health);
			} else {
				GetComponent<PlayerGui> ().setCurrentHealth (health);
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	public void TakeDamage(float amount)
	{
		if (!isServer)
			return;	
		health -= amount;
		RpcDamage(amount);
	}

	public float GetHealth() {
		return health;
	}

	public void ResetHealth() {
		if (!isServer)
			return;
		health = maxHealth;
	}
}
