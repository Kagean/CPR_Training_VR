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
    public TMPro.TextMeshProUGUI bpmText;

    private float lastCompressionTime = 0f;
    private List<float> compressionIntervals = new List<float>();
    private bool wasCompressing = false;
    private float bpmDisplayTimeout = 3f;

    // Renk sabitleme (debounce)
    private Color currentColor = Color.white;
    private Color targetColor = Color.white;
    private float colorChangeDelay = 0.2f;
    private float colorChangeTimer = 0f;

    void Update()
    {
        bool leftInZone = IsHandTouchingChestZone(leftHand);
        bool rightInZone = IsHandTouchingChestZone(rightHand);

        if (leftInZone && rightInZone)
        {
            float depth = chestSurface.position.y - Mathf.Min(leftHand.position.y, rightHand.position.y);

            if (depth >= minDepth && depth <= maxDepth)
            {
                targetColor = Color.green;

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
            else
            {
                // Eller temas ediyor ama basınç yok (hazır bekleme)
                targetColor = Color.red;
                wasCompressing = false;
            }
        }
        else
        {
            targetColor = Color.white;
            wasCompressing = false;

            // Uzun süre basılmadıysa BPM'i gizle
            if (lastCompressionTime > 0 && Time.time - lastCompressionTime > bpmDisplayTimeout)
            {
                HideBPM();
                compressionIntervals.Clear();
            }
        }

        // Renk geçişini yavaşlat (debounce logic)
        if (feedbackRenderer.material.color != targetColor)
        {
            colorChangeTimer += Time.deltaTime;

            if (colorChangeTimer >= colorChangeDelay)
            {
                feedbackRenderer.material.color = targetColor;
                colorChangeTimer = 0f;
            }
        }
        else
        {
            colorChangeTimer = 0f;
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
                bpmText.color = new Color(1f, 0.5f, 0f); // turuncu
        }

        Debug.Log("BPM: " + bpm);
    }

    void HideBPM()
    {
        if (bpmText != null)
        {
            bpmText.text = "0 BPM";
            bpmText.color = Color.white;
        }
    }

    bool IsHandTouchingChestZone(Transform hand)
    {
        Collider chestCollider = chestSurface.GetComponent<Collider>();
        return chestCollider != null && chestCollider.bounds.Contains(hand.position);
    }
}
