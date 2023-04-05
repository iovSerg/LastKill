using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	[ExecuteInEditMode]
	public enum HasClimb { Null = 0, Small = 1, Short = 2, High = 3 };
	public class DescriptionClimb
	{
		public HasClimb hasClimb;
		public Vector3 targetPosition;
		public RaycastHit raycastHit;
	}
	public class DetectionController : MonoBehaviour, IDetection
	{
		private IKController _IK;

		[Header("Param Raycast beetwen")]
		[SerializeField] private LayerMask climbLayre;
		[SerializeField] private LayerMask groundLayer;
		[SerializeField] private float lenghBeams = 0.4f;
		[SerializeField] private int countRaycastHeight = 13;
		[SerializeField] private int minRaycastHeight = 4;
		[SerializeField] private bool showDebug = true;

		private void Awake()
		{
			_IK = GetComponent<IKController>();
		}
		public float ColliderHeight()
		{
			return 0;
		}
		public bool IsGrounded()
		{
			if(Physics.SphereCast(transform.position,0.2f,-Vector3.up,out RaycastHit hit,groundLayer))
			{
				return true;
			}

			return false;
		}
		public bool CanGetUp(float offset)
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position + Vector3.up, Vector3.up, out hit, offset))
			{
				return true;
			}
			return false;
		}
		RaycastHit hit;

		public float ForwardHeight => fHeight;
		private float fHeight;
		private void OnDrawGizmos()
		{
			//Variant one
			if (showDebug)
			{
				for (int i = minRaycastHeight; i <= countRaycastHeight; i++)
				{
					if (!Physics.Raycast(transform.position + Vector3.up * (0.2f * i), transform.forward, out hit, lenghBeams, climbLayre))
					{
						Gizmos.color = Color.green;
						Gizmos.DrawRay(transform.position + Vector3.up * (0.2f * i), transform.forward * 0.5f);
						if (Physics.Raycast(transform.position + Vector3.up * (0.2f * i) + transform.forward * 0.5f, Vector3.down * 0.2f, out hit, 0.5f, climbLayre))
						{
							Gizmos.color = Color.cyan;
							Gizmos.DrawSphere(hit.point, 0.05f);
							Gizmos.color = Color.green;
							Gizmos.DrawSphere(transform.position + Vector3.up * (0.2f * i) + transform.forward * 0.5f + Vector3.down * 0.2f, 0.05f);
							Gizmos.DrawSphere(hit.point + transform.right * 0.4f, 0.05f);
							Gizmos.DrawSphere(hit.point - transform.right * 0.4f, 0.05f);
							Gizmos.color = Color.black;
						}
					}
					else
					{
						Gizmos.color = Color.red;
						Gizmos.DrawRay(transform.position + Vector3.up * (0.2f * i), transform.forward * 0.5f);
					}
				}
			}

			//Gizmos.color = Color.green;

			//Vector3 rightArm = transform.position + Vector3.up * 3f  + transform.right * 0.3f;
			//Vector3 leftArm = transform.position + Vector3.up * 3f - transform.right * 0.3f;
			//Vector3 root = transform.position + Vector3.up * 3f;

			//Gizmos.DrawRay(rightArm, transform.forward);
			//Gizmos.DrawRay(leftArm, transform.forward);
			//Gizmos.DrawRay(root, transform.forward);

			//if(Physics.Raycast(rightArm + transform.forward * 0.4f,Vector3.down,out RaycastHit rightHit,2f,climbLayre,QueryTriggerInteraction.Ignore))
			//{
			//	Gizmos.DrawSphere(rightHit.point, 0.02f);
			//}
			//if (Physics.Raycast(leftArm + transform.forward * 0.4f, Vector3.down, out RaycastHit leftHit, 2f, climbLayre, QueryTriggerInteraction.Ignore))
			//{
			//	Gizmos.DrawSphere(leftHit.point, 0.02f);
			//}
			//if (Physics.Raycast(root + transform.forward * 0.5f, Vector3.down, out RaycastHit rootHit, 2f, climbLayre, QueryTriggerInteraction.Ignore))
			//{
			//	Gizmos.DrawSphere(rootHit.point, 0.02f);
			//}
			//Gizmos.DrawRay(rightArm + transform.forward, Vector3.down * 2f);
			//Gizmos.DrawRay(leftArm + transform.forward, Vector3.down * 2f);
			//Gizmos.DrawSphere(rightHit.point - leftHit.point, 0.5f);
		}

		public RaycastHit ClimbTarget()
		{
			for (int i = minRaycastHeight; i <= countRaycastHeight; i++)
			{
				if (!Physics.Raycast(transform.position + Vector3.up * (0.2f * i), transform.forward, out hit, 0.5f, climbLayre))
				{
					if (Physics.Raycast(transform.position + Vector3.up * (0.2f * i) + transform.forward * 0.5f, Vector3.down * 0.2f, out hit, 0.5f, climbLayre))
					{
						_IK.TLeftHand.position = hit.point - transform.right * 0.4f;
						_IK.TRightHand.position = hit.point + transform.right * 0.4f;
						
						fHeight = i;
						return hit;
					}
				}

			}
			return hit;
		}
		public DescriptionClimb ClimbTargets()
		{
			Vector3 rightArm = transform.position + Vector3.up * 3f + transform.right * 0.3f;
			Vector3 leftArm = transform.position + Vector3.up * 3f - transform.right * 0.3f;
			Vector3 root = transform.position + Vector3.up * 3f;
			if (Physics.Raycast(rightArm + transform.forward * 0.4f, Vector3.down, out RaycastHit rightHit, 2f, climbLayre, QueryTriggerInteraction.Ignore))
			{
				_IK.TRightHand.position = rightHit.point;
			}
			
			if (Physics.Raycast(leftArm + transform.forward * 0.4f, Vector3.down, out RaycastHit leftHit, 2f, climbLayre, QueryTriggerInteraction.Ignore))
			{
				_IK.TLeftHand.position = leftHit.point;
			}
			if (Physics.Raycast(root + transform.forward * 0.4f, Vector3.down, out RaycastHit rootHit, 2f, climbLayre, QueryTriggerInteraction.Ignore))
			{
				float distance = rootHit.distance;
				HasClimb climb;
				if(distance < 0.5f)
				{
					climb = HasClimb.High;
				}
				else if(distance < 1.6f)
				{
					climb = HasClimb.Short;
				}
				else
				{
					climb = HasClimb.Small;
				}
				return new DescriptionClimb() { raycastHit = rootHit, targetPosition = rootHit.point, hasClimb = climb };
			}

			return new DescriptionClimb() { targetPosition = Vector3.zero, hasClimb = HasClimb.Null };
		}
	}
}
