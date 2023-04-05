using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class CameraController : MonoBehaviour ,ICamera
	{
		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public Transform currentCamera;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;

		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		[SerializeField] public float speedChangeRate = 2f;

		[SerializeField] private  Transform _crouchCamera;
		[SerializeField] private  Transform _locomotionCamera;
		[SerializeField] private  Transform _aimCamera;

		[SerializeField] private Cinemachine3rdPersonFollow _bodyFolow;
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;

		[SerializeField] private float cameraInputX;
		[SerializeField] private float cameraInputY;
		[SerializeField] private float sensivity = 5f;

		[SerializeField] float bodyCanAim;
		[SerializeField] float bodyAim;

		private Camera _camera;
		private IInput _input;
		private AbilityState _abilityState;

		public float Sensivity { get => sensivity; set => sensivity = value; }
		public Transform GetTransform => _camera.transform;

		[SerializeField] private Transform newPositionCamera;
		 private CameraData[] cameraData;
		 private CameraData currentCameraData;
		private void Awake()
		{
			//Download camera settings
			cameraData = Resources.LoadAll<CameraData>("Camera/");
			
			_camera = Camera.main;
			_input = GetComponent<IInput>();

			_abilityState = GetComponent<AbilityState>();
			_abilityState.OnStateStart += OnStateStart;
			_abilityState.OnStateStop += OnStateStop;
		}
		private void Start()
		{
			SetCurrentCameraData(CameraState.Locomotion);
			SceneCameraSetup();
		}
		private void OnStateStart(AbstractAbilityState obj)
		{
			SetCurrentCameraData(obj.cameraState);
		}

		private void OnStateStop(AbstractAbilityState obj)
		{
			SetCurrentCameraData(obj.cameraState);
		}
		private void SetCurrentCameraData(CameraState state)
		{
			foreach (CameraData data in cameraData)
				if (data.stateCamera == state)
				{
					currentCameraData = data;
				}
		}
		[SerializeField] private Vector3 follow;
		private void LateUpdate()
		{
			CameraRotation();
			if(follow.y != currentCameraData.position.y)
			{
				follow = Vector3.Lerp(currentCamera.localPosition, currentCameraData.position, Time.deltaTime * speedChangeRate);
				currentCamera.localPosition = follow;
			}
		}
		private void SceneCameraSetup()
		{
			if (cameraData != null)
			{
				GameObject cameraFollow = new GameObject("CameraTarget");
			
				cameraFollow.transform.SetParent(transform,true);
				cameraFollow.transform.localPosition = Vector3.zero;

				foreach (CameraData camera in cameraData)
					CameraSetup(cameraFollow.transform, camera);

				GameObject cameraFree = new GameObject("FreeCamera");
				cameraFree.transform.SetParent(cameraFollow.transform,true);
				cameraFree.transform.localPosition = Vector3.zero;
				cameraFree.transform.position = _locomotionCamera.position;
				currentCamera = cameraFree.transform;
			}
			//Read documentation body(Do nothing) change 3rd follow
			if (GameObject.FindAnyObjectByType<CinemachineVirtualCamera>() == null)
			{
				GameObject followCamera = new GameObject("PlayerFollowCamera");
				followCamera.AddComponent<CinemachineVirtualCamera>();
				_virtualCamera = followCamera.GetComponent<CinemachineVirtualCamera>();
				_virtualCamera.Follow = currentCamera;

				//bodyFolow = new Cinemachine3rdPersonFollow();
				//_virtualCamera.m_Transitions = bodyFolow;
			}
			else
			{
				_virtualCamera = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>();
				_bodyFolow = _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
				_virtualCamera.Follow = currentCamera;
				newPositionCamera = currentCamera;
			}
		}
		private void CameraSetup(Transform parent, CameraData camera)
		{
			GameObject temp = new GameObject(camera.name);
			temp.transform.SetParent(parent,true);
			temp.transform.localPosition = camera.position;

			switch (camera.stateCamera)
			{
				case CameraState.Locomotion:
					_locomotionCamera = temp.transform;
					break;
				case CameraState.Crouch:
					_crouchCamera = temp.transform;
					break;
				case CameraState.Aim:
					_aimCamera = temp.transform;
					break;
			}
		}
		private float BodyDistance(float a, float b, bool invert)
		{
			return Mathf.Lerp(a, b, invert ? -Time.deltaTime: Time.deltaTime);
		}
		private void CameraRotation()
		{
			cameraInputY += _input.Look.y  * sensivity;
			cameraInputX += _input.Look.x *  sensivity;

			cameraInputX = ClampAngle(cameraInputX, float.MinValue, float.MaxValue);
			cameraInputY = ClampAngle(cameraInputY, BottomClamp, TopClamp);

			currentCamera.transform.rotation = Quaternion.Euler(cameraInputY,cameraInputX , 0f);
		}
		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
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

	}
}
