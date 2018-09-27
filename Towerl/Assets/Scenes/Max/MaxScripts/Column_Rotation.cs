using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column_Rotation : MonoBehaviour {

    // Game Controller for persistent data
    MaxGameController MGC;

    [Header("Control Variables")]
    private float RotationSpeed = 25.0f;

	// Use this for initialization
	void Start () {
        // Get Game Cpntroller reference
        MGC = GameObject.Find("MaxGameController").GetComponent<MaxGameController>();
    }
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
        MGC.TowerAngle = transform.eulerAngles.y;

	}
}
