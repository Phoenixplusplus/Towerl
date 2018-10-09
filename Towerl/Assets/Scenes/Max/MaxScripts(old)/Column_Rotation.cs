using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column_Rotation : MonoBehaviour {

    // Game Controller for persistent data
    MGC Controller;

    [Header("Control Variables")]
    private float RotationSpeed = 25.0f;

	// Use this for initialization
	void Start () {
        // Get Game Cpntroller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
    }
	
	// Update is called once per frame
	void Update () {

        //transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime);
        Controller.TowerAngle = transform.eulerAngles.y;

	}
}
