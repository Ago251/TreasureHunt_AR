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
    private GameManager gameManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake() {
        arRaycastManager = GetComponent<ARRaycastManager>();
        gameManager = GetComponent<GameManager>();
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update() {
        if (!TryGetTouchPosition(out Vector2 touchPosition)) {
            return;
        }
        
        if (gameManager.placeMode) {
            if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.Planes)) {
                Pose hitPose = hits[0].pose;
                if (spawnedObject == null) {
                    spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                    gameManager.placeMode = false;
                }
            }
        }
        else {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                switch (hit.collider.gameObject.tag) {
                    case "Treasure":
                        hit.collider.gameObject.SetActive(false);
                        break;
                    case "Obstacle":
                        var rb = hit.collider.gameObject.GetComponent<Rigidbody>();
                        Vector3 force = hit.collider.gameObject.transform.position - Camera.main.transform.position;
                        force.Normalize();
                        rb.AddForceAtPosition(force, hit.point);
                        break;
                }
            }

        }
    }
}
