using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

    private MGC Controller;

    private Touch userTouch;
    public Vector2 touchRotation;
    public bool touchEnabled = false;

    void Start()
    {
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
    }

	// Update is called once per frame
	void Update ()
    {
        // if there's a touch detected
        if (Input.touchCount > 0)
        {
            touchEnabled = true;
            userTouch = Input.GetTouch(0);
            if (userTouch.phase == TouchPhase.Moved)
            {
                // get movement since last frame, apply to MGC
                Controller.TowerAngle -= userTouch.deltaPosition.x / 2;
            }
            // do other events based on touch below

            //// Handle finger movements based on TouchPhase
            //switch (touch.phase)
            //{
            //    //When a touch has first been detected, change the message and record the starting position
            //    case TouchPhase.Began:
            //        // Record initial touch position.
            //        startPos = touch.position;
            //        message = "Begun ";
            //        break;

            //    //Determine if the touch is a moving touch
            //    case TouchPhase.Moved:
            //        // Determine direction by comparing the current touch position with the initial one
            //        direction = touch.position - startPos;
            //        message = "Moving ";
            //        break;

            //    case TouchPhase.Ended:
            //        // Report that the touch has ended when it ends
            //        message = "Ending ";
            //        break;
            //}
        }
    }
}
