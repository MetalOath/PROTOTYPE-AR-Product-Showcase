using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARRayCaster : MonoBehaviour
{
    [SerializeField] private GameObject aRCamera, aRAnchor;

    public ARRaycastManager manager;
    public UnityEvent onBegan;
    public UnityEvent onEnded;

    [Serializable]
    public class Vector3UnityEvent : UnityEvent<Vector3> { }
    public Vector3UnityEvent hitPosition;

    public List<ARRaycastHit> raycastHitList = new List<ARRaycastHit>();

    private void Update()
    {
        if (!aRAnchor)
        {
            PerformARRaycast();
        }
    }
    public void PerformARRaycast()
    {
        bool hitARPlane;

        if (!aRAnchor)
        {
            hitARPlane = manager.Raycast(new Vector2(0f, 0f), raycastHitList, TrackableType.All);
        }
        else
        {
            hitARPlane = manager.Raycast(Input.GetTouch(0).position, raycastHitList, TrackableType.All);

            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    onBegan?.Invoke();
                    break;
                case TouchPhase.Moved:
                    if (hitARPlane)
                    {
                        hitPosition?.Invoke(raycastHitList[0].pose.position);
                    }
                    break;
                case TouchPhase.Ended:
                    onEnded?.Invoke();
                    break;
                default:
                    break;
            }
        }

        raycastHitList.Clear();
    }
}