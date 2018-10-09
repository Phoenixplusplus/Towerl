using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {

    private TierData TD = new TierData();
    private MGC Controller;
    private int Level;
    private int TierCount;

    public Transform Column;
    public Transform Seg15;

    // 268 colours, 134 combinations
    // even number is safe colour, odd number is hazard colour
    private string[] hexColours = { "#000000", "#FFFF00", "#1CE6FF", "#FF34FF", "#FF4A46", "#008941", "#006FA6", "#A30059", "#FFDBE5", "#7A4900", "#0000A6", "#63FFAC", "#B79762", "#004D43", "#8FB0FF", "#997D87", "#5A0007", "#809693", "#FEFFE6", "#1B4400", "#4FC601", "#3B5DFF", "#4A3B53", "#FF2F80", "#61615A", "#BA0900", "#6B7900", "#00C2A0", "#FFAA92", "#FF90C9", "#B903AA", "#D16100", "#DDEFFF", "#000035", "#7B4F4B", "#A1C299", "#300018", "#0AA6D8", "#013349", "#00846F", "#372101", "#FFB500", "#C2FFED", "#A079BF", "#CC0744", "#C0B9B2", "#C2FF99", "#001E09", "#00489C", "#6F0062", "#0CBD66", "#EEC3FF", "#456D75", "#B77B68", "#7A87A1", "#788D66", "#885578", "#FAD09F", "#FF8A9A", "#D157A0", "#BEC459", "#456648", "#0086ED", "#886F4C", "#34362D", "#B4A8BD", "#00A6AA", "#452C2C", "#636375", "#A3C8C9", "#FF913F", "#938A81", "#575329", "#00FECF", "#B05B6F", "#8CD0FF", "#3B9700", "#04F757", "#C8A1A1", "#1E6E00", "#7900D7", "#A77500", "#6367A9", "#A05837", "#6B002C", "#772600", "#D790FF", "#9B9700", "#549E79", "#FFF69F", "#201625", "#72418F", "#BC23FF", "#99ADC0", "#3A2465", "#922329", "#5B4534", "#FDE8DC", "#404E55", "#0089A3", "#CB7E98", "#A4E804", "#324E72", "#6A3A4C", "#83AB58", "#001C1E", "#D1F7CE", "#004B28", "#C8D0F6", "#A3A489", "#806C66", "#222800", "#BF5650", "#E83000", "#66796D", "#DA007C", "#FF1A59", "#8ADBB4", "#1E0200", "#5B4E51", "#C895C5", "#320033", "#FF6832", "#66E1D3", "#CFCDAC", "#D0AC94", "#7ED379", "#012C58", "#7A7BFF", "#D68E01", "#353339", "#78AFA1", "#FEB2C6", "#75797C", "#837393", "#943A4D", "#B5F4FF", "#D2DCD5", "#9556BD", "#6A714A", "#001325", "#02525F", "#0AA3F7", "#E98176", "#DBD5DD", "#5EBCD1", "#3D4F44", "#7E6405", "#02684E", "#962B75", "#8D8546", "#9695C5", "#E773CE", "#D86A78", "#3E89BE", "#CA834E", "#518A87", "#5B113C", "#55813B", "#E704C4", "#00005F", "#A97399", "#4B8160", "#59738A", "#FF5DA7", "#F7C9BF", "#643127", "#513A01", "#6B94AA", "#51A058", "#A45B02", "#1D1702", "#E20027", "#E7AB63", "#4C6001", "#9C6966", "#64547B", "#97979E", "#006A66", "#391406", "#F4D749", "#0045D2", "#006C31", "#DDB6D0", "#7C6571", "#9FB2A4", "#00D891", "#15A08A", "#BC65E9", "#FFFFFE", "#C6DC99", "#203B3C", "#671190", "#6B3A64", "#F5E1FF", "#FFA0F2", "#CCAA35", "#374527", "#8BB400", "#797868", "#C6005A", "#3B000A", "#C86240", "#29607C", "#402334", "#7D5A44", "#CCB87C", "#B88183", "#AA5199", "#B5D6C3", "#A38469", "#9F94F0", "#A74571", "#B894A6", "#71BB8C", "#00B433", "#789EC9", "#6D80BA", "#953F00", "#5EFF03", "#E4FFFC", "#1BE177", "#BCB1E5", "#76912F", "#003109", "#0060CD", "#D20096", "#895563", "#29201D", "#5B3213", "#A76F42", "#89412E", "#1A3A2A", "#494B5A", "#A88C85", "#F4ABAA", "#A3F3AB", "#00C6C8", "#EA8B66", "#958A9F", "#BDC9D2", "#9FA064", "#BE4700", "#658188", "#83A485", "#453C23", "#47675D", "#3A3F00", "#061203", "#DFFB71", "#868E7E", "#98D058", "#6C8F7D", "#D7BFC2", "#3C3E6E", "#D83D66", "#2F5D9B", "#6C5E46", "#D25B88", "#5B656C", "#00B57F", "#545C46", "#866097", "#365D25", "#252F99", "#00CCFF", "#674E60", "#FC009C", "#92896B" };
    private int baseColour, contrastColour;

    // Use this for initialization
    void Start () {
        // Get Game Controller reference
        Controller = GameObject.Find("MGC").GetComponent<MGC>();
    }

    public void BuildRandomLevel()
    {
        if (Controller == null) // just incase we don't have a reference fron the "Start()"
        {
            Controller = GameObject.Find("MGC").GetComponent<MGC>();
        }
        Level = Controller.CurrentLevel;
        TierCount = Controller.TiersPerLevel;

        MakeColumn();

        // Get random pair of colours for Tier segments
        baseColour = PickColour();
        contrastColour = baseColour + 1;

        // Make Tiers
        // Top Tier ... Orientation -7.5 to ensure bounce on a platform
        MakeTier(TierCount-1, 1, -7.5f);
        //Remaining Middle Tiers
        for (int i = TierCount - 2; i > 0  ; i--)
        {
            MakeTier(i, Random.Range( (int)(TD.GetPossibleTierCount() * 0.75), TD.GetPossibleTierCount()), Random.Range(-180, 181));
            //MakeTier(i, 1, 0); // SAfety development tier build for debugging // CAN DELETE LATER
        }
        // Bottom "Home" Tier
        MakeTier(0, 0, 0);

        // Get Controller to make ball
        Controller.ResetBall();
    }

    public void BuildLevel(int LevelNumber)
    {

        if (Controller == null) // just incase we don't have a reference fron the "Start()"
        {
            Controller = GameObject.Find("MGC").GetComponent<MGC>();
        }

        int[] LD = TD.GetLevelData(LevelNumber);

        MakeColumn();

        // Get random pair of colours for Tier segments
        baseColour = PickColour();
        contrastColour = baseColour + 1;

        TierCount = LD.Length;
        Debug.Log("Tier Count = " + TierCount.ToString());
        // Make Tiers
        // Top Tier ... Orientation -7.5 to ensure bounce on a platform
        MakeTier((TierCount/2) + 1, 1, -7.5f);
        
        //Remaining Middle Tiers (from LD (Level Data) array) 
        for (int i = 0; i < TierCount - 1; i= i +2)
        {
            MakeTier((i/2)+1, LD[i], LD[i+1]);
        }
        // Bottom "Home" Tier
        MakeTier(0, 0, 0);

        // Get Controller to make ball
        Controller.ResetBall();
    }

    private void MakeColumn ()
    {
        // make column (and Apply Controller scale factors)
        Transform clone = (Transform)Instantiate(Column, new Vector3(0f, (float)(Controller.TiersPerLevel / 2) + ((Controller.BallScale.y) / 2), 0), Quaternion.identity);
        clone.transform.localScale = Vector3.Scale(clone.transform.localScale, Controller.ColumnScale);
        clone.gameObject.tag = "Column";
    }

    private void MakeTier (int Height, int TierCode, float Rotation)
    {
        // go get the tier data (an int [24]) from the "Tier Data" object
        int[] data = TD.GetTierData(TierCode);

        // instantiate primary section = this will have the Tag and control script (further segments are childed to it)
        // IMPORTANT ... Segment 0 ALWAYS should be 1 (a solid vanilla platform)
        Transform Seg0 = (Transform)Instantiate(Seg15, new Vector3(0, Height - (Controller.BallScale.y)/2, 0), Quaternion.Euler(0, Rotation, 0));
        Seg0.gameObject.tag = Height.ToString(); // set tag
        Seg0.transform.localScale = Controller.SegmentScale; // set scale
        Seg0.gameObject.AddComponent<TierScript>(); // add tier control script (and configure it)
        Seg0.gameObject.GetComponent<TierScript>().myData = data;
        Seg0.gameObject.GetComponent<TierScript>().rotation = Rotation;
        Color safeColour;
        ColorUtility.TryParseHtmlString(hexColours[baseColour], out safeColour);
        Seg0.gameObject.GetComponentsInChildren<Renderer>()[0].material.color = safeColour;

        // make each 15 degree segment fanning around from the "base segement .. (above)"
        for (int i = 1; i < 24; i++)
        {
            if (data[i] > 0)
            {
            Transform segClone = (Transform)Instantiate(Seg15, new Vector3(0, Height - (Controller.BallScale.y) / 2, 0), Quaternion.Euler(0, (i * 15) + Rotation, 0));
            segClone.transform.localScale = Controller.SegmentScale;
            segClone.gameObject.GetComponentsInChildren<Renderer>()[0].material.color = safeColour;
                if (data[i] == 2)
                {
                    segClone.transform.localScale = Vector3.Scale(segClone.transform.localScale, Controller.HazardScaleModifier);
                    Color hazardColour;
                    ColorUtility.TryParseHtmlString(hexColours[contrastColour], out hazardColour);
                    segClone.gameObject.GetComponentsInChildren<Renderer>()[0].material.color = hazardColour;
                }
            segClone.transform.parent = Seg0.transform;
            }
        }
    }

    private int PickColour()
    {
        int maxRange = hexColours.Length - 2;
        int r = Random.Range(0, maxRange);
        if (r % 2 != 0) r += 1;
        return r;
    }

}
