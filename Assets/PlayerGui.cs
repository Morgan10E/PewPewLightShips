using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGui : MonoBehaviour {

	GUIControl guiControl;

	void Awake() {
		// initialization
		guiControl = GameObject.Find("GUICanvas").GetComponent<GUIControl>();

	}

	public void setCurrentHealth(float health) {
		guiControl.SetHealthSlider (health);
	}

	public void setCurrentAmmo(float ammo) {
		guiControl.SetAmmoSlider (ammo);
	}

	public void setAbilityAmmo(int count) {
		guiControl.SetQCount (count);
	}
}
