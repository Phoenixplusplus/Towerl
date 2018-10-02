using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGC : MonoBehaviour {

    public int TiersPerLevel = 35;

    [Header("Object & Game Scales")]
    public Vector3 SegmentScale = new Vector3(4f, 0.5f, 4f);
    public Vector3 ColumnScale = new Vector3(0.8f, 7.5f, 0.8f);
    public Vector3 BallScale = new Vector3(0.2f, 0.2f, 0.2f);
    public float SegmentBaseRotation = 0f;
    public float TierHeight = 1f;
    public float BallRadius = 0.1f;
    public float BallOffset = 0.8f;
    public float BallStartHeightRatio = 0.75f;
    public float BallMaxVelocity = 2f;
    public float Gravity = -3f;

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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
