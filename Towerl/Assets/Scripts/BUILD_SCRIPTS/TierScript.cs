using UnityEngine;

public class TierScript : MonoBehaviour {

    // Master Script for a tier object ... the FIRST segement has this script only

    private MGC Controller;

    public int[] myData = new int[32];
    public float rotation = 0;

	// Use this for initialization
	void Start () {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
    }
	
	// Update is called once per frame
	void Update () {
        // This scaling bit is purely for development experimentation
        // can be commented out for "Live Build"
        if (transform.localScale != Controller.SegmentScale)
        {
            transform.localScale = Controller.SegmentScale;
        }
        transform.localEulerAngles = new Vector3(0, Controller.TowerAngle + rotation, 0);
	}

    public int ReportType(float angle)
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
        int segmentNumber = 23 - (int)Mathf.Floor(R / 15); // Don't ask .. it's a winding, windy thing ....
        // SAVED FOR FUTURE DEV WORK ...logs Tier, segment, angle and what is returned
        // Debug.Log("Returning data for segment " + segmentNumber.ToString()+ " Tier Angle = "+ rotation.ToString() + " Adjusted angle = " + R.ToString() );

        return myData[segmentNumber];
    }

}
