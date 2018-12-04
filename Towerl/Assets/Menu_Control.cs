using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Control : MonoBehaviour {

    public Transform ball;
    public float Bottom = -0.61f;
    public float MaxVel = 0.11f;
    private Vector3 vel = new Vector3(0, 0.1f, 0);
    public Vector3 G = new Vector3(0, -0.25f, 0);
    public RectTransform Title;
    public float PhaseInTime = 2f;
    public float StartTime = 0f;
    private float Angle = 0;


	// Use this for initialization
	void Start () {
        StartCoroutine(PhaseIn());
	}
	
	// Update is called once per frame
	void Update ()
    {
        vel += G * Time.deltaTime;
        ball.position += vel;
        if (vel.y < 0 && ball.position.y < Bottom)
        {
            ball.position = new Vector3(ball.position.x, Bottom + ball.transform.localScale.y / 2, ball.position.z);
            vel.y = MaxVel;
        }
	}

    IEnumerator PhaseIn()
    {
        while (StartTime < PhaseInTime)
        {
            StartTime += Time.deltaTime;
            //float lerp = EaseLibSharp.EaseLibSharp.Bounce(StartTime / PhaseInTime);
            //Title.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, lerp);
            yield return null;
        }
        yield break;
    }
}
