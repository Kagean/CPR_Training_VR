using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChestDeformer : MonoBehaviour
{
    public Transform compressionHandle;
    public Transform chestVisual; // Deform olacak obje
    public float maxCompression = 0.05f; // Maksimum çökmüþ hali

    private Vector3 originalScale;
    private float startY;

    void Start()
    {
        originalScale = chestVisual.localScale;
        startY = compressionHandle.position.y;
    }

    void Update()
    {
        float depth = startY - compressionHandle.position.y;
        float t = Mathf.Clamp01(depth / maxCompression);

        float newY = Mathf.Lerp(originalScale.y, originalScale.y - maxCompression, t);
        chestVisual.localScale = new Vector3(originalScale.x, newY, originalScale.z);
    }
}
