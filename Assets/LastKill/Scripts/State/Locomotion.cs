using UnityEngine;

namespace LastKill
{
	public class Locomotion : AbstractAbilityState
	{
		[SerializeField] private float walkSpeed = 2f;
		[SerializeField] private float sprintSpeed = 5f;
		[SerializeField] private string animatorBlendState = "Locomotion";

		private int hashAnimState;
		private float targetSpeed = 0f;

		private void Awake()
		{
			hashAnimState = Animator.StringToHash(animatorBlendState);	
		}

		public override void OnStartState()
		{

			_animator.SetAnimationState(hashAnimState, 0, 0.25f);

			if (_input.Move.magnitude < 0.1f)
			{
				// reset movement parameters
				_animator.ResetMovementParametrs();
			}

		}
		public override void UpdateState()
		{
			targetSpeed = _input.Sprint ? walkSpeed : sprintSpeed;
			targetSpeed = _input.Move == Vector2.zero ? 0f : targetSpeed;
			_move.Move(_input.Move, targetSpeed);
		}

		public override bool ReadyToStart()
		{
			return _move.IsGrounded();
		}

		public override void FixedUpdateState()
		{
			
		}
	}
}
