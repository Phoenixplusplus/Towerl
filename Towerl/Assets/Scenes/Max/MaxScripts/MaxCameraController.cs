using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxCameraController : MonoBehaviour {

    // Game Controller for persistent data
    MaxGameController MGC;

    public int CurrentTier;


    // Use this for initialization
    void Start () {
        // Get Game Cpntroller reference
        MGC = GameObject.Find("MaxGameController").GetComponent<MaxGameController>();

        // Set camera to top
        transform.position = new Vector3(transform.position.x, MGC.levels, transform.position.z);

        CurrentTier = MGC.CurrentTier;
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentTier != MGC.CurrentTier)
        {
            transform.Translate(MGC.CurrentBallVelocity * Time.deltaTime);
            if (transform.position.y - MGC.BallRadius <= MGC.CurrentTier)
            {
                Debug.Log("Gor one");
                transform.position = new Vector3(transform.position.x, MGC.CurrentTier, transform.position.z);
                CurrentTier = MGC.CurrentTier;
            }
        }
	}
}
