using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfettiScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        transform.position = new Vector3(720, 1480, 0);
        GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.up * Time.deltaTime * 700;
        if (GameObject.Find("MGC").GetComponent<MGC>().isAnimating == false) Destroy(gameObject);
	}
}
