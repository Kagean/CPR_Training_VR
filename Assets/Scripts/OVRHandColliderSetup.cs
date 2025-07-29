using System.Collections.Generic;
using UnityEngine;

public class OVRHandColliderSetup : MonoBehaviour
{
    public OVRSkeleton skeleton;

    void Start()
    {
        foreach (var bone in skeleton.Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip ||
                bone.Id == OVRSkeleton.BoneId.Hand_ThumbTip)
            {
                if (bone.Transform.GetComponent<Collider>() == null)
                {
                    CapsuleCollider col = bone.Transform.gameObject.AddComponent<CapsuleCollider>();
                    col.radius = 0.005f;
                    col.height = 0.02f;
                    col.direction = 2;
                    col.isTrigger = true;
                }
            }
        }
    }
}
