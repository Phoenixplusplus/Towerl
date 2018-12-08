using UnityEngine;
using UnityEditor;

public class BreakawayAndDie : MonoBehaviour {

    private GameObject C_Controller;
    private MGC theMGC;
    public Material safeTransparentMaterial;
    public Material hazardTransparentMaterial;
    private Material childMaterial;
    private Material[] childMaterials;

    private Vector3 initialPosition, localDeathPosition, worldDeathPosition, newRotation;
    private float lerpSpeed = 2f;
    private float currentTime = 0f;
    private float rotationRate;
    private float timeout = 2f;
    private bool die = false;
    public bool isInvisible = false;

    // Use this for initialization
    void Start()
    {
        // Get Game Controller reference
        C_Controller = GameObject.Find("Column");
        theMGC = GameObject.Find("MGC").GetComponent<MGC>();
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

            // set position in world space from local death position
            transform.position = Vector3.Lerp(transform.position, worldDeathPosition, (Time.deltaTime * lerpSpeed));

            // set timer
            currentTime += Time.deltaTime;

            // change alpha of 'child material' ie. the segment material
            if (childMaterial != null)
            {
                Color finalColour = childMaterial.color;
                finalColour.a = timeout - currentTime;
                childMaterial.color = finalColour;
            }

            // change every material on the same object
            if (childMaterials != null)
            {
                for (int i = 0; i < childMaterials.Length; i++)
                {
                    Color finalColour = childMaterials[i].color;
                    finalColour.a = timeout - currentTime;
                    childMaterials[i].color = finalColour;
                }
            }
        }

