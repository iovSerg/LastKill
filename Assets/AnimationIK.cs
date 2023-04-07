using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AnimationIK : MonoBehaviour
{
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform leftArm;


    [SerializeField] private Transform rightFoot;
    [SerializeField] private Transform leftFoot;

    [SerializeField] private Transform rightLeg;
    [SerializeField] private Transform leftLeg;

    [SerializeField] private Transform Head;
    [SerializeField] private Transform Spine;

    [SerializeField] private float weight;


    [SerializeField] private Vector3 spineTarget = Vector3.zero;
    [SerializeField] Quaternion rotation;

    public float xAngle, yAngle, zAngle;
    [ContextMenu("RotateSpine")]
    public void RotateSpine()
	{
        Spine.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
        Spine.transform.Rotate(xAngle, yAngle, zAngle, Space.World);
    }
    HumanBodyBones human;
    Animator animator;
	private void Awake()
	{
        animator = GetComponent<Animator>();
        Spine = animator.GetBoneTransform(HumanBodyBones.Spine);

        human = HumanBodyBones.Spine;
	}
	private void Update()
	{
        

    }
	private void OnAnimatorIK(int layerIndex)
	{
        //Hand
        //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
        //animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);

        //animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
        //animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);

        animator.SetBoneLocalRotation(human, rotation);



        animator.SetLookAtPosition(Head.transform.position);
        animator.SetLookAtWeight(weight);

    }


}
