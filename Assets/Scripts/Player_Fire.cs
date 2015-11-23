using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Fire : NetworkBehaviour {

	public GameObject projectile;
	[SerializeField] Transform turretTransform;

	void Start() {

	}

	void Update() {
		MouseUpdate ();
	}

	void MouseUpdate () {
		if (Input.GetMouseButtonDown(0)) {
			ShootBullet();
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
		

}
