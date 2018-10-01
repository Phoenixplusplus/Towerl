using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    // Game Controller for persistent data
    MaxGameController MGC;
    private Vector3 vel;
    private Vector3 maxVel;
    private bool gameOver = false;


    // Use this for initialization
    void Start () {

   // Get Game Cpntroller reference
   MGC = GameObject.Find("MaxGameController").GetComponent<MaxGameController>();

        vel = new Vector3(0, 0, 0);
        //establish the top velocity using Newtonian physics
        // Velocity after falling height D with Acceleration G = Root(2 * D * G)
       // maxVel = new Vector3(0, Mathf.Sqrt(2 * ((MGC.BallStartHeightRatio * MGC.TierHeight) - MGC.BallRadius) * MGC.Gravity), 0);
       // BUGGER IT >>> Not working ... will come back to it .....

    }
	
	// Update is called once per frame
	void Update () {
        // check current Tier
        int currTier = (int)Mathf.Floor((transform.position.y - MGC.BallRadius) / MGC.TierHeight);
        // modify Velocty and apply it
        vel += Vector3.up * MGC.Gravity * Time.deltaTime;
        transform.position += vel * Time.deltaTime;

        // have be crossed a tier (whilst going down)?
        int newTier = (int)Mathf.Floor((transform.position.y - MGC.BallRadius) / MGC.TierHeight);
        if (newTier != currTier && vel.y < 0 )
        {
            int segment = (int)Mathf.Floor(MGC.TowerAngle / 30);
            // Bounce if he have to .....
            Debug.Log("data reads[ " + currTier.ToString() + " , " + segment.ToString() + "] = " + MGC.data[currTier, segment].ToString() + " TAngle: " + MGC.TowerAngle.ToString());

    
            if (MGC.data[currTier, segment] != 0 || currTier == 0)
            {
                // here we have a tier barrier hit
                vel.y = MGC.BallMaxVelocity;
            }
        }




	}
}
