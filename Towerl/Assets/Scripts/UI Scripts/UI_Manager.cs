using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{ 
    private bool m_IsMenuOpened = false;
    public Button BTN_Continue;
    public Button BTN_BackToMenu;
    public Button BTN_QuitGame;

    public void OpenOrCloseMenu()
    {
        /** If menu is not opened, open it */
        if (!m_IsMenuOpened)
        {
            BTN_Continue.gameObject.SetActive(true);
            BTN_BackToMenu.gameObject.SetActive(true);
            BTN_QuitGame.gameObject.SetActive(true);
            m_IsMenuOpened = true;
        }
        else /** If menu is opened, close it */
        {
            BTN_Continue.gameObject.SetActive(false);
            BTN_BackToMenu.gameObject.SetActive(false);
            BTN_QuitGame.gameObject.SetActive(false);
            m_IsMenuOpened = false;
        }
    }

    /** Destroy the current level and disable menu buttons */
    public void BackToMainMenu()
    {
        MGC.Instance.StopMe();
        BTN_Continue.gameObject.SetActive(false);
        BTN_BackToMenu.gameObject.SetActive(false);
        BTN_QuitGame.gameObject.SetActive(false);
        m_IsMenuOpened = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
