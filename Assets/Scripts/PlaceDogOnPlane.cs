using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class PlaceDogOnPlane : MonoBehaviour //place the dog in the first detected plane. (et forsøk på å spawne hunden bedre)
{

    public GameObject dogPrefab; 
    private GameObject spawnedDog;
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;

    void Awake(){
        raycastManager = FindObjectOfType<ARRaycastManager>();
        planeManager = FindObjectOfType<ARPlaneManager>();
    }

    void Start()
    {
        // Subscribe to the planesChanged event to detect new planes
        planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        // Check if the dog has not been spawned and a new plane has been detected
        if (spawnedDog == null && args.added != null && args.added.Count > 0)
        {
            ARPlane plane = args.added[0]; // Use the first detected plane
            Vector3 position = plane.center;


        
            Quaternion rotation = Quaternion.LookRotation(plane.normal);

                // dog faced towards the camera
            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.y = position.y; // Adjust y-coordinate to align with the plane's height
            rotation = Quaternion.LookRotation(cameraPosition - position);
            spawnedDog = Instantiate(dogPrefab, position, rotation);


            
            planeManager.planesChanged -= OnPlanesChanged;
        }
    }
}
