using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class Furniture : MonoBehaviour
{
    [Header("Furniture Prefabs")]
    public GameObject[] furniturePrefabs;
    private int selectedIndex = -1; // belum memilih furniture

    [Header("AR Components")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // ===== FITUR BARU =====
    private bool furniturePlaced = false;
    private List<GameObject> placedFurniture = new List<GameObject>();

    // ===== DIPANGGIL BUTTON =====
    public void SelectWhiteSofa()      { selectedIndex = 0; }
    public void SelectLightBrownSofa() { selectedIndex = 1; }
    public void SelectBrownSofa()      { selectedIndex = 2; }

    void Update()
    {
        if (selectedIndex == -1) return;       // belum pilih sofa
        if (furniturePlaced) return;           // area terkunci

        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began) return;

        // supaya tap button tidak spawn furniture
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            GameObject obj = Instantiate(
                furniturePrefabs[selectedIndex],
                pose.position,
                pose.rotation
            );

            placedFurniture.Add(obj);
            furniturePlaced = true;

            // matikan plane setelah furniture diletakkan
            foreach (var plane in planeManager.trackables)
                plane.gameObject.SetActive(false);

            planeManager.enabled = false;
        }
    }

    // ===== BUTTON DELETE =====
    public void DeleteAllFurniture()
    {
        foreach (GameObject obj in placedFurniture)
            Destroy(obj);

        placedFurniture.Clear();
        furniturePlaced = false;
        selectedIndex = -1;

        planeManager.enabled = true;
        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(true);
    }
}
