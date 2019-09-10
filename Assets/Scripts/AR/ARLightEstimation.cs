using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARCameraManager))]
public class ARLightEstimation : MonoBehaviour
{
    [SerializeField] Light lightToAffect;
    [SerializeField] ARCameraManager cameraManager;

    private void Start()
    {
        if (!cameraManager)
            cameraManager = GetComponentInChildren<ARCameraManager>(true);
    }

    private void OnEnable()     
    {
        cameraManager.frameReceived += AcceptLightEstimation;
    }

    private void OnDisable() 
    {
        cameraManager.frameReceived -= AcceptLightEstimation;
    }

    void AcceptLightEstimation(ARCameraFrameEventArgs arguments)
    {
        lightToAffect.intensity = (float)arguments.lightEstimation.averageBrightness;
        lightToAffect.color = (Color)arguments.lightEstimation.colorCorrection;
    }
}
