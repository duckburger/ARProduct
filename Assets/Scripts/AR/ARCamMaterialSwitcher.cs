using UnityEngine;
using UnityEngine.XR.ARFoundation;
 
[RequireComponent(typeof(ARCameraBackground))]
public class ARCamMaterialSwitcher : MonoBehaviour
{
    public Material _ARCoreBackground;
    public Material _ARKitBackground;
 
    void Start ()
    {
        ARCameraBackground arcbg = GetComponent<ARCameraBackground> ();
        arcbg.useCustomMaterial = true;
#if UNITY_IOS
        arcbg.customMaterial = _ARKitBackground;
#else
        arcbg.customMaterial = _ARCoreBackground;
#endif
    }
}
 