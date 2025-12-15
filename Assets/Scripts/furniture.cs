using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class Furniture : MonoBehaviour
{
    [Header("Furniture Prefabs")]
    public GameObject[] furniturePrefabs;
    private int selectedIndex = -1;

    [Header("AR Components")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // ===== DESAIN 2 =====
    private HashSet<TrackableId> occupiedPlanes = new HashSet<TrackableId>();
    private List<GameObject> placedFurniture = new List<GameObject>();

    // ===== BUTTON PILIH FURNITURE =====
    public void SelectWhiteSofa()      { selectedIndex = 0; }
    public void SelectLightBrownSofa() { selectedIndex = 1; }
    public void SelectBrownSofa()      { selectedIndex = 2; }

    void Update()
    {
        if (selectedIndex == -1) return;
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        // supaya tap UI tidak spawn
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            ARRaycastHit hit = hits[0];

            // ❌ Plane ini sudah terisi
            if (occupiedPlanes.Contains(hit.trackableId))
            {
                Debug.Log("Plane sudah ada furniture!");
                return;
            }

            // ✅ Spawn furniture
            GameObject obj = Instantiate(
                furniturePrefabs[selectedIndex],
                hit.pose.position,
                hit.pose.rotation
            );

            placedFurniture.Add(obj);
            occupiedPlanes.Add(hit.trackableId);
        }
    }

    // ===== BUTTON DELETE =====
    public void DeleteAllFurniture()
    {
        foreach (GameObject obj in placedFurniture)
            Destroy(obj);

        placedFurniture.Clear();
        occupiedPlanes.Clear();
        selectedIndex = -1;
    }
}
