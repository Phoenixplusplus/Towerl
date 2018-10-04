using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

    Touch userTouch;
    Vector2 userRotation;
    int userSensitivity = 1;

	// Update is called once per frame
	void Update ()
    {
        userTouch = Input.GetTouch(0);
        userRotation = userTouch.deltaPosition * userTouch.deltaTime * userSensitivity;
	}
}
