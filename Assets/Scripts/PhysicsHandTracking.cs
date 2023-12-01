#define OPEN_XR

#if OPEN_XR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using Microsoft.MixedReality.Toolkit.Input;
//using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine.XR.OpenXR.Input;

public class PhysicsHandTracking : MonoBehaviour
{
    public enum HandType
    {
        Left,
        Right
    };

    public HandType handType;
    private string hand;
    private string hand_Short;
    private Transform[] targetJoints = new Transform[26];
    private Transform targetRoot;
    private Transform[] followingJoints = new Transform[26];

    private Vector3 targePosition = new Vector3();
    private Quaternion targeRotation = new Quaternion();
    private Rigidbody rb;

    //public bool leftHand;
    private Vector3 rotOffsetPalm = new Vector3(0, 0, 0);
    private Vector3 rotOffsetFinger = new Vector3(0, 0, 0);

    void Start()
    {
        if (handType == HandType.Left)
        {
            hand = "Left";
            hand_Short = "L";
        }
        else
        {
            hand = "Right";
            hand_Short = "R";
        }

        targetRoot = GameObject.Find(hand + " Hand Tracking").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        targetJoints[0] = targetRoot.Find(hand_Short + "_Wrist/" + hand_Short + "_ThumbMetacarpal");
        targetJoints[1] = targetJoints[0].GetChild(0);
        targetJoints[2] = targetJoints[1].GetChild(0);
        targetJoints[3] = targetJoints[2].GetChild(0);

        targetJoints[4] = targetRoot.Find(hand_Short + "_Wrist/" + hand_Short + "_IndexMetacarpal");
        targetJoints[5] = targetJoints[4].GetChild(0);
        targetJoints[6] = targetJoints[5].GetChild(0);
        targetJoints[7] = targetJoints[6].GetChild(0);
        targetJoints[8] = targetJoints[7].GetChild(0);

        targetJoints[9] = targetRoot.Find(hand_Short + "_Wrist/" + hand_Short + "_MiddleMetacarpal");
        targetJoints[10] = targetJoints[9].GetChild(0);
        targetJoints[11] = targetJoints[10].GetChild(0);
        targetJoints[12] = targetJoints[11].GetChild(0);
        targetJoints[13] = targetJoints[12].GetChild(0);

        targetJoints[14] = targetRoot.Find(hand_Short + "_Wrist/" + hand_Short + "_RingMetacarpal");
        targetJoints[15] = targetJoints[14].GetChild(0);
        targetJoints[16] = targetJoints[15].GetChild(0);
        targetJoints[17] = targetJoints[16].GetChild(0);
        targetJoints[18] = targetJoints[17].GetChild(0);

        targetJoints[19] = targetRoot.Find(hand_Short + "_Wrist/" + hand_Short + "_LittleMetacarpal");
        targetJoints[20] = targetJoints[19].GetChild(0);
        targetJoints[21] = targetJoints[20].GetChild(0);
        targetJoints[22] = targetJoints[21].GetChild(0);
        targetJoints[23] = targetJoints[22].GetChild(0);

        targetJoints[24] = targetRoot.Find(hand_Short + "_Wrist/" + hand_Short + "_Palm");
        targetJoints[25] = targetRoot.Find(hand_Short + "_Wrist/");




        followingJoints[0] = transform.Find(hand_Short + "_Wrist/" + hand_Short + "_ThumbMetacarpal");
        followingJoints[1] = followingJoints[0].GetChild(0);
        followingJoints[2] = followingJoints[1].GetChild(0);
        followingJoints[3] = followingJoints[2].GetChild(0);

        followingJoints[4] = transform.Find(hand_Short + "_Wrist/" + hand_Short + "_IndexMetacarpal");
        followingJoints[5] = followingJoints[4].GetChild(0);
        followingJoints[6] = followingJoints[5].GetChild(0);
        followingJoints[7] = followingJoints[6].GetChild(0);
        followingJoints[8] = followingJoints[7].GetChild(0);

        followingJoints[9] = transform.Find(hand_Short + "_Wrist/" + hand_Short + "_MiddleMetacarpal");
        followingJoints[10] = followingJoints[9].GetChild(0);
        followingJoints[11] = followingJoints[10].GetChild(0);
        followingJoints[12] = followingJoints[11].GetChild(0);
        followingJoints[13] = followingJoints[12].GetChild(0);

        followingJoints[14] = transform.Find(hand_Short + "_Wrist/" + hand_Short + "_RingMetacarpal");
        followingJoints[15] = followingJoints[14].GetChild(0);
        followingJoints[16] = followingJoints[15].GetChild(0);
        followingJoints[17] = followingJoints[16].GetChild(0);
        followingJoints[18] = followingJoints[17].GetChild(0);

        followingJoints[19] = transform.Find(hand_Short + "_Wrist/" + hand_Short + "_LittleMetacarpal");
        followingJoints[20] = followingJoints[19].GetChild(0);
        followingJoints[21] = followingJoints[20].GetChild(0);
        followingJoints[22] = followingJoints[21].GetChild(0);
        followingJoints[23] = followingJoints[22].GetChild(0);

        followingJoints[24] = transform.Find(hand_Short + "_Wrist/" + hand_Short + "_Palm");
        followingJoints[25] = transform.Find(hand_Short + "_Wrist/");
    }


