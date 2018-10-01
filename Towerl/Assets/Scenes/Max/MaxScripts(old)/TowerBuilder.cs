using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour {


    // Game Controller for persistent data
    MaxGameController MGC;

    [Header("Segment Pre-fab")]
    public Transform segment;
    public Transform column;
    public Transform ball;

    // Use this for initialization
    void Start()
    {
        // Get Game Cpntroller reference
        MGC = GameObject.Find("MaxGameController").GetComponent<MaxGameController>();

        // make column (and Apply MGC scale factors)
        Transform clone = (Transform)Instantiate(column, new Vector3(0f,(float)MGC.levels * MGC.TierHeight / 2 ,0), Quaternion.identity);
        clone.transform.localScale = Vector3.Scale(clone.transform.localScale, MGC.ColumnScale);

        // make each layer (Apply MCG Scale factors and append to column)
        for (int level = 0; level < MGC.levels; level++)
        {
            for (int i = 0; i < 12; i++)
            {
                if (MGC.data[level, i] == 1) // default segment
                {
                    //Transform clone = (Transform)Instantiate(segment, new Vector3(0, 1, 0), Quaternion.identity);
                    //clone.Rotate(Vector3.up * i * 30);
                    Transform segClone = (Transform)Instantiate(segment, new Vector3(0, level * MGC.TierHeight, 0), Quaternion.Euler(0, (i * 30) + MGC.SegmentBaseRotation, 0));
                    segClone.gameObject.tag = level.ToString();
                    segClone.transform.localScale = Vector3.Scale(segClone.transform.localScale , MGC.SegmentScale);
                    segClone.transform.parent = clone.transform;
                }
            }
        }

        // create the ball
        Transform NewBall = (Transform)Instantiate(ball, new Vector3(MGC.BallOffset, (MGC.levels + MGC.BallStartHeightRatio - 1f) * MGC.TierHeight), Quaternion.identity);
        NewBall.transform.localScale = Vector3.Scale(NewBall.transform.localScale, MGC.BallScale);

        // Relay info to Game Controller
        MGC.BallHeight = transform.position.x;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
