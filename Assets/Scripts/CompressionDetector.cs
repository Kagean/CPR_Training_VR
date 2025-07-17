using UnityEngine;

public class CompressionDetector : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public float minDepth = 0.045f;
    public float maxDepth = 0.065f;
    public Transform chestSurface;
    public Renderer feedbackRenderer;

    private Vector3 previousPosition;
    private bool isCompressing = false;

    void Update()
    {
        bool leftTouching = IsHandTouchingChest(leftHand);
        bool rightTouching = IsHandTouchingChest(rightHand);

        if (leftTouching && rightTouching)
        {
            float depth = chestSurface.position.y - Mathf.Min(leftHand.position.y, rightHand.position.y);

            if (depth >= minDepth && depth <= maxDepth)
            {
                feedbackRenderer.material.color = Color.green;
                Debug.Log(" + Doðru Derinlik: " + depth.ToString("F2") + " m");

            }
            else
            {
                feedbackRenderer.material.color = Color.white;
                Debug.Log(" X Hatalý Derinlik: " + depth.ToString("F2") + " m");
            }

            isCompressing = true;
        }
        else
        {
            if (isCompressing)
            {
                feedbackRenderer.material.color = Color.blue;
                isCompressing = false;
            }
        }

        Debug.Log("Sol El Mesafe: " + Vector3.Distance(leftHand.position, chestSurface.position).ToString("F3"));
        Debug.Log("Sað El Mesafe: " + Vector3.Distance(rightHand.position, chestSurface.position).ToString("F3"));

        if (IsHandTouchingChestZone(leftHand) && IsHandTouchingChestZone(rightHand))
        {
            float depth = chestSurface.position.y - Mathf.Min(leftHand.position.y, rightHand.position.y);

            if (depth >= minDepth && depth <= maxDepth)
            {
                feedbackRenderer.material.color = Color.green;
            }
            else
            {
                feedbackRenderer.material.color = Color.white;
            }
        }
        else
        {
            feedbackRenderer.material.color = Color.red;
        }

    }

    bool IsHandTouchingChest(Transform hand)
    {
        float distance = Vector3.Distance(hand.position, chestSurface.position);
        return distance < 0.1f;
    }

    bool IsHandTouchingChestZone(Transform hand)
    {
        Collider chestCollider = chestSurface.GetComponent<Collider>();
        return chestCollider.bounds.Contains(hand.position);
    }

}
