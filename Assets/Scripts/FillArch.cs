using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillArch : MonoBehaviour
{
    [SerializeField] Image fillImage;

    public void SetFillValue(float val)
    {
        fillImage.fillAmount = Mathf.Clamp01(val);
    }
}
