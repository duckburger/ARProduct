using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSwitch : MonoBehaviour
{
    [SerializeField] GameObject arObject;
    [SerializeField] GameObject threedDObject;


    public void ToggleAR()
    {
        if (arObject.activeSelf)
        {
            arObject.SetActive(false);
            threedDObject.SetActive(true);
        }
        else
        {
            arObject.SetActive(true);
            threedDObject.SetActive(false);
        }
    }
    
}
