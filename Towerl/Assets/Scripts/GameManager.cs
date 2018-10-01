using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    /** Slow motion variable use to multiply the ball velocity and the tower rotations, keeps values 1.0 or 0.5
     * If the value is 1.0, the slow motion is not activated
     * If the value is 0.5, the slow motion is activated */
    private float m_slowMotion;

    /** Store the time value, which determine for how long the slow motion will be activated */
    private const int MAXIMUM_SLOW_MOTION_COUNT_DOWN_VALUE = 3;

    /** Store the current time value of how long is slow motion activated
     * If the value is equal or lower than 0.0f, counter will call DeactivateSlowMotion function */
    private int m_currSlowMotionCountDown;

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

    //LevelManager m_levelManager;

    // Count the slowMotion time
    public IEnumerator StartCountdownSlowMotionTime()
    {
        // Set the timer
        m_currSlowMotionCountDown = MAXIMUM_SLOW_MOTION_COUNT_DOWN_VALUE;

        while (m_currSlowMotionCountDown > 0)
        {
            //Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            m_currSlowMotionCountDown--;
        }

        // Deactivate slowMotion when the slow motion time has finished
        if (m_currSlowMotionCountDown == 0) DeactivateSlowMotion();
    }

    void Start()
    {
        // Deactivate slow motion
        m_slowMotion = 1.0f;
    }

    void Awake()
    {
        // Assign instance to this game object
        _instance = this;
        // Load level datas from mobile memory
        //m_levelManager.LoadLevelData();
    }

    void Update()
    {
        // TODO: DELETE THIS LEVEL MANAGER FUNCTIONS !!! DELETE THEM FROM LEVEL MANAGER CLASS TOO !!!!!
        // TODO: DELETE THIS LEVEL MANAGER FUNCTIONS !!! DELETE THEM FROM LEVEL MANAGER CLASS TOO !!!!!
        //if (Input.GetKeyUp(KeyCode.P)) m_levelManager.PrintHighScores();
        //if (Input.GetKeyUp(KeyCode.S)) m_levelManager.SetNewHigschores();
        //if (Input.GetKeyUp(KeyCode.R)) m_levelManager.ClearHighScores();
    }

    // Set the slowMotion variable value to 0.5f
    public void ActivateSlowMotion()
    {
        m_slowMotion = 0.5f;
        StartCoroutine(StartCountdownSlowMotionTime());
    }

    // Set the slowMotion variable value to 1.0f
    public void DeactivateSlowMotion()
    {
        m_slowMotion = 1.0f;
    }

    // Return the slowMotion value, always return 0.5f or 1.0f, depends if the slowMotion is activated or not
    public float GetSlowMotion()
    {
        return m_slowMotion;
    }
}