    void FixedUpdate()
    {
        try
        {
            // position
            rb.velocity = (targePosition - transform.position) / Time.fixedDeltaTime;

            // rotation
            Quaternion deltaRotation = targeRotation * Quaternion.Inverse(rb.rotation);
            deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
            //if (float.IsNaN(axis.x)| float.IsNaN(axis.y)| float.IsNaN(axis.z)) { return; }
            //if (float.IsInfinity(axis.x) | float.IsInfinity(axis.y) | float.IsInfinity(axis.z)) { return; }
            if (angle > 180f) { angle -= 360f; };
            Vector3 angularVelocity = angle * axis * Mathf.Deg2Rad / Time.fixedDeltaTime;
            if (float.IsNaN(angularVelocity.x) | float.IsNaN(angularVelocity.y) | float.IsNaN(angularVelocity.z)) { return; }
            rb.angularVelocity = angle * axis * Mathf.Deg2Rad / Time.fixedDeltaTime;
        }
        catch (Exception e)
        {
            //logText2.text += "\n" + e.ToString();
        }
    }


    void Update()
    {
        try
        {
            targePosition = targetJoints[25].position;
            targeRotation = targetJoints[25].rotation * Quaternion.Euler(rotOffsetPalm);

            for (int i = 0; i < 23; i++)
            {
                followingJoints[i].localPosition = targetJoints[i].localPosition;
                followingJoints[i].localRotation = targetJoints[i].localRotation * Quaternion.Euler(rotOffsetFinger);
            }
            
            
            //for (int i = 3; i < 26; i++)
            //{
            //    if (((i - 6) % 5 != 0) & ((i - 7) % 5 != 0))
            //    {
            //        if (HandJointUtils.TryGetJointPose((TrackedHandJoint)i, handedness, out pose))
            //        {
            //            bufJoints[(int)i - 3].position = pose.Position;
            //            bufJoints[(int)i - 3].rotation = pose.Rotation * Quaternion.Euler(rotOffsetFinger);

            //            Joints_left[(int)i - 3].localPosition =
            //                bufJoints[(int)i - 3].localPosition;// + new Vector3(0.01f, 0, 0);
            //            Joints_left[(int)i - 3].localRotation = bufJoints[(int)i - 3].localRotation;
            //        }
            //    }

            //}
        }
        catch (Exception e)
        {
            //logText2.text += "\n" + e.ToString();
        }

    }

    public Transform GetDistal(int fingerID)
    {
        Transform distalTransform = null;
        switch (fingerID)
        {
            case 0:
                distalTransform = GetThumbDistal();
                break;
            case 1:
                distalTransform = GetIndexDistal();
                break;
            case 2:
                distalTransform = GetMiddleDistal();
                break;
            case 3:
                distalTransform = GetRingDistal();
                break;
            case 4:
                distalTransform = GetPinkyDistal();
                break;
        }

        return distalTransform;
    }

    public Transform GetThumbDistal()
    {
        return followingJoints[2];
    }

    public Transform GetIndexDistal()
    {
        return followingJoints[7];
    }

    public Transform GetMiddleDistal()
    {
        return followingJoints[12];
    }

    public Transform GetRingDistal()
    {
        return followingJoints[17];
    }

    public Transform GetPinkyDistal()
    {
        return followingJoints[22];
    }
}


#elif MRTK

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using TMPro;


public class PhysicsHandTracking : MonoBehaviour
{
    private Transform[] Joints_left = new Transform[26];
    private Transform[] bufJoints = new Transform[26];

    private Vector3 targePosition = new Vector3();
    private Quaternion targeRotation = new Quaternion();
    private Rigidbody rb;
    MixedRealityPose pose;

    private Handedness handedness;
    public bool leftHand;
    public Vector3 rotOffsetPalm = new Vector3(0, -90, 180);
    public Vector3 rotOffsetFinger = new Vector3(180, 90, 0);

    //public TMP_Text logText, logText2;

