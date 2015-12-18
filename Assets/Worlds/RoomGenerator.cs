using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour {
	public int width = 100;
	public int height = 100;
	public List<Vector2> rooms;
	public bool useRandomSeed = true;
	public string seed = "test";

	// Use this for initialization
	void Start () {
		rooms = GenerateRoomCenters (3, 5, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			// run an iteration of force direct
			rooms = ForceDirectRooms(rooms, 5f, 0.05f, 40f, width, height, 1);
		}
	}

	void OnDrawGizmos() {
		if (rooms != null) {
			for (int i = 0; i < rooms.Count; i++) {
				Gizmos.DrawCube(new Vector3(rooms[i].x, rooms[i].y), new Vector3(5, 5, 5));
			}
		}
	}

	List<Vector2> GenerateRoomCenters(int numRooms, float radius, int borderWidth) {
		// create a box which the player will spawn in
		int low = (int) (0 + radius + borderWidth);
		int xHigh = (int) (width - radius - borderWidth);
		int yHigh = (int) (height - radius - borderWidth);

		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}

		System.Random prng = new System.Random (seed.GetHashCode ());
		List<Vector2> result = new List<Vector2> ();
		for (int i = 0; i < numRooms; i++) {
			// get a random center point
			int cx = prng.Next (low, xHigh);
			int cy = prng.Next (low, yHigh);
			result.Add (new Vector2 (cx, cy));
		}
		return result;

	}

	List<Vector2> ForceDirectRooms(List<Vector2> centers, float radius, float force, float dist, int width, int height, int numIterations) {
		// need to add in border width to radius
		Vector2[] forces = new Vector2[centers.Count];
		int low = (int) (0 + radius);
		int xHigh = (int) (width - radius);
		int yHigh = (int) (height - radius);
		for (int n = 0; n < numIterations; n++) {
			// 0 out the forces
			for (int i = 0; i < centers.Count; i++) {
				forces [i] = Vector2.zero;
			}

			// calculate the forces
			for (int i = 0; i < centers.Count; i++) {
				for (int j = i + 1; j < centers.Count; j++) {
					Vector2 delta = centers [i] - centers [j];
					if (delta.magnitude <= dist) {
						// repel each other
						Vector2 applyForce = delta.normalized * (dist - delta.magnitude) * force;
						forces [i] += applyForce;
						forces [j] -= applyForce;
					} else {
						// pull towards each other
						Vector2 applyForce = delta.normalized * (delta.magnitude - dist) * force;
						forces [i] -= applyForce;
						forces [j] += applyForce;
					}
				}
			}

			// apply the forces
			for (int i = 0; i < centers.Count; i++) {
				centers [i] += forces [i];
				// ensure that we haven't gone off the board
				if (centers [i].x < low) {
					centers [i] = new Vector2(low, centers[i].y);
				} else if (centers [i].x > xHigh) {
					centers [i] = new Vector2(xHigh, centers[i].y);
				}

				if (centers [i].y < low) {
					centers [i] = new Vector2(centers[i].x, low);
				} else if (centers [i].x > yHigh) {
					centers [i] = new Vector2(centers[i].x, yHigh);
				}
			}
		}
		return centers;

	}
}
