using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {

    [Header("Bounce control Variables")]
    public float Gravity = -10.0f;

    private float startHeight, segmentHeight, towerRotation;
    private int halfBounceHeight, maxTier;
    private Vector3 Speed = new Vector3(0, 0, 0);
    private GameObject Tower;
    private TowerController TowerController;
    
    // Use this for initialization
    public void Init()
    {
        Tower = GameObject.Find("Tower");
        TowerController = Tower.GetComponent<TowerController>();

            // ball calculations
            halfBounceHeight = TowerController.segmentspace;
            startHeight = TowerController.towerHeight + halfBounceHeight;
            segmentHeight = TowerController.segmentHeight;

            // level calculations
            maxTier = TowerController.tiers;

        // start position
        transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
        Debug.Log((startHeight - halfBounceHeight) + (segmentHeight * 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Speed * Time.fixedDeltaTime;

        // get tower rotation to determine if ball should bounce back
        towerRotation = TowerController.clone.transform.eulerAngles.y;

        // apply gravity
        Speed.y += Gravity * Time.deltaTime;

        // check for bounce
        ApplyBounceForce();

        Debug.Log(CheckTier());
    }


    // Functions
    public void ApplyBounceForce()
    {
        if (CheckTier() == "Tier0")
        {
            if (transform.position.y <= (startHeight - halfBounceHeight) + (segmentHeight * 2.0f))
            {
                if (towerRotation < 120 || towerRotation > 180)
                {
                    Speed.y = halfBounceHeight * 2;
                }
            }
        }

        if (CheckTier() == "Tier1")
        {
            if (transform.position.y <= (startHeight - halfBounceHeight * 2) + (segmentHeight * 2.0f))
            {
                if (towerRotation > 60 && towerRotation < 120 || towerRotation > 240)
                {
                    Speed.y = halfBounceHeight * 2;
                }
            }
        }

        if (CheckTier() == "Tier2")
        {
            if (transform.position.y <= (startHeight - halfBounceHeight * 3) + (segmentHeight * 2.0f))
            {
                if (towerRotation < 120 || towerRotation > 240)
                {
                    Speed.y = halfBounceHeight * 2;
                }
            }
        }

        if (CheckTier() == "Tier3")
        {
            if (transform.position.y <= (startHeight - halfBounceHeight * 4) + (segmentHeight * 2.0f))
            {
                if (towerRotation < 180 || towerRotation > 240 && towerRotation < 300)
                {
                    Speed.y = halfBounceHeight * 2;
                }
            }
        }

        if (CheckTier() == "Tier4")
        {
            if (transform.position.y <= (startHeight - halfBounceHeight * 5) + (segmentHeight * 2.0f))
            {
                Speed.y = halfBounceHeight * 2;
            }
        }
    }

    public string CheckTier()
    {
        string localString = "null";

        if (transform.position.y >= (startHeight - halfBounceHeight) + segmentHeight) return localString = "Tier0";
        else if (transform.position.y >= (startHeight - halfBounceHeight * 2) + segmentHeight) return localString = "Tier1";
        else if (transform.position.y >= (startHeight - halfBounceHeight * 3) + segmentHeight) return localString = "Tier2";
        else if (transform.position.y >= (startHeight - halfBounceHeight * 4) + segmentHeight) return localString = "Tier3";
        else if (transform.position.y >= (startHeight - halfBounceHeight * 5) + segmentHeight) return localString = "Tier4";
        else return localString;
    }
}
