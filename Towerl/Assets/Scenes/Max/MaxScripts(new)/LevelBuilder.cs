using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {

    public TierData TD;
    private MGC Controller;
    private int Level;
    private int TierCount;

    public Transform Column;
    public Transform Seg15;

	// Use this for initialization
	void Start () {

        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
        Level = Controller.CurrentLevel;
        TierCount = Controller.TiersPerLevel;

        MakeColumn();
        MakeTier(5,35,0f);
        MakeTier(4, 36, 90f);

    }

    private void MakeColumn ()
    {
        // make column (and Apply Controller scale factors)
        Transform clone = (Transform)Instantiate(Column, new Vector3(0f, (float)Controller.TiersPerLevel * Controller.TierHeight / 2, 0), Quaternion.identity);
        clone.transform.localScale = Vector3.Scale(clone.transform.localScale, Controller.ColumnScale);
    }

    private void MakeTier (int Height, int TierCode, float Rotation)
    {

        int[] data = TD.GetTierData(TierCode);

        // instantiate primary section = this will have the Tag and control script
        Transform Seg0 = (Transform)Instantiate(Seg15, new Vector3(0, Height, 0), Quaternion.Euler(0, Rotation, 0));
        Seg0.gameObject.tag = Height.ToString();
        //Seg0.transform.localScale = Vector3.Scale(Seg0.transform.localScale, Controller.SegmentScale);
        Seg0.transform.localScale = Controller.SegmentScale;
        Seg0.gameObject.AddComponent<TierScript>();
        Seg0.gameObject.GetComponent<TierScript>().myData = data;
        Seg0.gameObject.GetComponent<TierScript>().rotation = Rotation;

        for (int i = 1; i < 24; i++)
        {
            if (data[i] > 0)
            {
            Transform segClone = (Transform)Instantiate(Seg15, new Vector3(0, Height, 0), Quaternion.Euler(0, (i * 15) + Rotation, 0));
            segClone.transform.localScale = Controller.SegmentScale;
                if (data[i] == 2)
                {
                    // can't paint it
                    //segClone.gameObject.GetComponentsInChildren<MeshRenderer>().Materials[0].color = Color.red;
                    segClone.transform.localScale = Vector3.Scale(segClone.transform.localScale, new Vector3(1f,1.1f,1f));
                }
            segClone.transform.parent = Seg0.transform;
            }
        }
    }
	
}
