using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoint {

	public Vector2 loc;
	public List<Waypoint> connections;

	public Waypoint(Vector2 _loc) {
		loc = _loc;
		connections = new List<Waypoint> ();
	}

	public void AddConnection(Waypoint other) {
		Vector2 dir = other.loc - this.loc;
		RaycastHit2D hit = Physics2D.Raycast (this.loc, dir, dir.magnitude);
		if (hit.collider == null) {
			// we found a clear line of sight
			this.connections.Add (other);
			other.connections.Add (this);
			//Debug.Log ("connection made");
		} else {
			//Debug.Log (hit.collider);
		}
	}
}
