using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private const int MAX_ORB = 10;

	public GameObject orbPrefab;
	public GameObject canvasGame;
	public GameObject textScore;

	private int score = 0;
	private int nextScore = 100;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < MAX_ORB; i++) {
			CreateOrb ();
		}
		RefreshScoreText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateOrb(){
		GameObject orb = (GameObject)Instantiate (orbPrefab);
		orb.transform.SetParent (canvasGame.transform, false);
		orb.transform.localPosition = new Vector3 (
			UnityEngine.Random.Range (-300.0f, 300.0f),
			UnityEngine.Random.Range (-140.0f, -500.0f),
			0f);
	}

	public void GetOrb(){
		score += 1;
		RefreshScoreText ();
	}

	void RefreshScoreText(){
		textScore.GetComponent<Text>().text = "徳：" + score + " / " + nextScore;
	}
}
