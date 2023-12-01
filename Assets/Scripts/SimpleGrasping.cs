using System.Collections;
using System.Collections.Generic;
using HaptGlove;
using TMPro;
using UnityEngine;

public class SimpleGrasping : MonoBehaviour
{
    //public TMP_Text text;
    [SerializeField] private GameObject graspedObject;
    [SerializeField] private List<string> fingerLeftList = new List<string>();
    [SerializeField] private List<string> fingerRightList = new List<string>();
    [SerializeField] private GameObject targetHand;
    private bool leftGrasped = false;
    private bool rightGrasped = false;
    private FixedJoint fixedJoint;

    private void Start()
    {
        //gameObject.GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
    }

    void OnTriggerEnter(Collider col)
    {
        HaptGloveHandler gloveHandler = col.GetComponentInParent<HaptGloveHandler>();
        //GraspingLeft graspingScript = col.GetComponentInParent<GraspingLeft>();
        List<string> fingerList = new List<string>();

        if (gloveHandler != null)
        {
            if (gloveHandler.whichHand == HaptGloveHandler.HandType.Left)
            {
                fingerList = fingerLeftList;
            }
            else if (gloveHandler.whichHand == HaptGloveHandler.HandType.Right)
            {
                fingerList = fingerRightList;
            }

            targetHand = gloveHandler.gameObject;
            //Debug.Log(targetHand.name);
        }
        else
        {
            return;
        }

        if (fingerList.Contains(col.name))
        {
            return;
        }
        

        switch (col.name)
        {
            case "L_thumb_b":
                fingerList.Add("L_thumb_b");
                break;
            case "L_index_c":
                fingerList.Add("L_index_c");
                break;
            case "L_middle_c":
                fingerList.Add("L_middle_c");
                break;
            case "L_ring_c":
                fingerList.Add("L_ring_c");
                break;
            case "L_pinky_c":
                fingerList.Add("L_pinky_c");
                break;
        }
    }

    void OnTriggerExit(Collider col)
    {
        HaptGloveHandler gloveHandler = col.GetComponentInParent<HaptGloveHandler>();
        //GraspingLeft graspingScript = col.GetComponentInParent<GraspingLeft>();
        List<string> fingerList = new List<string>();

        if (gloveHandler != null)
        {
            if (gloveHandler.whichHand == HaptGloveHandler.HandType.Left)
            {
                fingerList = fingerLeftList;
            }
            else if (gloveHandler.whichHand == HaptGloveHandler.HandType.Right)
            {
                fingerList = fingerRightList;
            }

            targetHand = gloveHandler.gameObject;
        }
        else
        {
            return;
        }

        switch (col.name)
        {
            case "L_thumb_b":
                fingerList.Remove("L_thumb_b");
                break;
            case "L_index_c":
                fingerList.Remove("L_index_c");
                break;
            case "L_middle_c":
                fingerList.Remove("L_middle_c");
                break;
            case "L_ring_c":
                fingerList.Remove("L_ring_c");
                break;
            case "L_pinky_c":
                fingerList.Remove("L_pinky_c");
                break;
        }
    }

    void Update()
    {
        if ((fingerLeftList.Count >= 2) & fingerLeftList.Contains("L_thumb_b"))
        {
            if (leftGrasped == false)
            {
                leftGrasped = true;

                if (targetHand.GetComponent<FixedJoint>() == null)
                {
                    fixedJoint = targetHand.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = graspedObject.GetComponent<Rigidbody>();
                    fixedJoint.massScale = 1e-05f;
                    //fixedJoint = gameObject.AddComponent<FixedJoint>();
                    //fixedJoint.connectedBody = targetHand.GetComponent<Rigidbody>();
                    graspedObject.GetComponent<Rigidbody>().isKinematic = false;
                    //Debug.Log("isKinematic = false");
                }

            }
        }
        else
        {
            if (leftGrasped == true)
            {
                leftGrasped = false;
                //Destroy(targetHand.GetComponent<FixedJoint>());

                if (targetHand.GetComponent<FixedJoint>() != null)
                {
                    Destroy(targetHand.GetComponent<FixedJoint>());
                }

                graspedObject.GetComponent<Rigidbody>().isKinematic = true;
                //Debug.Log("isKinematic = true");

            }
        }

        if ((fingerRightList.Count >= 2) & fingerRightList.Contains("L_thumb_b"))
        {
            if (rightGrasped == false)
            {
                rightGrasped = true;

                if (targetHand.GetComponent<FixedJoint>() == null)
                {
                    fixedJoint = targetHand.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = graspedObject.GetComponent<Rigidbody>();
                    fixedJoint.massScale = 1e-05f;
                    //fixedJoint = gameObject.AddComponent<FixedJoint>();
                    //fixedJoint.connectedBody = targetHand.GetComponent<Rigidbody>();
                    graspedObject.GetComponent<Rigidbody>().isKinematic = false;

                    Debug.Log("Create fixed joint on right hand");
                    //Debug.Log("isKinematic = false");
                }


            }
        }
        else
        {
            if (rightGrasped == true)
            {
                rightGrasped = false;
                //Destroy(targetHand.GetComponent<FixedJoint>());

                if (targetHand.GetComponent<FixedJoint>() != null)
                {
                    Destroy(targetHand.GetComponent<FixedJoint>());
                }

                graspedObject.GetComponent<Rigidbody>().isKinematic = true;

                Debug.Log("Destroy fixed joint on right hand");
                //Debug.Log("isKinematic = true");
            }
        }

        //text.text = "right grasped = " + rightGrasped;
        //text.text += "\nfinger right count = " + fingerRightList.Count;
    }
}
