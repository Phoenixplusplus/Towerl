using System.Collections;
using System.Collections.Generic;
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
    private float Angle = 0;


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
            float lerp = EaseLibSharp.EaseLibSharp.Bounce(StartTime / PhaseInTime);
            Title.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, lerp);
            yield return null;
        }
        StartTime = 0f;
        yield break;
    }
}
