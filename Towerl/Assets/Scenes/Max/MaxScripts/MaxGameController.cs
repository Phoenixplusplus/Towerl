using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxGameController : MonoBehaviour {


    [Header("Level Data")]
    public int levels = 5;
    // LEVEL DATA CODE
    // 0 = a gap (nothing there)
    // 1 = A Slice (30 degrees)
    // 2+ = TBA (and currently won't do shit) 
    public int[,] data = new int[5, 12]
    {
        {1,1,1,1,1,1,0,0,1,1,1,1 },
        {1,1,1,1,0,0,0,0,1,1,0,0 },
        {1,1,1,1,0,0,0,0,1,1,1,1 },
        {0,0,1,1,0,0,1,1,1,1,1,1 },
        {1,1,0,0,0,1,0,0,1,1,1,1 }
    };

    [Header("Object Scales")]
    public Vector3 SegmentScale = new Vector3(1f, 1f, 1f);
    public Vector3 ColumnScale = new Vector3(0.1f, 2.5f, 0.1f);
    public Vector3 BallScale = new Vector3(1f, 1f, 1f);
    public float TierHeight = 1f;

    [Header("Game Play Details")]
    public float BallHeight;
    public float TowerAngle;
    public int CurrentTier;
    public float Gravity = -5f;

    // --------------------//
    // establish Singelton //
    // ------------------- //
    public static MaxGameController Instance
    {
        get
        {
            return instance;
        }
    }
    private static MaxGameController instance = null;
    void Awake()
    {
        if (instance)
        {
            Debug.Log("Already a MaxGameController - going to die now .....");
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //---------------------------//
    // Finished Singelton set up //
    // --------------------------//

	// Use this for initialization
	void Start () {
        CurrentTier = levels;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
