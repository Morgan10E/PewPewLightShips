using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIGlobalManager : MonoBehaviour {

	public WaypointGraph graph;
	public string seed = "destiny";
	public bool useRandomSeed = false;
	public float deltaDist = 1.0f;
	System.Random prng;
	public List<Rigidbody2D> players;

	public void setWaypointGraph (WaypointGraph graph) {
		this.graph = graph;
	}

	public Waypoint getNearest(Vector2 location) {
		return graph.getClosestWaypoint (location);
	}

	public Waypoint getRandomWaypoint() {
		return graph.waypoints [prng.Next (0, graph.waypoints.Count)];
	}

	public bool closeToWaypoint(Vector2 pos, Waypoint w) {
		return Vector2.Distance (w.loc, pos) < deltaDist;
	}

	public void addPlayer(Rigidbody2D body) {
		players.Add(body);
	}

	// Use this for initialization
	void Start () {
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}
		prng = new System.Random (seed.GetHashCode ());
		players = new List<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
