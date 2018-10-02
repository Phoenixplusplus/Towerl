using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour {

    private MGC Controller;
    private int CurrentTier;

    // Use this for initialization
    void Start () {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
        CurrentTier = Controller.TiersPerLevel;
        transform.position = new Vector3(transform.position.x, CurrentTier, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
