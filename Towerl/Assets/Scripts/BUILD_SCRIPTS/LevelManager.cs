using UnityEngine;

public enum MODE_TYPE
{
    RANDOM_MODE,
    STORY_ONE,
    STORY_TWO,
    STORY_THREE
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
    private static LevelManager _instance;

    /** Store the count of levels*/
    private const int NUMBER_OF_LEVELS = 30;

    private int m_SelectedLevel;

    public int CurrentPlayerCasualLevelReached;

    public Canvas CNVS_gameplay;
    public Canvas CNVS_mainMenu;
    public Canvas CNVS_LevelThemeChoose;
    public Canvas CNVS_ThemeOne;
    public Canvas CNVS_ThemeTwo;
    public Canvas CNVS_ThemeThree;

    public int gameMode;

    /** Array of level datas, store highscore, stars earned for each level */
    private LevelData[] m_levelData = new LevelData[NUMBER_OF_LEVELS];

    [SerializeField]
    private Sprite m_OneStarSprite; 
    [SerializeField]
    private Sprite m_TwoStarSprite;
    [SerializeField]
    private Sprite m_ThreeStarSprite;

    public static LevelManager Instance
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

    private void GetPlayerCasualLevel ()
    {
        CurrentPlayerCasualLevelReached = PlayerPrefs.GetInt("CurrentPlayerCasualLevelReached");
    }


    public int GetPlayerCasualLvl()
    {
        return CurrentPlayerCasualLevelReached;
    }

    public int GetSelectedLevel()
    {
        return m_SelectedLevel;
    }

    public void SetGameMode(int newGameMode)
    {
        switch (newGameMode)
        {
            case (int)MODE_TYPE.RANDOM_MODE:
                gameMode = (int)MODE_TYPE.RANDOM_MODE;
                CNVS_mainMenu.gameObject.SetActive(false);
                CNVS_gameplay.gameObject.SetActive(true);
                break;
            case (int)MODE_TYPE.STORY_ONE:
                gameMode = (int)MODE_TYPE.STORY_ONE;
                CNVS_ThemeOne.gameObject.SetActive(false);
                CNVS_gameplay.gameObject.SetActive(true);
                break;
            case (int)MODE_TYPE.STORY_TWO:
                gameMode = (int)MODE_TYPE.STORY_TWO;
                CNVS_ThemeTwo.gameObject.SetActive(false);
                CNVS_gameplay.gameObject.SetActive(true);
                break;
            case (int)MODE_TYPE.STORY_THREE:
                gameMode = (int)MODE_TYPE.STORY_THREE;
                CNVS_ThemeThree.gameObject.SetActive(false);
                CNVS_gameplay.gameObject.SetActive(true);
                break;
        };

    }

    public void UpdateCanvases()
    {
        switch (gameMode)
        {
            case (int)MODE_TYPE.RANDOM_MODE:
                CNVS_gameplay.gameObject.SetActive(false);
                CNVS_mainMenu.gameObject.SetActive(true);
                break;
            case (int)MODE_TYPE.STORY_ONE:
                CNVS_gameplay.gameObject.SetActive(false);
                CNVS_ThemeOne.gameObject.SetActive(true);
                break;
            case (int)MODE_TYPE.STORY_TWO:
                CNVS_gameplay.gameObject.SetActive(false);
                CNVS_ThemeTwo.gameObject.SetActive(true);
                break;
            case (int)MODE_TYPE.STORY_THREE:
                CNVS_gameplay.gameObject.SetActive(false);
                CNVS_ThemeThree.gameObject.SetActive(true);
                break;
        }
    }

    public void SetSelectedLevel(int newLevel)
    {
        m_SelectedLevel = newLevel;
    }

    public void SetPlayerCasualLevel(int value)
    {
        PlayerPrefs.SetInt("CurrentPlayerCasualLevelReached", value);
        PlayerPrefs.Save();
    }

    public void LoadLevelData()
    {
        // Initialize
        for (int i = 0; i < NUMBER_OF_LEVELS; i++)
        {
            m_levelData[i].highScoreString = "HIGH_SCORE_LEVEL_" + i;
            m_levelData[i].starsString = "STARS_LEVEL_" + i;
            m_levelData[i].levelLockString = "IS_LEVEL_LOCK_" + i;

            m_levelData[i].stars = Random.Range(1, 4);
            //m_levelData[i].stars = PlayerPrefs.GetInt(m_levelData[i].starsString);
            m_levelData[i].highScore = PlayerPrefs.GetInt(m_levelData[i].highScoreString);
            m_levelData[i].IsUnlocked = PlayerPrefs.GetInt(m_levelData[i].levelLockString);
        }
    }

    void Awake()
    {
        _instance = this;
        LoadLevelData();
        GetPlayerCasualLevel();
    }
    
    // Set new level highscore
    public void SetLevelHighScore(int requestedLevel, int newHighScore)
    {
        PlayerPrefs.SetInt(m_levelData[requestedLevel].highScoreString, newHighScore);
        m_levelData[requestedLevel].highScore = newHighScore;
        PlayerPrefs.Save();
    }

    // Set the how many stars has been achieved in the level
    public void SetLevelStars(int requestedLevel, int starsEearned)
    {
        PlayerPrefs.SetInt(m_levelData[requestedLevel].starsString, starsEearned);
        m_levelData[requestedLevel].stars = starsEearned;
        PlayerPrefs.Save();
    }

    // Set the requested level status, whenever is locked or unlocked
    public void SetLevelLockStatus(int requestedLevel, int _IsLocked)
    {
        PlayerPrefs.SetInt(m_levelData[requestedLevel].levelLockString, _IsLocked);
        m_levelData[requestedLevel].IsUnlocked = _IsLocked;
        PlayerPrefs.Save();
    }

    // Set the requested level status, whenever is locked or unlocked
    public void SetLevelLockStatus(int requestedLevel, bool _IsLocked)
    {
        switch (_IsLocked)
        {
            case false:
                PlayerPrefs.SetInt(m_levelData[requestedLevel].levelLockString, 0);
                m_levelData[requestedLevel].IsUnlocked = 0;
                PlayerPrefs.Save();
                break;
            case true:
                PlayerPrefs.SetInt(m_levelData[requestedLevel].levelLockString, 1);
                m_levelData[requestedLevel].IsUnlocked = 1;
                PlayerPrefs.Save();
                break;
        }
    }

    // Return the highscore of requested level
    public int GetLevelHighScore(int requestedLevel)
    {
        return m_levelData[requestedLevel].highScore;
    }

    // Return the how many stars has been achieved in the level
    public int GetLevelStars(int requestedLevel)
    {
        return m_levelData[requestedLevel].stars;
    }

    // Return the requested level status, whenever is locked or unlocked
    public int GetLevelLockStatus(int requestedLevel)
    {
        return m_levelData[requestedLevel].IsUnlocked;
    }

    // Return image with one star
    public Sprite GetOneStarSprite()
    {
        return m_OneStarSprite;
    }

    // Return image with two stars
    public Sprite GetTwoStarsSprite()
    {
        return m_TwoStarSprite;
    }

    // Return image with three stars
    public Sprite GetThreeStarsSprite()
    {
        return m_ThreeStarSprite;
    }
}
