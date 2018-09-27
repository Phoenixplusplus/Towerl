using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {

    [Header("Bounce control Variables")]
    public float Gravity = -10.0f;

    private float StartHeight, segHeight;
    private int MaxBounceHeight;
    private Vector3 Speed = new Vector3(0, 0, 0);
    private GameObject Tower;

    // Use this for initialization
    void Start()
    {
        GameObject Tower = GameObject.Find("Tower");
        TowerController TowerController = Tower.GetComponent<TowerController>();

        MaxBounceHeight = TowerController.segmentspace;
        //StartHeight = TowerController.clone.GetComponent<MeshRenderer>().bounds.size.y;
        StartHeight = TowerController.clone.transform.localScale.y + MaxBounceHeight;
        segHeight = TowerController.segHeight;

        transform.position = new Vector3(transform.position.x, StartHeight, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Speed.y += Gravity * Time.fixedDeltaTime;
        transform.position += Speed * Time.fixedDeltaTime;

        if (transform.position.y <= StartHeight - MaxBounceHeight + segHeight && Speed.y < 0)
        {
            Speed.y *= -1;
        }

    }
}
