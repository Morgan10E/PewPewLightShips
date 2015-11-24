using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AfterImage : NetworkBehaviour {
	public GameObject particlePrefab;
	GameObject afterImage = null;
	ParticleSystem pSys = null;

	// Use this for initialization
	void Start () {
		// TODO: have this happen when the player connects
		AttachParticleSystem ();
	}

	public void AttachParticleSystem() {
		afterImage = Instantiate(particlePrefab);
		afterImage.transform.parent = gameObject.transform;
		pSys = afterImage.GetComponent<ParticleSystem> ();
		pSys.enableEmission = false;
	}

	public void EnableAfterImage() {
//		if (pSys == null)
//			AttachParticleSystem ();
		pSys.startRotation = ((gameObject.transform.eulerAngles.z - 180) % 360) * Mathf.Deg2Rad;
		afterImage.transform.position = gameObject.transform.position;
		
		pSys.enableEmission = true;

		if (isLocalPlayer) {
			SetAfterImage(true);
		}
	}

	public void DisableAfterImage() {
		pSys.enableEmission = false;
		if (isLocalPlayer) {
			SetAfterImage(false);
		}
	}

	[ClientRpc]
	void RpcSetAfterImageClient(bool enable) {
		if (isLocalPlayer)
			return;
		if (enable) {
			pSys.startRotation = ((gameObject.transform.eulerAngles.z - 180) % 360) * Mathf.Deg2Rad;
			afterImage.transform.position = gameObject.transform.position;
			pSys.enableEmission = true;
		} else {
			pSys.enableEmission = false;
		}
	}

	[Command]
	void CmdEnableAfterImage(bool enable) {
		RpcSetAfterImageClient (enable);
	}

	[ClientCallback]
	void SetAfterImage(bool enable) {
		CmdEnableAfterImage (enable);
	}
}
