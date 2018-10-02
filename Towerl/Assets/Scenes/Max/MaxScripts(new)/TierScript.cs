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
        transform.localEulerAngles = new Vector3(0, Controller.TowerAngle + rotation, 0);
	}

    int ReportType(float angle)
    {
        float R = angle + rotation;
        if (R < 0)
        {
            while (R < 0) { R += 360f; }
        }
        else if (R > 360)
        {
            while (R > 360) { R -= 360f; }
        }
        return myData[(int)Mathf.Floor(R / 24)];
    }

}
