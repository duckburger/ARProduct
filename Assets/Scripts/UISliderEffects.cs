using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISliderEffects : MonoBehaviour
{
    [SerializeField] GameObject handleImage;
    [SerializeField] float scalingCoefficient = 2;
    public void ZoomHandle(bool enable)
    {
        if (!handleImage)
            return;

        if (enable)
        {
            LeanTween.scale(handleImage, Vector3.one * scalingCoefficient, 0.2f);
        }
        else
        {
            LeanTween.scale(handleImage, Vector3.one, 0.2f);
        }
    }
}
