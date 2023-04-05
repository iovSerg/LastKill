using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class Strafe : AbstractAbilityState
	{
		[SerializeField] private string animatorBlendState = "Strafe";
		[SerializeField] private float strafeSpeed = 3f;
		private int hashBlendState;
		private void Awake()
		{
			hashBlendState = Animator.StringToHash(animatorBlendState);
		}
		public override void OnStartState()
		{
			_animator.SetAnimationState(hashBlendState);
		}

		public override bool ReadyToStart()
		{
			return _input.Aim;
		}

		public override void UpdateState()
		{
			_move.Move(_input.Move, strafeSpeed,false);
			_animator.Animator.SetFloat("Horizontal", _input.Move.x, 0.1f, Time.deltaTime);
			_animator.Animator.SetFloat("Vertical", _input.Move.y, 0.1f, Time.deltaTime);
			transform.rotation = Quaternion.Euler(0, _camera.GetTransform.eulerAngles.y, 0);
			_animator.StrafeUpdate();
			if (!_input.Aim) StopState();
		}
		public override void OnStopState()
		{
			base.OnStopState();
		}
	}
}
