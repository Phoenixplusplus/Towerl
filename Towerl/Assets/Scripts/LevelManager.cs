using UnityEngine;
using UnityEngine.SceneManagement;


struct LevelData
{
    public int tiersPerLevel;
    public int[] tierData;
    public float [] tierRotation;
    
    
    
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
        InitializeTierData();
    }
    

    public void PlayGame()
    {
        SceneManager.LoadScene("P_Scene");
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

    // Return 
    public int [] GetTiersData()
    {
        return m_levelData[0].tierData;
    }

    // Return 
    public float [] GetTiersRotation()
    {
        return m_levelData[0].tierRotation;
    }

    private void InitializeTierData()
    {
        // Level 1
        m_levelData[0].tiersPerLevel = 35;
        m_levelData[0].tierData = new int[] { 82, 34, 12, 82, 24, 89, 33, 69, 42, 115, 1, 58, 95, 24, 10, 105, 78, 76, 48, 42, 111, 12, 76, 92, 101, 13, 43, 35, 91, 1, 92, 11, 81 };
        m_levelData[0].tierRotation = new float[] { 156.1657f, 156.9886f, -59.06171f, 37.10887f, -154.7912f, -134.2751f, 118.9105f, -62.12183f, 168.1163f, 30.16866f, 97.36208f, 59.79043f, 108.4861f, -96.00956f, 177.42f, -50.37872f, -129.1293f, 71.90333f, 78.19261f, 3.095032f, 179.8276f, 133.8281f, 58.11171f, 166.3284f, 130.8206f, 82.6125f, 17.28053f, 115.2303f, 63.02122f, -2.419464f, 100.1263f, -152.4574f, 59.49766f };
    }
}