    void Start()
    {
        if (leftHand)
        {
            handedness = Handedness.Left;
        }
        else
        {
            handedness = Handedness.Right;
            rotOffsetPalm = -rotOffsetPalm;
            rotOffsetFinger = -rotOffsetFinger;
        }
        rb = GetComponent<Rigidbody>();

        Joints_left[0] = transform.Find("L_Wrist/L_Palm/L_thumb_meta");
        Joints_left[1] = transform.Find("L_Wrist/L_Palm/L_thumb_meta/L_thumb_a");
        Joints_left[2] = transform.Find("L_Wrist/L_Palm/L_thumb_meta/L_thumb_a/L_thumb_b");
        Joints_left[3] = transform.Find("L_Wrist/L_Palm/L_thumb_meta/L_thumb_a/L_thumb_b/L_thumb_end");

        Joints_left[4] = transform.Find("L_Wrist/L_Palm/L_index_meta");
        Joints_left[5] = transform.Find("L_Wrist/L_Palm/L_index_meta/L_index_a");
        Joints_left[6] = transform.Find("L_Wrist/L_Palm/L_index_meta/L_index_a/L_index_b");
        Joints_left[7] = transform.Find("L_Wrist/L_Palm/L_index_meta/L_index_a/L_index_b/L_index_c");
        Joints_left[8] = transform.Find("L_Wrist/L_Palm/L_index_meta/L_index_a/L_index_b/L_index_c/L_index_end");

        Joints_left[9] = transform.Find("L_Wrist/L_Palm/L_middle_meta");
        Joints_left[10] = transform.Find("L_Wrist/L_Palm/L_middle_meta/L_middle_a");
        Joints_left[11] = transform.Find("L_Wrist/L_Palm/L_middle_meta/L_middle_a/L_middle_b");
        Joints_left[12] = transform.Find("L_Wrist/L_Palm/L_middle_meta/L_middle_a/L_middle_b/L_middle_c");
        Joints_left[13] = transform.Find("L_Wrist/L_Palm/L_middle_meta/L_middle_a/L_middle_b/L_middle_c/L_middle_end");

        Joints_left[14] = transform.Find("L_Wrist/L_Palm/L_ring_meta");
        Joints_left[15] = transform.Find("L_Wrist/L_Palm/L_ring_meta/L_ring_a");
        Joints_left[16] = transform.Find("L_Wrist/L_Palm/L_ring_meta/L_ring_a/L_ring_b");
        Joints_left[17] = transform.Find("L_Wrist/L_Palm/L_ring_meta/L_ring_a/L_ring_b/L_ring_c");
        Joints_left[18] = transform.Find("L_Wrist/L_Palm/L_ring_meta/L_ring_a/L_ring_b/L_ring_c/L_ring_end");

        Joints_left[19] = transform.Find("L_Wrist/L_Palm/L_pinky_meta");
        Joints_left[20] = transform.Find("L_Wrist/L_Palm/L_pinky_meta/L_pinky_a");
        Joints_left[21] = transform.Find("L_Wrist/L_Palm/L_pinky_meta/L_pinky_a/L_pinky_b");
        Joints_left[22] = transform.Find("L_Wrist/L_Palm/L_pinky_meta/L_pinky_a/L_pinky_b/L_pinky_c");
        Joints_left[23] = transform.Find("L_Wrist/L_Palm/L_pinky_meta/L_pinky_a/L_pinky_b/L_pinky_c/L_pinky_end");

        Joints_left[24] = transform.Find("L_Wrist/L_Palm");
        Joints_left[25] = transform.Find("L_Wrist");

        //////////////////////////////
        bufJoints[0] = transform.Find("L_Wrist_Ghost/L_Palm/L_thumb_meta");
        bufJoints[1] = transform.Find("L_Wrist_Ghost/L_Palm/L_thumb_meta/L_thumb_a");
        bufJoints[2] = transform.Find("L_Wrist_Ghost/L_Palm/L_thumb_meta/L_thumb_a/L_thumb_b");
        bufJoints[3] = transform.Find("L_Wrist_Ghost/L_Palm/L_thumb_meta/L_thumb_a/L_thumb_b/L_thumb_end");

        bufJoints[4] = transform.Find("L_Wrist_Ghost/L_Palm/L_index_meta");
        bufJoints[5] = transform.Find("L_Wrist_Ghost/L_Palm/L_index_meta/L_index_a");
        bufJoints[6] = transform.Find("L_Wrist_Ghost/L_Palm/L_index_meta/L_index_a/L_index_b");
        bufJoints[7] = transform.Find("L_Wrist_Ghost/L_Palm/L_index_meta/L_index_a/L_index_b/L_index_c");
        bufJoints[8] = transform.Find("L_Wrist_Ghost/L_Palm/L_index_meta/L_index_a/L_index_b/L_index_c/L_index_end");

        bufJoints[9] = transform.Find("L_Wrist_Ghost/L_Palm/L_middle_meta");
        bufJoints[10] = transform.Find("L_Wrist_Ghost/L_Palm/L_middle_meta/L_middle_a");
        bufJoints[11] = transform.Find("L_Wrist_Ghost/L_Palm/L_middle_meta/L_middle_a/L_middle_b");
        bufJoints[12] = transform.Find("L_Wrist_Ghost/L_Palm/L_middle_meta/L_middle_a/L_middle_b/L_middle_c");
        bufJoints[13] = transform.Find("L_Wrist_Ghost/L_Palm/L_middle_meta/L_middle_a/L_middle_b/L_middle_c/L_middle_end");

        bufJoints[14] = transform.Find("L_Wrist_Ghost/L_Palm/L_ring_meta");
        bufJoints[15] = transform.Find("L_Wrist_Ghost/L_Palm/L_ring_meta/L_ring_a");
        bufJoints[16] = transform.Find("L_Wrist_Ghost/L_Palm/L_ring_meta/L_ring_a/L_ring_b");
        bufJoints[17] = transform.Find("L_Wrist_Ghost/L_Palm/L_ring_meta/L_ring_a/L_ring_b/L_ring_c");
        bufJoints[18] = transform.Find("L_Wrist_Ghost/L_Palm/L_ring_meta/L_ring_a/L_ring_b/L_ring_c/L_ring_end");

        bufJoints[19] = transform.Find("L_Wrist_Ghost/L_Palm/L_pinky_meta");
        bufJoints[20] = transform.Find("L_Wrist_Ghost/L_Palm/L_pinky_meta/L_pinky_a");
        bufJoints[21] = transform.Find("L_Wrist_Ghost/L_Palm/L_pinky_meta/L_pinky_a/L_pinky_b");
        bufJoints[22] = transform.Find("L_Wrist_Ghost/L_Palm/L_pinky_meta/L_pinky_a/L_pinky_b/L_pinky_c");
        bufJoints[23] = transform.Find("L_Wrist_Ghost/L_Palm/L_pinky_meta/L_pinky_a/L_pinky_b/L_pinky_c/L_pinky_end");

        bufJoints[24] = transform.Find("L_Wrist_Ghost/L_Palm");
        bufJoints[25] = transform.Find("L_Wrist_Ghost");
    }


