using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MokugyoManager : MonoBehaviour {

    public GameObject gameManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TapMokugyo(){
        gameManager.GetComponent<GameManager>().CreateNewOrb();
    }
}
