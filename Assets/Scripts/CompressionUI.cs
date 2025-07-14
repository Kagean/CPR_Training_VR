using UnityEngine;
using TMPro;

public class CompressionUI : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform chestSurface;
    public TextMeshProUGUI debugText;

    void Update()
    {
        float leftDistance = Vector3.Distance(leftHand.position, chestSurface.position);
        float rightDistance = Vector3.Distance(rightHand.position, chestSurface.position);
        float depth = chestSurface.position.y - Mathf.Min(leftHand.position.y, rightHand.position.y);
        float roundedDepth = Mathf.Round(depth * 1000f) / 10f;

        debugText.text =
            "Sol Mesafe: " + leftDistance.ToString("F2") + " m\n" +
            "Sað Mesafe: " + rightDistance.ToString("F2") + " m\n" +
            "Derinlik: " + depth.ToString("F2") + " m";
    }
}
