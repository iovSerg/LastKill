using UnityEngine;

namespace LastKill
{
	[CreateAssetMenu(fileName = "Camera data", menuName = "LastKill/Camera data", order = 1)]
	public class CameraData : ScriptableObject
	{
		public Vector3 position;
		public Vector3 ShoulderOffset;
		public CameraState stateCamera;
		[Range(0, 1)]
		public float CameraSide;
		public float CameraDistance;
		public string cameraName;
		public string tagName;
		public int priority;
	}
}
