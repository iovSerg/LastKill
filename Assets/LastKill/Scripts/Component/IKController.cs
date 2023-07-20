using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class IKController : MonoBehaviour
	{
		[SerializeField] private Transform targetLeftHand;
		[SerializeField] private Transform targetRightHand;

		[SerializeField] private Transform targetRightFoot;
		[SerializeField] private Transform targetLeftFoot;
		[SerializeField] private Transform lookAtPosition;

		 public GameObject currentWeapon;

		[SerializeField] private Transform TempLeftHand;

		private Animator _animator;
		[SerializeField] private float leftHandWeight = 0f;
		[SerializeField] private float rightHandWeight = 0f;

		[SerializeField] private float lookWeight;
		[SerializeField] private float bodyWeight;
		[SerializeField] private float headWeight;
		[SerializeField] private float eyesWeight;
		[SerializeField] private float clampWeight;

		public Transform TLeftHand { get => targetLeftHand; set {
				targetLeftHand = value;
			} }
		public Transform TRightHand { get => targetRightHand; set => targetRightHand = value; }
		public float WLeftHand { get => leftHandWeight; set => leftHandWeight = value; }
		public float WRightHand { get => rightHandWeight; set => rightHandWeight = value; }

		public Vector3 right;
		public Vector3 left;


		private void OnDrawGizmos()
		{
			Gizmos.color = Color.black;
			if(targetLeftHand != null && targetRightHand != null)
			{
				Gizmos.DrawSphere(targetLeftHand.position, 0.05f);
				Gizmos.DrawSphere(targetRightHand.position, 0.05f);
			}
		}
		[SerializeField] private GameObject leftFoot;
		private void Awake()
		{
			GameObject target = new GameObject("TargetIK");

			target.transform.SetParent(transform);
			target.transform.localPosition = Vector3.zero;

			//targetLeftHand = new GameObject("LeftHandIK").transform.parent = target.transform;
			//targetRightHand = new GameObject("RightHandIK").transform.parent = target.transform;

			//targetLeftFoot = new GameObject("LeftFootIK").transform.parent = target.transform;
			//targetRightFoot = new GameObject("RightFootIK").transform.parent = target.transform;

			_animator = GetComponent<Animator>();

		}
		public bool debug = false;
		private void Update()
		{
			if(debug)
			{

				if(TempLeftHand != null)
				{
					TempLeftHand = currentWeapon.GetComponent<Transform>().Find("leftHandIK");
					TempLeftHand = GameObject.Find("leftHandIK").transform;
				}
				targetLeftHand = TempLeftHand;
			}	
			//leftHandWeight = _animator.GetFloat("LeftHand");
			//rightHandWeight = _animator.GetFloat("RightHand");

		}
		public Vector3 offsetLookAtPosition = Vector3.zero;
		private void OnAnimatorIK(int layerIndex)
		{
			
			_animator.SetLookAtPosition(lookAtPosition.position - offsetLookAtPosition);
			_animator.SetLookAtWeight(lookWeight, bodyWeight, headWeight,eyesWeight,clampWeight);
			//LeftHand
			if(targetLeftHand != null)
			{
				_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
				_animator.SetIKPosition(AvatarIKGoal.LeftHand, targetLeftHand.position);

				_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
				_animator.SetIKRotation(AvatarIKGoal.LeftHand, targetLeftHand.rotation);
			}

			//RightHand
			if(targetRightHand != null)
			{
				_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
				_animator.SetIKPosition(AvatarIKGoal.RightHand, targetRightHand.position);

				_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
				_animator.SetIKRotation(AvatarIKGoal.RightHand, targetRightHand.rotation);
			}

			//RightFoot
			if(targetRightFoot != null)
			{

			}

			//LeftFoot
			if(targetLeftFoot != null)
			{

			}
		}

		

	}
}
