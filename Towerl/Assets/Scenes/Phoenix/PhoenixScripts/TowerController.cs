using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

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

    [Header("Level Attributes")]
    public int segmentspace;

        public Transform clone;
        public float segHeight;

    // Use this for initialization
    void Start()
    {
        // make column
        clone = (Transform)Instantiate(column, new Vector3(0, (float)levels / 2, 0), Quaternion.identity);
        clone.transform.localScale = new Vector3(1.0f, 10.0f, 1.0f);

        // make each layer
        for (int level = 4; level >= 0; level--)
        {
            for (int i = 0; i < 12; i++)
            {
                if (data[level, i] == 1)
                {
                    Transform segClone = (Transform)Instantiate(segment, new Vector3(0, clone.transform.localScale.y - level * segmentspace, 0), Quaternion.Euler(0, i * 30, 0));
                    segClone.transform.localScale = new Vector3(300.0f, 10.0f, 300.0f);
                    segHeight = segClone.GetComponent<MeshRenderer>().bounds.size.y;
                    segClone.gameObject.tag = level.ToString();
                    segClone.transform.parent = clone.transform;
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
