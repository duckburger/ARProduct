using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Scaler : MonoBehaviour
{
    [SerializeField] ARSessionOrigin sessionOrigin;
    [SerializeField] Transform spawnedObject;

    CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void AnimateIn()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        LeanTween.alphaCanvas(canvasGroup, 1, 0.23f);
    }

    public void AnimateOut()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        LeanTween.alphaCanvas(canvasGroup, 0, 0.15f);
    }

    public void AssignSpawnedObject(Transform obj)
    {
        spawnedObject = obj;
    }

    public void ApplyScale(float value)
    {
        if (!sessionOrigin || !spawnedObject)
        {
            Debug.LogError($"Connect components to the Scaler");
            return;
        }

        Vector3 newTargetScale = new Vector3(value, value, value);
        sessionOrigin.MakeContentAppearAt(spawnedObject, spawnedObject.position, spawnedObject.rotation);
        LeanTween.scale(sessionOrigin.gameObject, newTargetScale, 0.13f).setEase(LeanTweenType.easeOutBack);
    }


    private void OnEnable()
    {
        AssignSpawnedObject(null);
        canvasGroup.alpha = 0;
    }

    private void OnDisable()
    {
        AssignSpawnedObject(null);
        AnimateOut();
    }
}
