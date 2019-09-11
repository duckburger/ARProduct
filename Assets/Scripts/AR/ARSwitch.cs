using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARSwitch : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject arObject;
    [SerializeField] GameObject threedDObject;
    CanvasGroup textCG;

    private void Start()
    {
        textCG = buttonText.GetComponent<CanvasGroup>();
    }

    public void ToggleAR()
    {
        if (arObject.activeSelf)
        {
            arObject.SetActive(false);
            threedDObject.SetActive(true);
            LeanTween.cancel(textCG.gameObject);
            LeanTween.alphaCanvas(textCG, 0, 0.23f)
                .setOnComplete(() => 
                {
                    buttonText.text = "AR";
                    LeanTween.alphaCanvas(textCG, 1, 0.23f);
                });                
        }
        else
        {
            arObject.SetActive(true);
            threedDObject.SetActive(false);
            LeanTween.cancel(textCG.gameObject);
            LeanTween.alphaCanvas(textCG, 0, 0.23f)
                .setOnComplete(() => 
                {
                    buttonText.text = "3D";
                    LeanTween.alphaCanvas(textCG, 1, 0.23f);
                });                
        }
    }
    
}
