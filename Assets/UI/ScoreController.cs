using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	Text scoreText;
	int[] teamScores;

	void Awake() {

	}


	// Use this for initialization
	void Start () {
		scoreText = GetComponent<Text> ();
		TeamManager tm = GameObject.Find ("TeamManager").GetComponent<TeamManager> ();
		teamScores = new int[tm.numPlayerTeams];
		updateDisplay ();
	}

	void updateDisplay() {
		string temp = "" + teamScores[0].ToString();
		for (int i = 1; i < teamScores.Length; i++) {
			temp = temp + " | " + teamScores [i];
		}
		scoreText.text = temp;
	}
		
	public void teamScored(int teamNum) {
		teamScores[teamNum] = teamScores[teamNum] + 1;
		updateDisplay ();
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
