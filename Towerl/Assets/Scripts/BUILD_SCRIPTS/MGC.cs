using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGC : MonoBehaviour {

    // Minor change since Jakub broke Git ....
    public int TiersPerLevel = 35;
    [Header("Necessary Scene Object/Camera References")]
    public Transform Ball;
    public new Camera camera;
    public LevelBuilder levelBuilder;
    public LevelManager levelManager; // for ingame UI
    public SoundManager SM;
    public GameObject scoreSprite;
    public GameObject bonusSprite;
    public GameObject startSprite;
    public GameObject notbadSprite;
    public GameObject niceSprite;
    public GameObject amazingSprite;
    public GameObject oneStarEarnedSprite;
    public GameObject twoStarsEarnedSprite;
    public GameObject threeStarsEarnedSprite;
    public GameObject tryAgainSprite;
    public GameObject levelUpSprite;
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
    public int SkinType;

    [Header("Game In-Play Details")]
    public float BallHeight;
    public Vector3 CurrentBallVelocity;
    public float TowerAngle;
    public int CurrentTier;
    public bool BallFalling = false; // used by camera to establish if if needs to track down
    public bool GameRunning = false;
    public bool isAnimating = false;
    public bool resultGameOver = false;
    private float animationTimer = 0f;
    private float maxAnimationTime = .5f;
    private bool startAnimation = false;
    [SerializeField]
    private float SquishTimer = 0f;
    [SerializeField]
    private float SquishDuration = 0.5f;
    private int TiersPassed = 0; // n.b. as in "Tiers passed in a row and whether we need "POWER BALL" .. also use for score calculation
    public float CurrentGameTime = 0; // Auto incremented in "Update()"
    private float LastGameTime = 0;
    public int CurrentGameScore = 0; // Modified in "MoveTheBall()" (called in Update() .. if required)
    private int LastGameScore = 0;
    private int storyBonusMultiplier = 3;

    [Header("Global Game Control Flags/States")]
    [SerializeField] bool DEVELOPMENTBUILD = true;
    public int CurrentLevel;
    public int CasualLevel = 0;
    public int CurrentGameMode = 1; // 1 = Classic, 2 = Timed/Story, 3 = Chase // QUESTION - ARE WE USING THIS ANYWHERE ?
    private int CurrentScreen = 1; // use 1 for "Playing Game" ... others TBA (menu, splash, help etc) // QUESTION - ARE WE USING THIS ANYWHERE ?

    [Header("Level Difficulty Calculator Variables")]
    public int LevelSpanForZeroTo100Percent = 20; // anything over this will be 100% difficulty
    public float PercentOfPossibleTiersInPool = 0.25f; // i.e. .25 means level 0 = first 25% of tiers, 1.0 = last 25%
    public float CurrentDifficulty = 0f; // Used by Tiers to establish whether they need to have a rotate chance .. set to 0 on "PlayMe()" ... only given a value 0-1 when difficulty used.

    [Header("Music/SFX bools & Music Selection")]
    // N.B. Sound Manager looks for changes in these and behaves accordingly
    // GUI can access to update in various functions below
    public bool Music_ON = true;    // change with ToggleMusic()
    [Range(0,1)]
    public float Music_Vol = 1f;    // change with ChangeMusicVolume(float)
    public bool SFX_ON = true;      // change with ToggleSFX()
    [Range(0, 1)]
    public float SFX_Vol = 1f;      // change with ChangeMusicVolume(float)
    public Music MusicChoice;       // Current Music Choice // cycle with ChangeMusicTrack() // WARNING ?? Revisit if number of BG tracks changes from 10

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
        // Grab level manager for UI changes
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        // Populate Player's Casual Start Level
        CasualLevel = LevelManager.Instance.GetPlayerCasualLevel();
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
        CurrentDifficulty = 0; // used to establish if Tiers need to rotate ...
        TiersPassed = 0;
        Ball.GetComponent<TrailRenderer>().enabled = false;
        StartCoroutine(ActivateTrail(Ball.GetComponent<TrailRenderer>()));
        levelManager.ChangeScore(LastGameScore, CurrentGameScore, true);
        camera.GetComponent<CameraController2>().EnableAdventureMap(false);

        //Destroy any existing Towers
        DestroyLevel();

        switch (LevelManager.Instance.GetGameMode())
        {
            case (int)MODE_TYPE.CASUAL:
                // incase we don't have it
                if (CasualLevel == 0)
                {
                    CasualLevel = LevelManager.Instance.GetPlayerCasualLevel();
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
                    difficulty = (float)(CasualLevel - 1) / LevelSpanForZeroTo100Percent;
                    Debug.Log("difficulty =: " + (CasualLevel - 1).ToString() + "/" + LevelSpanForZeroTo100Percent.ToString() + " equals " + difficulty.ToString());
                }
                CurrentDifficulty = difficulty;
                Debug.Log(difficulty.ToString() + " ergo Difficulty level: " + (difficulty * 100).ToString() + "% for CasualLevel: " + CasualLevel.ToString());
                levelBuilder.BuildLevelofDifficulty(difficulty);
                // if casual mode, grab saved score
                LastGameScore = levelManager.LoadPlayerCasualScore();
                CurrentGameScore = LastGameScore;
                levelManager.ChangeScore(LastGameScore, CurrentGameScore, true);
                // change the progress bar too
                levelManager.ChangeProgressBar(0f, CasualLevel, true);
                // change backdrop
                camera.GetComponent<CameraController2>().ChangeBackdropMaterial(LevelManager.Instance.GetGameMode());
                break;
            default:
                // ADD FURTHER GAME MODES HERE
                //levelBuilder.BuildRandomLevel();
                levelBuilder.BuildLevel(LevelManager.Instance.GetSelectedLevel());
                levelManager.ChangeScore(0, 0, true);
                // change backdrop
                camera.GetComponent<CameraController2>().ChangeBackdropMaterial(LevelManager.Instance.GetGameMode());
                break;
        };
        GameRunning = true;

        // start sprite
        GameObject s_startSprite = Instantiate(startSprite, Ball.transform.position - new Vector3(0, 1.0f, -1), new Quaternion(0, 180, -60, 1));
        s_startSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
    }

    public void StopMe()
    {
        DestroyLevel();
        GameRunning = false;
        isAnimating = false;

        LevelManager.Instance.SetSelectedLevel(0);
        LevelManager.Instance.UpdateCanvases();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(CurrentGameScore); // might need it again one day
        if (GameRunning)
        {
            //if (startAnimation == false)
            //startAnimation = true;

            CurrentGameTime += Time.deltaTime; // Adds the game time (GUI ... Talk to me, baby !!)

            if (CurrentGameTime > maxAnimationTime)
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
            }
        }

        // Enable Ctrl+R to "Reset Player Casual Level"
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerCasualLevel();
        }

        // adroid back button ^_^
        if (Input.GetKeyDown(KeyCode.Escape)) StopMe();

        // animation and gameover
        if (isAnimating == true) UpdateAnimation(maxAnimationTime, resultGameOver);
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
                    SM.PlaySFX(SFX.Woohoo);
                    resultGameOver = true;
                    isAnimating = true; // trigger the function at the end of Update()
                    // end game sprite
                    SpawnWinGameSprite();
                    //GameOver(resultGameOver);
                }
                else
                {
                    int TierToCheck = (int)Mathf.Floor(NewBallHeight + 1);
                    int SurfaceHit = GetTierSegmentType(TierToCheck, TowerAngle);
                    switch (SurfaceHit)
                    {
                        case 0: // 0 = gap -- FALL THROUGH
                            BallFalling = true;
                            
                            // Find and run break scripts for current tier's parent segment, and its children
                            BreakawayAndDie[] childrenSegs;
                            GameObject parentSeg = GameObject.FindWithTag(TierToCheck.ToString());
                            if (parentSeg != null)
                            {
                                childrenSegs = parentSeg.GetComponentsInChildren<BreakawayAndDie>();
                                parentSeg.GetComponent<BreakawayAndDie>().KillSegment(2f, 1f);
                                foreach (BreakawayAndDie child in childrenSegs)
                                {
                                    child.gameObject.tag = "Fragment";
                                    child.KillSegment(2f, 1f);
                                }
                                TiersPassed++;
                                SM.PlaySFX(SFX.Whoosh);
                                // Set progress bar amount, if casual mode
                                if (LevelManager.Instance.GetGameMode() == 0) levelManager.ChangeProgressBar(1.0f - (BallHeight / TiersPerLevel), CasualLevel, false);
                                // Set powerball aplha/colour coroutine
                                StartCoroutine(PowerballStatus(Ball.GetChild(0).gameObject));
                                // Set score
                                CurrentGameScore += (TiersPassed * TiersPassed) * 10;
                                // Spawn score sprite
                                GameObject sprite = Instantiate(scoreSprite, Ball.transform.position - new Vector3(0, 0.4f, 0), new Quaternion(0, 180, 0, 1));
                                sprite.GetComponent<TextMesh>().text = "+" + ((TiersPassed * TiersPassed) * 10).ToString();
                                // Change score on UI
                                int PreviousGameScore = CurrentGameScore - (TiersPassed * TiersPassed) * 10;
                                levelManager.ChangeScore(PreviousGameScore, CurrentGameScore, false);
                            }
                            break;

                        case 1: // 1 = normal platform --- BOUNCE
                            BallFalling = false; // required for Camera to track ball
                            // ADD "BREAK Tier if TiersPassed > ???" Here
                            if (TiersPassed >= 3) BreakthroughTier(TierToCheck);
                            TiersPassed = 0;
                            camera.GetComponent<CameraController2>().SetToHeight(TierToCheck + 1);
                            CurrentBallVelocity = new Vector3(0, BallMaxVelocity, 0);
                            SM.PlaySFX(SFX.Laser);
                            // Reset powerball colour
                            break;

                        case 2: // 2 = Hazard ---- GAME OVER (will just reset)
                            // Tier Breakaway Code // does not kill game
                            if (TiersPassed >= 3)
                            {
                                BreakthroughTier(TierToCheck);
                                BallFalling = false;
                                TiersPassed = 0;
                                camera.GetComponent<CameraController2>().SetToHeight(TierToCheck + 1);
                                CurrentBallVelocity = new Vector3(0, BallMaxVelocity, 0);
                                break;
                            }
                            SquishTimer = SquishDuration;
                            StartCoroutine(Squish());
                            SM.PlaySFX(SFX.Titter);
                            resultGameOver = false;
                            isAnimating = true; // trigger the function at the end of Update()
                            // try again sprite
                            if (LevelManager.Instance.GetGameMode() == 0)
                            {
                                GameObject s_tryAgainSprite = Instantiate(tryAgainSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                                s_tryAgainSprite.transform.localScale = new Vector3(1.0f, 0.25f, 0.5f);
                                s_tryAgainSprite.GetComponent<EarnedSprite>().time = 0.3f;
                            }
                            else
                            {
                                GameObject s_tryAgainSprite = Instantiate(tryAgainSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                                s_tryAgainSprite.transform.localScale = new Vector3(1.0f, 0.25f, 0.5f);

                            }
                            //GameOver(resultGameOver);
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        BallHeight = Ball.transform.position.y; // IMPORTANT - this gives us frame to frame comparison
    }

    public void BreakthroughTier (int TierToDie)
    {
        // Let's make some noise about it ...
        SM.PlaySFX(SFX.Boom);
        // and give it some vibration
        StartCoroutine(ActivateVibration(0.2f, true));
        // Find and run break scripts for current tier's parent segment, and its children
        BreakawayAndDie[] childrenBreakSegs;
        GameObject parentBreakSeg = GameObject.FindWithTag(TierToDie.ToString());
        childrenBreakSegs = parentBreakSeg.GetComponentsInChildren<BreakawayAndDie>();
        parentBreakSeg.GetComponent<BreakawayAndDie>().KillSegment(2f, 1f);
        foreach (BreakawayAndDie child in childrenBreakSegs)
        {
            child.gameObject.tag = "Fragment";
            child.BreakthroughSegment(2f, 1f);
        }
        // also increment the score/BONUS score here, as it's only typically done when a segment is 0, but we should add a 'multiplier' score for successful breakthrough
        if (LevelManager.Instance.GetGameMode() == 0) CurrentGameScore += (TiersPassed * TiersPassed) * (10 * (TiersPassed + CasualLevel));
        else CurrentGameScore += (TiersPassed * TiersPassed) * (10 * (TiersPassed + storyBonusMultiplier));
        // Spawn score sprite
        GameObject sprite = Instantiate(scoreSprite, Ball.transform.position + new Vector3(0, 0.4f, 0), new Quaternion(0, 180, 0, 1));
        if (LevelManager.Instance.GetGameMode() == 0) sprite.GetComponent<TextMesh>().text = "+" + ((TiersPassed * TiersPassed) * (10 * (TiersPassed + CasualLevel))).ToString();
        else sprite.GetComponent<TextMesh>().text = "+" + ((TiersPassed * TiersPassed) * (10 * (TiersPassed + storyBonusMultiplier))).ToString();
        sprite.GetComponent<TextMesh>().color = new Color(0.7176471f, 0.09803922f, 0.145098f, 1);
        // Spawn bonus sprite
        GameObject bonussprite = Instantiate(bonusSprite, Ball.transform.position + new Vector3(0, 0.2f, 0), new Quaternion(0, 180, 0, 1));
        // Change BONUS score on UI
        if (LevelManager.Instance.GetGameMode() == 0)
        {
            int PreviousGameScore = CurrentGameScore - (TiersPassed * TiersPassed) * (10 * (TiersPassed + CasualLevel));
            levelManager.ChangeScore(PreviousGameScore, CurrentGameScore, false);
        }
        else
        {
            int PreviousGameScore = CurrentGameScore - (TiersPassed * TiersPassed) * (10 * (TiersPassed + storyBonusMultiplier));
            levelManager.ChangeScore(PreviousGameScore, CurrentGameScore, false);
        }
        // enable camera shake (strength based on tiers passed)
        camera.GetComponent<CameraController2>().EnableCameraShake(0.005f * TiersPassed, 1.0f, 1.0f);
    }

    // Game over result true = won it, result false = blew it
    public void GameOver(bool result)
    {
        switch (LevelManager.Instance.GetGameMode())
        {
            case (int)MODE_TYPE.CASUAL:
                if (result == true) // only need to change stuff if it's a win
                {
                    CasualLevel++;
                    LevelManager.Instance.SetPlayerCasualLevel(CasualLevel);
                    LevelManager.Instance.SetPlayerCasualScore(CurrentGameScore);
                }
                PlayMe(); // then re-start (either same dificulty, or higher)
                break;
            default:
                // ADD STORY GAME MODE BEHAVIOUR HERE
                if (result) // win
                {
                    DestroyLevel();
                    // set stars on score
                    switch (SkinType)
                    {
                        case 1:
                            if (CurrentGameScore < 7500 ) LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 1);
                            else if (CurrentGameScore > 7500 && CurrentGameScore < 15000) LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 2);
                            else LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 3);
                            LevelManager.Instance.UnlockLevel(LevelManager.Instance.GetSelectedLevel() + 1);
                            LevelManager.Instance.userInterface.GetStarsButtons(LevelManager.Instance.GetSelectedLevel());
                            break;
                        case 2:
                            if (CurrentGameScore < 3000) LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 1);
                            else if (CurrentGameScore < 8000) LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 2);
                            else LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 3);
                            LevelManager.Instance.UnlockLevel(LevelManager.Instance.GetSelectedLevel() + 1);
                            LevelManager.Instance.userInterface.GetStarsButtons(LevelManager.Instance.GetSelectedLevel());
                            break;
                        case 3:
                            if (CurrentGameScore < 2000) LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 1);
                            else if (CurrentGameScore < 7000) LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 2);
                            else LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 3);
                            LevelManager.Instance.UnlockLevel(LevelManager.Instance.GetSelectedLevel() + 1);
                            LevelManager.Instance.userInterface.GetStarsButtons(LevelManager.Instance.GetSelectedLevel());
                            break;
                    }

                    StopMe();
                    camera.GetComponent<CameraController2>().EnableAdventureMap(true);
                }
                else
                {
                    DestroyLevel();
                    PlayMe();
                }
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

        // Find and kill any "Fragments" from exploded segements
        GameObject[] frags = GameObject.FindGameObjectsWithTag("Fragment");
        foreach(GameObject frag in frags)
        {
            Destroy(frag);
        }
    }

    // Finds and asks the relevant tier to return the segement code we are interested in
    public int GetTierSegmentType(int TierToCheck, float TowerAngle)
    {
        // find the relevant Tier Object
        // Debug log saved for possible future use
        // Debug.Log("Trying to find " + TierToCheck.ToString() + " @ " + TowerAngle.ToString() + " degrees -  Ball Height: " + BallHeight.ToString());
        GameObject Tier = GameObject.FindWithTag(TierToCheck.ToString());
        if (Tier != null)
        {
            int answer = Tier.GetComponent<TierScript>().ReportType(TowerAngle);
            // Debug.Log("Returning " + answer.ToString());
            return answer;
        }
        else
        {
            Debug.Log("Tried to find Tier " + TierToCheck.ToString() + " but fell over gracefully and returned a 'hole'");
            return 0;
        }
    }


    public void ResetBall() // called by LevelBuilder AFTER it has finished making the Level
    {
        BallHeight = TiersPerLevel + BallStartHeightRatio - 1;
        Ball.transform.position = new Vector3(0f, BallHeight, BallOffset);
        CurrentBallVelocity = new Vector3(0, 0, 0);
        StopCoroutine(Squish());
        SquishTimer = 0f;
        Ball.transform.localScale = BallScale; // useful since we may have scaled the ball upon death
        camera.gameObject.GetComponent<CameraController2>().ResetCameraToTop();
        GameRunning = true;
        TowerAngle = 0f;
    }

    public void UpdateAnimation(float maxAnimationTime, bool result)
    {
        GameRunning = false;
        BallFalling = false;

        if (LevelManager.Instance.GetGameMode() == 0) // casual mode
        {
            if (animationTimer < maxAnimationTime)
            {
                animationTimer += Time.deltaTime;
            }
            else
            {
                isAnimating = false;
                animationTimer = 0;
                GameOver(result); // this ultimately sets gameRunning to true again
            }
        }
        else // story mode, give a bit more time for transition to read displayed score
        {
            if (animationTimer < maxAnimationTime * 2)
            {
                animationTimer += Time.deltaTime;
            }
            else
            {
                isAnimating = false;
                animationTimer = 0;
                GameOver(result); // this ultimately sets gameRunning to true again
            }
        }
    }

    // used to spawn sprites to screen once game is won
    public void SpawnWinGameSprite()
    {
        if (LevelManager.Instance.GetGameMode() == 0)
        {
            GameObject s_levelSprite = Instantiate(levelUpSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
            s_levelSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
        }
        if (LevelManager.Instance.GetGameMode() != 0)
        {
            switch (SkinType)
            {
                case 1:
                    if (CurrentGameScore < 7500)
                    {
                        LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 1);
                        GameObject s_notbadSprite = Instantiate(notbadSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                        s_notbadSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
                        GameObject s_oneSprite = Instantiate(oneStarEarnedSprite, Ball.transform.position - new Vector3(0, 0.5f, -1), new Quaternion(0, 180, -60, 1));
                        s_oneSprite.transform.localScale = new Vector3(1.3f, 0.65f, 0.5f);
                    }
                    else if (CurrentGameScore < 15000)
                    {
                        LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 2);
                        GameObject s_niceSprite = Instantiate(niceSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                        s_niceSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
                        GameObject s_twoSprite = Instantiate(twoStarsEarnedSprite, Ball.transform.position - new Vector3(0, 0.5f, -1), new Quaternion(0, 180, -60, 1));
                        s_twoSprite.transform.localScale = new Vector3(1.3f, 0.65f, 0.5f);
                    }
                    else
                    {
                        LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 3);
                        GameObject s_amazingSprite = Instantiate(amazingSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                        s_amazingSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
                        GameObject s_threeSprite = Instantiate(threeStarsEarnedSprite, Ball.transform.position - new Vector3(0, 0.5f, -1), new Quaternion(0, 180, -60, 1));
                        s_threeSprite.transform.localScale = new Vector3(1.3f, 0.65f, 0.5f);
                        LevelManager.Instance.UnlockLevel(LevelManager.Instance.GetSelectedLevel() + 1);
                        LevelManager.Instance.userInterface.GetStarsButtons(LevelManager.Instance.GetSelectedLevel());
                    }
                    break;
                case 2:
                    if (CurrentGameScore < 3000)
                    {
                        LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 1);
                        GameObject s_notbadSprite = Instantiate(notbadSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                        s_notbadSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
                        GameObject s_oneSprite = Instantiate(oneStarEarnedSprite, Ball.transform.position - new Vector3(0, 0.5f, -1), new Quaternion(0, 180, -60, 1));
                        s_oneSprite.transform.localScale = new Vector3(1.3f, 0.65f, 0.5f);
                    }
                    else if (CurrentGameScore < 8000)
                    {
                        LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 2);
                        GameObject s_niceSprite = Instantiate(niceSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                        s_niceSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
                        GameObject s_twoSprite = Instantiate(twoStarsEarnedSprite, Ball.transform.position - new Vector3(0, 0.5f, -1), new Quaternion(0, 180, -60, 1));
                        s_twoSprite.transform.localScale = new Vector3(1.3f, 0.65f, 0.5f);
                    }
                    else
                    {
                        LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 3);
                        GameObject s_amazingSprite = Instantiate(amazingSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                        s_amazingSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
                        GameObject s_threeSprite = Instantiate(threeStarsEarnedSprite, Ball.transform.position - new Vector3(0, 0.5f, -1), new Quaternion(0, 180, -60, 1));
                        s_threeSprite.transform.localScale = new Vector3(1.3f, 0.65f, 0.5f);
                        LevelManager.Instance.UnlockLevel(LevelManager.Instance.GetSelectedLevel() + 1);
                        LevelManager.Instance.userInterface.GetStarsButtons(LevelManager.Instance.GetSelectedLevel());
                    }
                    break;
                case 3:
                    if (CurrentGameScore < 2000)
                    {
                        LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 1);
                        GameObject s_notbadSprite = Instantiate(notbadSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                        s_notbadSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
                        GameObject s_oneSprite = Instantiate(oneStarEarnedSprite, Ball.transform.position - new Vector3(0, 0.5f, -1), new Quaternion(0, 180, -60, 1));
                        s_oneSprite.transform.localScale = new Vector3(1.3f, 0.65f, 0.5f);
                    }
                    else if (CurrentGameScore < 7000)
                    {
                        LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 2);
                        GameObject s_niceSprite = Instantiate(niceSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                        s_niceSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
                        GameObject s_twoSprite = Instantiate(twoStarsEarnedSprite, Ball.transform.position - new Vector3(0, 0.5f, -1), new Quaternion(0, 180, -60, 1));
                        s_twoSprite.transform.localScale = new Vector3(1.3f, 0.65f, 0.5f);
                    }
                    else
                    {
                        LevelManager.Instance.SetLevelStars(LevelManager.Instance.GetSelectedLevel(), 3);
                        GameObject s_amazingSprite = Instantiate(amazingSprite, Ball.transform.position - new Vector3(0, 0, -1), new Quaternion(0, 180, -60, 1));
                        s_amazingSprite.transform.localScale = new Vector3(1, 0.3f, 0.5f);
                        GameObject s_threeSprite = Instantiate(threeStarsEarnedSprite, Ball.transform.position - new Vector3(0, 0.5f, -1), new Quaternion(0, 180, -60, 1));
                        s_threeSprite.transform.localScale = new Vector3(1.3f, 0.65f, 0.5f);
                        LevelManager.Instance.UnlockLevel(LevelManager.Instance.GetSelectedLevel() + 1);
                        LevelManager.Instance.userInterface.GetStarsButtons(LevelManager.Instance.GetSelectedLevel());
                    }
                    break;
            }
        }
    }

    IEnumerator Squish()
    {

        while (SquishDuration > 0)
        { 
            SquishTimer -= Time.deltaTime;
            if (SquishTimer <= 0) break;
            float lerp = (SquishDuration - SquishTimer) / SquishDuration;
            Debug.Log("Squish Lerp: " + lerp.ToString());
            Ball.localScale = Vector3.Lerp(BallScale, new Vector3(BallScale.x * 3f, BallScale.y * 0.05f, BallScale.z * 3f), lerp);
            Ball.position = Vector3.Lerp(Ball.position, new Vector3(Ball.position.x, Mathf.FloorToInt(Ball.position.y) + 1.01f - (BallScale.y / 2), Ball.position.z), lerp);
            yield return null;
        }
        SquishTimer = 0f;
        yield break;
    }

    IEnumerator PowerballStatus(GameObject Powerball)
    {
        //GameObject Powerball;
        if (Powerball.activeInHierarchy == false) Powerball.SetActive(true);

        Color powerballColour;
        // set alpha to 0 first, for some reason in editor it is 0, but when activate it starts at 1
        powerballColour = Powerball.GetComponent<MeshRenderer>().material.color;
        powerballColour.a = 0f;
        Powerball.GetComponent<MeshRenderer>().material.color = powerballColour;

        while (Powerball.activeInHierarchy == true)
        {
            // enable trail whenever powerball is active
            //Ball.GetComponent<TrailRenderer>().enabled = true;

            float powerballAlpha = powerballColour.a;

            powerballAlpha = (CurrentBallVelocity.y - (CurrentBallVelocity.y * 6)) / 40f; // default = (CurrentBallVelocity.y - (CurrentBallVelocity.y * 2)) / 10f

            powerballColour.a = powerballAlpha;
            Powerball.GetComponent<MeshRenderer>().material.color = powerballColour;

            if (CurrentBallVelocity.y > 0f || Ball.transform.position.y > 34.0f)
            {
                //Ball.GetComponent<TrailRenderer>().enabled = false;
                Ball.GetComponent<TrailRenderer>().material.color = new Color(0,0,0.5f,1);
                Powerball.GetComponent<MeshRenderer>().material.color = new Color(powerballColour.r, powerballColour.g, powerballColour.b, 0f);
                Powerball.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator ActivateTrail(TrailRenderer Trail)
    {
        float time = 0f;
        while (Trail.enabled == false)
        {
            time += Time.deltaTime;
            yield return null;
            if (time > 0.3f) Trail.enabled = true; 
        }
    }

    IEnumerator ActivateVibration(float delay, bool enabled)
    {
        float time = 0;
        while (enabled == true)
        {
            time += Time.deltaTime;
            yield return null;
            if (time > delay)
            {
                Handheld.Vibrate();
                time = 0;
                enabled = false;
                yield break;
            }
        }
    }

    /////////////////
    // SOUND Changers
    public void ToggleMusic()
    {
        Music_ON = !Music_ON;
    }

    public void ToggleSFX()
    {
        SFX_ON = !SFX_ON;
    }

    public void ChangeMusicVolume (float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        Music_Vol = volume;
    }

    public void ChangeSFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        SFX_Vol = volume;
    }

    // DANGER ... MAGIC NUMBER ALERT .. FIXED INTO 10 Background Music Tracks
    public void ChangeMusicTrack() 
    {
        if (MusicChoice >= (Music)9)
            { MusicChoice = 0; }
        else
        {
            MusicChoice++;
        }
    }
    // END OF SOUND Changers
    ////////////////////////



}

