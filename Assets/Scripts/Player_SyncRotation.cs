using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player_SyncRotation : NetworkBehaviour {

	[SyncVar /*(hook = "OnPlayerRotationSync")*/] private Quaternion syncRotation;

	[SerializeField] Transform myTransform;
	[SerializeField] float LerpRate = 15;
//	private float normalLerpRate = 15;
//	private float fasterLerpRate = 25;

//	private List<float> syncRotationList = new List<float>();
//	private float closeEnough = 0.5f;

//	[SerializeField] bool useHistoricalLerp = false;

	// Update is called once per frame
	void FixedUpdate () {
		TransmitRotation ();
		LerpRotation ();
	}

	void LerpRotation() {
		if (!isLocalPlayer) {
			myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncRotation, Time.deltaTime * LerpRate);
		}
	}

	[Command]
	void CmdProvideRotationToServer(Quaternion rotation) {
		syncRotation = rotation;
	}
	
	[ClientCallback]
	void TransmitRotation() {
		if (isLocalPlayer) {
			CmdProvideRotationToServer (myTransform.rotation);
		}
	}
}
