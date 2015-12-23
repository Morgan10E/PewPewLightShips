using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointGraph {

	public List<Waypoint> waypoints;
	

	public WaypointGraph(List<Vector2> points, int width, int height) {
		waypoints = new List<Waypoint> ();
		for (int i = 0; i < points.Count; i++) {
			waypoints.Add (new Waypoint (points [i] - new Vector2(width/2, height/2)));
		}

		// build the waypoint connections. NOTE: the mesh
		// must already have been built before this is called
		for (int i = 0; i < waypoints.Count; i++) {
			for (int j = i + 1; j < waypoints.Count; j++) {
				waypoints [i].AddConnection (waypoints [j]);
			}
		}
	}

	public void drawGizmos() {
		Gizmos.color = Color.cyan;
		foreach (Waypoint point in waypoints) {
			Vector2 location = point.loc;
			Gizmos.DrawCube (location, Vector3.one * 3);
			foreach (Waypoint connection in point.connections) {
				//Debug.Log ("drawing connection");
				Vector2 end = connection.loc;
				Gizmos.DrawLine (location, end);
			}
		}
	}
}
