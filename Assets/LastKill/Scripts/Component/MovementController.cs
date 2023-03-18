using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class MovementController : MonoBehaviour , IMove, ICapsule
	{
		[Header("Player")]
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;
		public bool _useCameraOrientation = true;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Gravity")]
		[Tooltip("Should apply gravity?")]
		[SerializeField] private bool UseGravity = true;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		[SerializeField] private float Gravity = -15.0f;


		// player
		private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _terminalVelocity = 53.0f;
		private float _initialCapsuleHeight = 2f;
		private float _initialCapsuleRadius = 0.28f;


		[SerializeField] private float rotationSpeed = 15f;
		[SerializeField] private Vector3 velocity;
		// animation ID


		private Animator _animator;
		private CharacterController _controller;
		private PlayerInput _input;



		private ICamera _camera;

		private void Awake()
		{
			_camera = GetComponent<ICamera>();
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<PlayerInput>();

			_initialCapsuleHeight = _controller.height;
			_initialCapsuleRadius = _controller.radius;
		}

		private void Move(float speed, bool rotateCharacter = true)
		{
			Vector3 targetDirection = _camera.GetCameraDirection(_input.Move);
			_controller.Move(targetDirection * speed *  Time.deltaTime);


			if (targetDirection == Vector3.zero) 
				targetDirection = transform.forward;

			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
			Quaternion playerRotatin = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

			transform.rotation = playerRotatin;
		}
		private void FixedUpdate()
		{
			Move(5f);
		}
	}
}
