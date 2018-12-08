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
// Menu_Control.cs                      //
//////////////////////////////////////////

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Control : MonoBehaviour {

    public Transform ball;
    public float Bottom = -0.61f;
    public float MaxVel = 0.11f;
    private Vector3 vel = new Vector3(0f, 0f, 0f);
    public Vector3 G = new Vector3(0, -0.25f, 0);
    public RectTransform Title;
    public CanvasGroup Select_Menu_Canvas;
    public Camera Main_Camera;
    public Camera Select_Menu_Camera;
    public UI_Manager UI_Manager;
    public LevelManager Level_Manager;
    public MGC Controller;
    public CameraController2 CamCon2;
    public Canvas Level_Choose;
    public float PhaseInTime = 2f;
    public float StartTime = 0f;
    public float Angle = 0;
    // Variables array for Bounce Function.  Declared here for efficiency
    private float[] B = {  4.0f / 11.0f,
                           6.0f / 11.0f,
                           8.0f / 11.0f,
                           3.0f / 4.0f,
                           9.0f / 11.0f,
                           10.0f / 11.0f,
                           15.0f / 16.0f,
                           21.0f / 22.0f,
                           63.0f / 64.0f,
                           1.0f / (4.0f / 11.0f) / (4.0f / 11.0f)};
    public Image goButton;
    public Sprite casualPlay, storyPlay, quitGame;


	// Use this for initialization
	void Start () {
        Enter();
	}
	
    public void Enter()
    {
        CamTog(Main_Camera, false);
        CamTog(Select_Menu_Camera, true);
        GUITog(Select_Menu_Canvas, true);
        ball.position = new Vector3(-10f, 0.8f, -9.34f);
        vel = new Vector3(0f, 0f, 0f);
        StartCoroutine(PhaseIn());
    }

    public void Leave()
    {
        Main_Camera.enabled = true;
        Select_Menu_Camera.enabled = false;
        GUITog(Select_Menu_Canvas, false);
    }

    void CamTog(Camera cam, bool status)
    {
        cam.enabled = status;
        cam.GetComponent<AudioListener>().enabled = status;
    }

    void GUITog(CanvasGroup CG, bool status)
    {
        if (status) { CG.alpha = 1f; }
        else { CG.alpha = 0f; }
        CG.interactable = status;
    }

	// Update is called once per frame
	void Update ()
    {
        // Move Ball
        vel += G * Time.deltaTime;
        ball.position += vel;
        if (vel.y < 0 && ball.position.y < Bottom)
        {
            ball.position = new Vector3(ball.position.x, Bottom + ball.transform.localScale.y / 2, ball.position.z);
            vel.y = MaxVel;
        }
        // Process Input
        Angle -= Input.GetAxis("Horizontal") * 80 * Time.deltaTime; // Keyboard input applied to TowerAngle                                                                                       // Normalise Tower angle to something between 0-360
        if (Angle < 0f)
        {
            while (Angle < 0f) { Angle += 360f; }
        }
        if (Angle > 360f)
        {
            while (Angle > 360f) { Angle -= 360f; }
        }
        // rotate self (ie the column assembly)
        transform.localEulerAngles = new Vector3(0, Angle, 0);
        // Debug.Log(Angle.ToString());

        if (Angle >= 30f && Angle < 150f) // STORY MODE
        {
            if (goButton.sprite != storyPlay) goButton.sprite = storyPlay;
        }
        else if (Angle >= 150f && Angle < 270f) // QUIT
        {
            if (goButton.sprite != quitGame) goButton.sprite = quitGame;
        }
        else
        {
            if (goButton.sprite != casualPlay) goButton.sprite = casualPlay;
        }
    }

    public void GO()
    {
        Debug.Log("Go called");
        if (Angle >= 30f && Angle < 150f) // STORY MODE
        {
            Leave();
            Level_Choose.gameObject.SetActive(false);
            CamCon2.EnableAdventureMap(true);
        }
        else if (Angle >=150f && Angle < 270f) // QUIT
        {
            UI_Manager.QuitGame();
            Application.Quit();
        }
        else // CASUAL MODE       
        {
            Leave();
            Level_Manager.SetGameMode(0);
            Controller.PlayMe();
        }
    }


    IEnumerator PhaseIn()
    {
        while (StartTime < PhaseInTime)
        {
            StartTime += Time.deltaTime;
            float lerp = Bounce(StartTime / PhaseInTime);
            Title.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, lerp);
            yield return null;
        }
        StartTime = 0f;
        yield break;
    }

    // Thanks to https://github.com/d3/d3-ease/blob/master/src/bounce.js#L12 (24 Nov 2018)
    float Bounce(float t)
    {
        if (t < B[0]) return B[9] * t * t;
        else if (t < B[2])
            {
                t = t - B[1];
                return B[9] * t * t + B[3];
            }
        else if (t < B[5])
            {
                t = t - B[4];
                return B[9] * t * t + B[6];
            }
        else
            {
                t = t - B[7];
                return B[9] * t * t + B[8];
            }
    }


}
