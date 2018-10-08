using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierScript : MonoBehaviour {

    // Master Script for a tier object ... the FIRST segement has this script only

    private MGC Controller;

    public int[] myData = new int[32];
    public int rotation = 0;

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
        transform.localEulerAngles = new Vector3(0, Controller.TowerAngle + (float)rotation, 0);
	}

    public int ReportType(float angle)
    {
        float R = angle + (float)rotation;
        if (R < 0)
        {
            while (R < 0) { R += 360f; }
        }
        else if (R > 360)
        {
            while (R > 360) { R -= 360f; }
        }
        int segmentNumber = (int)Mathf.Floor(R / 15);
        segmentNumber = 23 - segmentNumber;
        Debug.Log("Returning data for segment " + segmentNumber.ToString()+ " Tier Angle = "+ rotation.ToString() + " Adjusted angle = " + R.ToString() );
        return myData[segmentNumber];
    }

}
