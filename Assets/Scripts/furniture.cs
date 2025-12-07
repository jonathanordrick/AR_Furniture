using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;

public class furniture : MonoBehaviour
{
    [Header("Furniture Prefabs")]
    public GameObject[] furniturePrefabs;   // isi 3 sofa di Inspector
    private int selectedIndex = 0;          // sofa yang lagi dipilih

    [Header("AR Components")]
    public XROrigin origin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    private static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    // ==== DIPANGGIL OLEH BUTTON ====
    public void SelectWhiteSofa()      { selectedIndex = 0; }
    public void SelectLightBrownSofa() { selectedIndex = 1; }
    public void SelectBrownSofa()      { selectedIndex = 2; }

    private void Update()
    {
        // kalau tidak ada touch, keluar
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        // abaikan kalau touch di-drag dll, kita cuma mau saat baru tap
        if (touch.phase != TouchPhase.Began)
            return;

        // Raycast ke plane AR
        bool collision = raycastManager.Raycast(
            touch.position,
            raycastHits,
            TrackableType.PlaneWithinPolygon
        );

        if (collision)
        {
            Pose hitPose = raycastHits[0].pose;

            // pastikan index valid & prefab terisi
            if (selectedIndex >= 0 &&
                selectedIndex < furniturePrefabs.Length &&
                furniturePrefabs[selectedIndex] != null)
            {
                Instantiate(
                    furniturePrefabs[selectedIndex],
                    hitPose.position,
                    hitPose.rotation
                );
            }
        }

        // (opsional) kalau di tutorial plane-nya dimatikan setelah spawn pertama,
        // bagian ini bisa kamu pindah ke kondisi tertentu, atau hapus saja.
        /*
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        planeManager.enabled = false;
        */
    }
}
