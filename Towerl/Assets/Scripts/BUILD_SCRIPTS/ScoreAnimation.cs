using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAnimation : MonoBehaviour {

    private float animationTime = 1f;
    private float initialNumber, desiredNumber;
    private bool isInit;
	
	// Update is called once per frame
	void Update ()
    {
		if (initialNumber != desiredNumber && isInit == false)
        {
            initialNumber += (animationTime * Time.deltaTime) * (desiredNumber - initialNumber);
            gameObject.GetComponent<Text>().text = initialNumber.ToString("0");
            if (initialNumber >= desiredNumber) initialNumber = desiredNumber;
        }
        if (isInit) gameObject.GetComponent<Text>().text = initialNumber.ToString("0");
    }

    public void SetNumber(int previousScore, int currentScore, bool localBool)
    {
        initialNumber = previousScore;
        desiredNumber = currentScore;
        isInit = localBool;
    }
}
