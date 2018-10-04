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
                // get movement since last frame
                Controller.TowerAngle -= userTouch.deltaPosition.x / 2;
            }
            // do other events based on touch below
        }
    }
}
