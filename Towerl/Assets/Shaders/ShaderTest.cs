using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderTest : MonoBehaviour {

    private float m_glowValue;
    private Renderer rend;

    // Use this for initialization
    void Start ()
    {
       rend = GetComponent<Renderer>();
        m_glowValue = 0.0f;
       //rend.material.GetFloat("_Shininess");
    }
	
	// Update is called once per frame
	void Update ()
    {

        m_glowValue += Time.deltaTime;
      //  rend.material.SetFloat("_Test", Mathf::Sin
      //      Mathm_glowValue);

        Debug.Log(m_glowValue);

    }
}
