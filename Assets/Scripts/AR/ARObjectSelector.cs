using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSessionOrigin))]
public class ARObjectSelector : MonoBehaviour
{
    [SerializeField] float activationDelay = 0.23f;
    [SerializeField] float floatingHeight = 1.8f;
    [SerializeField] float timeToActivate = 2.3f;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] Camera arCamera;
    [SerializeField] ARRaycastManager arRaycaster;
    [SerializeField] GameObject hitObject;
    
    [SerializeField] bool holdingOverObject = false;
    [Space]
    [SerializeField] GameObject uiArchPrefab;

    FillArch spawnedArch;
    RotationUIPanel selectedObjectRotataionIndicator;
    Transform shadowQuad;
    float objectOriginalY = 0;
    float timer = 0;
    float normalizedTimer = 0;
    bool movingObject = false;

    float preHoldTimer = 0;
    private void OnEnable() 
    {
        if (!arCamera)
            arCamera = transform.GetChild(0).GetComponent<Camera>();
        if (!arRaycaster)
            arRaycaster = GetComponent<ARRaycastManager>();

        Reset();
    }

    private void Update()
    {
        if (!arCamera)
            return;

        if (TryGetRayPosition(out Vector2 rayPos))
        {
            Ray ray = arCamera.ScreenPointToRay(rayPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                preHoldTimer += Time.deltaTime;
                if (preHoldTimer < activationDelay)
                    return;

                if (!movingObject && hit.collider.gameObject.layer == 8)
                {
                    if (!spawnedArch)
                    {
                        spawnedArch = Instantiate(uiArchPrefab, Input.mousePosition + new Vector3(0f, 50f, 0f), Quaternion.identity, uiCanvas.transform).GetComponent<FillArch>();
                        Debug.Log($"Insatntiating new arch");
                    }
                    spawnedArch.transform.position = Input.mousePosition + new Vector3(0f, 50f, 0f);
                    spawnedArch.SetFillValue(normalizedTimer);
                    holdingOverObject = true;
                    hitObject = hit.collider.gameObject;   
                    selectedObjectRotataionIndicator = hitObject.GetComponentInChildren<RotationUIPanel>();
                    shadowQuad = hitObject.GetComponentInChildren<ARShadowQuad>().transform;
                    objectOriginalY = hitObject.transform.localPosition.y;             
                    timer += Time.deltaTime;
                    normalizedTimer = timer / timeToActivate;
                
                    if (normalizedTimer >= 1)
                    {
                        DestroyHoldIndicator();
                        LiftObject(hitObject);            
                    }
                }    
                else 
                {
                    if (spawnedArch)
                    {
                        DestroyHoldIndicator();
                        LowerObject(hitObject);
                    }
                    if (movingObject)
                        MoveObject();
                }            
            }
            else
            {
                if (movingObject)
                    MoveObject();                  
            }
        }   
        else
        {
            if (hitObject)
                LeanTween.cancel(hitObject);
            if (hitObject && hitObject.transform.localPosition.y != objectOriginalY)
                LowerObject(hitObject);

            Reset();
        }
    }

    private void Reset()
    {
        movingObject = false;
        DestroyHoldIndicator();
        timer = 0;
        preHoldTimer = 0;
        normalizedTimer = 0;
        hitObject = null;
        holdingOverObject = false;
        objectOriginalY = 0;
    }

    private void DestroyHoldIndicator()
    {
        if (spawnedArch)
        {
            Destroy(spawnedArch.gameObject);
            spawnedArch = null;
        }
    }

    private void MoveObject()
    {
        List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
        if (arRaycaster.Raycast(Input.mousePosition, hitResults, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinInfinity))
        {
            Pose hitPose = hitResults[0].pose;
            Vector3 position = new Vector3(hitPose.position.x, hitObject.transform.position.y, hitPose.position.z);
            hitObject.transform.position = Vector3.Lerp(hitObject.transform.position, position, Time.deltaTime * 5f);
        }
    }

    bool TryGetRayPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    void LiftObject(GameObject obj)
    {
        if (!hitObject)
            return;
        Debug.Log($"Lifting object");

        if (selectedObjectRotataionIndicator)
            selectedObjectRotataionIndicator?.Enable(false);
        LeanTween.moveLocalY(obj, objectOriginalY + floatingHeight, 0.23f).setEase(LeanTweenType.easeInOutSine);
        if (shadowQuad)
            LeanTween.moveLocalY(shadowQuad.gameObject, shadowQuad.localPosition.y - floatingHeight, 0.23f).setEase(LeanTweenType.easeInOutSine);
        movingObject = true;
    }

    void LowerObject(GameObject obj)
    {
        if (!hitObject)
            return;
        Debug.Log($"Lowering object");
        
        LeanTween.moveLocalY(obj, objectOriginalY, 0.1f).setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() => 
            {
                if (selectedObjectRotataionIndicator)
                    selectedObjectRotataionIndicator?.Enable(true);
            });
        if (shadowQuad)
            LeanTween.moveLocalY(shadowQuad.gameObject, 0.02f, 0.1f).setEase(LeanTweenType.easeInOutSine);
        movingObject = false;
    }

}
