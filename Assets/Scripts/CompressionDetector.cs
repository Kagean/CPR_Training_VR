using UnityEngine;
using System.Collections.Generic;

public class CompressionDetector : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public float minDepth = 0.045f;
    public float maxDepth = 0.065f;
    public Transform chestSurface;
    public Renderer feedbackRenderer;
    public TMPro.TextMeshProUGUI bpmText; // UI feedback (optional)

    private float lastCompressionTime = 0f;
    private List<float> compressionIntervals = new List<float>();
    private float minInterval = 0.5f; // 120 bpm
    private float maxInterval = 0.6f; // 100 bpm
    private bool wasCompressing = false;

    void Update()
    {
        bool leftInZone = IsHandTouchingChestZone(leftHand);
        bool rightInZone = IsHandTouchingChestZone(rightHand);

        if (leftInZone && rightInZone)
        {
            float depth = chestSurface.position.y - Mathf.Min(leftHand.position.y, rightHand.position.y);

            if (depth >= minDepth && depth <= maxDepth)
            {
                feedbackRenderer.material.color = Color.green;

                // Ritim kontrolü burada
                if (!wasCompressing)
                {
                    float now = Time.time;

                    if (lastCompressionTime > 0)
                    {
                        float interval = now - lastCompressionTime;
                        compressionIntervals.Add(interval);
                        if (compressionIntervals.Count > 5)
                            compressionIntervals.RemoveAt(0);

                        float avgInterval = 0f;
                        foreach (float i in compressionIntervals)
                            avgInterval += i;
                        avgInterval /= compressionIntervals.Count;

                        int bpm = Mathf.RoundToInt(60f / avgInterval);
                        ShowBPM(bpm);
                    }

                    lastCompressionTime = now;
                    wasCompressing = true;
                }
            }
            else if (depth >= -0.01f && depth <= 0.01f)
            {
                feedbackRenderer.material.color = Color.blue;
                wasCompressing = false;
            }
            else
            {
                feedbackRenderer.material.color = Color.red;
                wasCompressing = false;
            }
        }
        else
        {
            feedbackRenderer.material.color = Color.white;
            wasCompressing = false;
        }
    }

    void ShowBPM(int bpm)
    {
        if (bpmText != null)
        {
            bpmText.text = bpm + " BPM";

            if (bpm >= 100 && bpm <= 120)
                bpmText.color = Color.green;
            else if (bpm < 100)
                bpmText.color = Color.red;
            else
                bpmText.color = new Color(1f, 0.5f, 0f);
        }

        Debug.Log("BPM: " + bpm);
    }

    bool IsHandTouchingChestZone(Transform hand)
    {
        Collider chestCollider = chestSurface.GetComponent<Collider>();
        return chestCollider != null && chestCollider.bounds.Contains(hand.position);
    }
}