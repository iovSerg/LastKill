using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class JumpAndFalling : AbstractAbilityState
	{
		[Header("Animation State")]
		[SerializeField] private string s_animJumpState = "JumpLanding.Jump";
		[SerializeField] private string s_animFallState = "JumpLanding.Falling";
		[SerializeField] private string s_animHardLandState = "JumpLanding.HardLand";
		[SerializeField] private string s_animSoftLandState = "JumpLanding.SoftLand";
		[SerializeField] private string s_animDeathState = "JumpLanding.Death";
		private string currentAnimState;
		
		[Header("Jump parameters")]
		[SerializeField] private float jumpHeight = 1.2f;
		[SerializeField] private float speedOnAir = 6f;
		[SerializeField] private float airControl = 0.5f;
		[Header("Landing")]
		[SerializeField] private float heightForSoftLand = 2f;
		[SerializeField] private float heightForHardLand = 4f;
		[SerializeField] private float heightForKillOnLand = 7f;
		[Header("Sound FX")]
		[SerializeField] private AudioClip jumpEffect;
		[SerializeField] private AudioClip hardLandClip;
		[SerializeField] private AudioClip softLandClip;
		[SerializeField] private AudioClip deathClip;

		private int hashJump;
		private int hashFalling;
		private int hashHardLand;
		private int hashSoftLand;
		private int hashCurrentAnim;
		

		private float startSpeed;
		private Vector2 startInput;

		private Vector2 inputVel;
		private float angleVel;

		private float targetRotation;

		// vars to control landing
		private float highestPosition = 0;
		private bool landing = false;

		private void Awake()
		{
			hashJump = Animator.StringToHash(s_animJumpState);
			hashFalling = Animator.StringToHash(s_animFallState);
			hashHardLand = Animator.StringToHash(s_animHardLandState);
			hashSoftLand = Animator.StringToHash(s_animSoftLandState);
		}
		private void DD()
		{
		}

		public override void OnStartState()
		{

			

			startInput = _input.Move;
			targetRotation = _camera.GetTransform.eulerAngles.y;

			if (_input.Jump && _move.IsGrounded())
				PerformJump();
			else
			{
				_animator.SetAnimationState(hashFalling, 0, 0.25f);
				startSpeed = Vector3.Scale(_move.GetVelocity(), new Vector3(1, 0, 1)).magnitude;

				startInput.x = Vector3.Dot(_camera.GetTransform.right, transform.forward);
				startInput.y = Vector3.Dot(Vector3.Scale(_camera.GetTransform.forward, new Vector3(1, 0, 1)), transform.forward);

				//_startInput = _cameraController.GetCameraDirection();

				if (startSpeed > 3.5f)
					startSpeed = speedOnAir;
			}

			highestPosition = transform.position.y;
			landing = false;
		}

		private void PerformJump()
		{
			Vector3 velocity = _move.GetVelocity();
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * _move.GetGravity());

			_move.SetVelocity(velocity);
			_animator.SetAnimationState(hashJump, 0, 0.1f);
			startSpeed = speedOnAir;

			if (startInput.magnitude > 0.1f)
				startInput.Normalize();

			//_audio.PlayEffect(jumpEffect);
		}

		public override bool ReadyToStart()
		{
			return _input.Jump || !_move.IsGrounded();
		}

		public override void UpdateState()
		{
			if (landing)
			{
				// apply root motion
				_move.ApplyRootMotion(Vector3.one, false);

				// wait animation finish
				if (_animator.HasFinishedAnimation(currentAnimState, 0))
					StopState();

				return;
			}

			if (_move.IsGrounded())
			{
				if (highestPosition - transform.position.y >= heightForKillOnLand)
				{
					Landing(true, s_animDeathState, deathClip);
					_input.OnDied?.Invoke();
					return;
				}
				else if (highestPosition - transform.position.y >= heightForHardLand)
				{
					Landing(true, s_animHardLandState, hardLandClip);
					return;
				}
				else if (highestPosition - transform.position.y >= heightForSoftLand)
				{
					Landing(true, s_animSoftLandState, softLandClip);
					return;
				}
				StopState();

			}

			if (transform.position.y > highestPosition)
				highestPosition = transform.position.y;

			startInput = Vector2.SmoothDamp(startInput, _input.Move, ref inputVel, airControl);
			_move.Move(startInput, startSpeed, false);


		}
	    public override void OnStopState()
		{
			base.OnStopState();
			_input.OnDied -= DD;
			highestPosition = 0;
			_move.StopRootMotion();
		}
		private void Landing(bool landing, string animState, AudioClip clipLanding)
		{
			this.landing = landing;
			currentAnimState = animState;
			_animator.SetAnimationState(currentAnimState, 0, 0.02f);
			//_audio.PlayEffect(clipLanding);

			// call event
			//OnLanded.Invoke();
		}
	}

}