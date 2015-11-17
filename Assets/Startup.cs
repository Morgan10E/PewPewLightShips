using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {

	public static int mapSize = 256;
	private static int BLOCK = 1;
	private static int EXPLODIE = 2;

	public Transform block;
	public Transform explodieBlock;
	public Transform debugBlock;
	public static double F2 = 0.5*(Mathf.Sqrt(3)-1.0);
	public static double G2 = (3.0-Mathf.Sqrt(3))/6.0;
	public static Vector3 [] grad3 = new Vector3[12] {new Vector3(1,1,0),new Vector3(-1,1,0),new Vector3(1,-1,0),new Vector3(-1,-1,0),
									 new Vector3(1,0,1),new Vector3(-1,0,1),new Vector3(1,0,-1),new Vector3(-1,0,-1),
									 new Vector3(0,1,1),new Vector3(0,-1,1),new Vector3(0,1,-1),new Vector3(0,-1,-1)};

	public int [,] map = new int[mapSize,mapSize];
	public double frequency = 3.0 / mapSize;
	public static int [] p;
	public static int [] perm = new int[512];
	public static int [] permMod12 = new int[512];
	// Use this for initialization
	void Start () {
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Motherships"), LayerMask.NameToLayer("Little Enemies"));

		p = randomArray();
		for(int i=0; i<512; i++)
		{
			perm[i]=p[i & 255];
			permMod12[i] = (short)(perm[i] % 12);
		}

		double [,] simplexMap = generateSimplexMap ();
		generateMap (simplexMap);
		buildMap ();
	}

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
		}

		for (int r = row; r < row + size; r++) {
			for (int c = column; c < column + size; c++) {
				map[r, c] = -1;
			}
		}

		return size;
	}

	void buildMap() {
		//buildMapDebug ();

		for (int row = 0; row < mapSize; row++) {
			for (int column = 0; column < mapSize; column++) {
				if (map[row, column] != 0 && map[row, column] != -1)
					buildNextBlock(row, column);
			}
		}

	}

	/*
	void buildMap() {
		for (int row = 0; row < mapSize; row++) {
			for (int column = 0; column < mapSize; column++) {
				if (map[row,column] == 1) {
					Instantiate(block, new Vector3(row, column, 0), Quaternion.identity);
				}
			}
		}
	}*/


	void buildMapDebug() {
		for (int row = 0; row < mapSize; row++) {
			for (int column = 0; column < mapSize; column++) {
				if (map[row,column] == 1) {
					Instantiate(debugBlock, new Vector3(row, column, 0), Quaternion.identity);
				}
			}
		}
	}

	void generateMap (double[,] simplexMap) {
		for (int r = 0; r < mapSize; r++) {
			for (int c = 0; c < mapSize; c++) {
				if (simplexMap[r,c] > 0.5) {
					map[r,c] = BLOCK;
				} else if (simplexMap[r,c] > 0.48) {
					map[r,c] = EXPLODIE;
				} else {
					map[r,c] = 0;
				}
			}
		}
	}

	double [,] generateSimplexMap () {
		double [,] simplexMap = new double[mapSize,mapSize];
		for (int row = 0; row < mapSize; row++) {
			for (int column = 0; column < mapSize; column++) {
				simplexMap[row, column] = noise ((double)row * frequency, (double)column * frequency);
				simplexMap[row, column] = (simplexMap[row, column] + 1)/2;
			}
		}
		return simplexMap;
	}

	double dot(Vector2 g, double x, double y) {
		return g.x*x + g.y*y;
	}

	int [] randomArray() {
		int [] numbers = new int[256];
		for (int i = 0; i < 256; i++) {
			numbers[i] = i;
		}
		for (int i = 0; i < 256; i++) {
			int randNum = Random.Range (0, 256);
			int temp = numbers[i];
			numbers[i] = numbers[randNum];
			numbers[randNum] = temp;
		}
		return numbers;
	}

	int fastfloor(double x) {
		int xi = (int)x;
		return x<xi ? xi-1 : xi;
	}

	double noise(double xin, double yin) {
		double n0, n1, n2; // Noise contributions from the three corners
		// Skew the input space to determine which simplex cell we're in
		double s = (xin+yin)*F2; // Hairy factor for 2D
		int i = fastfloor(xin+s);
		int j = fastfloor(yin+s);
		double t = (i+j)*G2;
		double X0 = i-t; // Unskew the cell origin back to (x,y) space
		double Y0 = j-t;
		double x0 = xin-X0; // The x,y distances from the cell origin
		double y0 = yin-Y0;
		// For the 2D case, the simplex shape is an equilateral triangle.
		// Determine which simplex we are in.
		int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
		if(x0>y0) {i1=1; j1=0;} // lower triangle, XY order: (0,0)->(1,0)->(1,1)
		else {i1=0; j1=1;}      // upper triangle, YX order: (0,0)->(0,1)->(1,1)
		// A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
		// a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
		// c = (3-sqrt(3))/6
		double x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
		double y1 = y0 - j1 + G2;
		double x2 = x0 - 1.0 + 2.0 * G2; // Offsets for last corner in (x,y) unskewed coords
		double y2 = y0 - 1.0 + 2.0 * G2;
		// Work out the hashed gradient indices of the three simplex corners
		int ii = i & 255;
		int jj = j & 255;
		int gi0 = permMod12[ii+perm[jj]];
		int gi1 = permMod12[ii+i1+perm[jj+j1]];
		int gi2 = permMod12[ii+1+perm[jj+1]];
		// Calculate the contribution from the three corners
		double t0 = 0.5 - x0*x0-y0*y0;
		if(t0<0) n0 = 0.0;
		else {
			t0 *= t0;
			n0 = t0 * t0 * dot(grad3[gi0], x0, y0);  // (x,y) of grad3 used for 2D gradient
		}
		double t1 = 0.5 - x1*x1-y1*y1;
		if(t1<0) n1 = 0.0;
		else {
			t1 *= t1;
			n1 = t1 * t1 * dot(grad3[gi1], x1, y1);
		}
		double t2 = 0.5 - x2*x2-y2*y2;
		if(t2<0) n2 = 0.0;
		else {
			t2 *= t2;
			n2 = t2 * t2 * dot(grad3[gi2], x2, y2);
		}
		// Add contributions from each corner to get the final noise value.
		// The result is scaled to return values in the interval [-1,1].
		return 70.0 * (n0 + n1 + n2);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
