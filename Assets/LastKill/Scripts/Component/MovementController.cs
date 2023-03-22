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


		[SerializeField] private float rotationSpeed = 15f;
		[SerializeField] private Vector3 velocity;
		[SerializeField] private float speed;

		private CharacterController _controller;
		private PlayerInput _input;

	


		private ICamera _camera;

		private void Awake()
		{
			_camera = GetComponent<ICamera>();
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<PlayerInput>();

			initialCapsuleHeight = _controller.height;
			initialCapsuleRadius = _controller.radius;
			initialCapsuleCenter = _controller.center.y;
		}
		private void GravityControl()
		{
			if (_controller.isGrounded)
			{
				// stop our velocity dropping infinitely when grounded
				if (velocity.y < 2.0f)
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
		private void GroundedCheck()
		{
			// set sphere position, with offset
			//Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			//Grounded = Physics.CheckSphere(spherePosition, _controller.radius, GroundLayers, QueryTriggerInteraction.Ignore);
			//RaycastHit hit;
			//Grounded = Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), Vector3.down, out hit, 0.5f);
			Grounded = Physics.CheckSphere(transform.position, GroundedRadius, GroundLayers);
			if (!Grounded && !_controller.isGrounded) return;
		}
		private void Move(float targetSpeed, bool rotateCharacter = true)
		{
			

		}
		private void FixedUpdate()
		{
			GroundedCheck();
			GravityControl();

			if (_input.Move == Vector2.zero) return;

			_controller.Move(velocity * Time.deltaTime);
		}

		public void Move(Vector2 moveInput, float targetSpeed, bool rotateCharacter = true)
		{
			speed = Mathf.Lerp(speed, targetSpeed * _input.Magnituda, Time.deltaTime * SpeedChangeRate);
			if (speed < 0.3f) speed = 0f;

			if (moveInput != Vector2.zero)
			{
				targetRotation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + (useCameraOrientation ? _camera.GetTransform.eulerAngles.y : 0);
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);


				if (rotateCharacter)
					transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

				Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
				velocity = targetDirection.normalized * speed + new Vector3(0.0f, velocity.y, 0.0f);

			}
		}

		public void Move(Vector2 moveInput, float targetSpeed, Quaternion cameraRotation, bool rotateCharacter = true)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public void SetVelocity(Vector3 velocity)
		{
			throw new NotImplementedException();
		}

		public Vector3 GetVelocity()
		{
			throw new NotImplementedException();
		}

		public float GetGravity()
		{
			throw new NotImplementedException();
		}

		public void EnableGravity()
		{
			throw new NotImplementedException();
		}

		public void DisableGravity()
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public void StopRootMotion()
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public void DisableCollision()
		{
			throw new NotImplementedException();
		}

		
	}
}
