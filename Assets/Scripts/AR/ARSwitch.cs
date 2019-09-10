using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSwitch : MonoBehaviour
{
    [SerializeField] GameObject regularCamera;
    [Space]
    [SerializeField] GameObject session;
    [SerializeField] GameObject sessionOrigin;  
    [SerializeField] GameObject arUI;
    [SerializeField] GameObject threeDObject;
    [SerializeField] GameObject arLight;
    [SerializeField] Scaler scaler;
    ARSession sessionController;
    ARSessionOrigin sessionOriginController;
    UIManager uiController;
    
    private void Start()
    {
        sessionController = session.GetComponent<ARSession>();
        sessionOriginController = sessionOrigin.GetComponent<ARSessionOrigin>();
        uiController = arUI.GetComponent<UIManager>();
    }

    public void Toggle()
    {
        if (session.gameObject.activeSelf)
        {
            DisableAR();
        }
        else
        {
            EnableAR();
        }
    }

    public void EnableAR()
    {
        session.SetActive(true);
        sessionOrigin.SetActive(true);
        arUI.SetActive(true);
        regularCamera.SetActive(false);
        threeDObject.SetActive(false);
        arLight.SetActive(true);
    }

    public void DisableAR()
    {
        session.SetActive(false);
        sessionOrigin.SetActive(false);
        arUI.SetActive(false);
        uiController?.Reset();
        regularCamera.SetActive(true);
        threeDObject.SetActive(true);
        arLight.SetActive(false);
        
        sessionOrigin.GetComponent<PlaceOnPlane>().Reset();
    }
}
