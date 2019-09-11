using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSessionController : MonoBehaviour
{
    //[SerializeField] ARSession arSession;
    [SerializeField] GameObject arContentsPrefab;
    GameObject currentARContents;

    private void OnEnable()
    {
        currentARContents = Instantiate(arContentsPrefab, transform);
        //arSession?.Reset();
    }

    private void OnDisable()
    {
        if (currentARContents)
            Destroy(currentARContents);
        currentARContents = null;
    }
}
