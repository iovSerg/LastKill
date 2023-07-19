using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class CameraController : MonoBehaviour ,ICamera
	{
		private Camera _camera;
		private PlayerInput _input;
		private AbilityState _abilityState;
		private CameraData[] cameraData;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	    private Cinemachine3rdPersonFollow _bodyFolow;
		private CinemachineVirtualCamera _virtualCamera;
		private Transform _followCamera;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;

		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		[Header("State Camera")]
		[SerializeField] private CameraData currentCameraData;


		[SerializeField] private float speedChangeRate = 5f;
		[SerializeField] private float cameraInputX;
		[SerializeField] private float cameraInputY;
		[SerializeField] private float sensivity = 5f;
		private float timer = 0;

		public float Sensivity { get => sensivity; set => sensivity = value; }
		public Transform GetTransform => _camera.transform;

		private void Awake()
		{
			//Download camera settings
			cameraData = Resources.LoadAll<CameraData>("Camera/");
			
			_camera = Camera.main;
			_input = GetComponent<PlayerInput>();

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
			timer = 0f;
			SetCurrentCameraData(obj.cameraState);
		}

		private void OnStateStop(AbstractAbilityState obj)
		{
			timer = 0f;
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

		private void LateUpdate()
		{
			CameraRotation();
			//if(timer < 2f)
			//{
			//	timer += Time.deltaTime;
			//	//_followCamera.localPosition = Vector3.Lerp(_followCamera.localPosition, currentCameraData.position, Time.deltaTime * speedChangeRate);
			//	_bodyFolow.ShoulderOffset = Vector3.Lerp(_bodyFolow.ShoulderOffset, currentCameraData.ShoulderOffset, Time.deltaTime * speedChangeRate);
			//	_bodyFolow.CameraDistance = Mathf.Lerp(_bodyFolow.CameraDistance, currentCameraData.CameraDistance, Time.deltaTime * speedChangeRate);

			//	//??? Not working
			//	//_bodyFolow.CameraSide = Mathf.Lerp(_bodyFolow.CameraSide, currentCameraData.CameraSide, Time.deltaTime * speedChangeRate);
			//	//_bodyFolow.CameraSide = currentCameraData.CameraSide;
			//}
		}
		private void SceneCameraSetup()
		{
			if (cameraData != null)
			{
				GameObject cameraFollow = new GameObject("CameraTarget");
			
				cameraFollow.transform.SetParent(transform,true);
				cameraFollow.transform.localPosition = new Vector3(0,1.5f,0);
				_followCamera = cameraFollow.transform;
			}
			//Learn how to change body(Do nothing) change 3rd follow
			//Узнать как изменить Body(Do nothing) на 3rd Follow
			if (GameObject.FindAnyObjectByType<CinemachineVirtualCamera>() == null)
			{
				GameObject followCamera = new GameObject("PlayerFollowCamera");
				followCamera.AddComponent<CinemachineVirtualCamera>();
				_virtualCamera = followCamera.GetComponent<CinemachineVirtualCamera>();
				_virtualCamera.Follow = _followCamera;

				//bodyFolow = new Cinemachine3rdPersonFollow();
				//_virtualCamera.m_Transitions = bodyFolow;
			}
			else
			{
				_virtualCamera = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>();
				_bodyFolow = _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
				_virtualCamera.Follow = _followCamera;

				///Temp
				_bodyFolow.CameraDistance = 3;
				_bodyFolow.CameraSide = 0.8f;
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

			_followCamera.transform.rotation = Quaternion.Euler(cameraInputY,cameraInputX , 0f);
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
