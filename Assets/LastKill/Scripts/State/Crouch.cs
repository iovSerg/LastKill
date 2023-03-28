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
        private int hashAnimState;

        private void Awake()
        {
            hashAnimState = Animator.StringToHash(crouchBlendState);
        }
        public override void OnStartState()
        {
            if (_debug.ShowDebugLog)
                Debug.Log("Crouch state");

            defaultCapsuleRadius = _capsule.GetCapsuleRadius();
            defaultCapsuleHeight = _capsule.GetCapsuleHeight();

            _capsule.SetCapsuleSize(capsuleHeightOnCrouch, _capsule.GetCapsuleRadius());
            _move.Move(new Vector3(0, 0.5f, 0));
            _animator.SetAnimationState(hashAnimState,0, 0.25f);
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
            if(_debug.ShowDebugCrouch)
                _debug.DebugCrouch(defaultCapsuleHeight,offsetHitCrouch);

            _move.Move(_input.Move, crouchSpeed);

            //if (!_input.Crouch && !ForceCrouchByHeight() && !_animator.Animator.GetBool("isStrafe"))
            if (!_input.Crouch && !ForceCrouchByHeight())
                    StopState();

        }
		//private void OnDrawGizmos()
		//{

  //              Gizmos.color = Color.yellow;
  //              Gizmos.DrawRay(transform.position + Vector3.up * defaultCapsuleHeight, transform.forward * offsetHitCrouch);
  //              Gizmos.DrawRay(transform.position, Vector3.up * offsetHitCrouch);

  //      }
		private bool ForceCrouchByHeight()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.up, out hit, offsetHitCrouch))
            {
                return true;
            }       
            return false;
        }
    }
}
