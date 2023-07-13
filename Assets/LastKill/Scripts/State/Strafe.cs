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
		}

		public override bool ReadyToStart()
		{
			return _input.Aim;
		}

		public override void UpdateState()
		{
			_move.Move(_input.Move, strafeSpeed,false);

			if(_input.Move != Vector2.zero)
			{
				_animator.Animator.SetFloat("Horizontal", _input.Move.x, 0.1f, Time.deltaTime);
				_animator.Animator.SetFloat("Vertical", _input.Move.y, 0.1f, Time.deltaTime);
			}
			else
			{
				_animator.Animator.SetFloat("Horizontal",0f, 0.2f,Time.deltaTime);
				_animator.Animator.SetFloat("Vertical", 0f);
			}

			Quaternion rot = Quaternion.LookRotation(_camera.GetTransform.transform.forward);
			transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * speedRotate);
			//transform.rotation = Quaternion.Euler(0, _camera.GetTransform.eulerAngles.y, 0);


			if (!_input.Aim) StopState();
		}
		public override void OnStopState()
		{
			base.OnStopState();
		}

		public override void FixedUpdateState()
		{
			
		}
	}
}
