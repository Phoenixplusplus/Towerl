using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakawayAndDie : MonoBehaviour {

    private Vector3 initialPosition, localDeathPosition, worldDeathPosition, newRotation;
    private float speed = 2f;
    private float currentTime = 0f;
    private float rotationRate;
    public bool die;
    public float timeout;

    // Use this for initialization
    void Start ()
    {
        initialPosition = transform.position;

        // set death position for each segment locally
        localDeathPosition = new Vector3(initialPosition.x, initialPosition.y - Random.Range(0.3f, 0.5f), initialPosition.z + Random.Range(0.7f, 1.0f));

        // translate it to world transform
        worldDeathPosition = transform.TransformDirection(localDeathPosition);

        rotationRate = Random.Range(-5.0f, 5.0f) * 0.1f;

        // detach from parent
        transform.parent = null;

        die = false;
        timeout = 2f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // on death
        if (die == true)
        {
            // rotate in world space
            newRotation = new Vector3(rotationRate, rotationRate, rotationRate);
            transform.Rotate(newRotation, Space.World);

            // set position in world space from local death position
            transform.position = Vector3.Lerp(transform.position, worldDeathPosition, (Time.deltaTime * speed));

            // set timer
            currentTime += Time.deltaTime;
        }

        // on timeout
        if (currentTime >= timeout) Destroy(transform.parent.gameObject);
    }
}
