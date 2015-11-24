using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player_SyncPosition : NetworkBehaviour {

	[SyncVar (hook = "SyncPositionValues")] private Vector3 syncPos;
	[SyncVar] private Vector3 syncTurretPos;

	[SerializeField] Transform myTransform;
	[SerializeField] Transform turretTransform;
	[SerializeField] float LerpRate = 15;
	private float normalLerpRate = 15;
	private float fasterLerpRate = 25;

	private List<Vector3> syncPosList = new List<Vector3>();
	[SerializeField] bool useHistoricalLerping = false;
	private float closeEnough = 0.1f;

	// Update is called once per frame
	void FixedUpdate () {
		TransmitPosition ();
		LerpPosition ();
	}

	void LerpPosition() {
		if (!isLocalPlayer) {
			if (useHistoricalLerping) {
				HistoricalLerp();
			} else {
				OrdinaryLerp();
			}

		}
	}

	void SyncPositionValues(Vector3 latestPos) {
		syncPos = latestPos;
		syncPosList.Add (syncPos);
	}

	void OrdinaryLerp() {
		Vector2 deltaVec = myTransform.position - syncPos;
		float distanceDelta = Mathf.Abs(deltaVec.x) + Mathf.Abs (deltaVec.y);
		if (distanceDelta > 0.1) {
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * LerpRate);
		} else {
			myTransform.position = syncPos;
		}
		turretTransform.position = Vector3.Lerp (turretTransform.position, syncTurretPos, Time.deltaTime * LerpRate);
	}

	void HistoricalLerp() {
		if (syncPosList.Count > 0) {
			myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * LerpRate);

			if (Vector3.Distance(myTransform.position, syncPosList[0]) < closeEnough) {
				syncPosList.RemoveAt(0);
			}

			if (syncPosList.Count > 10) {
				LerpRate = fasterLerpRate;
			} else {
				LerpRate = normalLerpRate;
			}
		}
	}

	[Command]
	void CmdProvidePositionToServer(Vector3 pos, Vector3 turPos) {
		syncPos = pos;
		syncTurretPos = turPos;
	}

	[ClientCallback]
	void TransmitPosition() {
		if (isLocalPlayer) {
			CmdProvidePositionToServer (myTransform.position, turretTransform.position);
		}
	}
}
