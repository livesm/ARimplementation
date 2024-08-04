using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

public class PlantPlacementManager : MonoBehaviour //script til birdgame - skal plassere blomster/plantane når dei blir trigga(foundational step)
{
    public GameObject[] flowers;

    public ARRaycastManager raycastManager;//funka ikkje på mobilen1 - litt dårlig spawn

    public ARPlaneManager planeManager;
    
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private List<GameObject> placedObjects = new List<GameObject>(); // List of placed objects -testing
    private bool isActive = false; 

    // Toggle plant placement activation
    public void TogglePlantPlacement(){
        isActive = !isActive;
        HandlePlacedObjects();
    }

    private void HandlePlacedObjects()
    {
        if (!isActive){
            foreach (GameObject obj in placedObjects)
            {
                Destroy(obj); // destroy object for collision
            }
            placedObjects.Clear(); 

            foreach (var plane in planeManager.trackables){
                plane.gameObject.SetActive(false);
            }
            planeManager.enabled = false;
        }
    }

    private void Update()
    {
        if (isActive && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            bool collision = raycastManager.Raycast(Input.GetTouch(0).position, raycastHits, TrackableType.PlaneWithinPolygon);
            if (collision){
                GameObject _object = Instantiate(flowers[Random.Range(0, flowers.Length)]);
                _object.transform.position = raycastHits[0].pose.position;
                placedObjects.Add(_object); 

                // isActive = false;
            }


        }
    }




}
