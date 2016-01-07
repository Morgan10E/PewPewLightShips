using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIControl : MonoBehaviour {

	public Slider healthSlider;
	public Slider ammoSlider;
	public Image ammo1;
	public Image ammo2;
	private float ammoValue = 0;
	private float healthValue = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		LerpAmmo ();
		LerpHealth ();
	}

	public void SetQCount(int value) {
		if (value == 0) {
			ammo1.enabled = false;
			ammo2.enabled = false;
		} else if (value == 1) {
			ammo1.enabled = true;
			ammo2.enabled = false;
		} else if (value == 2) {
			ammo1.enabled = true;
			ammo2.enabled = true;
		}
	}

	private void LerpAmmo() {
		float val = Mathf.Lerp (ammoSlider.value, ammoValue, 0.1f);
		ammoSlider.value = val;
	}

	private void LerpHealth() {
		float val = Mathf.Lerp (healthSlider.value, healthValue, 0.1f);
		healthSlider.value = val;
	}

	public void SetHealthSlider(float value) {
		healthValue = value;
	}

	public void SetAmmoSlider(float value) {
		ammoValue = value;

	}
}
