//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 1: Mobile Game            //
//                                      //
// Team Heron                           //
//                                      //
// December 2018                        //
//                                      //
// TOWERL Code                          //
// TouchController.cs                   //
//////////////////////////////////////////
using UnityEngine;

public class TouchController : MonoBehaviour {

    public Camera Camera;
    public Menu_Control menuTower;
    private MGC Controller;
    private CameraController2 c_Camera;

    private Touch userTouch;
    public Vector2 touchRotation, screenDimensions;
    public bool touchEnabled = false;

    void Start()
    {
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
        c_Camera = Camera.GetComponent<CameraController2>();

        screenDimensions = new Vector2(Screen.width, Screen.height);
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
                if (!Controller.isAnimating)
                {
                    // get movement since last frame, normalise, multiply by sensitivity from manager
                    Controller.TowerAngle -= (userTouch.deltaPosition.x / screenDimensions.x) * Controller.TouchControlSensetivity;
                    menuTower.Angle -= (userTouch.deltaPosition.x / screenDimensions.x) * Controller.TouchControlSensetivity;
                }

                if (c_Camera.enableCameraPan)
                {
                    Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y - ((userTouch.deltaPosition.y / screenDimensions.y) * (Controller.TouchControlSensetivity / 4)), Camera.transform.position.z);
                }
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
