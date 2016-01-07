using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TeamIdentity : NetworkBehaviour {

	Color[] teamColors = {Color.red, Color.green};

	SpriteRenderer sprite;
	HealthBarPositionScript hbar;

	void Awake() {
		hbar = GetComponent<HealthBarPositionScript> ();
		sprite = GetComponent<SpriteRenderer> ();
		if (sprite == null)
			sprite = GetComponentInChildren<SpriteRenderer> ();
		StartCoroutine (SetHealthColor ());
	}

	IEnumerator SetHealthColor() {
		while (teamNum < 0)
			yield return new WaitForSeconds(0.5f);
		if (hbar != null && hbar.enabled && teamNum >= 0) {
			hbar.SetHealthBarColor (teamColors [teamNum]);
			hbar.colorSet = true;
		}

		if (isLocalPlayer) {
			// set your health bar color to match
			GameObject.Find("HealthFill").GetComponent<Image>().color = teamColors[teamNum];
		}

		if (tag == "PlayerSpawn") {
			sprite.color = teamColors [teamNum];
		}

	}
		

	[SyncVar]
	public int teamNum = -1;

	public void SetTeam(int num) {
		teamNum = num;
	}

	public int GetTeam() {
		return teamNum;
	}
}
