using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnController : MonoBehaviour {

    private MGC Controller;


    // Use this for initialization
    void Start () {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
    }
	
	// Update is called once per frame
	void Update () {
        // Column just rotates to match the (User Controller) TowerAngle held in the Game Controller
        transform.localEulerAngles = new Vector3(0, Controller.TowerAngle,0);
	}
}
