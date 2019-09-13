using UnityEngine;
namespace Cinemachine
{
	[RequireComponent(typeof(CinemachineFreeLook))]
	class CinemachineFreeLookZoom : MonoBehaviour
	{
		private CinemachineFreeLook freelook;
		private CinemachineFreeLook.Orbit[] originalOrbits;
		[Tooltip("The minimum scale for the orbits")]
		[Range(0.01f, 1f)]
		public float minScale = 0.5f;

		[Tooltip("The maximum scale for the orbits")]
		[Range(1F, 5f)]
		public float maxScale = 1;

		[Tooltip("The zoom axis.  Value is 0..1.  How much to scale the orbits")]
		[AxisStateProperty]
		public AxisState zAxis = new AxisState(0, 1, false, true, 50f, 0.1f, 0.1f, "Mouse ScrollWheel", false);
        float scale;
		void OnValidate()
		{
			minScale = Mathf.Max(0.01f, minScale);
			maxScale = Mathf.Max(minScale, maxScale);
		}

		void Awake()
		{
			freelook = GetComponentInChildren<CinemachineFreeLook>();
			if (freelook != null)
			{
				originalOrbits = new CinemachineFreeLook.Orbit[freelook.m_Orbits.Length];
				for (int i = 0; i < originalOrbits.Length; i++)
				{
					originalOrbits[i].m_Height = freelook.m_Orbits[i].m_Height;
					originalOrbits[i].m_Radius = freelook.m_Orbits[i].m_Radius;
				}
#if UNITY_EDITOR
                SaveDuringPlay.SaveDuringPlay.OnHotSave -= RestoreOriginalOrbits;
                SaveDuringPlay.SaveDuringPlay.OnHotSave += RestoreOriginalOrbits;
#endif
                scale = 1;
			}
		}

#if UNITY_EDITOR
        private void OnDestroy()
        {
            SaveDuringPlay.SaveDuringPlay.OnHotSave -= RestoreOriginalOrbits;
        }
 
        private void RestoreOriginalOrbits()
        {
            if (originalOrbits != null)
            {
                for (int i = 0; i < originalOrbits.Length; i++)
                {
                    freelook.m_Orbits[i].m_Height = originalOrbits[i].m_Height;
                    freelook.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius;
                }
            }
        }
#endif

		void Update()
		{
			if (originalOrbits != null)
			{
				zAxis.Update(Time.deltaTime);
                if (Input.touchCount == 2)
                {           
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    // Find the position in the previous frame of each touch.
                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    // Find the magnitude of the vector (the distance) between the touches in each frame.
                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    // Find the difference in the distances between each frame.
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                    scale = Mathf.Lerp(scale, scale + deltaMagnitudeDiff / 8f * Time.deltaTime, Time.deltaTime * 5f);
                    scale = Mathf.Clamp(scale, minScale, maxScale);
                }
                else 
                {
#if !UNITY_IOS
                    Debug.Log($"Z Axis is {zAxis.Value}");
                    scale = Mathf.Lerp(minScale, maxScale, zAxis.Value);
#endif
                }

                Debug.LogError($"Scale is {scale}");
				for (int i = 0; i < originalOrbits.Length; i++)
				{
					freelook.m_Orbits[i].m_Height = originalOrbits[i].m_Height * scale;
					freelook.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * scale;
				}
			}
		}
	}
}
