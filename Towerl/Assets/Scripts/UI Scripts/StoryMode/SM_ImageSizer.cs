using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_ImageSizer : MonoBehaviour
{
    /** Store button location */
    public string buttonName;
    /** Store image positions between images, store number from 1 to 3 */
    public int buttonPosition;
    /** Store the button off set, use to calculate button position */
    private float heightOffSet;

    /** Use this for initialization */
    void Start ()
    {
        /** Get object references */
        GameObject theBar = GameObject.Find(buttonName);
        var theBarRectTransform = theBar.transform as RectTransform;

        /** Check which button is it 
         * If it is one of the scenes button, set the button size on the 32% of the screen size
         * If it is back to menu button, set the button size on the 10% of the screen size */
        if (buttonPosition != 4)  heightOffSet = (Screen.height / 100.0f) * 31.0f;
        else  heightOffSet = (Screen.height / 100.0f) * 10.0f;

        /** Set the image size, base on the calculation before */
        theBarRectTransform.sizeDelta = new Vector2(Screen.width, heightOffSet);

        switch (buttonPosition)
        {
            case 1: /** Set the first button position */
                theBarRectTransform.localPosition -= new Vector3(0.0f, (theBarRectTransform.sizeDelta.y / 2.0f), 0.0f);
                break;
            case 4: /** Set the last button position */
                theBarRectTransform.localPosition += new Vector3(0.0f, (theBarRectTransform.sizeDelta.y / 2.0f), 0.0f);
                break;
            default: /** Set the buttons positions which are between buttons, these buttons are not position on the top or bottom */
                theBarRectTransform.localPosition += new Vector3(0.0f, ((theBarRectTransform.sizeDelta.y / 2.0f) - (theBarRectTransform.sizeDelta.y * buttonPosition)), 0.0f);
                break;
        }
    }
}
