using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {

    [Header("Bounce control Variables")]
    public float Gravity = -10.0f;

    private float startHeight, segmentHeight, towerRotation;
    private int maxBounceHeight, level, maxLevel;
    private bool[] levelBools;
    private Vector3 Speed = new Vector3(0, 0, 0);
    private GameObject Tower;
    private TowerController TowerController;
    
    // Use this for initialization
    public void Init()
    {
        Tower = GameObject.Find("Tower");
        TowerController = Tower.GetComponent<TowerController>();

            // ball calculations
            maxBounceHeight = TowerController.segmentspace;
            startHeight = TowerController.towerHeight + maxBounceHeight;
            segmentHeight = TowerController.segmentHeight;

            // level calculations
            maxLevel = TowerController.levels;
            level = 0;
            levelBools = new bool[maxLevel];

            for (int i = level; i < maxLevel; i++)
            {
                if (i == 0) levelBools[i] = true;
                else levelBools[i] = false;
            }

        // start position
        transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // get tower rotation to determine if ball should boucne back
        towerRotation = TowerController.clone.transform.eulerAngles.y;

        if (levelBools[0] == true)
        {
            if (towerRotation < 120 || towerRotation > 180)
            {
                //Debug.Log("Yes");
                if (transform.position.y <= (startHeight - maxBounceHeight) + (segmentHeight * 2.0f))
                {
                    Speed.y *= -1;
                }
            }
        }

        // perfectly elastic ball based on maxBounceHeight and height of flood below it
        transform.position += Speed * Time.fixedDeltaTime;



        if (transform.position.y >= (startHeight - maxBounceHeight) + (segmentHeight * 2.0f))
        {
            Speed.y += Gravity * Time.fixedDeltaTime;
        }

    }
}
