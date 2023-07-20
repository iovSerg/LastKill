using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{

    public class IKController : MonoBehaviour
	{
		private Animator _animator;
		private ICamera _camera;
		[SerializeField] private Transform targetLeftHand;
		[SerializeField] private Transform targetRightHand;

		[SerializeField] private Transform targetRightFoot;
		[SerializeField] private Transform targetLeftFoot;
		[SerializeField] private Transform lookAtPosition;

		[SerializeField] private Transform bodySpine;

		 public GameObject currentWeapon;

		[SerializeField] private float leftHandWeight = 0f;
		[SerializeField] private float rightHandWeight = 0f;

		[SerializeField] private float lookWeight;
		[SerializeField] private float bodyWeight;
		[SerializeField] private float headWeight;
		[SerializeField] private float eyesWeight;
		[SerializeField] private float clampWeight;

		public Transform TLeftHand { get => targetLeftHand; set { targetLeftHand = value;} }
		public Transform TRightHand { get => targetRightHand; set => targetRightHand = value; }
		public float WLeftHand { get => leftHandWeight; set => leftHandWeight = value; }
		public float WRightHand { get => rightHandWeight; set => rightHandWeight = value; }


		private void OnDrawGizmos()
		{
			Gizmos.color = Color.black;
			if(targetLeftHand != null && targetRightHand != null)
			{
				Gizmos.DrawSphere(targetLeftHand.position, 0.05f);
				Gizmos.DrawSphere(targetRightHand.position, 0.05f);
			}
		}

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_camera = GetComponent<ICamera>();
		}
		private void Update()
		{
			leftHandWeight = _animator.GetFloat("LeftHand");
		}

		private void OnAnimatorIK(int layerIndex)
		{
			_animator.SetLookAtPosition(lookAtPosition.position);
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
