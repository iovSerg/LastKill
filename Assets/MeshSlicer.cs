using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshSlicer : MonoBehaviour
{
    public GameObject hand;
    public GameObject hand_1;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    [ContextMenu("Hand")]
    public void HandSlice()
    {
        //hand.GetComponent<SkinnedMeshRenderer>().rootBone = null;
        hand.GetComponent<Rigidbody>().isKinematic = false;
        hand.transform.parent = null;
        hand.GetComponent<Rigidbody>().AddForce(Vector3.left * 100, ForceMode.Force);
	}
}
