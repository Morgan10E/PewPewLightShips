using UnityEngine;
using System.Collections;

public class WorldGeneration : MonoBehaviour {

	public int [,] map;

	public static int mapSize = 256;
	public static int EMPTY_BLOCK = 0;
	public static int BLOCK = 1;
	public static int EXPLODIE = 2;
	public static int BREAKABLE = 3;
	
	public Transform block;
	public Transform explodieBlock;
	public Transform breakableBlock;

	int buildNextBlock(int row, int column) {
		bool stillGood = true;
		int blockType = map [row, column];
		int size = 1;
		while (stillGood) {
			if (column + size >= mapSize || row + size >= mapSize) {
				break;
			}
			for (int r = row; r < row + size; r++) {
				if ( map[r,column + size] != blockType) {
					stillGood = false;
					break;
				}
			}
			if (!stillGood) break;
			for (int c = column; c < column + size; c++) {
				if (map[row + size, c] != blockType) {
					stillGood = false;
					break;
				}
			}
			if (stillGood) size++;
		}
		if (blockType == BLOCK) {
			Transform myBlock = (Transform)Instantiate (block, new Vector3 (row, column, 0), Quaternion.identity);
			myBlock.localScale = new Vector3 (size, size, 1);
		} else if (blockType == EXPLODIE) {
			Transform myBlock = (Transform)Instantiate (explodieBlock, new Vector3 (row, column, 0), Quaternion.identity);
			myBlock.localScale = new Vector3 (size, size, 1);
		} else if (blockType == BREAKABLE) {
			Transform myBlock = (Transform)Instantiate (breakableBlock, new Vector3 (row, column, 0), Quaternion.identity);
			myBlock.localScale = new Vector3 (size, size, 1);
		}
		
		for (int r = row; r < row + size; r++) {
			for (int c = column; c < column + size; c++) {
				map[r, c] = -1;
			}
		}
		
		return size;
	}
	
	public void buildMap(int[,] dataMap) {
		//buildMapDebug ();
		Debug.Log ("Building Map...");
		this.map = dataMap;
		for (int row = 0; row < mapSize; row++) {
			for (int column = 0; column < mapSize; column++) {
				if (map[row, column] != 0 && map[row, column] != -1)
					buildNextBlock(row, column);
			}
		}
		
	}

}
