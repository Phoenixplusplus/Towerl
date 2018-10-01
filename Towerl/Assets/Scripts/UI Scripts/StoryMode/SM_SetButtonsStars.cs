using UnityEngine.UI;
using UnityEngine;

public class SM_SetButtonsStars : MonoBehaviour {
    
    /** Store the level value */
    public int level;

    /** Image reference */
    private Image m_Image;


    void Start()
    {
        //Fetch the Image from the GameObject
        m_Image = GetComponent<Image>();

        // Store the sta
        int levelStars = LevelManager.Instance.GetLevelStars(level - 1);

        switch (levelStars)
        {
            case 1: /** Set the first button position */
                m_Image.sprite = LevelManager.Instance.GetOneStarSprite(); break;
            case 2: /** Set the last button position */
                m_Image.sprite = LevelManager.Instance.GetTwoStarsSprite(); break;
            case 3: /** Set the buttons positions which are between buttons, these buttons are not position on the top or bottom */
                m_Image.sprite = LevelManager.Instance.GetThreeStarsSprite(); break;
        }

    }

    void Update()
    {

    }


}
	

