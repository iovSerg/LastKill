using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastKill
{
    [DisallowMultipleComponent]
    public class Crouch : AbstractAbilityState
    {
        [SerializeField] private string crouchBlendState = "Crouch";
        [SerializeField] private LayerMask obstaclesMask;
        [SerializeField] private float capsuleHeightOnCrouch = 1f;
        [SerializeField] private float crouchSpeed = 2f;
        [SerializeField] private float offsetHitCrouch = 2f;

        private float defaultCapsuleHeight = 0f;
        private float defaultCapsuleRadius = 0f;

        private bool gizmoCrouch;
        private int hashBlendState;

        private void Awake()
        {
            hashBlendState = Animator.StringToHash(crouchBlendState);
        }
        public override void OnStartState()
        {
            defaultCapsuleRadius = _capsule.GetCapsuleRadius();
            defaultCapsuleHeight = _capsule.GetCapsuleHeight();

            _capsule.SetCapsuleSize(capsuleHeightOnCrouch, _capsule.GetCapsuleRadius());
            _move.Move(new Vector3(0, 0.5f, 0));
            _animator.SetAnimationState(hashBlendState,0, 0.25f);
        }

        public override void OnStopState()
        {
            _capsule.ResetCapsuleSize();
        }

        public override bool ReadyToStart()
        {
            // return _move.IsGrounded() && _input.Crouch && !_animator.Animator.GetBool("isStrafe");
            return _move.IsGrounded() && _input.Crouch;
        }

        public override void UpdateState()
        {

            _move.Move(_input.Move, crouchSpeed);

            //if (!_input.Crouch && !ForceCrouchByHeight() && !_animator.Animator.GetBool("isStrafe"))
            if (!_input.Crouch && !_detection.CanGetUp(offsetHitCrouch))
                    StopState();

        }

    }
}
