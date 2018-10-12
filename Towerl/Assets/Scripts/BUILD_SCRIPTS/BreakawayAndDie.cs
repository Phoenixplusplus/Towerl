using UnityEngine;

public class BreakawayAndDie : MonoBehaviour {

    private GameObject C_Controller;

    private Vector3 initialPosition, localDeathPosition, worldDeathPosition, newRotation;
    private float lerpSpeed = 2f;
    private float currentTime = 0f;
    private float rotationRate;
    private float timeout = 2f;
    private bool die = false;

    // Use this for initialization
    void Start()
    {
        // Get Game Controller reference
        C_Controller = GameObject.Find("Column");
    }

    // Update is called once per frame
    void Update()
    {
        // on death
        if (die == true)
        {
            // rotate in world space
            newRotation = new Vector3(rotationRate, rotationRate, rotationRate);
            transform.Rotate(newRotation, Space.World);


            // rotate the world death position based on tower rotation
            //worldDeathPosition = RotatePointAroundPivot(localDeathPosition, initialPosition, C_Controller.transform.eulerAngles);


            // set position in world space from local death position
            transform.position = Vector3.Lerp(transform.position, worldDeathPosition, (Time.deltaTime * lerpSpeed));

            // set timer
            currentTime += Time.deltaTime;
        }

        // on timeout
        if (currentTime >= timeout) Destroy(gameObject);
    }

    // to kill segment
    public void KillSegment(float segLerpSpeed, float segTimeout)
    {
        initialPosition = transform.position;

        // set death position for each segment locally
        localDeathPosition = new Vector3(initialPosition.x, initialPosition.y - Random.Range(0.3f, 0.5f), initialPosition.z + Random.Range(0.7f, 1.0f));

        // translate it to world transform
        worldDeathPosition = transform.TransformDirection(localDeathPosition);

        rotationRate = Random.Range(-5.0f, 5.0f) * 0.1f;

        // detach from parent
        transform.parent = null;

        timeout = segTimeout;
        lerpSpeed = segLerpSpeed;
        die = true;
    }

    // to factor in the rotation of tower rotation
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
