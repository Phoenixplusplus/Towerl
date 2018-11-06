using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour {

    private MGC Controller;
    public GameObject adventureBackdrop;

    [Header("CameraShake")]
    public float shakePower = 0.2f;
    public float shakeAbsorb = 1f;
    public float Duration = 1f;
    public bool enableShake = false;
    public bool enableCameraPan = false;
    Vector3 startPosition;
    Vector3 initialPosition;


    // Use this for initialization
    void Start () {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
        ResetCameraToTop();
        initialPosition = transform.position;
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

        // for adventure menu mode
        if (enableCameraPan)
        {
            if (Input.GetKey("up"))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 5f, transform.position.z);
            }

            if (Input.GetKey("down"))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * 5f, transform.position.z);
            }
        }
	}

    // Called on Start (after # Tiers in game has been declared)
    // Subsequently available for the Game Controller (usually upon Ball Reset to top))
    public void ResetCameraToTop () 
    {
        transform.position = new Vector3(transform.position.x, Controller.TiersPerLevel, transform.position.z);

        // and reset rotation
        transform.eulerAngles = new Vector3(31.247f, 180f, 0f);
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

    // adventure map stuff
    public void EnableAdventureMap(bool localBool)
    {
        if (localBool)
        {
            transform.position = initialPosition;
            transform.eulerAngles = new Vector3(-30f, 180f, 0f);
            transform.GetChild(0).gameObject.SetActive(false);
            adventureBackdrop.SetActive(true);
            enableCameraPan = true;
        }
        else
        {
            transform.position = initialPosition;
            transform.GetChild(0).gameObject.SetActive(true);
            adventureBackdrop.SetActive(false);
            enableCameraPan = false;
        }
    }
}
