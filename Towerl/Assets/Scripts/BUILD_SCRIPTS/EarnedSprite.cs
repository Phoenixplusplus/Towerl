using System.Collections;
using System.Collections.Generic;
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
