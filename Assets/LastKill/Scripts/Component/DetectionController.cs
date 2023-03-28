using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class DetectionController : MonoBehaviour, IDetection
	{
		[SerializeField] private float offsetHitCrouch = 2f;
		public bool CanGetUp()
		{
			
			RaycastHit hit;
			if (Physics.Raycast(transform.position + Vector3.up, Vector3.up, out hit, offsetHitCrouch))
			{
				return true;
			}
			return false;
		}
	}
}
