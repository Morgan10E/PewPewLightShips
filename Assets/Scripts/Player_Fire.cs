using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Fire : NetworkBehaviour {

	public GameObject projectile;
	public float fireRate = 0.1f;
	[SerializeField] Transform turretTransform;
	private bool canShoot = true;

	void Start() {

	}

	void FixedUpdate() {
		MouseUpdate ();
	}

	void MouseUpdate () {
		if (Input.GetMouseButton(0) && canShoot) {
			//ShootBullet();
			StartCoroutine (FireAfterTime ());
			//Debug.Log(bullet.GetComponent<Rigidbody2D>().velocity);
		}
	}

	[Command]
	void CmdSpawnBasicBullet(float x, float y, float vx, float vy) {
		GameObject bullet = Instantiate(projectile, new Vector2(x, y), Quaternion.identity) as GameObject;
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(vx, vy);
		NetworkServer.Spawn (bullet);
	}

	[ClientCallback]
	void ShootBullet() {
		float xPos = turretTransform.position.x;//this.transform.position.x;
		float yPos = turretTransform.position.y;//this.transform.position.y;
		Vector3 dir = GetComponent<ShipControl> ().getDirection () * 20;
		CmdSpawnBasicBullet (xPos, yPos, dir.x, dir.y);

	}
		
	IEnumerator FireAfterTime()
	{
		canShoot = false;
		ShootBullet ();
		yield return new WaitForSeconds(fireRate);
		canShoot = true;
	}

}
