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
		[SerializeField] private float speedVectorUp = 2f;
		[Header("Animation")]
		//[SerializeField] private string smallAnimState = "Climb.StepUp";
		[SerializeField] private string shortAnimState = "Climb.ClimbUp";
		[SerializeField] private string highAnimState = "Climb.Jump";
		[SerializeField] private string playAnimState;
		[SerializeField] private float startNormalizeTime;
		[SerializeField] private float targetNormalizeTime;


		[SerializeField] private Vector3 targetPosition;
		[SerializeField] private RaycastHit targetHit;

		[SerializeField] private DescriptionClimb climb = new DescriptionClimb();


		private int hashShortClimb;
		private int hashClimbUp;

		//ѕодн€ть по высоте до выполнени€ анимации
		private bool highUp;
		private bool hasMatchTarget;

		private void Awake()
		{
			hashShortClimb = Animator.StringToHash(shortAnimState);
			hashClimbUp = Animator.StringToHash(highAnimState);
		}
		
		public override void OnStartState()
		{
			//Need refactoring
			//switch(climb.hasClimb)
			//{
			//	case LastKill.HasClimb.Small:
			//		playAnimState = smallAnimState;
			//		break;
			//	case LastKill.HasClimb.Short:
			//		playAnimState = shortAnimState;

			//		break;
			//	case LastKill.HasClimb.High:
			//		playAnimState = highAnimState;
			//		_animator.SetAnimationState("Climb.Jump",0, 0.2f);
			//		break;
			//}
			//highUp = true;
			//targetHit = climb.raycastHit;
			//targetPosition = climb.targetPosition;
		
			_capsule.DisableCollision();
			_move.DisableGravity();
			_move.ApplyRootMotion(Vector3.one * 0.5f);
			_move.StopMovement();

			if (height <= 10)
			{
				playAnimState = shortAnimState;
				_animator.SetAnimationState(shortAnimState, 0, 0.1f);
			}
			else
			{
				playAnimState = highAnimState;
				_animator.SetAnimationState(highAnimState, 0, 0.2f);

			}
			hasMatchTarget = false;
		}

		public override bool ReadyToStart()
		{
			//Variant one
			if (_move.IsGrounded() && _input.Jump)
				if (HasClimbs())
					return true;
			return false;


			//if(_move.IsGrounded() && _input.Jump)
			//{
			//	climb = _detection.ClimbTargets();

			//	if (climb.hasClimb == LastKill.HasClimb.Null)
			//	{
			//		return false;
			//	}
			//	else return true;
			//}
			//return false;
		}
		[SerializeField] private float height;
		private bool HasClimbs()
		{
			targetHit = _detection.ClimbTarget();
			targetPosition = targetHit.point;
			height = _detection.ForwardHeight;
			if (targetHit.point == Vector3.zero || height == 0) return false;


			return true;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(targetHit.point, 0.05f);
			Gizmos.DrawRay(transform.position + Vector3.up * 1.8f, transform.forward);
		}

		public override void UpdateState()
		{
			//transform up //pick up the game
			if(highUp)
			{
				if (Physics.Raycast(transform.position + Vector3.up * 2.2f, transform.forward, 1f))
				{
					Vector3 up = transform.position;
					up.y += Time.deltaTime * speedVectorUp;
					transform.position = up;
					return;
				}
				else
				{
					highUp = false;
					_animator.SetAnimationState(playAnimState, 0, 0.2f);
				}
			}

			//need rafactoring
			var state = _animator.Animator.GetCurrentAnimatorStateInfo(0);

			if (_animator.Animator.IsInTransition(0) || !state.IsName(playAnimState)) return;

			var normalizedTime = Mathf.Repeat(state.normalizedTime, 1f);

			//if (_animator.HasFinishedAnimation(playAnimState, 0))
			//	StopState();

			if (!_animator.Animator.isMatchingTarget && !hasMatchTarget)
			{
				_animator.Animator.MatchTarget(targetPosition, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(Vector3.one,0f), startNormalizeTime, targetNormalizeTime, hasMatchTarget);

				hasMatchTarget = true;
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
			height = 0;
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
