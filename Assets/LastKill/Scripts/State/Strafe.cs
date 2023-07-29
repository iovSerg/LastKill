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
			return _input.Aim || _input.Fire;
		}
		public float smooth = 2f;
		public override void UpdateState()
		{
			_animator.XYMove();
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
