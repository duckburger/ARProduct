using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARCameraManager))]
public class ARLightEstimation : MonoBehaviour
{
    [SerializeField] Light lightToAffect;
    [SerializeField] ARCameraManager cameraManager;

    /// <summary>
    /// The estimated brightness of the physical environment, if available.
    /// </summary>
    public float? brightness { get; private set; }

    /// <summary>
    /// The estimated color temperature of the physical environment, if available.
    /// </summary>
    public float? colorTemperature { get; private set; }

    /// <summary>
    /// The estimated color correction value of the physical environment, if available.
    /// </summary>
    public Color? colorCorrection { get; private set; }


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
        
        

        if (arguments.lightEstimation.averageBrightness.HasValue)
        {
            brightness = arguments.lightEstimation.averageBrightness.Value;
            lightToAffect.intensity = brightness.Value;
        }
            
        if (arguments.lightEstimation.colorCorrection.HasValue)
        {
            lightToAffect.color = colorCorrection.Value;
            colorCorrection = arguments.lightEstimation.colorCorrection.Value;
        }
            
    }
}