        // on timeout
        if (currentTime >= timeout) Destroy(gameObject);
    }

    // to kill segment
    public void KillSegment(float segLerpSpeed, float segTimeout)
    {
        if (!isInvisible)
        {
            initialPosition = transform.position;

            // set death position for each segment locally
            localDeathPosition = new Vector3(initialPosition.x, initialPosition.y - Random.Range(0.3f, 0.5f), initialPosition.z + Random.Range(0.7f, 1.0f));

            // translate it to world transform
            worldDeathPosition = transform.TransformDirection(localDeathPosition);

            rotationRate = Random.Range(-5.0f, 5.0f) * 0.1f;

            // detach from parent
            transform.parent = null;

            // set transparent material
            if (safeTransparentMaterial != null)
            {
                safeTransparentMaterial.color = gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = safeTransparentMaterial;
            }
            if (hazardTransparentMaterial != null)
            {
                if (gameObject.transform.childCount > 0)
                {
                    Component[] childrenRenderers = gameObject.transform.GetComponentsInChildren<Renderer>();

                    foreach (Renderer r in childrenRenderers)
                    {
                        hazardTransparentMaterial.color = r.material.color;
                        r.material = hazardTransparentMaterial;
                    }


                    //hazardTransparentMaterial.color = gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
                    //gameObject.transform.GetChild(0).GetComponent<Renderer>().material = hazardTransparentMaterial;
                }
                else // some custom meshes do not have children..
                {
                    hazardTransparentMaterial.color = gameObject.GetComponent<Renderer>().material.color;
                    gameObject.GetComponent<Renderer>().material = hazardTransparentMaterial;

                    // they also may have more than 1 texture on them..
                    Material[] newMats = new Material[gameObject.GetComponent<Renderer>().materials.Length];
                    childMaterials = new Material[gameObject.GetComponent<Renderer>().materials.Length];
                    if (gameObject.GetComponent<Renderer>().materials.Length > 0)
                    {
                        for (int i = 0; i < gameObject.GetComponent<Renderer>().materials.Length; i++)
                        {
                            newMats[i] = hazardTransparentMaterial;
                            childMaterials[i] = hazardTransparentMaterial;
                        }
                    }

                    // also cannot change the materials in the array directly, must make a new array and overwrite..
                    gameObject.GetComponent<Renderer>().materials = newMats;
                }
            }

            // grab this so that we can tint alpha over time after this function is called ~(saves us finding components per update)
            // some hazard segments do not have children, checking is a MUST
            if (gameObject.transform.childCount > 0) childMaterial = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
            else childMaterial = gameObject.GetComponent<Renderer>().material;

            if (theMGC.SkinType == 2)
            {
                Component[] childrenRenderers2 = gameObject.transform.GetComponentsInChildren<Renderer>();

                foreach (Renderer r in childrenRenderers2)
                {
                    r.material = childMaterial;
                }
            }

            timeout = segTimeout;
            lerpSpeed = segLerpSpeed;
            die = true;
        }
        else Destroy(gameObject);
    }

    // to breakthrough segment
    public void BreakthroughSegment(float segLerpSpeed, float segTimeout)
    {


        // find the ball and get it's velocity, so we can shoot segments off according to this value, bit more reaslistic
        // also find the colour of the Powerball at the moment, to put its colour onto the segments
        float BallVeloc = GameObject.Find("MGC").GetComponent<MGC>().CurrentBallVelocity.y;
        Color BallColour = GameObject.Find("MGC").GetComponent<MGC>().Ball.transform.GetChild(0).GetComponent<Renderer>().material.color;

        initialPosition = transform.position;

        // set death position for each segment locally

        localDeathPosition = new Vector3(initialPosition.x, initialPosition.y + (BallVeloc * 0.5f), initialPosition.z + Random.Range(0.7f, 1.0f));

        // translate it to world transform
        worldDeathPosition = transform.TransformDirection(localDeathPosition);

        rotationRate = Random.Range(-5.0f, 5.0f) * 0.1f;

        // detach from parent
        transform.parent = null;

        // set transparent material
        if (safeTransparentMaterial != null)
        {
            //safeTransparentMaterial.color = gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
            gameObject.transform.GetChild(0).GetComponent<Renderer>().material = safeTransparentMaterial;
            gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = BallColour;
        }
        if (hazardTransparentMaterial != null)
        {
            if (gameObject.transform.childCount > 0)
            {
                Component[] childrenRenderers = gameObject.transform.GetComponentsInChildren<Renderer>();

                foreach (Renderer r in childrenRenderers)
                {
                    r.material = hazardTransparentMaterial;
                    r.material.color = BallColour;
                }
                //hazardTransparentMaterial.color = gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
                //gameObject.transform.GetChild(0).GetComponent<Renderer>().material = hazardTransparentMaterial;
                //gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = BallColour;
            }
            else // some custom meshes do not have children..
            {
                gameObject.GetComponent<Renderer>().material = hazardTransparentMaterial;

                // they also may have more than 1 texture on them..
                Material[] newMats = new Material[gameObject.GetComponent<Renderer>().materials.Length];
                childMaterials = new Material[gameObject.GetComponent<Renderer>().materials.Length];
                if (gameObject.GetComponent<Renderer>().materials.Length > 0)
                {
                    for (int i = 0; i < gameObject.GetComponent<Renderer>().materials.Length; i++)
                    {
                        childMaterials[i] = hazardTransparentMaterial;
                        newMats[i] = hazardTransparentMaterial;
                        newMats[i].color = BallColour;
                    }
                }

                // also cannot change the materials in the array directly, must make a new array and overwrite..
                gameObject.GetComponent<Renderer>().materials = newMats;
            }
        }
        // grab this so that we can tint alpha over time after this function is called ~(saves us finding components per update)
        // some hazard segments do not have children, checking is a MUST
        if (gameObject.transform.childCount > 0) childMaterial = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;

        if (theMGC.SkinType == 2)
        {
            Component[] childrenRenderers2 = gameObject.transform.GetComponentsInChildren<Renderer>();

            foreach (Renderer r in childrenRenderers2)
            {
                r.material = childMaterial;
            }
        }

        timeout = segTimeout;
        lerpSpeed = segLerpSpeed;
        die = true;
    }

    // to factor in the rotation of tower rotation
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) { return Quaternion.Euler(angles) * (point - pivot) + pivot; }
}
