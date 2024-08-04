using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System.Collections.Generic;

public class ImageTrackingManager : MonoBehaviour //this controls the flowers on the green-wall. Marker based in the sense of the physical wall being a marker and when detected - the flowers are spawned. 
{
    [SerializeField]
    private List<GameObject> flowerPrefabs;  // List of flower prefabs to place (low poly flowers assigned in the inspector)

    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, List<GameObject>> spawnedFlowers = new Dictionary<string, List<GameObject>>();
    private const int maxFlowers = 100;  //wall full of flowers(the number found through testing)
    private const float decreaseRate = 5.0f;  //decreasing rate for the flowers - withering away
    private float nextDecreaseTime = 0f;

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        if (trackedImageManager == null)
        {
            Debug.LogError("Failed to find ARTrackedImageManager component.");
        }
    }

    void OnEnable()
    {
        if (trackedImageManager != null){
            trackedImageManager.trackedImagesChanged += OnImageChanged;
        }
        nextDecreaseTime = Time.time + decreaseRate;
    }


    void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnImageChanged;
        }
    }

    void Update()
    {
        if (Time.time >= nextDecreaseTime){
            DecreaseFlowers();
            nextDecreaseTime = Time.time + decreaseRate;
        }
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            HandleNewTrackedImage(trackedImage);
        }
    }

    private void HandleNewTrackedImage(ARTrackedImage trackedImage)
    {
        var name = trackedImage.referenceImage.name;
        List<GameObject> flowers = new List<GameObject>();

        float rectangleWidth = 0.5f;  //Laga et rektangel som skal matche forma til green-wall
        float rectangleLength = 1.6f;  // burde finnes en litt bedre måte å gjere det her på - hjørnedeteksjon funka dårlig

        int numberOfFlowers = maxFlowers/10;

        for (int i = 0; i < numberOfFlowers; i++)
        {
            float offsetX = Random.Range(-rectangleWidth / 2, rectangleWidth / 2);
            float offsetZ = Random.Range(-rectangleLength / 2, rectangleLength / 2);


            Vector3 spawnPosition = trackedImage.transform.position +
                                    (trackedImage.transform.right * offsetX) +
                                    (trackedImage.transform.forward * offsetZ);
            Quaternion spawnRotation = Quaternion.Euler(90, 0, 90);//vinkling av blomstrane -- ganske sårbart
            GameObject flowerPrefab = flowerPrefabs[Random.Range(0, flowerPrefabs.Count)];

            var flower = Instantiate(flowerPrefab, spawnPosition, spawnRotation, trackedImage.transform);
            flowers.Add(flower);
        }
 
        if (spawnedFlowers.ContainsKey(name))//withers away same rate as watering bowl decrease
        {
            foreach (GameObject flower in spawnedFlowers[name]){
                Destroy(flower);
            }
            spawnedFlowers[name] = flowers;
        }
        else{
            spawnedFlowers.Add(name, flowers);
        }

        Debug.Log($"Added {numberOfFlowers} flowers around marker '{name}' within a rectangular area."); //debug for å sjekke om dei blir plassert
    }

    private void DecreaseFlowers()
    {
        foreach (var pair in spawnedFlowers)
        {
            if (pair.Value.Count > 0)
            {
                Destroy(pair.Value[pair.Value.Count - 1]);
                pair.Value.RemoveAt(pair.Value.Count - 1);
                Debug.Log($"Decreased flowers for marker: {pair.Key}. Total now: {pair.Value.Count}");
            }
        }
    }

        
    public void ResetFlowers() //resette alle blomstrane når knappen blir trykt (samme logikk som ista - burde nok løst det meir elegant, dårlig parksis:/)
    {
        foreach (var pair in spawnedFlowers)
        {
            // Remove all existing flowers for this marker
            foreach (GameObject flower in pair.Value){
                Destroy(flower);
            }
            pair.Value.Clear();

            ARTrackedImage trackedImage = null;
            foreach (var image in trackedImageManager.trackables){
                if (image.referenceImage.name == pair.Key){
                    trackedImage = image;
                    break;
                }
            }
            if (trackedImage != null){

                float rectangleWidth = 0.5f;
                float rectangleLength = 1.6f; 

                for (int i = 0; i < maxFlowers; i++)
                {
                    float offsetX = Random.Range(-rectangleWidth / 2, rectangleWidth / 2);
                    float offsetZ = Random.Range(-rectangleLength / 2, rectangleLength / 2);
                    Vector3 spawnPosition = trackedImage.transform.position +
                                            (trackedImage.transform.right * offsetX) +
                                            (trackedImage.transform.forward * offsetZ);
                    Quaternion spawnRotation = Quaternion.Euler(90, 0, 90);
                    GameObject flowerPrefab = flowerPrefabs[Random.Range(0, flowerPrefabs.Count)];
                    var flower = Instantiate(flowerPrefab, spawnPosition, spawnRotation, trackedImage.transform);
                    pair.Value.Add(flower);
                }
            }
            Debug.Log($"Reset flowers for marker: {pair.Key}. Total now: {pair.Value.Count}");
        }


    }


}


