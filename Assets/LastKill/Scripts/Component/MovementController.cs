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
		public bool useCameraOrientation = true;

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
		private float targetRotation = 0.0f;
		private float rotationVelocity;
		private float terminalVelocity = 53.0f;
		[SerializeField] private float initialCapsuleHeight = 1.8f;
		[SerializeField] private float initialCapsuleRadius = 0.2f;
		[SerializeField] private float initialCapsuleCenter = 0.93f;


		// variables for root motion
		private bool useRootMotion = false;
		private Vector3 rootMotionMultiplier = Vector3.one;
		private bool useRotationRootMotion = true;

		//[SerializeField] private float rotationSpeed = 15f;
		[SerializeField] private Vector3 velocity;
		[SerializeField] private float speed;

		private CharacterController _controller;
		private PlayerInput _input;

	


		private ICamera _camera;
		private IAnimator _animator;
		private void Awake()
		{
			_camera = GetComponent<ICamera>();
			_animator = GetComponent<IAnimator>();
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<PlayerInput>();

			initialCapsuleHeight = _controller.height;
			initialCapsuleRadius = _controller.radius;
			initialCapsuleCenter = _controller.center.y;
		}
		private void GravityControl()
		{
			if (Grounded)
			{
				// stop our velocity dropping infinitely when grounded
				if (velocity.y < 2f)
				{
					velocity.y = -2f;
				}
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (UseGravity && velocity.y < terminalVelocity)
			{
				velocity.y += Gravity * Time.deltaTime;
			}
		}
		[SerializeField] Vector3 rayOrigin;
		[SerializeField] Vector3 targetPosition;
		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Gizmos.DrawSphere(spherePosition, GroundedRadius);
		}
		private void Move(float targetSpeed, bool rotateCharacter = true)
		{
			

		}
		private void FixedUpdate()
		{
			GroundedCheck();
			GravityControl();

			if (useRootMotion) return;
			if (!_controller.enabled) return;

			_controller.Move(velocity * Time.fixedDeltaTime);
		}
		private void OnAnimatorMove()
		{
			if (!useRootMotion) return;

			if (_controller.enabled)
				_controller.Move(_animator.Animator.deltaPosition);
			else
				_animator.Animator.ApplyBuiltinRootMotion();

			transform.rotation *= _animator.Animator.deltaRotation;
		}
		public void Move(Vector2 moveInput, float targetSpeed, bool rotateCharacter = true)
		{
			speed = Mathf.Lerp(speed, targetSpeed * _input.Magnituda, Time.deltaTime * SpeedChangeRate);
			if (speed < 0.1f) speed = 0f;

			if (moveInput != Vector2.zero)
			{
				targetRotation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + (useCameraOrientation ? _camera.GetTransform.eulerAngles.y : 0);
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);


				if (rotateCharacter)
					transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}
				

				Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
				velocity = targetDirection.normalized * speed + new Vector3(0.0f, velocity.y, 0.0f);

		}

		public void Move(Vector2 moveInput, float targetSpeed, Quaternion cameraRotation, bool rotateCharacter = true)
		{
			
		}

		public void Move(Vector3 velocity)
		{
			Vector3 newVelocity = velocity;
			if (UseGravity)
				newVelocity.y = this.velocity.y;

			this.velocity = newVelocity;
		}

		public void StopMovement()
		{
			velocity = Vector3.zero;
			speed = 0;
		}

		public void SetVelocity(Vector3 velocity)
		{
			this.velocity = velocity;
		}

		public Vector3 GetVelocity()
		{
			return velocity;
		}

		public float GetGravity()
		{
			return Gravity;
		}

		public void EnableGravity()
		{
			UseGravity = true;
		}

		public void DisableGravity()
		{
			UseGravity = false;
		}

		public void SetPosition(Vector3 newPosition)
		{
			throw new NotImplementedException();
		}

		public Quaternion GetRotationFromDirection(Vector3 direction)
		{
			throw new NotImplementedException();
		}

		public bool IsGrounded()
		{
			return Grounded;
		}

		public Collider GetGroundCollider()
		{
			throw new NotImplementedException();
		}

		public void ApplyRootMotion(Vector3 multiplier, bool applyRotation = false)
		{
			useRootMotion = true;
			rootMotionMultiplier = multiplier;
			useRotationRootMotion = applyRotation;
		}

		public void StopRootMotion()
		{
			useRootMotion = false;
			useRotationRootMotion = false;
		}

		public Vector3 GetRelativeInput(Vector2 input)
		{
			throw new NotImplementedException();
		}

		public void SetCapsuleSize(float newHeight, float newRadius)
		{
			_controller.radius = newRadius;
			_controller.height = newHeight;

			_controller.center = new Vector3(0, newHeight * 0.5f, 0);
		}

		public void ResetCapsuleSize()
		{
			SetCapsuleSize(initialCapsuleHeight, initialCapsuleRadius);
		}

		public float GetCapsuleHeight()
		{
			return _controller.height;
		}

		public float GetCapsuleRadius()
		{
			return _controller.radius;
		}
		public float GetCapsuleCenter()
		{
			return _controller.center.y;
		}

		public void EnableCollision()
		{
			_controller.enabled = true;
		}

		public void DisableCollision()
		{
			_controller.enabled = false;
		}

		
	}
}
