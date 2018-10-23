using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour {

    private MGC Controller;

    [Header("CameraShake")]
    public float shakePower = 0.2f;
    public float shakeAbsorb = 1f;
    public float Duration = 1f;
    public bool enableShake = false;
    Vector3 startPosition;


    // Use this for initialization
    void Start () {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
        ResetCameraToTop();
    }
	
	// Update is called once per frame
	void Update () {
        // Camera only pans with the Ball when controller flag bool "BallFalling" is true
		if (Controller.BallFalling)
        {
            transform.position = new Vector3(transform.position.x, Controller.BallHeight + 1, transform.position.z);
        }

        // camera shake condition
        if (enableShake == true)
        {
            if (Duration > 0)
            {
                transform.localPosition = startPosition + Random.insideUnitSphere * shakePower;
                Duration -= Time.deltaTime * shakeAbsorb;
            }
            else
            {
                enableShake = false;
                Duration = 0;
                transform.localPosition = startPosition;
            }
        }
	}

    // Called on Start (after # Tiers in game has been declared)
    // Subsequently available for the Game Controller (usually upon Ball Reset to top))
    public void ResetCameraToTop () 
    {
        transform.position = new Vector3(transform.position.x, Controller.TiersPerLevel, transform.position.z);
    }

    // Called by the Controller when the ball stops falling ... to lock the camera @ the correct level
    public void SetToHeight(int Height)
    {
        transform.position = new Vector3(transform.position.x, Height, transform.position.z);
    }

    // camera shake
    public void EnableCameraShake(float l_shakePower, float l_shakeAbsorb, float l_Duration)
    {
        startPosition = transform.localPosition;
        enableShake = true;
        shakePower = l_shakePower;
        shakeAbsorb = l_shakeAbsorb;
        Duration = l_Duration;
    }
}
