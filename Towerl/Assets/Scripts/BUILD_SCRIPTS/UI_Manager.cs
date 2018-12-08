//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 1: Mobile Game            //
//                                      //
// Team Heron                           //
//                                      //
// December 2018                        //
//                                      //
// TOWERL Code                          //
// UI_Manager.cs                        //
//////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    private bool m_IsMenuOpened = false;
    private bool m_IsDevOpened = false;

    public Button BTN_Continue;
    public Button BTN_BackToMenu;
    public Button BTN_QuitGame;
    public Button BTN_ResetCasualLevelData;
    public Button BTN_ShuffleLevel;
    public Text TXT_DevData;


    public int starsEarnedTotal;
    public int starsEarnedThemeOne;
    public int starsEarnedThemeTwo;
    public int starsEarnedThemeThree;

    private SM_ButtonLevel [] m_buttonLevelArray;


    /** Three image variables, they can not be changed during the game */
    [SerializeField] private Sprite m_NoStarsEarnedSprite;
    [SerializeField] private Sprite m_OneStarSprite;
    [SerializeField] private Sprite m_TwoStarSprite;
    [SerializeField] private Sprite m_ThreeStarSprite;
    [SerializeField] private Sprite m_LevelIsLocked;
    [SerializeField] private Sprite m_LevelIsUnlocked;

    /** Canvases references */
    public Canvas CNVS_gameplay;
    public Canvas CNVS_mainMenu;
    public Canvas CNVS_LevelThemeChoose;
    public Canvas CNVS_ThemeOne;
    public Canvas CNVS_ThemeTwo;
    public Canvas CNVS_ThemeThree;

    // And the NEW MENU
    public Menu_Control Select_Menu;

    //DEV
    MGC mgc;
    LevelManager lvlmgr;

    void Awake()
    {
        starsEarnedTotal = 0;
        starsEarnedThemeOne = 0;
        starsEarnedThemeTwo = 0;
        starsEarnedThemeThree = 0;

        //DEV
        mgc = GameObject.Find("MGC").GetComponent<MGC>();
        lvlmgr = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void Start()
    {
        m_buttonLevelArray = FindObjectsOfType(typeof(SM_ButtonLevel)) as SM_ButtonLevel[];

        CNVS_LevelThemeChoose.gameObject.SetActive(false);
        CNVS_ThemeOne.gameObject.SetActive(false);
        CNVS_ThemeTwo.gameObject.SetActive(false);
        CNVS_ThemeThree.gameObject.SetActive(false);
    }

    public void GetStarsButtons(int value)
    {
        for (int i = 0; i < 29; i++)
        {
            if (m_buttonLevelArray[i].level == value)
            {
                m_buttonLevelArray[i].UpdateStars();
                return;
            }
        }
        Debug.Log("Could not set new stars sprite to level " + value);
    }

    public void OpenOrCloseMenu()
    {
        ///** If menu is not opened, open it */
        //if (!m_IsMenuOpened)
        //{
        //    if (BTN_ResetCasualLevelData.isActiveAndEnabled == true) OpenOrCloseDevMenu();

        //    BTN_Continue.gameObject.SetActive(true);
        //    BTN_BackToMenu.gameObject.SetActive(true);
        //    BTN_QuitGame.gameObject.SetActive(true);
        //    m_IsMenuOpened = true;
        //}
        //else /** If menu is opened, close it */
        //{
        //    BTN_Continue.gameObject.SetActive(false);
        //    BTN_BackToMenu.gameObject.SetActive(false);
        //    BTN_QuitGame.gameObject.SetActive(false);
        //    m_IsMenuOpened = false;
        //}
    }

    public void OpenOrCloseDevMenu()
    {
        if (BTN_Continue.isActiveAndEnabled == true) OpenOrCloseMenu();

        if (BTN_ResetCasualLevelData.isActiveAndEnabled == true)
        {
            BTN_ResetCasualLevelData.gameObject.SetActive(false);
            BTN_ShuffleLevel.gameObject.SetActive(false);
            TXT_DevData.gameObject.SetActive(false);
            m_IsDevOpened = false;
        }
        else
        {
            BTN_ResetCasualLevelData.gameObject.SetActive(true);
            BTN_ShuffleLevel.gameObject.SetActive(true);
            TXT_DevData.gameObject.SetActive(true);
            m_IsDevOpened = true;
        }
    }

    /** Destroy the current level and disable menu buttons */
    public void CloseMenu()
    {
        BTN_Continue.gameObject.SetActive(false);
        BTN_BackToMenu.gameObject.SetActive(false);
        BTN_QuitGame.gameObject.SetActive(false);
        m_IsMenuOpened = false;
        BTN_ResetCasualLevelData.gameObject.SetActive(false);
        m_IsDevOpened = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /** Return image with white pixels 2x2 */
    public Sprite GetNoStarsEarnedSprite() { return m_NoStarsEarnedSprite; }
    /** Return image with one star */
    public Sprite GetOneStarSprite() { return m_OneStarSprite; }
    /** Return image with two stars */
    public Sprite GetTwoStarsSprite() { return m_TwoStarSprite; }
    /** Return image with three stars */
    public Sprite GetThreeStarsSprite() { return m_ThreeStarSprite; }
    /** Return image "level is lock" */
    public Sprite GetLevelIsLockSprite() { return m_LevelIsLocked; }
    /** Return image "level is unlock" */
    public Sprite GetLevelIsUnlockedSprite() { return m_LevelIsUnlocked; }




    // DEV MENU STUFF
    
    public void DEVResetCasualLevelData()
    {
        // ie. also resets difficulty
        mgc.CasualLevel = 0;
        mgc.StopMe();
        lvlmgr.SetPlayerCasualLevel(0);
        lvlmgr.LoadPlayerCausalLevel();
        lvlmgr.SetPlayerCasualScore(0);
        lvlmgr.LoadPlayerCasualScore();
    }

    public void DEVShuffleLevel()
    {
        mgc.GameOver(false);
    }

    void Update()
    {
        // just debug stuff we can see onscreen
        if (m_IsDevOpened)
        {
            TXT_DevData.text = "Ball\nHeight: " + mgc.BallHeight + "\nYVelocity: " + mgc.CurrentBallVelocity.y + "\nMaxYVelocty: " + mgc.BallMaxVelocity + "\nGravity: " + mgc.Gravity + "\nBallFalling: " + mgc.BallFalling + "\n" + "\nTower\nAngle: " + mgc.TowerAngle + "\nTotalTiers: " + mgc.TiersPerLevel +
                "\nCurrentTier: " + mgc.CurrentTier + "\n" + "\nGame\nisRunning: " + mgc.GameRunning + "\nTime: " + mgc.CurrentGameTime + "\nGameMode: " + mgc.CurrentGameMode + "\nMaxDiffLvl+: " + mgc.LevelSpanForZeroTo100Percent + "\nTierPool: " + mgc.PercentOfPossibleTiersInPool + "\n" + "\n" + "\nChambawamba\nApproved";
        }
    }

}
