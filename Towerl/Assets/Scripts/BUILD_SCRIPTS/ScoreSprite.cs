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
// ScoreSprite.cs                       //
//////////////////////////////////////////
using UnityEngine;

public class ScoreSprite : MonoBehaviour {

    private float time = 0f;
    private float scale = 1f;
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;

        if (time >= 0.8f) Destroy(this.gameObject);
        else
        {
            scale += 0.0007f;
            transform.localScale = transform.localScale * scale;
        }
	}
}
