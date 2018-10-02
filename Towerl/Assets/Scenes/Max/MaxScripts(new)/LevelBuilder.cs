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

        for (int i = 0; i < TierCount ; i++)
        {
            MakeTier(i, Random.Range(0, TD.GetPossibleTierCount() - 1), Random.Range(-180f, 180f));
        }
    }

    private void MakeColumn ()
    {
        // make column (and Apply Controller scale factors)
        Transform clone = (Transform)Instantiate(Column, new Vector3(0f, (float)Controller.TiersPerLevel / 2, 0), Quaternion.identity);
        clone.transform.localScale = Vector3.Scale(clone.transform.localScale, Controller.ColumnScale);


    }

    private void MakeTier (int Height, int TierCode, float Rotation)
    {

        int[] data = TD.GetTierData(TierCode);

        // instantiate primary section = this will have the Tag and control script
        // IMPORTANT ... Segment 0 ALWAYS should be 1 (a solid vanilla platform)
        Transform Seg0 = (Transform)Instantiate(Seg15, new Vector3(0, Height, 0), Quaternion.Euler(0, Rotation, 0));
        Seg0.gameObject.tag = Height.ToString();
        Seg0.transform.localScale = Controller.SegmentScale;
        Seg0.gameObject.AddComponent<TierScript>();
        Seg0.gameObject.GetComponent<TierScript>().myData = data;
        Seg0.gameObject.GetComponent<TierScript>().rotation = Rotation;
        Seg0.gameObject.GetComponentsInChildren<Renderer>()[0].material.color = Color.green;

        for (int i = 1; i < 24; i++)
        {
            if (data[i] > 0)
            {
            Transform segClone = (Transform)Instantiate(Seg15, new Vector3(0, Height, 0), Quaternion.Euler(0, (i * 15) + Rotation, 0));
            segClone.transform.localScale = Controller.SegmentScale;
            segClone.gameObject.GetComponentsInChildren<Renderer>()[0].material.color = Color.green;
                if (data[i] == 2)
                {
                    segClone.transform.localScale = Vector3.Scale(segClone.transform.localScale, new Vector3(1f,1.1f,1f));
                    segClone.gameObject.GetComponentsInChildren<Renderer>()[0].material.color = Color.red;
                }
            segClone.transform.parent = Seg0.transform;
            }
        }
    }
	
}
