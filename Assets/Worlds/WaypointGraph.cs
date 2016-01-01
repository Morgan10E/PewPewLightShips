using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointGraph {

	public List<Waypoint> waypoints;
	public List<Waypoint> unconnected;
	

	public WaypointGraph(List<Vector2> points, int width, int height) {
		waypoints = new List<Waypoint> ();
		unconnected = new List<Waypoint> ();
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

		pruneGraph ();
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

	public Waypoint getClosestWaypoint(Vector2 location) {
		Waypoint closest = waypoints [0];
		float minDistance = Vector2.Distance (closest.loc, location);

		foreach (Waypoint w in waypoints) {
			float dist = Vector2.Distance (w.loc, location);
			if (dist < minDistance) {
				minDistance = dist;
				closest = w;
			}
		}

		return closest;
	}

	public void pruneGraph() {
		foreach (Waypoint w in waypoints) {
			if (w.connections.Count == 0) {
				unconnected.Add (w);
			}
		}

		foreach (Waypoint w in unconnected) {
			waypoints.Remove (w);
		}
	}

	public List<Waypoint> ShortestPath(Waypoint a, Waypoint b) {
		Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint> ();
		Dictionary<Waypoint, float> costSoFar = new Dictionary<Waypoint, float> ();
		PQueue frontier = new PQueue();
		cameFrom[a] = null;
		costSoFar [a] = 0f;
		frontier.push (a, 0);
		while (!frontier.empty ()) {
			Waypoint current = frontier.pop ();
			if (current == b)
				break;
			foreach (Waypoint next in current.connections) {
				float newCost = costSoFar [current] + Waypoint.distance (current, next);
				if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
					costSoFar [next] = newCost;
					frontier.push (next, newCost);
					cameFrom [next] = current;
				}
			}
		}

		List<Waypoint> path = new List<Waypoint> ();
		Waypoint point = b;
		while (point != a) {
			path.Add (point);
			point = cameFrom[point];
		}
		path.Add (a);
		path.Reverse ();
		return path;
	}
}
