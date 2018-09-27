using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Camera Pre-fab")]
    public Transform maincamera;

    [Header("Ball To Focus")]
    public Transform ball;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        maincamera.position = new Vector3(ball.position.x + ball.transform.localScale.x * 10, ball.position.y + ball.transform.localScale.y * 2, ball.position.z);
        maincamera.LookAt(new Vector3(ball.position.x, ball.position.y - ball.transform.localScale.y * 3, ball.position.z));
	}
}
