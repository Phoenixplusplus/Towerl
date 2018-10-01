using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // spawn a game manager and initialise variables based on 'mode' or stuff
    void Start()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("P_Scene");
    }

}
