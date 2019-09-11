using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] GameObject mainObject;
    [SerializeField] float rotationSpeed = 10f;
    Vector3 rotSpeed;

    private void Update()
    {
        if (mainObject)
        {
            RotateObject();
            ApplyRotation();
        }           
    }


    void RotateObject()
    {
        if (Input.GetMouseButton(0))
        {
            rotSpeed = new Vector3(0, Input.GetAxis("Mouse X"), 0);
        }
        else if (Input.touchCount > 0)
        {
            rotSpeed = new Vector3(0, Input.GetAxis("Mouse X"), 0);
        }
    }

    void ApplyRotation()
    {
        rotSpeed = Vector3.Lerp(rotSpeed, Vector3.zero, Time.deltaTime * 5f);
        mainObject.transform.Rotate(rotSpeed * rotationSpeed, Space.World);
    }
}
