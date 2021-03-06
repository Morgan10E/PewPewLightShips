﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public int width = 100;
	public int height = 100;

	public string seed = "test";
	public bool useRandomSeed = true;
	public bool pvp = false;

	[Range(0, 100)]
	public int randomFillPercent = 50;
	public bool clickToRegen = true;
	public int roomsToGen = 2;
	public int roomSize = 25;

	public GameObject spawnPrefab;
	List<Vector2> enemySpawnLocations;

	public AIGlobalManager aiManager;

	int[,] map;

	// Use this for initialization
	void Start () {
		GenerateMap ();
	}

	void Update() {
		if (clickToRegen && Input.GetMouseButtonDown (0)) {
			GenerateMap ();
		}
	}

	public List<Vector2> GetEnemySpawnLocation() {
		return enemySpawnLocations;
	}
	
	void GenerateMap() {
		map = new int[width, height];
		//RandomFillMap ();
		// create the initial perlin noise map
		PerlinFillMap();
		IdentifyPoints();

		// Generate the main rooms
		List<Vector2> rooms = GenerateRoomCenters(roomsToGen, roomSize, 2);
		rooms = ForceDirectRooms (rooms, roomSize, 0.1f, 80, width, height, 20);
		for (int i = 0; i < rooms.Count; i++) {
			EmptyCircle((int) rooms[i].x, (int) rooms[i].y, 15);
		}


		for (int i = 0; i < 5; i++) {
			// use cellular automata to smooth out the map
			SmoothMap ();
		}

		// add in the spawn point
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}
		System.Random prng = new System.Random (seed.GetHashCode ());
		int spawnIndex = prng.Next (0, rooms.Count);
		CreateSpawnPoint(rooms [spawnIndex]);
		rooms.RemoveAt (spawnIndex);
		if (pvp) {
			CreateSpawnPoint(rooms [spawnIndex]);
		} else {
			// spawn in a single enemy
			int enemyIndex = prng.Next (0, rooms.Count);
			StoreEnemySpawns (rooms);
		}


		// add a border around the map
		int borderSize = 5;
		int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];
		for (int x = 0; x < borderedMap.GetLength(0); x++) {
			for (int y = 0; y < borderedMap.GetLength(1); y++) {
				if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
					borderedMap[x,y] = map[x-borderSize,y-borderSize];
				} else {
					borderedMap[x,y] = 1;
				}
			}
		}

		// generate the mesh
		GetComponent<MeshGenerator> ().GenerateMesh (borderedMap, 1);

		// see which nodes are connected
		// note that we need to call debug perlin first for this to work
		graph = new WaypointGraph (debugPoints, width, height);
		// test the waypoint graph a bit
		Waypoint startPoint = graph.waypoints[prng.Next(0, graph.waypoints.Count)];
		Waypoint endPoint = graph.waypoints[prng.Next(0, graph.waypoints.Count)];
		// attach the waypoint graph to the ai manager
		aiManager.setWaypointGraph(graph);

	}

	void CreateSpawnPoint(Vector2 spawnLoc) {
		spawnLoc -= new Vector2(width/2, height/2);
		Instantiate(spawnPrefab, new Vector3(spawnLoc.x, spawnLoc.y), Quaternion.identity);
	}

	void StoreEnemySpawns(List<Vector2> rooms) {
		enemySpawnLocations = new List<Vector2> (rooms);
		for (int i = 0; i < enemySpawnLocations.Count; i++) {
			enemySpawnLocations[i] -= new Vector2(width/2, height/2);
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
		float low = radius + 1;
		float xHigh = width - radius - 1;
		float yHigh = height - radius - 1;
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
				} else if (centers [i].y > yHigh) {
					centers [i] = new Vector2(centers[i].x, yHigh);
				}
			}
		}
		return centers;

	}



	void EmptyCircle(int cx, int cy, float radius) {
		int rad = (int)Mathf.Floor (radius);
		for (int x = cx - rad; x <= cx + rad; x++) {
			for (int y = cy - rad; y <= cy + rad; y++) {
				float dist = new Vector2 (cx - x, cy - y).magnitude;
				if (x < 0 || x >= width || y < 0 || y >= height) {
					Debug.Log ("x: " + x.ToString () + ", y: " + y.ToString ());
					continue;
				}
				if (dist < radius) {
					map [x, y] = 0;
				}
			}
		}
	}

	void RandomFillMap() {
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}

		System.Random prng = new System.Random (seed.GetHashCode ());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;
				} else {
					map [x, y] = (prng.Next (0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
		}
	}



	void PerlinFillMap() {
		// the scale for what we decide is filled or not
		//float thresh = 0.5f;
		float thresh = randomFillPercent / 100f;

		// the zoom we have on the noise
		float scale = 10.0f / (Mathf.Max (width, height));

		// random offset into the perlin noise sample
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}
		System.Random prng = new System.Random (seed.GetHashCode ());
		float xOffset = (float) prng.NextDouble () * 10;
		float yOffset = (float) prng.NextDouble () * 10;

		// go through and sample the noise
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;
				} else {
					float xCoord = x * scale + xOffset;
					float yCoord = y * scale + yOffset;
					map [x, y] = (Mathf.PerlinNoise(xCoord, yCoord) < thresh) ? 1 : 0;
				}
			}
		}

	}

	void OnDrawGizmos() {
		if (graph != null) {
			graph.drawGizmos ();
		}
	}

	List<Vector2> debugPoints;
	WaypointGraph graph;

	void IdentifyPoints() {

		debugPoints = new List<Vector2> ();
		// the scale for what we decide is filled or not
		//float thresh = 0.5f;
		float thresh = 1 - (randomFillPercent / 100f) + 0.1f;

		// the zoom we have on the noise
		float scale = 10.0f / (Mathf.Max (width, height));

		// random offset into the perlin noise sample
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}
		System.Random prng = new System.Random (seed.GetHashCode ());
		float xOffset = (float) prng.NextDouble () * 10;
		float yOffset = (float) prng.NextDouble () * 10;

		int[,] data = new int[width, height];

		// go through and sample the noise
		for (int x = 1; x < width - 1; x++) {
			for (int y = 1; y < height - 1; y++) {
				float xCoord = x * scale + xOffset;
				float yCoord = y * scale + yOffset;
				if (Mathf.PerlinNoise (xCoord, yCoord) > thresh) {
					//debugPoints.Add (new Vector2 (x, y));
					data [x, y] = 0;
				} else {
					data [x, y] = 1;
				}
			}
		}

		// now, we use flood fill to detect each "room" and compute centroids
		FloodFillCentroids(data);
	}

	void FloodFillCentroids(int[,] data) {
		for (int x = 0; x < data.GetLength (0); x++) {
			for (int y = 0; y < data.GetLength (1); y++) {
				if (data [x, y] == 1)
					continue;
				Vector2 centroid = GetCentroidFloodFill (data, x, y);
				debugPoints.Add (centroid);
			}
		}
	}

	Vector2 GetCentroidFloodFill(int[,] data, int x, int y) {
		List<Vector2> points = new List<Vector2> ();
		FloodFillHelper(data, points, x, y);
		Vector2 result = Vector2.zero;
		foreach (Vector2 point in points) {
			result += point;
		}
		return result / points.Count;
	}

	void FloodFillHelper(int[,] data, List<Vector2> points, int x, int y) {
		points.Add (new Vector2 (x, y));
		//Debug.Log ("x: " + x + ", y: " + y);
		data [x, y] = 1;
		if (x > 0 && data [x - 1, y] == 0)
			FloodFillHelper (data, points, x - 1, y);
		if (x < data.GetLength(0) - 1 && data[x+1, y] == 0) 
			FloodFillHelper(data, points, x + 1, y);
		if (y > 0 && data[x, y-1] == 0) 
			FloodFillHelper(data, points, x, y - 1);
		if (y < data.GetLength(1) - 1 && data[x, y+1] == 0) 
			FloodFillHelper(data, points, x, y + 1);
		
	}

	void SmoothMap() {
		int[,] newMap = (int[,]) map.Clone();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighbors = GetSurroundingWallCount (x, y);
				if (neighbors > 4) {
					newMap [x, y] = 1;
				} else if (neighbors < 4) {
					newMap [x, y] = 0;
				}
			}
		}
		map = newMap;
	}

	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int x = gridX - 1; x <= gridX + 1; x++) {
			for (int y = gridY - 1; y <= gridY + 1; y++) {
				if (x != gridX || y != gridY) {
					if (x >= 0 && y >= 0 && x < width && y < height) {
						wallCount += map [x, y];
					} else {
						wallCount++;
					}
				} 
			}
		}
		return wallCount;
	}
		
		
}
