using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Fire : NetworkBehaviour {

	public GameObject projectile;
	public float fireRate = 0.1f;
	public float bulletSpeed = 30;
	public float overheatRate = 5.0f;
	public float overheatCoolRate = 0.1f;
	public float overheatCooldown = 0.1f;
	[SerializeField] Transform turretTransform;
	private bool canShoot = true;
	private bool overheated = false;
	PlayerGui pGui;
	public float overheatValue = 0.0f;
	public float timeSinceLastShot = 0.0f;
	public float timeToStartCooldown = 1.0f;

	void Awake() {
		pGui = GetComponent<PlayerGui> ();
	}

	void Start() {
		//StartCoroutine (OverheatManager ());
	}

	void FixedUpdate() {
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

	[Command]
	void CmdSpawnBasicBullet(float x, float y, float vx, float vy, Quaternion rot) {
		GameObject bullet = Instantiate(projectile, new Vector2(x, y), rot) as GameObject;
		if (bullet.GetComponent<TeamIdentity> () != null && gameObject.GetComponent<TeamIdentity>() != null)
			bullet.GetComponent<TeamIdentity> ().SetTeam (gameObject.GetComponent<TeamIdentity> ().GetTeam ());
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(vx, vy);
		NetworkServer.Spawn (bullet);
	}

	[ClientCallback]
	void ShootBullet() {
		overheatValue += overheatRate;
		float xPos = turretTransform.position.x;//this.transform.position.x;
		float yPos = turretTransform.position.y;//this.transform.position.y;
		Vector3 dir = GetComponent<ShipControl> ().getDirection () * bulletSpeed;
		CmdSpawnBasicBullet (xPos, yPos, dir.x, dir.y, turretTransform.rotation);

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
