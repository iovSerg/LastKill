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

		private Animator _animator;
		[SerializeField] private float weightLeftHand = 0f;
		[SerializeField] private float weightRightHand = 0f;

		public Transform TLeftHand { get => targetLeftHand; set => targetLeftHand = value; }
		public Transform TRightHand { get => targetRightHand; set => targetRightHand = value; }
		public float WLeftHand { get => weightLeftHand; set => weightLeftHand = value; }
		public float WRightHand { get => weightRightHand; set => weightRightHand = value; }

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
		private void Update()
		{
			//weightLeftHand = _animator.GetFloat("LeftHand");
			//weightRightHand = _animator.GetFloat("RightHand");
		}

		private void OnAnimatorIK(int layerIndex)
		{
			//LeftHand
			if(targetLeftHand != null)
			{
				_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weightLeftHand);
				_animator.SetIKPosition(AvatarIKGoal.LeftHand, targetLeftHand.position);

				_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weightLeftHand);
				_animator.SetIKRotation(AvatarIKGoal.LeftHand, targetLeftHand.rotation);
			}

			//RightHand
			if(targetRightHand != null)
			{
				_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weightRightHand);
				_animator.SetIKPosition(AvatarIKGoal.RightHand, targetRightHand.position);

				_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weightRightHand);
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
