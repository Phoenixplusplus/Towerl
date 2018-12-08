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
// EarnedSprite.cs                      //
//////////////////////////////////////////

using UnityEngine;

public class EarnedSprite : MonoBehaviour
{
    public float time = 0f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.8f) Destroy(this.gameObject);
    }
}
