
using UnityEngine;

namespace LastKill
{
	public class CustomDebug : MonoBehaviour, IDebug
	{
		[SerializeField] private bool showDebug;
		[SerializeField] private bool showDebugDrawRay;
		public bool ShowDebugLog { get => showDebug; set => showDebug = value; }
		public bool ShowDebugDrawRay { get => showDebugDrawRay; set => showDebugDrawRay = value; }

		public void ShowCapsuleCrouch()
		{
			
		}
		

	}
}
