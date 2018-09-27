using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour {

    [Header("Level Data")]
    private int levels = 5;
    public int[,] data = new int[5, 12]
    {
        {1,1,1,1,1,1,0,0,1,1,1,1 },
        {1,1,1,1,0,0,0,0,1,1,0,0 },
        {1,1,1,1,0,0,0,0,1,1,1,1 },
        {0,0,1,1,0,0,1,1,1,1,1,1 },
        {1,1,0,0,0,1,0,0,1,1,1,1 }
    };


    [Header("Segment Pre-fab")]
    public Transform segment;
    public Transform column;

    // Use this for initialization
    void Start()
    {
        // make column
        Transform clone = (Transform)Instantiate(column, new Vector3(0, (float)levels / 2, 0), Quaternion.identity);
        clone.transform.localScale = new Vector3(0.1f, 2.5f, 0.1f);

        // make each layer
        for (int level = 4; level >= 0; level--)
        {
            for (int i = 0; i < 12; i++)
            {
                if (data[level, i] == 1)
                {
                    //Transform clone = (Transform)Instantiate(segment, new Vector3(0, 1, 0), Quaternion.identity);
                    //clone.Rotate(Vector3.up * i * 30);
                    Transform segClone = (Transform)Instantiate(segment, new Vector3(0, level, 0), Quaternion.Euler(0, i * 30, 0));
                    segClone.gameObject.tag = level.ToString();
                    segClone.transform.parent = clone.transform;
                }
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
