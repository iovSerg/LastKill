using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class Strafe : AbstractAbilityState
	{
		[SerializeField] private string animatorBlendState = "Strafe";
		[SerializeField] private float strafeSpeed = 3f;
		[SerializeField] private float speedRotate = 2f;
		private int hashBlendState;
		private void Awake()
		{
			hashBlendState = Animator.StringToHash(animatorBlendState);
		}
		public override void OnStartState()
		{
			_animator.SetAnimationState(hashBlendState);
			_animator.Aiming = true;
		}

		public override bool ReadyToStart()
		{
			return _input.Aim || _input.Fire && _animator.noAiming;
		}

		public override void UpdateState()
		{
			if(_input.Move != Vector2.zero)
			{
				_animator.Animator.SetFloat("Horizontal", _input.Move.x, 0.1f, Time.deltaTime);
				_animator.Animator.SetFloat("Vertical", _input.Move.y, 0.1f, Time.deltaTime);
			}
			else
			{

				_animator.Animator.SetFloat("Horizontal",0f);
				_animator.Animator.SetFloat("Vertical", 0f);
			}
			_move.Move(_input.Move, strafeSpeed, false);

			Quaternion rot = Quaternion.LookRotation(_camera.GetTransform.transform.forward);
			transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * speedRotate);
			transform.rotation = Quaternion.Euler(0, _camera.GetTransform.eulerAngles.y, 0);

			if (!_input.Aim && !_input.Fire) StopState();
		}
		public override void OnStopState()
		{
			base.OnStopState();
			_animator.Aiming = false;
		}

		public override void FixedUpdateState()
		{
			
		}
	}
}
