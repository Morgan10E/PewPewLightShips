using UnityEngine;
using System.Collections;

public class HealthBarPositionScript : MonoBehaviour {

	//private GameObject ship;
	public GameObject healthBarPrefab;
	private GameObject healthBar;
	private float height;
	public float maxHealth = 100;
	public float currentHealth = 100;
	private float baseScale = 1;

	// Use this for initialization
	void Start () {
		CreateHealthBar ();
	}
	
	// Update is called once per frame
	void Update () {
		// have the health bar follow the game object
		//Debug.Log (this.ship);
//		Debug.Log (this.healthBar);
		Transform parent = GetComponent<Transform> ();
		if (healthBar != null) {
			// position
			Transform healthTrans = healthBar.GetComponent<Transform>();
			healthTrans.position = new Vector2 (parent.position.x, parent.position.y + height);

			// percentage
			float healthPercentage = currentHealth / maxHealth;
			healthTrans.localScale = new Vector3(healthPercentage,healthTrans.localScale.y,healthTrans.localScale.z);
		}
	}

	public void RemoveHealthBarSprite() {
		//Destroy (healthBar);
		//healthBar = null;
		healthBar.GetComponent<SpriteRenderer> ().enabled = false;
	}

	public void EnableHealthBarSprite() {
		//CreateHealthBar ();
		healthBar.GetComponent<SpriteRenderer> ().enabled = true;
		setCurrentHealth(maxHealth);
		
	}

	public void DestroyHealthBar() {
		Destroy (healthBar);
	}

	void CreateHealthBar() {
		Transform parent = GetComponent<Transform> ();
		height = parent.localScale.y / GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		// create the health bar
		healthBar = Instantiate(healthBarPrefab, new Vector2(parent.position.x, parent.position.y + height), Quaternion.identity) as GameObject;
		// get the width and height of our attached object
		baseScale = healthBar.transform.lossyScale.x;
	}

	public void setCurrentHealth(float val) {
		currentHealth = val;
	}
}
