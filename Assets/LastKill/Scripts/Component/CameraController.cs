using Cinemachine;
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

		[SerializeField] public float cameraPositionChangeRate = 2f;

		[SerializeField]private  Transform crouchCamera;
		[SerializeField]private  Transform locomotionCamera;
		[SerializeField]private  Transform aimCamera;

		[SerializeField] private Cinemachine3rdPersonFollow bodyFolow;
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;

		[SerializeField] private float cameraInputX;
		[SerializeField] private float cameraInputY;
		[SerializeField] private float sensivity = 5f;

		[SerializeField] float bodyCanAim;
		[SerializeField] float bodyAim;

		private Camera _camera;
		private IInput _input;

		public float Sensivity { get => sensivity; set => sensivity = value; }
		public Transform GetTransform => _camera.transform;

		public CameraData[] cameras;
		public CinemachineVirtualCameraBase.TransitionParams m_Transitions;

		private void Awake()
		{
			//Download camera settings
			cameras = Resources.LoadAll<CameraData>("Camera/");

			if (cameras != null)
			{
				GameObject targetCamera = new GameObject("CameraTarget");
				targetCamera.transform.SetParent(this.transform);
				targetCamera.transform.localPosition = Vector3.zero;
				foreach (CameraData camera in cameras)
					CameraSetup(targetCamera.transform, camera);
			}
			if (GameObject.FindAnyObjectByType<CinemachineVirtualCamera>() == null)
			{
				GameObject followCamera = new GameObject("PlayerFollowCamera");
				followCamera.AddComponent<CinemachineVirtualCamera>();
				_virtualCamera = followCamera.GetComponent<CinemachineVirtualCamera>();
				_virtualCamera.Follow = locomotionCamera;

				bodyFolow = new Cinemachine3rdPersonFollow();
				//_virtualCamera.m_Transitions = bodyFolow;
			}
			else
			{
				_virtualCamera = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>();
				_virtualCamera.Follow = locomotionCamera;
				bodyFolow = _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
			}
			_camera = Camera.main;
			_input = GetComponent<IInput>();

		}

		private void LateUpdate()
		{
			CameraRotation();

			if(_input.Crouch || _input.Crawl)
			{
				currentCamera = crouchCamera;
				_virtualCamera.Follow = crouchCamera; 
				bodyFolow.CameraDistance = 4;
			}
			else if(_input.Aim)
			{
				if(bodyFolow.CameraDistance > 1f)
					bodyFolow.CameraDistance = bodyFolow.CameraDistance - Time.deltaTime * cameraPositionChangeRate;

				bodyFolow.ShoulderOffset.y = 0.2f;
			}
			else
			{

				if (bodyFolow.CameraDistance < 4f)
					bodyFolow.CameraDistance = bodyFolow.CameraDistance + Time.deltaTime * cameraPositionChangeRate;

				bodyFolow.ShoulderOffset.y = 0f;
				currentCamera = locomotionCamera;
				_virtualCamera.Follow = locomotionCamera;
			}
		}

		private void CameraSetup(Transform parent, CameraData camera)
		{
			GameObject temp = new GameObject(camera.name);
			temp.transform.SetParent(parent);
			temp.transform.localPosition = camera.position;

			switch (camera.cameraName)
			{
				case "Locomotion":
					locomotionCamera = temp.transform;
					currentCamera = locomotionCamera;
					break;
				case "Crouch":
					crouchCamera = temp.transform;
					break;
				case "Aim":
					aimCamera = temp.transform;
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
