//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 1: Mobile Game            //
//                                      //
// Team Heron                           //
//                                      //
// December 2018                        //
//                                      //
// TOWERL Code                          //
// ColumnController.cs                  //
//////////////////////////////////////////

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
