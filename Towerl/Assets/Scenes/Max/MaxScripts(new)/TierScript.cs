using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierScript : MonoBehaviour {

    // Master Script for a tier object ... the FIRST segement has this script only

    private MGC Controller;

    public int[] myData = new int[32];
    public float rotation = 0f;

	// Use this for initialization
	void Start () {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.localScale != Controller.SegmentScale)
        {
            transform.localScale = Controller.SegmentScale;
        }
	}

    int ReportType(float angle)
    {
        if (angle + rotation < 0)
        {
            angle += 360;
        }
        else if (angle + rotation > 360)
        {
            angle -= 360;
        }
        return myData[(int)Mathf.Floor(angle / 24)];
    }

}
