using UnityEngine;
using UnityEngine.UI;

public enum MODE_TYPE
{
    CASUAL,
    STORY_MODE_THEME_ONE,
    STORY_MODE_THEME_TWO,
    STORY_MODE_THEME_THREE
}

public enum SKIN_TYPE
{
    CASUAL,
    TREE,
    ROCK,
    NEON
}

struct LevelData
{
    public string highScoreString;
    public string starsString;
    public string levelLockString;
    public int highScore;
    public int stars;
    public int IsUnlocked;
};

public class LevelManager : MonoBehaviour
{
    /** Level manager instance */
    private static LevelManager _instance = null;
    /** Store the count of levels*/
    private const int NUMBER_OF_LEVELS = 30;
    /** Store the selected level, used for building specific level */
    private int m_selectedLevel;
    /** Store the causal level, used for random mode*/
    private int CurrentPlayerCasualLevelReached;
    /** Game mode variable, store current game mode */
    private int m_gameMode;
    /** Skin mode variable, store current skin mode */
    private int m_skinType;
    /** Array of level datas, store highscore, stars earned for each level */
    private LevelData[] m_levelData = new LevelData[NUMBER_OF_LEVELS];
    /** Userinterface Manager*/
    public UI_Manager userInterface;
    /** Create singleton */
    public static LevelManager Instance
    {
        get
        {
            // If we have not created one GameManager yet, create one
            if (_instance == null)
            {
                GameObject go = new GameObject("LevelManager");
                go.AddComponent<LevelManager>();
            }
            return _instance;
        }
    }
    /** For Score Text animation **/
    public Transform scoreText;
    private ScoreAnimation scoreAnimation;

    void Awake()
    {
        _instance = this;
        scoreText = userInterface.CNVS_gameplay.gameObject.transform.Find("Score");
        scoreAnimation = userInterface.CNVS_gameplay.gameObject.transform.Find("Score").GetComponent<ScoreAnimation>();
        LoadLevelData();
        LoadPlayerCausalLevel();
        //
        userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground").gameObject.SetActive(false);
        userInterface.CNVS_gameplay.gameObject.transform.Find("Score").gameObject.SetActive(false);
    }

    /// ////////////////////////////////
    // GET/SET Player CASUAL Mode stats
    public void LoadPlayerCausalLevel()
    {
        CurrentPlayerCasualLevelReached = PlayerPrefs.GetInt("CurrentPlayerCasualLevelReached");
    }

    public int GetPlayerCasualLevel()
    {
        return CurrentPlayerCasualLevelReached;
    }

    public void SetPlayerCasualLevel(int value)
    {
        PlayerPrefs.SetInt("CurrentPlayerCasualLevelReached", value);
        PlayerPrefs.Save();
    }

    public int LoadPlayerCasualScore() { return PlayerPrefs.GetInt("PlayerCasualScore"); }

    public void SetPlayerCasualScore(int score)
    {
        PlayerPrefs.SetInt("PlayerCasualScore", score);
        PlayerPrefs.Save();
    }
    // End of CASUAL mode get/sets
    /// //////////////////////////

    /** Set the game mode, this function is always called from UI Button.Click functions, 
     *  When player click on levels from range 1 to 30, it always assign the store */
    public void SetGameMode(int newGameMode)
    {
        switch (newGameMode)
        {
            case (int)MODE_TYPE.CASUAL:
                m_gameMode = (int)MODE_TYPE.CASUAL;
                m_skinType = (int)SKIN_TYPE.CASUAL;
                GameObject.Find("MGC").GetComponent<MGC>().SkinType = m_skinType;
                userInterface.CNVS_mainMenu.gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.SetActive(true);
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground").gameObject.SetActive(true);
                userInterface.CNVS_gameplay.gameObject.transform.Find("Score").gameObject.SetActive(true);
                break;
            case (int)MODE_TYPE.STORY_MODE_THEME_ONE:
                m_gameMode = (int)MODE_TYPE.STORY_MODE_THEME_ONE;
                m_skinType = (int)SKIN_TYPE.TREE;
                GameObject.Find("MGC").GetComponent<MGC>().SkinType = m_skinType;
                userInterface.CNVS_ThemeOne.gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.SetActive(true);
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground").gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.transform.Find("Score").gameObject.SetActive(true);
                break;
            case (int)MODE_TYPE.STORY_MODE_THEME_TWO:
                m_gameMode = (int)MODE_TYPE.STORY_MODE_THEME_TWO;
                m_skinType = (int)SKIN_TYPE.ROCK;
                GameObject.Find("MGC").GetComponent<MGC>().SkinType = m_skinType;
                userInterface.CNVS_ThemeTwo.gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.SetActive(true);
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground").gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.transform.Find("Score").gameObject.SetActive(true);
                break;
            case (int)MODE_TYPE.STORY_MODE_THEME_THREE:
                m_gameMode = (int)MODE_TYPE.STORY_MODE_THEME_THREE;
                m_skinType = (int)SKIN_TYPE.NEON;
                GameObject.Find("MGC").GetComponent<MGC>().SkinType = m_skinType;
                userInterface.CNVS_ThemeThree.gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.SetActive(true);
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground").gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.transform.Find("Score").gameObject.SetActive(true);
                break;
        };

    }

