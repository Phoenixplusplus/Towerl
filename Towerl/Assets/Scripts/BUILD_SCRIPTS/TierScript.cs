using UnityEngine;

public class TierScript : MonoBehaviour {

    // Master Script for a tier object ... the FIRST segement has this script only

    private MGC Controller;

    public int[] myData = new int[32];
    public float rotation = 0;
    [SerializeField]
    private float DeltaRot = 0f;
    private bool Populated = false;
    [SerializeField]
    private float[] RotOdds = { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, -1f, 1f };

	// Use this for initialization
	void Awake () {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
        // Casual mode
        if(Controller.CurrentDifficulty != 0 && tag !="0" && tag != "34")
        {
            float mod = Mathf.Clamp((float) Controller.CasualLevel / (float) Controller.LevelSpanForZeroTo100Percent,0f,2.5f);
            // Debug.Log("Tier " + tag + " mod = " + mod.ToString());
            int Magic = Random.Range(0, 10);
            DeltaRot = 10f * RotOdds[Magic] * mod;
        }
        // story mode
        else if (tag =="33" || tag == "28" || tag == "23" || tag == "18" || tag == "13" || tag =="8" || tag == "3")
        {
            float mod = Mathf.FloorToInt(Controller.CurrentLevel / 10) + 1;
            DeltaRot = 10f * mod;
        }
        else if (tag == "31" || tag == "27" || tag == "26" || tag == "17" || tag == "9" || tag == "4" || tag == "2")
        {
            float mod = Mathf.FloorToInt(Controller.CurrentLevel / 10) + 1;
            DeltaRot = -10f * mod;
        }


    }
	
	// Update is called once per frame
	void Update () {

        if(DeltaRot != 0 && Controller.GameRunning)
        {
            rotation += DeltaRot * Time.deltaTime;
        }

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
