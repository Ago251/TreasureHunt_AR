using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlayerController : MonoBehaviour {
    public GameObject gameObjectToInstantiate;

    private GameObject spawnedObject;
    private ARRaycastManager arRaycastManager;
    [SerializeField] private LineRenderer lineRenderer;
    private Touch touch;
    [SerializeField] private float force;
    [SerializeField] private GameObject placeIndicator;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake() {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    private bool TryGetTouchPosition() {
        if (Input.touchCount > 0) {
            touch = Input.GetTouch(0);
            return true;
        }
        
        return false;
    }

    private void Update() {

        if (GameManager.instance.placeMode) {
            DrawPlaceIndicator();
        }


        if (!TryGetTouchPosition()) {
            return;
        }
        
        if (touch.phase == TouchPhase.Began) {
            if (GameManager.instance.placeMode) {
                if (placeIndicator.activeSelf) {
                    if (spawnedObject == null) {
                        GameManager.instance.DisablePlaceMode();
                        spawnedObject = Instantiate(gameObjectToInstantiate, placeIndicator.transform.position, Quaternion.identity);
                        placeIndicator.SetActive(false);
                    }
                }
            }
            else {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    switch (hit.collider.gameObject.tag) {
                        case "Treasure":
                            hit.collider.gameObject.SetActive(false);
                            break;
                        case "Obstacle":
                            var rb = hit.collider.gameObject.GetComponent<Rigidbody>();
                            Vector3 direction = hit.collider.gameObject.transform.position - Camera.main.transform.position;
                            direction.Normalize();
                            rb.AddForceAtPosition(direction * force, hit.point);
                            break;
                    }
                }

            }
        }
    }

    private void DrawPlaceIndicator() {
        if (arRaycastManager.Raycast(new Vector2(Screen.width / 2f, Screen.height / 2f), hits, TrackableType.Planes)) {
            placeIndicator.SetActive(true);
            Pose hitPose = hits[0].pose;
            placeIndicator.transform.position = hitPose.position;
        }
    }
}
