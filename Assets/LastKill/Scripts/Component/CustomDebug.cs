
using UnityEngine;

namespace LastKill
{
	public class CustomDebug : MonoBehaviour, IDebug
	{
		public bool debugLog = false;
		public bool debugCrouch = false;

		[SerializeField] private LayerMask shortClimbMask;
		[SerializeField] private float overlapRadius = 0.75f;
		[SerializeField] private float capsuleCastRadius = 0.2f;
		[SerializeField] private float capsuleCastHeight = 1f;
		[SerializeField] private float minClimbHeight = 0.5f;
		[SerializeField] private float maxClimbHeight = 1.5f;

		public bool ShowDebugLog { get => debugLog; set => debugLog = value	; }
		public bool ShowDebugCrouch { get => debugCrouch; set => debugCrouch = value; }
		public bool ShowDebugCrawl { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public bool ShowDebugRoll { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public bool ShowDebugShortClimb { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

		public void DebugCrouch(float height, float hit)
		{
			Debug.DrawRay(transform.position + Vector3.up * height, transform.forward * hit, Color.green);
			Debug.DrawRay(transform.position, Vector3.up * hit, Color.green);
		}

		private void Update()
		{
		
		}
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;

			Vector3 p1 = transform.position + Vector3.up * (minClimbHeight + capsuleCastRadius);
			Vector3 p2 = transform.position + Vector3.up * (capsuleCastHeight - capsuleCastRadius);
			Vector3 overlapCenter = transform.position + Vector3.up * overlapRadius;

			Gizmos.DrawWireSphere(overlapCenter, 0.2f);
			//Gizmos.DrawWireSphere(p1, 0.2f);
			//Gizmos.DrawWireSphere(p2, 0.2f);

		}
	}
}
