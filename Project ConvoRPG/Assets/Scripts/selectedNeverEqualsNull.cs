using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class selectedNeverEqualsNull : MonoBehaviour
{
    GameObject lastSelectedObject;
    // Start is called before the first frame update
    void Start()
    {
        lastSelectedObject = EventSystem.current.currentSelectedGameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject != lastSelectedObject)
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(lastSelectedObject);
            }
            else 
            {
                lastSelectedObject = EventSystem.current.currentSelectedGameObject;
            }
        }
    }
}
