using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColliderAdder : MonoBehaviour
{
    public Transform handRoot;
    public float colliderRadius = 0.01f;

    void Start()
    {
        if (handRoot == null)
        {
            Debug.LogWarning("Hand Root atanmadý!");
            return;
        }

        foreach (Transform bone in handRoot.GetComponentsInChildren<Transform>())
        {
            if (bone.name.ToLower().Contains("index") || bone.name.ToLower().Contains("palm"))
            {
                SphereCollider col = bone.gameObject.AddComponent<SphereCollider>();
                col.radius = colliderRadius;

                Rigidbody rb = bone.gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }
    }
}
