using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    Dictionary<TrackableId, GameObject> gameobjectDictionary = new();

    private ARTrackedImageManager trackedImageManager;

    [SerializeField] private List<ImagePrefabMapping> imagePrefabMappings;

    [Serializable]
    public class ImagePrefabMapping
    {
        public string imageName;
        public GameObject prefab;
    }

    private void Awake()
    {
        this.trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            int index = 0;
            foreach (var item in this.trackedImageManager.trackables)
            {
                if (item.trackableId == newImage.trackableId)
                {
                    GameObject prefabToInstantiate = this.imagePrefabMappings[index].prefab;

                    var go = Instantiate(prefabToInstantiate, trackedImageManager.trackables[newImage.trackableId].transform);
                    go.transform.position = go.transform.parent.transform.position;
                    gameobjectDictionary.TryAdd(newImage.trackableId, go);
                }
                index++;
            }
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            Debug.Log(updatedImage.ToString());
        }


        foreach (var removedImage in eventArgs.removed)
        {
            Debug.Log($"Removed image: {removedImage.size}");
            ListAllImages();
        }
    }

    void ListAllImages()
    {
        Debug.Log(
            $"There are {this.trackedImageManager.trackables.count} images being tracked.");

        foreach (ARTrackedImage trackedImage in this.trackedImageManager.trackables)
        {
            Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                      $"{trackedImage.transform.position}" +
                      $"{trackedImage.transform.localScale}" +
                      $"{trackedImage.trackableId}"
                      );
        }
    }
}
