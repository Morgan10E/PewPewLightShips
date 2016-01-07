using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrackingShot : NetworkBehaviour {

	PlayerGui pGui;

	System.Random rand;
	Rigidbody2D body;
	public GameObject bullet;
	public float bulletSpeed = 30f;
	public float chargeTime = 5.0f;
	public int numShots = 5;
	public int maxAmmo = 2;
	bool charging = false;
	public int ammo;

	// Use this for initialization
	void Start () {
		pGui = GetComponent<PlayerGui> ();
		rand = new System.Random ();
		body = GetComponent<Rigidbody2D> ();
		ammo = maxAmmo;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q) && ammo > 0) {
			// fire bullet
			FireTrackingShot();
			ammo--;
			pGui.setAbilityAmmo (ammo);
		}
		if (ammo < maxAmmo && !charging) {
			charging = true;
			StartCoroutine (ChargeShots ());
		}
	}

	IEnumerator ChargeShots() {
		yield return new WaitForSeconds (chargeTime);
		ammo++;
		pGui.setAbilityAmmo (ammo);
		charging = false;
	}

	[ClientCallback]
	void FireTrackingShot() {
		Vector3 mouse = Input.mousePosition;
		mouse.z = 0;
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint (mouse);
		Vector2 target = new Vector2 (worldPoint.x, worldPoint.y) + body.position;
		// pick a random angle
		float baseAngle = body.rotation;

		for (int i = 0; i < numShots; i++) {
			float angleOffset = (float)(rand.NextDouble () * Mathf.PI) - (Mathf.PI / 2);
			float angle = baseAngle + angleOffset + Mathf.PI / 2;
			Vector2 vel = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
			Vector2 pos = body.position + vel;
			Quaternion q = Quaternion.AngleAxis (angleOffset, Vector3.forward);
			CmdFireTrackingShot (pos, vel, q, target);
		}
	}

	[Command]
	void CmdFireTrackingShot(Vector2 pos, Vector2 vel, Quaternion rot, Vector2 dest) {
		GameObject trackingBullet = Instantiate (bullet, pos, rot) as GameObject;
		Rigidbody2D bulletBody = trackingBullet.GetComponent<Rigidbody2D> ();
		bulletBody.velocity = vel * bulletSpeed;
		trackingBullet.GetComponent<TargetTracker> ().setDest (dest);
		if (bullet.GetComponent<TeamIdentity> () != null && gameObject.GetComponent<TeamIdentity>() != null)
			bullet.GetComponent<TeamIdentity> ().SetTeam (gameObject.GetComponent<TeamIdentity> ().GetTeam ());
		NetworkServer.Spawn (trackingBullet);
	}
}
