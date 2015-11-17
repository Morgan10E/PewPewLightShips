using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class Loader : MonoBehaviour {

	public static int MAP_SIZE = 256;

	/*private bool Load(string fileName) {
		// Handle any problems that might arise when reading the text
		try
		{
			string line;
			// Create a new StreamReader, tell it which file to read and what encoding the file
			// was saved as
			StreamReader reader = new StreamReader(fileName, Encoding.Default);
			
			// Immediately clean up the reader after this block of code is done.
			// You generally use the "using" statement for potentially memory-intensive objects
			// instead of relying on garbage collection.
			// (Do not confuse this with the using directive for namespace at the 
			// beginning of a class!)
			using (reader)
			{
				// While there's lines left in the text file, do this:
				do
				{
					line = reader.ReadLine();
					
					if (line != null)
					{
						// Do whatever you need to do with the text line, it's a string now
						// In this example, I split it into arguments based on comma
						// deliniators, then send that array to DoStuff()
						string[] entries = line.Split(',');
						if (entries.Length > 0)
							DoStuff(entries);
					}
				}
				while (line != null);
				
				// Done reading, close the reader and return true to broadcast success    
				reader.Close();
				return true;
			}
		}
		
		// If anything broke in the try block, we throw an exception with information
		// on what didn't work
		catch (IOException e)
		{
			Console.WriteLine("{0}\n", e.Message);
			return false;
		}
	}*/



	// Use this for initialization
	public void LoadMap () {
		//if (Load("data.txt")) Debug.Log("Text file loaded successfully");
		string data = System.IO.File.ReadAllText("Assets/worlds/raid_map_0.txt");
		string[] entries = data.Split(',');
		//int[] intData = new int[entries.Length];
		int [,] map = new int[MAP_SIZE,MAP_SIZE];
		for (int i = 0; i < MAP_SIZE; i++) {
			for (int j = 0; j < MAP_SIZE; j++) {
				map[i, j] = int.Parse(entries[i * MAP_SIZE + j]);
			}
		}

		GetComponent<WorldGeneration>().buildMap(map);
		//generator.buildMap(map);
	}

}
