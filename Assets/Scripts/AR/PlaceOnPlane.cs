using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]    
    [SerializeField] Scaler scaler;
    [Space(10)]
    [SerializeField] GameObject m_PlacedPrefab;
    [SerializeField] GameObject rotationIndicator;
    [SerializeField] GameObject shadowQuad;
    [Space(10)]

    ARPlaneManager planeManager;
    ARPointCloudManager cloudManager;
    public Action onPlacedObject;
    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    
    [Space(15)]
    public GameObject spawnedObject;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        cloudManager = GetComponent<ARPointCloudManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
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

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            Pose hitPose = s_Hits[0].pose;

            if (spawnedObject == null)
            {
                SpawnARObject(hitPose);
            }            
        }
    }

    void DeletePlanes()
    {
        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }

    void DeleteClouds()
    {
        foreach (ARPointCloud cloud in cloudManager.trackables)
        {
            cloud.gameObject.SetActive(false);
        }
    }

    public void TestSpawnObject()
    {
        SpawnARObject(new Pose(Vector3.zero, Quaternion.identity));
    }

    public void SpawnARObject(Pose hitPose)
    {
        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
        scaler?.AssignSpawnedObject(spawnedObject.transform);
        scaler?.AnimateIn();
        scaler?.ApplyScale(1.3f);

        SpawnRotator();
        SpawnShadowPlane();

        onPlacedObject?.Invoke();

        planeManager.enabled = false;
        cloudManager.enabled = false;

        DeletePlanes();
        DeleteClouds();
    }

    void SpawnRotator()
    {
        if (!rotationIndicator)
        {
            Debug.LogError($"No rotation indicator connected to PlaceOnPlane");
            return;
        }

        GameObject rotIndicator = Instantiate(rotationIndicator, spawnedObject.transform);
        rotIndicator.transform.localPosition = new Vector3(0, 0.022f, 0);
        Camera mainCamera = GetComponent<ARSessionOrigin>().camera;
        Canvas canv = rotIndicator.GetComponent<Canvas>();
        canv.worldCamera = mainCamera;
        RotationUIPanel panelController = rotIndicator.GetComponent<RotationUIPanel>();
        panelController.AssignRefences(spawnedObject, mainCamera);
    }

    void SpawnShadowPlane()
    {
        if (!shadowQuad)
        {
            Debug.LogError($"No shadow quad connected to PlaceOnPlane");
            return;
        }

        GameObject shadowPlane = Instantiate(shadowQuad, spawnedObject.transform);
        shadowPlane.transform.localPosition = new Vector3(0, 0.02f, 0);
    }
        
    private void OnDisable() 
    {
        Reset();    
    }

    public void Reset()
    {
        if (spawnedObject)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
            scaler?.AssignSpawnedObject(null);
            scaler?.AnimateOut();
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}
