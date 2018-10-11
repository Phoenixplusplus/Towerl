using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGC : MonoBehaviour {

    // Minor change since Jakub broke Git ....
    public int TiersPerLevel = 35;
    [Header("Necessary Scene Object/Camera References")]
    public Transform Ball;
    public Camera camera;
    public LevelBuilder levelBuilder;
    [Header("GUI Elements")]

    [Header("Object & Game Scales")]
    public Vector3 SegmentScale = new Vector3(100f, 10f, 100f);
    public Vector3 HazardScaleModifier = new Vector3(1.1f, 1.1f, 1.1f);
    public Vector3 ColumnScale = new Vector3(0.8f, 17.5f, 0.8f);
    public Vector3 BallScale = new Vector3(0.18f, 0.18f, 0.18f);
    public float SegmentBaseRotation = 0f;
    public float BallRadius = 0.1f;
    public float BallOffset = 0.8f;
    public float BallStartHeightRatio = 0.75f;
    public float BallMaxVelocity = 2f;
    public float Gravity = -3f;
    public float KeyboardControlSensetivity = 80f;
    public float TouchControlSensetivity = 110f;

    [Header("Game In-Play Details")]
    public float BallHeight;
    public Vector3 CurrentBallVelocity;
    public float TowerAngle;
    public int CurrentTier;
    public bool BallFalling = false;
    private bool GameRunning = false;
    private int TiersPassed = 0; // n.b. as in "Tiers passed in a row and whether we need "POWER BALL" .. also use for score calculation
    public float CurrentGameTime = 0; // Auto incremented in "Update()"
    private float LastGameTime = 0;
    public int CurrentGameScore = 0; // Modified in "MoveTheBall()" (called in Update() .. if required)
    private int LastGameScore = 0;

    [Header ("Global Game Control Flags/States")]
    public int CurrentLevel;
    public int CasualLevel = 0;
    public int CurrentGameMode = 1; // 1 = Classic, 2 = Timed/Story, 3 = Chase
    private int CurrentScreen = 1; // use 1 for "Playing Game" ... others TBA (menu, splash, help etc)

    [Header("Level Difficulty Calculator Variables")]
    public int LevelSpanForZeroTo100Percent = 5; // anything over this will be 100% difficulty
    public float PercentOfPossibleTiersInPool = 0.25f; // i.e. .25 means level 0 = first 25% of tiers, 1.0 = last 25%

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
        // Populate Player's Casual Start Level
        CasualLevel = LevelManager.Instance.GetPlayerCasualLvl();
        // will only be 0 if unasigned therefore NEW DEVICE ... best set it to 1
        if (CasualLevel == 0)
        {
            CasualLevel = 1;
            LevelManager.Instance.SetPlayerCasualLevel(CasualLevel);
        }
    }




    public void PlayMe()
    {
        CurrentTier = TiersPerLevel;
        TowerAngle = 0f;
        LastGameScore = CurrentGameScore;
        LastGameTime = CurrentGameTime;
        CurrentGameTime = 0;
        CurrentGameScore = 0;
        TiersPassed = 0;


        //Destroy any existing Towers
        DestroyLevel();

        switch (LevelManager.Instance.gameMode)
        {
            case (int)MODE_TYPE.CASUAL:  
               // incase we don't have it
               if (CasualLevel == 0)
                    {
                        CasualLevel = LevelManager.Instance.GetPlayerCasualLvl();
                        if (CasualLevel == 0)
                        {
                            CasualLevel++;
                            LevelManager.Instance.SetPlayerCasualLevel(CasualLevel);
                        }
                    }
               // OK, 100% have the player CasualLevel status now
                float difficulty = 1f;
                if (CasualLevel <= LevelSpanForZeroTo100Percent)
                {
                    difficulty = (CasualLevel - 1) / LevelSpanForZeroTo100Percent;
                    Debug.Log("difficulty =: " + (CasualLevel - 1).ToString() + "/" + LevelSpanForZeroTo100Percent.ToString()+ " equals "+ difficulty.ToString());
                }
                Debug.Log(difficulty.ToString()+ " ergo Difficulty level: " + (difficulty*100).ToString() + "% for CasualLevel: " + CasualLevel.ToString());
                levelBuilder.BuildLevelofDifficulty(difficulty);
                break;
            default:
                // ADD FURTHER GAME MODES HERE
                //levelBuilder.BuildRandomLevel();
                levelBuilder.BuildLevel(LevelManager.Instance.GetCurrentLevel());
            break;
        };
        GameRunning = true;
    }

    public void StopMe()
    {
        DestroyLevel();
        GameRunning = false;

        LevelManager.Instance.SetCurrentLevel(0);
        LevelManager.Instance.UpdateCanvases();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameRunning)
        {
            // CONTROLS (only need Controls is Game is Running)
            TowerAngle -= Input.GetAxis("Horizontal") * KeyboardControlSensetivity * Time.deltaTime; // Keyboard input applied to TowerAngle
            // Normalise Tower angle to something between 0-360
            if (TowerAngle < 0f)
            {
                while (TowerAngle < 0f) { TowerAngle += 360f; }
            }
            if (TowerAngle > 360f)
            {
                while (TowerAngle > 360f) { TowerAngle -= 360f; }
            }
            MoveTheBall();
            CurrentGameTime += Time.deltaTime; // Adds the game time (GUI ... Talk to me, baby !!)
        }

        // Enable Ctrl+R to "Reset Player Casual Level"
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerCasualLevel();
        }

    }

    public void MoveTheBall()
    {
        // MOVE THE BALL
        CurrentBallVelocity += Vector3.up * Gravity * Time.deltaTime;
        Ball.transform.position += CurrentBallVelocity * Time.deltaTime;
        // At this stage we are able to detect tier transitions
        // Can compare where the Ball WAS (stored in BallHeight) against the New Height
        float NewBallHeight = Ball.transform.position.y;

        if (CurrentBallVelocity.y < 0) // Only need ANY checking if ball is moving downwards
        {
            if ((int)Mathf.Floor(NewBallHeight) == (int)Mathf.Floor(BallHeight))
            {
                // DO NOTHING .. Are on same Tier between frames
            }
            else // here's the Ball Mechanics, folks.  Hang onto your hats
            {
                if (NewBallHeight <= 0) // Have Reached the bottom
                {
                    GameOver(true);
                    // Add Game Over (Win) complete code here ... or call a function ;p
                    // but for our purposes now
                    //double Start = Time.realtimeSinceStartup;
                    //DestroyLevel();
                    //levelBuilder.BuildRandomLevel();
                    //ResetBall();
                    //Debug.Log("Time to Destroy, Rebuild and Reset = " + (Time.realtimeSinceStartup - Start).ToString());
                }
                else
                {
                    int TierToCheck = (int)Mathf.Floor(NewBallHeight + 1);
                    int SurfaceHit = GetTierSegmentType(TierToCheck, TowerAngle);
                    switch (SurfaceHit)
                    {
                        case 0: // 0 = gap -- FALL THROUGH
                            BallFalling = true;
                            TiersPassed++;
                            break;
                        case 1: // 1 = normal platform --- BOUNCE
                            BallFalling = false; // required for Camera to track ball
                            CurrentGameScore += (TiersPassed * TiersPassed) * 10;
                            // ADD "BREAK Tier if TiersPassed > ???" Here
                            TiersPassed = 0;
                            camera.GetComponent<CameraController2>().SetToHeight(TierToCheck + 1);
                            CurrentBallVelocity = new Vector3(0, BallMaxVelocity, 0);
                            break;
                        case 2: // 2 = Hazard ---- GAME OVER (will just reset)
                            GameRunning = false;
                            //ResetBall();
                            GameOver(false);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        BallHeight = Ball.transform.position.y; // IMPORTANT - this gives us frame to frame comparison
        if (BallHeight < 0) { ResetBall(); }
    }

    // Game over result true = won it, result false = blew it
    private void GameOver(bool result)
    {
        switch (LevelManager.Instance.gameMode)
        {
            case (int)MODE_TYPE.CASUAL:
                if (result == true) // only need to change stuff if it's a win
                {
                    CasualLevel++;
                    LevelManager.Instance.SetPlayerCasualLevel(CasualLevel);
                }
                PlayMe(); // then re-start (either same dificulty, or higher)
                break;
            default:
                // ADD STORY GAME MODE BEHAVIOUR HERE
                levelBuilder.BuildLevel(LevelManager.Instance.GetCurrentLevel());
                break;
        };
    }

    // called with Ctrl+R to allow testing
    private void ResetPlayerCasualLevel()
    {
        CasualLevel = 0;
        LevelManager.Instance.SetPlayerCasualLevel(CasualLevel);
    }

    public void DestroyLevel() // called by MGC to find and destroy all tiers and the column
    {
        // Find & kill the column
        GameObject ThingIWantToKill = GameObject.FindWithTag("Column");
        if (ThingIWantToKill != null) Destroy(ThingIWantToKill);

        // Find and kill the Tiers (Magic Number of 35 Maximum Game Tiers used)
        for (int i = 0; i < 35; i++)
        {
            ThingIWantToKill = GameObject.FindWithTag(i.ToString());
            if (ThingIWantToKill != null) Destroy(ThingIWantToKill);
        }
    }

    // Finds and asks the relevant tier to return the segement code we are interested in
    public int GetTierSegmentType(int TierToCheck, float TowerAngle)
    {
        // find the relevant Tier Object
        Debug.Log("Trying to find " + TierToCheck.ToString() + " @ " + TowerAngle.ToString() + " degrees -  Ball Height: " + BallHeight.ToString());
        GameObject Tier = GameObject.FindWithTag(TierToCheck.ToString());
        if (Tier != null)
        { 
            int answer = Tier.GetComponent<TierScript>().ReportType(TowerAngle);
            Debug.Log("Returning " + answer.ToString());
            return answer;
        }
        else
        {
            Debug.Log("Tried to find Tier " + TierToCheck.ToString() + " but fell over gracefully and returned a 'hole'");
            return 0;
        }
    }


    public void ResetBall () // called by LevelBuilder AFTER it has finished making the Level
    {
        BallHeight = TiersPerLevel + BallStartHeightRatio - 1;
        Ball.transform.position = new Vector3(0f, BallHeight, BallOffset);
        CurrentBallVelocity = new Vector3(0, 0, 0);
        Ball.transform.localScale = BallScale;
        camera.gameObject.GetComponent<CameraController2>().ResetCameraToTop();
        GameRunning = true;
        TowerAngle = 0f;
    }

}
