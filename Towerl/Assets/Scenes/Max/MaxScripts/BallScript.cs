using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    // Game Controller for persistent data
    MaxGameController MGC;


    // Use this for initialization
    void Start () {

   // Get Game Cpntroller reference
   MGC = GameObject.Find("MaxGameController").GetComponent<MaxGameController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
