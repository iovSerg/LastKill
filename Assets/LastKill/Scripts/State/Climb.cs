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
		[SerializeField] private string shortAnimState = "Climb.ShortClimb";
		[SerializeField] private string highAnimState = "Climb.ClimbUp";

		[SerializeField] private Vector3 targetPosition = Vector3.zero;
		[SerializeField] private DescriptionClimb climb = new DescriptionClimb();

		private int hashShortClimb;
		private int hashClimbUp;

		[SerializeField] private RaycastHit targetHit;

		private bool hasMatchTarget;

		private void Awake()
		{
			hashShortClimb = Animator.StringToHash(shortAnimState);
			hashClimbUp = Animator.StringToHash(highAnimState);
		}
		public override void OnStartState()
		{
			Physics.Raycast(targetPosition, Vector3.down, out targetHit, 0.5f);
			//_capsule.DisableCollision();
			_move.DisableGravity();
			_move.ApplyRootMotion(Vector3.one);
			_move.StopMovement();

			_animator.SetAnimationState(height>7 ? highAnimState: shortAnimState, 0,0.1f);
			hasMatchTarget = false;
		}

		public override bool ReadyToStart()
		{
			if(_move.IsGrounded() && _input.Jump)
				if(HasClimbs())
			return true;
			return false;
		}
		[SerializeField] private float height;
		private bool HasClimbs()
		{
			targetPosition = _detection.ClimbTarget();
			if (targetPosition == Vector3.zero) return false;
			height = _detection.ForwardHeight;

			return true;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(targetHit.point, 0.05f);
			Gizmos.DrawSphere(targetPosition, 0.05f);
		}

		public override void UpdateState()
		{
		
			var state = _animator.Animator.GetCurrentAnimatorStateInfo(0);

			if (_animator.Animator.IsInTransition(0) || !state.IsName(height > 7 ? highAnimState : shortAnimState)) return;

			var normalizedTime = Mathf.Repeat(state.normalizedTime, 1f);
			if (!_animator.Animator.isMatchingTarget && !hasMatchTarget)
			{
				// calculate target position
				Debug.Log(_capsule.GetCapsuleRadius() * 0.5f);
				 targetPosition = targetHit.point - targetHit.normal * _capsule.GetCapsuleRadius() * 0.5f;
				_animator.Animator.MatchTarget(targetPosition, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(Vector3.one,0f), startNormalizeTime, targetNormalizeTime, hasMatchTarget);

				//_animator.Animator.MatchTarget(targetPosition, Quaternion.identity, AvatarTarget.Root,
				//	new MatchTargetWeightMask(Vector3.one, 0f), 0.15f, 0.42f);

				hasMatchTarget = true;

			}

			if (normalizedTime > 0.95f)
				StopState();
		}
		[SerializeField] private float startNormalizeTime;
		[SerializeField] private float targetNormalizeTime;
		public override void OnStopState()
		{
			base.OnStopState();
			//_capsule.EnableCollision();
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
						targetHit = shortHit;
						targetHit.normal = Vector3.Scale(forwardHit.normal, new Vector3(1, 0, 1)).normalized;
						

						return true;
					}

				    sphereStart = forwardHit.point;
					sphereStart.y = transform.position.y + climbUpHeight + capsuleCastRadius;

					if (Physics.SphereCast(sphereStart, capsuleCastRadius, Vector3.down, out RaycastHit topHit, climbUpHeight - minClimbHeight,
						climbMask, QueryTriggerInteraction.Collide))
					{
						targetHit = topHit;
						targetHit.normal = Vector3.Scale(forwardHit.normal, new Vector3(1, 0, 1)).normalized;
						

						return true;
					}
				}

			}

			return false;
		}
	}
}
