using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {

    [Header("Bounce control Variables")]
    public float Gravity = -10.0f;

    private float startHeight, segmentHalfHeight, towerRotation;
    private int maxBounceHeight, segment;
    private int[,] levelData;
    private Vector3 Speed = new Vector3(0, 0, 0);
    private TowerController TowerController;
    private GameObject Tower;
    
    // Use this for initialization
    public void Init()
    {
        Tower = GameObject.Find("Tower");
        TowerController = Tower.GetComponent<TowerController>();

            //
            maxBounceHeight = TowerController.segmentspace;
            startHeight = TowerController.towerHeight + maxBounceHeight;
            segmentHalfHeight = TowerController.segmentHalfHeight;
            levelData = TowerController.data;

        // start position
        transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
        Debug.Log((startHeight - maxBounceHeight) + (segmentHalfHeight * 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        // calculate current tier // note: it's important it is done before other calculations to have a chance to have a different result than newTier
        int currTier = (int)Mathf.Floor((transform.position.y - segmentHalfHeight * 2) / maxBounceHeight);

        // apply gravity
        Speed.y += Gravity * Time.deltaTime;
        transform.position += Speed * Time.fixedDeltaTime;

        // get tower rotation to determine if ball should bounce back
        towerRotation = TowerController.clone.transform.eulerAngles.y;

        // check for bounce
        ApplyBounceForce(currTier);

        // check for segment based on rotation of tower
        //segment = (int)Mathf.Floor(towerRotation / 30);

        //Debug.Log((int)Mathf.Floor((transform.position.y - segmentHalfHeight * 2) / maxBounceHeight));

        int newTier = (int)Mathf.Floor((transform.position.y - segmentHalfHeight * 2) / maxBounceHeight);
        if (newTier != currTier && Speed.y < 0)
        {
            Debug.Log("Tier now " + currTier);
        }
            //int segment = (int)Mathf.Floor(towerRotation / 30);
            //Debug.Log("data reads[ " + currTier.ToString() + " , " + segment.ToString() + "] = " + levelData[levelData.GetLength(0) - currTier, segment].ToString());

        //    if (levelData[levelData.GetLength(0) - currTier, segment] != 0 || currTier == 0)
        //    {
        //        Speed.y = maxBounceHeight * 2;
        //    }
        //}

        Debug.Log(levelData.GetLength(0) + " " + levelData.GetLength(1));
    }


    // On-tick functions
    public void ApplyBounceForce(int tier)
    {
        switch (tier)
        {
            case 5:
                if (transform.position.y <= (startHeight - maxBounceHeight) + (segmentHalfHeight * 2.0f))
                {
                    if (towerRotation < 120 || towerRotation > 180)
                    {
                        Speed.y = maxBounceHeight * 2;
                    }
                }
                break;
            case 4:
                if (transform.position.y <= (startHeight - maxBounceHeight * 2) + (segmentHalfHeight * 2.0f))
                {
                    if (towerRotation > 60 && towerRotation < 120 || towerRotation > 240)
                    {
                        Speed.y = maxBounceHeight * 2;
                    }
                }
                break;
            case 3:
                if (transform.position.y <= (startHeight - maxBounceHeight * 3) + (segmentHalfHeight * 2.0f))
                {
                    if (towerRotation < 120 || towerRotation > 240)
                    {
                        Speed.y = maxBounceHeight * 2;
                    }
                }
                break;
            case 2:
                if (transform.position.y <= (startHeight - maxBounceHeight * 4) + (segmentHalfHeight * 2.0f))
                {
                    if (towerRotation < 180 || towerRotation > 240 && towerRotation < 300)
                    {
                        Speed.y = maxBounceHeight * 2;
                    }
                }
                break;
            case 1:
                {
                    if (transform.position.y <= (startHeight - maxBounceHeight * 5) + (segmentHalfHeight * 2.0f))
                    {
                        Speed.y = maxBounceHeight * 2;
                    }
                }
                break;
        }
    }

    public string CheckTier()
    {
        string localString = "null";

        if (transform.position.y >= (startHeight - maxBounceHeight) + segmentHalfHeight) return localString = "Tier0";
        else if (transform.position.y >= (startHeight - maxBounceHeight * 2) + segmentHalfHeight) return localString = "Tier1";
        else if (transform.position.y >= (startHeight - maxBounceHeight * 3) + segmentHalfHeight) return localString = "Tier2";
        else if (transform.position.y >= (startHeight - maxBounceHeight * 4) + segmentHalfHeight) return localString = "Tier3";
        else if (transform.position.y >= (startHeight - maxBounceHeight * 5) + segmentHalfHeight) return localString = "Tier4";
        else return localString;
    }
}
