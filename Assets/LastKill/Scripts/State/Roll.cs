using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class Roll : AbstractAbilityState
	{
        [SerializeField] private string freeRollAnimation = "Roll";
        //[SerializeField] private string aimedRollAnimation = "Roll.RollAimForward";

        private int hashAimRoll;
        private int hashFreeRoll;
        private int layerIndex = 0;

        [SerializeField] private float rollSpeed = 7f;
        [SerializeField] private float capsuleHeightOnRoll = 1f;

        [SerializeField] private AudioClip _freeRollAudio;
        [SerializeField] private AudioClip _aimedRollAudio;

        private void Awake()
        {

        }
        public override void OnStartState()
        {
            _animator.SetAnimationState(freeRollAnimation, 0, 0.1f);
            _capsule.SetCapsuleSize(capsuleHeightOnRoll, _capsule.GetCapsuleRadius());
        }

        public override bool ReadyToStart()
        {
            return _input.Roll && _move.IsGrounded();
        }

        public override void UpdateState()
        {
            _move.StopMovement();
            _move.Move(transform.forward * rollSpeed);

            if (_animator.HasFinishedAnimation(layerIndex, 0.8f))
                StopState();
        }
        public override void OnStopState()
        {
            base.OnStopState();
            _move.StopMovement();
            _capsule.ResetCapsuleSize();
        }

		public override void FixedUpdateState()
		{
			
		}
	}
}