    void FixedUpdate()
    {
        try
        {
            // position
            rb.velocity = (targePosition - transform.position) / Time.fixedDeltaTime;

            // rotation
            Quaternion deltaRotation = targeRotation * Quaternion.Inverse(rb.rotation);
            deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
            //if (float.IsNaN(axis.x)| float.IsNaN(axis.y)| float.IsNaN(axis.z)) { return; }
            //if (float.IsInfinity(axis.x) | float.IsInfinity(axis.y) | float.IsInfinity(axis.z)) { return; }
            if (angle > 180f) { angle -= 360f; };
            Vector3 angularVelocity = angle * axis * Mathf.Deg2Rad / Time.fixedDeltaTime;
            if (float.IsNaN(angularVelocity.x) | float.IsNaN(angularVelocity.y) | float.IsNaN(angularVelocity.z)) { return; }
            rb.angularVelocity = angle * axis * Mathf.Deg2Rad / Time.fixedDeltaTime;
        }
        catch (Exception e)
        {
            //logText2.text += "\n" + e.ToString();
        }

    }


    void Update()
    {
        try
        {
            //Wrist
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, handedness, out pose))
            {
                targePosition = pose.Position;
                targeRotation = pose.Rotation * Quaternion.Euler(rotOffsetPalm);

                bufJoints[25].position = pose.Position;
                bufJoints[25].rotation = pose.Rotation * Quaternion.Euler(rotOffsetPalm);
            }

            ////Palm
            //if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out pose))
            //{
            //    bufJoints[24].position = pose.Position;
            //    bufJoints[24].rotation = pose.Rotation * Quaternion.Euler(offsetPalm);

            //    Joints_left[24].localPosition = bufJoints[24].localPosition;// + new Vector3(0,-0.017f,0);
            //    Joints_left[24].localRotation = bufJoints[24].localRotation;
            //    //Joints_left[24].rotation = pose.Rotation * Quaternion.Euler(offset1);
            //}

            for (int i = 3; i < 26; i++)
            {
                if (((i - 6) % 5 != 0) & ((i - 7) % 5 != 0))
                {
                    if (HandJointUtils.TryGetJointPose((TrackedHandJoint)i, handedness, out pose))
                    {
                        bufJoints[(int)i - 3].position = pose.Position;
                        bufJoints[(int)i - 3].rotation = pose.Rotation * Quaternion.Euler(rotOffsetFinger);

                        Joints_left[(int)i - 3].localPosition =
                            bufJoints[(int)i - 3].localPosition;// + new Vector3(0.01f, 0, 0);
                        Joints_left[(int)i - 3].localRotation = bufJoints[(int)i - 3].localRotation;
                    }
                }

            }
        }
        catch (Exception e)
        {
            //logText2.text += "\n" + e.ToString();
        }


    }

    
}

#endif
