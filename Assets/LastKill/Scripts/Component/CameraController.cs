using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class CameraController : MonoBehaviour ,ICamera
	{
		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public Transform followCamera;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;
		[Tooltip("Speed of camera turn")]
		public Vector2 CameraTurnSpeed = new Vector2(300.0f, 200.0f);
		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;


	[SerializeField]	private float cameraInputX;
	[SerializeField]	private float cameraInputY;
		[SerializeField] private float sensivity = 5f;

		private Camera _camera;
		private IInput _input;

		public float Sensivity { get => sensivity; set => sensivity = value; }

		private void Awake()
		{
			_camera = Camera.main;
			_input = GetComponent<IInput>();
		}
		private void LateUpdate()
		{
			CameraRotation();
		}
		private void CameraRotation()
		{
			cameraInputX += _input.Look.x * CameraTurnSpeed.x * sensivity * Time.deltaTime;
			cameraInputY += _input.Look.y * CameraTurnSpeed.y * sensivity * Time.deltaTime;

			cameraInputX = ClampAngle(cameraInputX, float.MinValue, float.MaxValue);
			cameraInputY = ClampAngle(cameraInputY, BottomClamp, TopClamp);

			followCamera.transform.rotation = Quaternion.Euler(cameraInputY,cameraInputX , 0f);
		}
		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}
		public Vector3 GetCameraDirection(Vector2 moveInput)
		{
			Vector3 moveDirection = _camera.transform.forward * moveInput.y;
			moveDirection += _camera.transform.right * moveInput.x;
			moveDirection.Normalize();
		    moveDirection.y = 0f;

			return moveDirection;
		}

	}
}
