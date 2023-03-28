using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class Climb : AbstractAbilityState
	{
		[SerializeField] private LayerMask climbMask;
		[SerializeField] private float overlapRadius = 0.75f;
		[SerializeField] private float capsuleCastRadius = 0.2f;
		[SerializeField] private float capsuleCastHeight = 1f;
		[SerializeField] private float minClimbHeight = 0.5f;
		[SerializeField] private float climbHeight = 1f;
		[SerializeField] private float climbUpHeight = 1.5f;
		[Header("Animation")]
		[SerializeField] private string s_shortClimbAnimState = "Climb.ShortClimb";
		[SerializeField] private string s_climbUpAnimState = "Climb.ClimbUp";

		private int hashShortClimb;
		private int hashClimbUp;

		private RaycastHit _targetHit;
		private bool _hasMatchTarget;
		private bool climb = false;
		private void Awake()
		{
			hashShortClimb = Animator.StringToHash(s_shortClimbAnimState);
			hashClimbUp = Animator.StringToHash(s_climbUpAnimState);
		}
		public override void OnStartState()
		{
			if (_debug.ShowDebugLog)
				Debug.Log("Climb State");

			_capsule.DisableCollision();
			_move.DisableGravity();
			_move.ApplyRootMotion(Vector3.one);
			_move.StopMovement();

			_animator.SetAnimationState(climb ? s_climbUpAnimState : s_shortClimbAnimState, 0,0.1f);

			_hasMatchTarget = false;
		}

		public override bool ReadyToStart()
		{
			return !_move.IsGrounded() && HasClimb();
		}

		public override void UpdateState()
		{
			var state = _animator.Animator.GetCurrentAnimatorStateInfo(0);

			if (_animator.Animator.IsInTransition(0) || !state.IsName(climb ? s_climbUpAnimState : s_shortClimbAnimState)) return;

			var normalizedTime = Mathf.Repeat(state.normalizedTime, 1f);
			if (!_animator.Animator.isMatchingTarget && !_hasMatchTarget)
			{
				// calculate target position
				Vector3 targetPosition = _targetHit.point - _targetHit.normal * _capsule.GetCapsuleRadius() * 0.5f;
				_animator.Animator.MatchTarget(targetPosition, Quaternion.identity, AvatarTarget.Root,
					new MatchTargetWeightMask(Vector3.one, 0f), 0.15f, 0.42f);

				_hasMatchTarget = true;

			}

			if (normalizedTime > 0.95f)
				StopState();
		}
		public override void OnStopState()
		{
			base.OnStopState();
			_capsule.EnableCollision();
			_move.EnableGravity();
			_move.StopRootMotion();
			_move.StopMovement();
		}

		private bool HasClimb()
		{
			Vector3 overlapCenter = transform.position + Vector3.up * overlapRadius;

			if (Physics.OverlapSphere(overlapCenter, overlapRadius, climbMask, QueryTriggerInteraction.Collide).Length > 0)
			{ // found some short climb object

				// capsule cast points
				Vector3 p1 = transform.position + Vector3.up * (minClimbHeight + capsuleCastRadius);
				Vector3 p2 = transform.position + Vector3.up * (capsuleCastHeight - capsuleCastRadius);
				Vector3 castDirection = transform.forward;

				if (Physics.CapsuleCast(p1, p2, capsuleCastRadius, castDirection, out RaycastHit forwardHit,
					overlapRadius, climbMask, QueryTriggerInteraction.Collide))
				{

					Vector3 sphereStart = forwardHit.point;
					sphereStart.y = transform.position.y + climbHeight + capsuleCastRadius;

					// check top
					if (Physics.SphereCast(sphereStart, capsuleCastRadius, Vector3.down, out RaycastHit shortHit, climbHeight - minClimbHeight,
						climbMask, QueryTriggerInteraction.Collide))
					{
						_targetHit = shortHit;
						_targetHit.normal = Vector3.Scale(forwardHit.normal, new Vector3(1, 0, 1)).normalized;
						climb = false;

						return true;
					}

				    sphereStart = forwardHit.point;
					sphereStart.y = transform.position.y + climbUpHeight + capsuleCastRadius;

					if (Physics.SphereCast(sphereStart, capsuleCastRadius, Vector3.down, out RaycastHit topHit, climbUpHeight - minClimbHeight,
						climbMask, QueryTriggerInteraction.Collide))
					{
						_targetHit = topHit;
						_targetHit.normal = Vector3.Scale(forwardHit.normal, new Vector3(1, 0, 1)).normalized;
						climb = true;

						return true;
					}
				}

			}

			return false;
		}
	}
}
