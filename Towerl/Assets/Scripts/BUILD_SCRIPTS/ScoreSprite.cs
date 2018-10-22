using System.Collections;
using System.Collections.Generic;
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
