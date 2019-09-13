using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TouchRotate : MonoBehaviour
{
    float lastXValue;
    float lastYValue;

    [SerializeField] CinemachineFreeLookZoom freelookZoom;
    private void Start()
    {
        CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
    }

    float HandleAxisInputDelegate(string axisName)
     {

        if (Input.touchCount == 2)
        {
            return 0f;
        }

        switch (axisName)
         {
 
             case "Mouse X":
              
                if (Input.touchCount == 1)
                {
                lastXValue = Input.touches[0].deltaPosition.x / 15f;
                return Input.touches[0].deltaPosition.x / 15f;
                }
                else if (Input.GetMouseButton(0))
                {
                lastXValue = -Input.GetAxis(axisName);
                return -Input.GetAxis(axisName);
                }
                 
                return 0;
 
             case "Mouse Y":
                if (Input.touchCount == 1)
                {
                lastYValue = Input.touches[0].deltaPosition.y / 15f;
                return Input.touches[0].deltaPosition.y / 15f;
                }
                else if (Input.GetMouseButton(0))
                {
                lastYValue = Input.GetAxis(axisName);
                return Input.GetAxis(axisName);
                }

                return 0;

            case "Mouse ScrollWheel":
                return Input.GetAxis(axisName);
             default:
                Debug.LogError("Input <"+axisName+"> not recognyzed.",this);
                break;
         }
 
         return 0f;
     }
}
