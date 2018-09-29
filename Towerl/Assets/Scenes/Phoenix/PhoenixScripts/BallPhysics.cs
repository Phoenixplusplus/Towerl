using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {

    [Header("Bounce control Variables")]
    public float Gravity = -10.0f;

    private float startHeight, segmentHeight, towerRotation;
    private int halfBounceHeight, maxTier;
    private int[,] levelData, newLevelData;
    private Vector3 Speed = new Vector3(0, 0, 0);
    private TowerController TowerController;
    
    // Use this for initialization
    public void Init()
    {
        TowerController = GameObject.Find("Tower").GetComponent<TowerController>();

            // ball calculations
            halfBounceHeight = TowerController.segmentspace;
            startHeight = TowerController.towerHeight + halfBounceHeight;
            segmentHeight = TowerController.segmentHeight;

            // level calculations
            maxTier = TowerController.tiers;
            levelData = TowerController.data;
            newLevelData = new int [levelData.GetLength(0), levelData.GetLength(1)];
            readLevelData();

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


    // On-tick functions
    public void ApplyBounceForce()
    {
        if (CheckTier() == "Tier0")
        {
            if (transform.position.y <= (startHeight - halfBounceHeight) + (segmentHeight * 2.0f))
            {
                //for (int i = 0; i < newLevelData.GetLength(1); i++)
                //{
                //    if (towerRotation < newLevelData[0, i] && towerRotation > newLevelData[0, i] + 30)
                //    {
                //        Speed.y = halfBounceHeight * 2;
                //    }
                //}


                if (towerRotation < 120 || towerRotation > 180)
                {
                    Speed.y = halfBounceHeight * 2;
                }
            }
            return;
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
            return;
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
            return;
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
            return;
        }

        if (CheckTier() == "Tier4")
        {
            if (transform.position.y <= (startHeight - halfBounceHeight * 5) + (segmentHeight * 2.0f))
            {
                Speed.y = halfBounceHeight * 2;
            }
            return;
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

    // On-start functions
    public void readLevelData()
    {
        for (int tier = 0; tier < levelData.GetLength(0); tier++)
        {
            for (int segment = 0; segment < levelData.GetLength(1); segment++)
            {
                // refering to data structure:
                // 0 = a gap (nothing there)
                // 1 = A Slice (30 degrees)
                // each element is refered to the degrees that is the end of the object:
                // 360,330,300,270,240,210,180,150,120,90,60,30
                if (levelData[tier, segment] == 0)
                {
                    int angle = 360 - (segment * (360 / levelData.GetLength(1)));

                    storeLevelData(tier, segment, angle);
                }
            }
        }
    }

    public void storeLevelData(int tier, int segment, int angle)
    {
        newLevelData[tier, segment] = angle;
    }
}
