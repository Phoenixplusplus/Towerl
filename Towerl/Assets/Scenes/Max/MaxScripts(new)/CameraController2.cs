using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour {

    private MGC Controller;

    // Use this for initialization
    void Start () {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
        ResetCameraToTop();
    }
	
	// Update is called once per frame
	void Update () {
		if (Controller.BallFalling)
        {
            transform.position = new Vector3(transform.position.x, Controller.BallHeight + 1, transform.position.z);
        }
	}

    public void ResetCameraToTop ()
    {
        transform.position = new Vector3(transform.position.x, Controller.TiersPerLevel, transform.position.z);
    }

    public void SetToHeight(int Height)
    {
        transform.position = new Vector3(transform.position.x, Height, transform.position.z);
    }

}