    /** Function is always called when player goes back from gameplay to menu 
     *  If player played Causal Mode, it always return him into MainMenu screen
     *  If player played Story Mode, it always return him into Story theme which he has chosen to play */
    public void UpdateCanvases()
    {
        switch (m_gameMode)
        {
            case (int)MODE_TYPE.CASUAL:
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_MenuBackground").GetComponent<RectTransform>().anchoredPosition = new Vector2(844.47f, 0.46021f); // needs to be reset before setting active to false
                //userInterface.CNVS_gameplay.gameObject.SetActive(false);
                userInterface.CNVS_mainMenu.gameObject.SetActive(true);
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground").gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.transform.Find("Score").gameObject.SetActive(false);
                GameObject.Find("Main Camera").GetComponent<CameraController2>().EnableAdventureMap(false);
                break;
            case (int)MODE_TYPE.STORY_MODE_THEME_ONE:
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_MenuBackground").GetComponent<RectTransform>().anchoredPosition = new Vector2(844.47f, 0.46021f);
                //userInterface.CNVS_gameplay.gameObject.SetActive(false);
                //userInterface.CNVS_ThemeOne.gameObject.SetActive(true);
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground").gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.transform.Find("Score").gameObject.SetActive(false);
                //GameObject.Find("Main Camera").GetComponent<CameraController2>().EnableAdventureMap(true);
                break;
            case (int)MODE_TYPE.STORY_MODE_THEME_TWO:
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_MenuBackground").GetComponent<RectTransform>().anchoredPosition = new Vector2(844.47f, 0.46021f);
                //userInterface.CNVS_gameplay.gameObject.SetActive(false);
                //userInterface.CNVS_ThemeTwo.gameObject.SetActive(true);
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground").gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.transform.Find("Score").gameObject.SetActive(false);
                //GameObject.Find("Main Camera").GetComponent<CameraController2>().EnableAdventureMap(true);
                break;
            case (int)MODE_TYPE.STORY_MODE_THEME_THREE:
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_MenuBackground").GetComponent<RectTransform>().anchoredPosition = new Vector2(844.47f, 0.46021f);
                //userInterface.CNVS_gameplay.gameObject.SetActive(false);
                //userInterface.CNVS_ThemeThree.gameObject.SetActive(true);
                userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground").gameObject.SetActive(false);
                userInterface.CNVS_gameplay.gameObject.transform.Find("Score").gameObject.SetActive(false);
                //GameObject.Find("Main Camera").GetComponent<CameraController2>().EnableAdventureMap(true);
                break;
        }
    }

    // Grab variables needed for updating ingame UI below
    public void ChangeProgressBar(float amount, int currentLevel, bool isInit)
    {
        Image progressBar;
        Text thisLevelText;
        Text nextLevelText;
        Image thisLevelImage;
        Image nextLevelImage;

        progressBar = userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground/IMG_ProgressBar").GetComponent<Image>();
        progressBar.fillAmount = amount;

        thisLevelText = userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground/IMG_ProgressBar/ThisLevel/Text").GetComponent<Text>();
        thisLevelText.text = currentLevel.ToString();

        nextLevelText = userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground/IMG_ProgressBar/NextLevel/Text").GetComponent<Text>();
        nextLevelText.text = (currentLevel + 1).ToString();

        // don't change colour of background on leve linitialise, but change thereafter (ie, when player gets through a tier)
        if (!isInit)
        {
            thisLevelImage = userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground/IMG_ProgressBar/ThisLevel").GetComponent<Image>();
            thisLevelImage.color = progressBar.color;
        }
        else
        {
            thisLevelImage = userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground/IMG_ProgressBar/ThisLevel").GetComponent<Image>();
            thisLevelImage.color = Color.white;

            nextLevelImage = userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground/IMG_ProgressBar/NextLevel").GetComponent<Image>();
            nextLevelImage.color = Color.white;
        }

        if (amount > 0.95f)
        {
            nextLevelImage = userInterface.CNVS_gameplay.gameObject.transform.Find("IMG_ProgressBarBackground/IMG_ProgressBar/NextLevel").GetComponent<Image>();
            nextLevelImage.color = progressBar.color;
        }
    }

