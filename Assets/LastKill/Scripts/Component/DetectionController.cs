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
		[SerializeField] private LayerMask climbLayre;
		private void Awake()
		{

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
			//Gizmos.color = Color.green;
			//Gizmos.DrawRay(transform.position + Vector3.up * 1.2f, transform.forward * 0.5f);

			//if(Physics.Raycast(transform.position + Vector3.up * 1.2f,transform.forward,out hit,0.5f,climbLayre))
			//{
			//	Gizmos.color = Color.red;
			//	Gizmos.DrawSphere(hit.point, 0.05f);
			//}
		}
		private void OnDrawGizmosSelected()
		{
			for (int i = 4; i <= 14; i++)
			{

				if (Physics.Raycast(transform.position + Vector3.up * (0.2f * i), transform.forward, out hit, 0.5f, climbLayre))
				{
					Debug.DrawRay(transform.position + Vector3.up * (0.2f * i), transform.forward * 0.5f, Color.green);
					Gizmos.color = Color.green;
					Gizmos.DrawSphere(hit.point, 0.05f);
				}
				else
				{
					Debug.DrawRay(transform.position + Vector3.up * (0.2f * i), transform.forward * 0.5f, Color.red);
					Gizmos.color = Color.red;
					Gizmos.DrawSphere(transform.position + Vector3.up * (0.2f * i) + transform.forward * 0.5f, 0.02f);
					if (Physics.Raycast(transform.position + Vector3.up * (0.2f * i) + transform.forward * 0.5f, Vector3.down * 0.2f, out hit, 0.5f, climbLayre))
					{
						Gizmos.color = Color.blue;
						Gizmos.DrawSphere(hit.point, 0.02f);
					}
				}

			}
			//Gizmos.color = Color.yellow;
			//Gizmos.DrawRay(transform.position + Vector3.up + transform.forward, Vector3.down);
			//Gizmos.color = Color.yellow;
			//Gizmos.DrawRay(transform.position + Vector3.up + transform.forward, Vector3.down);
			//Gizmos.color = Color.blue;
			//Gizmos.DrawRay(transform.position + Vector3.up * 1.4f,transform.forward * 0.5f);
			//Gizmos.DrawRay(transform.position + Vector3.up * 1.4f + transform.forward * 0.5f, Vector3.down * 0.2f);

		}
		public Vector3 ClimbTarget()
		{
			for (int i = 4; i <= 14; i++)
			{

				if (!Physics.Raycast(transform.position + Vector3.up * (0.2f * i), transform.forward, out hit, 0.5f, climbLayre))
				{
					if (Physics.Raycast(transform.position + Vector3.up * (0.2f * i) + transform.forward * 0.5f, Vector3.down, out hit, 0.5f, climbLayre))
					{
						fHeight = i;
						return transform.position + Vector3.up * (0.2f * i) + transform.forward * 0.5f;
					}
				}

			}
			return Vector3.zero;
		}
		public DescriptionClimb ClimbTargets()
		{

			if (Physics.Raycast(transform.position + Vector3.up + transform.forward, Vector3.down, out hit, 0.5f, climbLayre))
			{
				Debug.Log("Small");
				return new DescriptionClimb() { targetPosition = Vector3.zero, raycastHit = hit, hasClimb = HasClimb.Small };
			}
			if(Physics.Raycast(transform.position + Vector3.up * 1.4f + transform.forward * 0.5f,transform.forward * 0.5f))
			{
				if(Physics.Raycast(transform.position + Vector3.up * 1.4f + transform.forward * 0.5f,Vector3.down * 0.2f, out hit))
				{
					Debug.Log("Short");
					return new DescriptionClimb() { targetPosition = Vector3.zero, raycastHit = hit, hasClimb = HasClimb.Short };
				}
			}
			if (Physics.Raycast(transform.position + Vector3.up * 2f + transform.forward * 0.5f, transform.forward * 0.5f))
			{
				if (Physics.Raycast(transform.position + Vector3.up * 1.4f + transform.forward * 0.5f, Vector3.down * 0.2f, out hit))
				{
					Debug.Log("High");
					return new DescriptionClimb() { targetPosition = Vector3.zero, raycastHit = hit, hasClimb = HasClimb.Short };
				}
			}


			return new DescriptionClimb() { targetPosition = Vector3.zero, hasClimb = HasClimb.Null };
		}
	}
}
