using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

	private const int MAX_ORB = 10;
	private const int RESPAWNTIME = 5;
	private const int MAXLEVEL = 2;

	public GameObject orbPrefab;
	public GameObject smokePrefab;
	public GameObject kusudamaPrefab;
	public GameObject canvasGame;
	public GameObject textScore;
	public GameObject imageTemple;
    public GameObject imageMokugyo;


	private int score = 0;
	private int nextScore = 10;
	private int currentOrb = 0;
	private int templeLevel;
	private DateTime lastDateTime;
	private int[] nextScoreTable = new int[] {10, 100, 1000};
    private int numOfOrb;

	// Use this for initialization
	void Start () {
        score = PlayerPrefs.GetInt("SCORE", 0);
        templeLevel = PlayerPrefs.GetInt("LEVEL", 0);

		nextScore = nextScoreTable [templeLevel];
		imageTemple.GetComponent<TempleManager> ().SetTemplePicture (templeLevel);
		imageTemple.GetComponent<TempleManager> ().SetTempleScale (score, nextScore);

		RefreshScoreText ();
	}
	
	// Update is called once per frame
	void Update () {
        while(numOfOrb > 0){
            Invoke("CreateNewOrb", 0.1f * numOfOrb);
            numOfOrb--;
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
			UnityEngine.Random.Range (-100.0f, 100.0f),
			UnityEngine.Random.Range (-300.0f, -450.0f),
			0f);
        

		int kind = UnityEngine.Random.Range (0, templeLevel + 1);
		switch (kind) {
		case 0:
			orb.GetComponent<OrbManager> ().SetKind (OrbManager.ORB_KIND.BLUE);
			break;
		case 1:
			orb.GetComponent<OrbManager> ().SetKind (OrbManager.ORB_KIND.GREEN);
			break;
		case 2:
			orb.GetComponent<OrbManager> ().SetKind (OrbManager.ORB_KIND.PURPLE);
			break;
		}

        orb.GetComponent<OrbManager>().FlyOrb();


        AnimatorStateInfo stateInfo = imageMokugyo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if(stateInfo.fullPathHash == Animator.StringToHash("Base Layer.get@ImageMokugyo")){
            imageMokugyo.GetComponent<Animator>().Play(stateInfo.fullPathHash, 0, 0.0f);
        }else{
            imageMokugyo.GetComponent<Animator>().SetTrigger("isGetScore");
        }
	}

	public void GetOrb(int getScore){

        AnimatorStateInfo stateInfo = imageMokugyo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if(stateInfo.fullPathHash == Animator.StringToHash("Base Layer.get@ImageMokugyo")){
            imageMokugyo.GetComponent<Animator>().Play(stateInfo.fullPathHash, 0, 0.0f);
        }else{
            imageMokugyo.GetComponent<Animator>().SetTrigger("isGetScore");
        }

		if (score < nextScore) {
			score += getScore;

			if (score > nextScore) {
				score = nextScore;
			}

			TempleLevelUp ();
			RefreshScoreText ();

			imageTemple.GetComponent<TempleManager> ().SetTempleScale (score, nextScore);
			if ((score == nextScore) && (templeLevel == MAXLEVEL)) {
				ClearEffect ();
			}
		}

		currentOrb--;

        SaveGameData();
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

    void SaveGameData(){
        PlayerPrefs.SetInt("SCORE", score);
        PlayerPrefs.SetInt("LEVEL", templeLevel);
        PlayerPrefs.SetInt("ORB", currentOrb);
        PlayerPrefs.SetString("TIME", lastDateTime.ToBinary().ToString());

        PlayerPrefs.Save();
    }
}
