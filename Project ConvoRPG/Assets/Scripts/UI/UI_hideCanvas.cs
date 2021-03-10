using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_hideCanvas : MonoBehaviour
{
    public void hideCanvas(GameObject oldCanvas)
    {
        oldCanvas.SetActive(false);
    }
    
    public void showCanvas(GameObject newCanvas)
    {
        newCanvas.SetActive(true);
    }
}
