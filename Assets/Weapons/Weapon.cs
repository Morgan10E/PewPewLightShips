using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour {

	public GameObject projectile;
	public float fireRate = 0.1f;
	public float bulletSpeed = 30;
	public float overheatRate = 5.0f;
	public float overheatCoolRate = 0.1f;
	public float overheatCooldown = 0.1f;
	private bool canShoot = true;
	private bool overheated = false;
	PlayerGui pGui;
	float overheatValue = 0.0f;
	float timeSinceLastShot = 0.0f;
	public float timeToStartCooldown = 1.0f;

	TurretControl turretControl;
	TeamIdentity teamIdentity;


	void Awake() {
		pGui = transform.parent.gameObject.GetComponent<PlayerGui> ();
		teamIdentity = transform.parent.gameObject.GetComponent<TeamIdentity> ();
		turretControl = GetComponent<TurretControl> ();
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		MouseUpdate ();
		UpdateOverheat ();
	}

	void MouseUpdate () {
		if (Input.GetMouseButton (0) && canShoot && !overheated) {
			StartCoroutine (FireAfterTime ());
			timeSinceLastShot = 0;
		} else {
			timeSinceLastShot += Time.fixedDeltaTime;
		}
	}
		
	[ClientCallback]
	void ShootBullet() {
		overheatValue += overheatRate;
		float xPos = transform.position.x;
		float yPos = transform.position.y;
		Vector3 dir = turretControl.getDirection () * bulletSpeed;
		CmdSpawnBullet (new Vector2(xPos, yPos), new Vector2(dir.x, dir.y), transform.rotation);
	}

	[Command]
	void CmdSpawnBullet(Vector2 loc, Vector2 vel, Quaternion rot) {

		GameObject bullet = Instantiate(projectile, loc, rot) as GameObject;
		if (bullet.GetComponent<TeamIdentity> () != null && teamIdentity != null)
			bullet.GetComponent<TeamIdentity> ().SetTeam (teamIdentity.GetTeam ());
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(vel.x, vel.y);
		NetworkServer.Spawn(bullet);
	}

	void UpdateOverheat() {
		if (overheated) {
			overheatValue -= overheatCooldown;
			if (overheatValue <= 0) {
				overheated = false;
			}
		} else if (overheatValue > 100) {
			overheated = true;
		} else {
			// if we should start cooling down, do so
			if (timeSinceLastShot > timeToStartCooldown) 
				overheatValue -= overheatCoolRate;
		}

		if (overheatValue < 0)
			overheatValue = 0;

		// update the GUI
		pGui.setCurrentAmmo(overheatValue);
	}


	IEnumerator FireAfterTime()
	{
		canShoot = false;
		ShootBullet ();
		yield return new WaitForSeconds(fireRate);
		canShoot = true;
	}
}
