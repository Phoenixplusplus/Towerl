using UnityEngine;
using UnityEngine.UI;

public class SM_ButtonLevel : MonoBehaviour {

    public int level;
    private Image m_StarsImage;
    private Image m_ButtonImage;

    // Use this for initialization
    void Start ()
    {
        Transform GO = this.gameObject.transform.GetChild(0);
        m_StarsImage = GO.GetComponent<Image>();
        m_ButtonImage = GetComponent<Image>();

        // Store the star data
        int levelStars = LevelManager.Instance.GetLevelStars(level);

        /** Increase earned stars for specifics scene */
        if (level < 10) LevelManager.Instance.userInterface.starsEarnedThemeOne += levelStars;
        else if (level > 9 && level < 20) LevelManager.Instance.userInterface.starsEarnedThemeTwo += levelStars;
        else if (level > 19 && level < 30) LevelManager.Instance.userInterface.starsEarnedThemeThree += levelStars;


        /** Increase total earned stars */
        LevelManager.Instance.userInterface.starsEarnedTotal += levelStars;

        /** If the level is unlocked, and player has earned some stars before, change the sprite */
        if (LevelManager.Instance.GetLevelLockStatus(level) == 1)
        {
            switch (levelStars)
            {
                case 0: /** Set sprite to sprite without any stars */
                    m_StarsImage.sprite = LevelManager.Instance.userInterface.GetNoStarsEarnedSprite(); break;
                case 1: /** Set sprite to sprite with one star */
                    m_StarsImage.sprite = LevelManager.Instance.userInterface.GetOneStarSprite(); break;
                case 2: /** Set sprite to sprite with two stars */
                    m_StarsImage.sprite = LevelManager.Instance.userInterface.GetTwoStarsSprite(); break;
                case 3: /** Set sprite to sprite with three stars */
                    m_StarsImage.sprite = LevelManager.Instance.userInterface.GetThreeStarsSprite(); break;
            }

            /** Activate button and load image "level is unlocked" */
            //m_ButtonImage.sprite = LevelManager.Instance.userInterface.GetLevelIsUnlockedSprite();
            GetComponent<Button>().enabled = true;
        }
        else
        {
            /** Deactivate button and load image "level is locked" */
            //m_ButtonImage.sprite = LevelManager.Instance.userInterface.GetLevelIsLockSprite();
            GetComponent<Button>().interactable = false;
        }
    }

    public void UpdateStars()
    {
        // Store the star data
        int levelStars = LevelManager.Instance.GetLevelStars(level);

        switch (levelStars)
        {
            case 1: /** Set the first button position */
                m_StarsImage.sprite = LevelManager.Instance.userInterface.GetOneStarSprite(); break;
            case 2: /** Set the last button position */
                m_StarsImage.sprite = LevelManager.Instance.userInterface.GetTwoStarsSprite(); break;
            case 3: /** Set the buttons positions which are between buttons, these buttons are not position on the top or bottom */
                m_StarsImage.sprite = LevelManager.Instance.userInterface.GetThreeStarsSprite(); break;
        }

        /** Activate button and load image "level is unlocked" */
        //m_ButtonImage.sprite = LevelManager.Instance.userInterface.GetLevelIsUnlockedSprite();
        GetComponent<Button>().enabled = true;
        GetComponent<Button>().interactable = true;
    }
}
