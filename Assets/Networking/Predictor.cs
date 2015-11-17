using UnityEngine;
using System.Collections;

public class Predictor : MonoBehaviour {

	public Transform observedTransform;
	//public Rigidbody2D observedBody;
	public PlayerControlClient receiver;
	public float pingMargin = 0.5f;

	private float clientPing;
	private NetState[] serverStateBuffer = new NetState[20];

	void Awake() {
		receiver = GetComponent<PlayerControlClient>();
		observedTransform = this.transform;
		//observedBody = this.rigidbody2D;
	}

	public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		Vector3 pos = observedTransform.position;
		Quaternion rot = observedTransform.rotation;
		//Vector2 velocity = observedBody.velocity;
		//Vector3 velForSending = new Vector3(velocity.x, velocity.y);

		if (stream.isWriting) {
			// Debug.Log("Server is writing");
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			//stream.Serialize(ref velForSending);
		} else {
			// we are the client
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			//stream.Serialize(ref velForSending);
			//velocity = new Vector2(velForSending.x, velForSending.y);
			receiver.serverPosition = pos;
			receiver.serverRotation = rot;
			//receiver.rigidbody2D.velocity = velocity;

			// Smooth client position
			receiver.lerpToTarget();
			receiver.positionToAim = pos;

			for (int i = serverStateBuffer.Length - 1; i >= 1; i--) {
				serverStateBuffer[i] = serverStateBuffer[i - 1];
			}

			serverStateBuffer[0] = new NetState((float) info.timestamp, pos, rot);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.isServer || (Network.player == receiver.GetOwner ())) {
			return; //Only for remote peers
		}

		clientPing = (Network.GetAveragePing(Network.connections[0]) / 100) + pingMargin;
		float interpolationTime = ((float) Network.time) - clientPing;

		// make sure buffer has at least one element
		if (serverStateBuffer[0] == null) {
			serverStateBuffer[0] = new NetState(0, transform.position, transform.rotation);
		}

		if (serverStateBuffer[0].timestamp > interpolationTime) {
			for (int i = 0; i < serverStateBuffer.Length; i++) {
				if (serverStateBuffer[i] == null) {
					continue;
				}

				if (serverStateBuffer[i].timestamp <= interpolationTime || i == serverStateBuffer.Length) {
					NetState bestTarget = serverStateBuffer[Mathf.Max (i-1, 0)];
					NetState bestStart = serverStateBuffer[i];
					
					float timeDiff = bestTarget.timestamp - bestStart.timestamp;
					float lerpTime = 0f;
					
					if (timeDiff > 0.0001) {
						lerpTime = ((interpolationTime - bestStart.timestamp) / timeDiff);
					}
					
					transform.position = Vector3.Lerp (bestStart.pos, bestTarget.pos, lerpTime);
					transform.rotation = Quaternion.Slerp (bestStart.rot, bestTarget.rot, lerpTime);
					//rigidbody2D.velocity = bestTarget.velocity;
					return; // we have found the correct target
				}
			}


		} else {
			NetState latest = serverStateBuffer[0];
			transform.position = Vector3.Lerp(transform.position, latest.pos, 0.5f);
			transform.rotation = Quaternion.Slerp(transform.rotation, latest.rot, 0.5f);
			//rigidbody2D.velocity = latest.velocity;
		}
	}
}
