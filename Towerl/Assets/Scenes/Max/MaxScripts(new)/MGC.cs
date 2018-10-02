using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGC : MonoBehaviour {

    public int TiersPerLevel = 35;

    [Header("Object & Game Scales")]
    public Vector3 SegmentScale = new Vector3(80f, 10f, 80f);
    public Vector3 ColumnScale = new Vector3(0.8f, 17.5f, 0.8f);
    public Vector3 BallScale = new Vector3(0.2f, 0.2f, 0.2f);
    public Transform Ball;
    public float SegmentBaseRotation = 0f;
    public float TierHeight = 1f;
    public float BallRadius = 0.1f;
    public float BallOffset = 0.8f;
    public float BallStartHeightRatio = 0.75f;
    public float BallMaxVelocity = 2f;
    public float Gravity = -3f;
    public float KeyboardControlSensetivity = 30f;
    public float TouchControlSensetivity = 30f;

    [Header("Game Play Details")]
    public float BallHeight;
    public Vector3 CurrentBallVelocity;
    public float TowerAngle;
    public int CurrentTier;
    public int CurrentLevel;
    public int CurrentGameMode = 1; // 1 = Classic, 2 = Timed/Story, 3 = Chase


    // --------------------//
    // establish Singelton //
    // ------------------- //
    public static MGC Instance
    {
        get
        {
            return instance;
        }
    }
    private static MGC instance = null;
    void Awake()
    {
        if (instance)
        {
            Debug.Log("Already an MGC (MaxGameController) - going to die now .....");
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //---------------------------//
    // Finished Singelton set up //
    // --------------------------//

    // Use this for initialization
    void Start()
    {
        CurrentTier = TiersPerLevel;
        TowerAngle = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        TowerAngle += Input.GetAxis("Horizontal") * KeyboardControlSensetivity * Time.deltaTime;
        if (TowerAngle < 0)
        {
            while (TowerAngle < 0) { TowerAngle += 360; }
        }
        if (TowerAngle > 360)
        {
            while (TowerAngle > 360) { TowerAngle -= 360; }
        }
    }

    void SetBallToTop ()
    {

    }

}
