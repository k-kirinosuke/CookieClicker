using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

	private const int MAX_ORB = 10;
	private const int RESPAWNTIME = 1;
	private const int MAXLEVEL = 2;

	public GameObject orbPrefab;
	public GameObject smokePrefab;
	public GameObject kusudamaPrefab;
	public GameObject canvasGame;
	public GameObject textScore;
	public GameObject imageTemple;

	private int score = 0;
	private int nextScore = 10;
	private int currentOrb = 0;
	private int templeLevel;
	private DateTime lastDateTime;
	private int[] nextScoreTable = new int[] {10, 10, 10};

	// Use this for initialization
	void Start () {
		currentOrb = 10;
		for (int i = 0; i < currentOrb; i++) {
			CreateOrb ();
		}
		lastDateTime = DateTime.UtcNow;
		nextScore = nextScoreTable [templeLevel];
		imageTemple.GetComponent<TempleManager> ().SetTemplePicture (templeLevel);
		imageTemple.GetComponent<TempleManager> ().SetTempleScale (score, nextScore);

		RefreshScoreText ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentOrb < MAX_ORB) {
			TimeSpan timeSpan = DateTime.UtcNow - lastDateTime;

			if (timeSpan >= TimeSpan.FromSeconds (RESPAWNTIME)) {
				while (timeSpan >= TimeSpan.FromSeconds (RESPAWNTIME)) {
					CreateNewOrb ();
					timeSpan -= TimeSpan.FromSeconds (RESPAWNTIME);
				}
			}
		}
	}

	public void CreateNewOrb(){
		lastDateTime = DateTime.UtcNow;
		if (currentOrb >= MAX_ORB) {
			return;
		}
		CreateOrb ();
		currentOrb++;
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

		if(score > nextScore){
			score = nextScore;
		}

		TempleLevelUp();
		RefreshScoreText ();

		imageTemple.GetComponent<TempleManager>().SetTempleScale(score, nextScore);
		if((score == nextScore) && (templeLevel == MAXLEVEL)){
			ClearEffect();
		}

		currentOrb--;
	}

	void RefreshScoreText(){
		textScore.GetComponent<Text>().text = "徳：" + score + " / " + nextScore;
	}

	void TempleLevelUp(){
		if (score >= nextScore) {
			if (templeLevel < MAXLEVEL) {
				templeLevel++;
				score = 0;

				TempleLevelUpEffect ();

				nextScore = nextScoreTable [templeLevel];
				imageTemple.GetComponent<TempleManager> ().SetTemplePicture (templeLevel);
			}
		}
	}

	void TempleLevelUpEffect(){
		GameObject smoke = (GameObject)Instantiate (smokePrefab);
		smoke.transform.SetParent (canvasGame.transform, false);
		smoke.transform.SetSiblingIndex (2);

		Destroy (smoke, 0.5f);
	}

	void ClearEffect(){
		GameObject kusudama = (GameObject)Instantiate (kusudamaPrefab);
		kusudama.transform.SetParent (canvasGame.transform, false);
	}
}
