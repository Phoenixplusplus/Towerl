using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    /** Slow motion variable use to multiply the ball velocity and the tower rotations, keeps values 1.0 or 0.5
     * If the value is 1.0, the slow motion is not activated
     * If the value is 0.5, the slow motion is activated */
    private float slowMotion;

    /** Store the time value, which determine for how long the slow motion will be activated */
    private const int MAXIMUM_SLOW_MOTION_COUNT_DOWN_VALUE = 3;

    /** Store the current time value of how long is slow motion activated
     * If the value is equal or lower than 0.0f, counter will call DeactivateSlowMotion function */
    private int currSlowMotionCountDown;


    // TODO: 
    // 1. CHANGE THE "slowMotion" VARIABLE TO PRIVATE! OR ? KEEP IT PUBLIC? DISCUSS THIS WITH TEAMMATES
    // 2. CHANGE THE "currSlowMotionCountDown" VARIABLE TO PRIVATE! OR ? KEEP IT PUBLIC? DISCUS THIS WITH TEAMMATES
    // 3. Make a reset function ? It would reset all variables on each level start, or level restarts

    public static GameManager Instance
    {
        get
        {
            // If we have not created one GameManager yet, create one
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    void Update()
    {
    }

    // Count the slowMotion time
    public IEnumerator StartCountdownSlowMotionTime()
    {
        // Set the timer
        currSlowMotionCountDown = MAXIMUM_SLOW_MOTION_COUNT_DOWN_VALUE;

        while (currSlowMotionCountDown > 0)
        {
            //Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currSlowMotionCountDown--;
        }

        // Deactivate slowMotion when the slow motion time has finished
        if (currSlowMotionCountDown == 0) DeactivateSlowMotion();
    }

    void Awake()
    {
        // Assign instance to this game object
        _instance = this;


        // TODO: Check if it is the right place for it, OnStart function might be better place
        // TODO: Check if it is the right place for it, OnStart function might be better place
        // TODO: Check if it is the right place for it, OnStart function might be better place
        // TODO: Check if it is the right place for it, OnStart function might be better place
        // TODO: Check if it is the right place for it, OnStart function might be better place
        slowMotion = 1.0f;


        // TODO: DELETE THIS !!!!!
        // TODO: DELETE THIS !!!!!
        // TODO: DELETE THIS !!!!!
        // TODO: DELETE THIS !!!!!
        //StartCoroutine(StartCountdownSlowMotionTime());
    }

    // Set the slowMotion variable value to 0.5f
    public void ActivateSlowMotion()
    {
        slowMotion = 0.5f;
        StartCoroutine(StartCountdownSlowMotionTime());
    }

    // Set the slowMotion variable value to 1.0f
    public void DeactivateSlowMotion()
    {
        slowMotion = 1.0f;
    }

    // Return the slowMotion value, always return 0.5f or 1.0f, depends if the slowMotion is activated or not
    public float GetSlowMotion()
    {
        return slowMotion;
    }
}
