using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

    [Header("Level Data")]
    public int tiers = 5;
    public int[,] data = new int[5, 12]
    {
        // 0 = a gap (nothing there)
        // 1 = A Slice (30 degrees)
        // each element is refered to the degrees that is the end of the object:
        // 360,330,300,270,240,210,180,150,120,90,60,30
        {1,1,1,1,1,1,0,0,1,1,1,1 },
        {1,1,1,1,0,0,0,0,1,1,0,0 },
        {1,1,1,1,0,0,0,0,1,1,1,1 },
        {0,0,1,1,0,0,1,1,1,1,1,1 },
        {1,1,1,1,1,1,1,1,1,1,1,1 }
    };


    [Header("Segment Pre-fab")]
    public Transform segment;
    public Transform column;
    public Transform clone;

    [Header("Level Attributes")]
    public int segmentspace;

        public float segmentHeight, towerHeight;

    // Use this for initialization
    void Start()
    {
        // make column
        clone = (Transform)Instantiate(column, new Vector3(0, 0, 0), Quaternion.identity);
        clone.transform.localScale = new Vector3(1.0f, tiers * segmentspace / 2.0f, 1.0f);
        towerHeight = clone.GetComponent<MeshRenderer>().bounds.size.y;
        clone.transform.position += new Vector3(0, (towerHeight / 2.0f) + segmentspace, 0);

        // make each layer
        for (int level = 4; level >= 0; level--)
        {
            for (int i = 0; i < 12; i++)
            {
                if (data[level, i] == 1)
                {
                    Transform segClone = (Transform)Instantiate(segment, new Vector3(0, towerHeight - level * segmentspace, 0), Quaternion.Euler(0, i * 30, 0));
                    segClone.transform.localScale = new Vector3(300.0f, 10.0f, 300.0f);
                    segmentHeight = segClone.GetComponent<MeshRenderer>().bounds.size.y;
                    segClone.gameObject.tag = level.ToString();
                    segClone.transform.parent = clone.transform;
                    Debug.Log(segmentHeight);
                }
            }
        }

        BallPhysics BallPhysics = GameObject.Find("Ball").GetComponent<BallPhysics>();
        BallPhysics.Init();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
