using LastKill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private InputController _input;
	private Camera _camera;	

	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	public GameObject CinemachineCameraTarget;

	[Tooltip("How far in degrees can you move the camera up")]
	public float TopClamp = 70.0f;

	[Tooltip("How far in degrees can you move the camera down")]
	public float BottomClamp = -30.0f;

	[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
	public float CameraAngleOverride = 0.0f;

	[Tooltip("For locking the camera position on all axis")]
	public bool LockCameraPosition = false;

	// cinemachine
	[SerializeField] private float _cinemachineTargetYaw;
	[SerializeField] private float _cinemachineTargetPitch;

	[SerializeField, Range(0.0f, 10.0f)] private float sensivity;


	private const float _threshold = 0.01f;

	public bool IsCurrentDeviceMouse = true;


	private void Start()
	{
		_camera = Camera.main;
		_input = GetComponent<InputController>();
	}

	private void LateUpdate()
	{
	}
	private void FixedUpdate()
	{
		CameraRotation();
		
	}
	public Vector3 GetCameraDirection(Vector2 moveInput)
	{
		Vector3 moveDirection = Vector3.zero;
		moveDirection = _camera.transform.forward * moveInput.y;
		moveDirection += _camera.transform.right * moveInput.x;

		moveDirection.Normalize();
		moveDirection.y = 0f;

		return moveDirection;
	}
	private void CameraRotation()
	{
		// if there is an input and camera position is not fixed
		if (_input.Look.sqrMagnitude >= _threshold)
		{
			//Don't multiply mouse input by Time.deltaTime;
			float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

			_cinemachineTargetYaw += _input.Look.x * deltaTimeMultiplier * sensivity;
			_cinemachineTargetPitch += _input.Look.y * deltaTimeMultiplier * sensivity;
		}

		// clamp our rotations so our values are limited 360 degrees
		_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
		_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

		// Cinemachine will follow this target
		CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
			_cinemachineTargetYaw, 0.0f);
	}
	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}
}
