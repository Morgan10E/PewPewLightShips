using UnityEngine;
using System.Collections;

public class NavMesh : MonoBehaviour {

	public int[,] map;
	public int width, height;

	void CreateNavMesh(int[,] _map) {
		map = _map;
		width = map.GetLength (0);
		height = map.GetLength (1);
	}

	void DoSomething() {
	}

}