    public void ChangeScore(int previousScore, int currentScore, bool isInit) { scoreAnimation.SetNumber(previousScore, currentScore, isInit); }


    /** Set the selected level to new level, called when player click on the one of the thirty levels buttons or
     * when player click on play random level (causal mode) it is always set to 0 */
    public void SetSelectedLevel(int newLevel)
    {
        m_selectedLevel = newLevel;
    }


    /** Load level data from memory and initialize it to data array's variables 
     * Load data for 30 level as are:
     * 1. Highscores 
     * 2. How many stars earned for level
     * 3. If the level is unlocked or not, if player never played the level before, the following level is always locked.
     *    player always have to finish the first level to unlock the second one */
    private void LoadLevelData()
    {
        /** Loop them number of level times, and fill up strings with names
         * subsequently load data from Android/IOS device's memory */
        for (int i = 0; i < NUMBER_OF_LEVELS; i++)
        {
            m_levelData[i].highScoreString = "HIGH_SCORE_LEVEL_" + i;
            m_levelData[i].starsString = "STARS_LEVEL_" + i;
            m_levelData[i].levelLockString = "IS_LEVEL_LOCK_" + i;

            //SetLevelStars(i, 0);
           // LockLevel(i);
            m_levelData[i].stars = PlayerPrefs.GetInt(m_levelData[i].starsString);
            m_levelData[i].highScore = PlayerPrefs.GetInt(m_levelData[i].highScoreString);
            m_levelData[i].IsUnlocked = PlayerPrefs.GetInt(m_levelData[i].levelLockString);
        }
        m_levelData[0].IsUnlocked = 1;
    }

    /** Set new level highscore */
    public void SetLevelHighScore(int requestedLevel, int newHighScore)
    {
        PlayerPrefs.SetInt(m_levelData[requestedLevel].highScoreString, newHighScore);
        m_levelData[requestedLevel].highScore = newHighScore;
        PlayerPrefs.Save();
    }

    /** Set the how many stars has been achieved in the level */
    public void SetLevelStars(int requestedLevel, int starsEearned)
    {
        PlayerPrefs.SetInt(m_levelData[requestedLevel].starsString, starsEearned);
        m_levelData[requestedLevel].stars = starsEearned;
        PlayerPrefs.Save();
    }

    /** Unlock the requested level status */
    public void UnlockLevel(int requestedLevel)
    {
        PlayerPrefs.SetInt(m_levelData[requestedLevel].levelLockString, 1);
        m_levelData[requestedLevel].IsUnlocked = 1;
        PlayerPrefs.Save();
    }

    /** Lock the requested level status */
    private void LockLevel(int requestedLevel)
    {
        if (requestedLevel == 0)
        {
            PlayerPrefs.SetInt(m_levelData[requestedLevel].levelLockString, 1);
            m_levelData[requestedLevel].IsUnlocked = 1;
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt(m_levelData[requestedLevel].levelLockString, 0);
            m_levelData[requestedLevel].IsUnlocked = 0;
            PlayerPrefs.Save();
        }
    }


    /** Return the levels count */
    public int GetLevelsCount()                                 { return NUMBER_OF_LEVELS; }
    /** Return the selectedLevel variable */
    public int GetSelectedLevel()                               { return m_selectedLevel; }
    /** Return selected game mode */
    public int GetGameMode()                                    { return m_gameMode; }
    /** Return the highscore of requested level */
    public int GetLevelHighScore(int requestedLevel)            { return m_levelData[requestedLevel].highScore; }
    /** Return the how many stars has been achieved in the level */
    public int GetLevelStars(int requestedLevel)                { return m_levelData[requestedLevel].stars; }
    /** Return the requested level status, whenever is locked or unlocked */
    public int GetLevelLockStatus(int requestedLevel)           { return m_levelData[requestedLevel].IsUnlocked; }
}
