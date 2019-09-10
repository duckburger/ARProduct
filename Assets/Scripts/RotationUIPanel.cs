using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class RotationUIPanel : MonoBehaviour
{
    [SerializeField] float disappearAfterSec = 3.0f;
    [SerializeField] float rotationSpeed;
    [Space]
    [SerializeField] Camera mainCam;
    [SerializeField] BoxCollider arrowsCollider;
    GameObject trackedObject;
    Collider trackedCollider;
    RectTransform rect;
    CanvasGroup canvasGroup;
    float timer = 0; 
    Vector3 rotationChange;
    bool isOn = true;
    [HideInInspector] public bool isRotating = false;

    private void Start()
    {
        trackedObject = transform.parent.gameObject;
        trackedCollider = trackedObject.GetComponent<Collider>();
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();  

        if (!arrowsCollider)
            arrowsCollider = GetComponent<BoxCollider>();    
            
        SizeToCollider();        
    }

    private void OnEnable() 
    {
        // Will only work if the object is spawned with AR enabled
        ARSessionOrigin sessionOrigin = FindObjectOfType<ARSessionOrigin>();
        if (sessionOrigin)
        {
            mainCam = sessionOrigin.camera;
        }
    }

    private void Update()
    {
        CheckRaycast();
    }

    void CheckRaycast()
    {
        if (!mainCam)
            return;

        if (!isOn)
            return;

        Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButton(0) && Physics.Raycast(camRay, out RaycastHit hit))
        {
            if (hit.collider == arrowsCollider)
            {
                // Hit this arrows
                rotationChange = new Vector3(trackedObject.transform.eulerAngles.x, -Input.GetAxis("Mouse X"), trackedObject.transform.eulerAngles.z);                     
                AnimateIn();
                timer = disappearAfterSec;
            }
        }
        else
        {   
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                AnimateOut();
            }
        }
        Rotate();
    }

    public void SizeToCollider()
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, trackedCollider.bounds.size.x);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, trackedCollider.bounds.size.y);
        arrowsCollider.size = new Vector3(trackedCollider.bounds.size.x, trackedCollider.bounds.size.y, 0.1f);
    }

    void Rotate()
    {        
        rotationChange = Vector3.Lerp(rotationChange, Vector3.zero, Time.deltaTime * rotationSpeed * 2f);
        trackedObject.transform.Rotate(rotationSpeed * rotationChange, Space.World);
    }


    public void Enable(bool enable)
    {
        switch (enable)
        {
            case true:
                AnimateIn();
                isOn = true;
                break;
            case false:
                AnimateOut();
                isOn = false;
                break;
        }
    }

#region Animating In / Out
    
    void AnimateIn()
    {
        if (canvasGroup.alpha == 0)
            LeanTween.alphaCanvas(canvasGroup, 1, 0.23f);
    }

    void AnimateOut()
    {
        if ( canvasGroup.alpha == 1)
            LeanTween.alphaCanvas(canvasGroup, 0, 0.17f);
    }

#endregion
}
