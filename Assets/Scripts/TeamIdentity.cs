using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TeamIdentity : NetworkBehaviour {

	public Color[] teamColors = {Color.red, Color.green};

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
			yield return new WaitForSeconds(0.05f);

		if (tag == "Ship") {
			// set your health bar color to match
			sprite.color = teamColors[teamNum];
			ParticleSystem pSys = GetComponentInChildren<ParticleSystem> ();
			pSys.startColor = teamColors [teamNum];
			SpriteRenderer childSprite = GetComponentInChildren<SpriteRenderer> ();
			childSprite.color = teamColors [teamNum];
		}

		if (tag == "PlayerSpawn") {
			sprite.color = teamColors [teamNum];
		}

		if (tag == "Projectile") {
			sprite.color = teamColors [teamNum];
		}

	}
		

	[SyncVar]
	public int teamNum = -1;

	public void SetTeam(int num) {
		teamNum = num;
		if (tag == "Projectile" && sprite != null) {
			sprite.color = teamColors [teamNum];
		}
	}

	public int GetTeam() {
		return teamNum;
	}
}
