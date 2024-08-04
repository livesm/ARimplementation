using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class FlowerPlacement : MonoBehaviour
{
    
    public GameObject[] flowerPrefabs; // Array of different flower prefabs
    private ARRaycastManager raycastManager; //funka dårlig med mobilen?
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>(); // List to store raycast hits

    void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        Debug.Log("FlowerPlacement script attached and ARRaycastManager found.");
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hitResults, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hitResults[0].pose;
                    PlaceFlower(hitPose.position);
                }
            }
        }
    }

    void PlaceFlower(Vector3 position){
        int index = Random.Range(0, flowerPrefabs.Length);
        GameObject flower = Instantiate(flowerPrefabs[index], position, Quaternion.identity);

        flower.transform.localScale = Vector3.one * 2; // Scale the flower up 3 times its original size


        Debug.Log("Placed flower at: " + position + " with scale: " + flower.transform.localScale);
    }

    public void CollectFlower(GameObject flower)
    {
        if (GameManager.instance != null){
            GameManager.instance.AddScore(1);
            Debug.Log("Score updated: " + GameManager.instance.score);
        }
        else{
            Debug.LogError("GameManager instance is null");
        }

        AudioSource audioSource = flower.GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null){
            audioSource.Play();
            Destroy(flower, audioSource.clip.length);
        }
        else{
            Destroy(flower);
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Flower")){
            CollectFlower(other.gameObject);
        }
    }



}
