using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
    }

    // Update is called once per frame
    void Update ()
    {
        // TODO: DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS
        // DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS
        // DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS DELETE THISSSS
        if (this.transform.position.z >= -3.0f && this.transform.position.z < 3.0f) this.transform.Translate(this.transform.forward * Time.deltaTime);
        else this.transform.position = new Vector3(0.0f, 1.5f, -3.0f);
    }


    void OnTriggerEnter(Collider other)
    {
        // Activate slow motion
        GameManager.Instance.ActivateSlowMotion();
        // Destroy the slow motion power up
        Destroy(this.gameObject);
    }
}
