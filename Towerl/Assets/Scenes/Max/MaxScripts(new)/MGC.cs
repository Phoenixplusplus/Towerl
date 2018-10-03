using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGC : MonoBehaviour {

    public int TiersPerLevel = 35;

    [Header("Object & Game Scales")]
    public Vector3 SegmentScale = new Vector3(80f, 10f, 80f);
    public Vector3 HazardScaleModifier = new Vector3(1.1f, 1.1f, 1.1f);
    public Vector3 ColumnScale = new Vector3(0.8f, 17.5f, 0.8f);
    public Vector3 BallScale = new Vector3(0.18f, 0.18f, 0.18f);
    public Transform Ball;
    public Camera camera;
    public float SegmentBaseRotation = 0f;
    public float TierHeight = 1f;
    public float BallRadius = 0.1f;
    public float BallOffset = 1f;
    public float BallStartHeightRatio = 0.75f;
    public float BallMaxVelocity = 2f;
    public float Gravity = -3f;
    public float KeyboardControlSensetivity = 80f;
    public float TouchControlSensetivity = 30f;

    [Header("Game Play Details")]
    public float BallHeight;
    public Vector3 CurrentBallVelocity;
    public float TowerAngle;
    public int CurrentTier;
    public int CurrentLevel;
    public int CurrentGameMode = 1; // 1 = Classic, 2 = Timed/Story, 3 = Chase
    private int CurrentScreen = 1; // use 1 for "Playing Game" ... others TBA (menu, splash, help etc)
    public bool BallFalling = false;
    private bool GameRunning = true;


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
        
        if (GameRunning)
        { 
            // CONTROLS (only need Controls is Game is Running)
            TowerAngle -= Input.GetAxis("Horizontal") * KeyboardControlSensetivity * Time.deltaTime;
            if (TowerAngle < 0)
            {
                while (TowerAngle < 0) { TowerAngle += 360; }
            }
            if (TowerAngle > 360)
            {
                while (TowerAngle > 360) { TowerAngle -= 360; }
            }


            // MOVE THE BALL
            CurrentBallVelocity += Vector3.up * Gravity * Time.deltaTime;
            Ball.transform.position += CurrentBallVelocity * Time.deltaTime;
            // At this stage we are able to detect tier transitions
            // Can compare where the Ball WAS (stored in BallHeight) against the New Height
            float NewBallHeight = Ball.transform.position.y;
            if ((int)Mathf.Floor(NewBallHeight) == (int)Mathf.Floor(BallHeight))
            {
                // DO NOTHING .. Are on same Tier between frames
            }
            else // here's the Ball Mechanics, folks.  Hang onto your hats
            {
                if (NewBallHeight <=0) // Have Reached the bottom
                {
                    // Add Game Over (Win) complete code here ... or call a function ;p
                    // but for our purposes now
                    ResetBall();
                }
                else
                {
                    int TierToCheck = (int)Mathf.Floor(NewBallHeight );
                    int SurfaceHit = GetTierSegmentType(TierToCheck, TowerAngle);
                    switch (SurfaceHit)
                    {
                        case 0: // 0 = gap -- FALL THROUGH
                            BallFalling = true;
                            break;
                        case 1: // 1 = normal platform --- BOUNCE
                            BallFalling = false;
                            camera.GetComponent<CameraController2>().SetToHeight(TierToCheck + 1);
                            CurrentBallVelocity = new Vector3(0, BallMaxVelocity, 0);
                            break;
                        case 2: // 2 = Hazard ---- GAME OVER (will just reset)
                            GameRunning = false;
                            ResetBall();
                            break;
                        default:
                            break;
                    }
                }
            }


            BallHeight = Ball.transform.position.y;
            if (BallHeight < 0) { ResetBall(); }


            
        }
    }

    public int GetTierSegmentType(int TierToCheck, float TowerAngle)
    {
        // find the relevant Tier Object
        Debug.Log("Trying to find " + TierToCheck.ToString() + " @ " + TowerAngle.ToString()+ " Ball Height: " + BallHeight.ToString());
        GameObject Tier = GameObject.FindWithTag(TierToCheck.ToString());
        int answer = Tier.GetComponent<TierScript>().ReportType(TowerAngle);
        Debug.Log("Returning " + answer.ToString());
        return answer;
    }


    public void ResetBall () // called by LevelBuilder AFTER it has finished making the Level
    {
        BallHeight = TiersPerLevel + BallStartHeightRatio - 1;
        Ball.transform.position = new Vector3(0f, BallHeight, BallOffset);
        CurrentBallVelocity = new Vector3(0, 0, 0);
        Ball.transform.localScale = BallScale;
        camera.gameObject.GetComponent<CameraController2>().ResetCameraToTop();
        GameRunning = true;
    }

    void SetBallToTop ()
    {

    }

}
