using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class AnimatorController : MonoBehaviour,IAnimator
	{
		private	Animator _animator;
		private IInput _input;
		private AbilityState _abilityState;

		[Header("Animator Parameters")]
		[SerializeField] private string magnituda = "Magnituda";
		[SerializeField] private string horizontal = "Horizontal";
		[SerializeField] private string vertical = "Vertical";
		[SerializeField] private string crouch = "Crouch";
		

		//hash animation
		private int hashMagnituda;
		private int hashHorizontal;
		private int hashVertical;
		private int hashCrouch;
		
		private float speedAnimation;

		[SerializeField] private float speedChangeRate = 10f;

		public Animator Animator => _animator;

		public bool isAiming => throw new NotImplementedException();

		public bool isDrawWeapon => throw new NotImplementedException();


		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_input = GetComponent<IInput>();
			_abilityState = GetComponent<AbilityState>();

			_abilityState.OnStateStart += OnStateStart;
			_abilityState.OnStateStop += OnStateStop;
		}



		private void Start()
		{
			AssignAnimationIDs();
		}

		private void AssignAnimationIDs()
		{
			hashMagnituda = Animator.StringToHash(magnituda);
			hashHorizontal = Animator.StringToHash(horizontal);
			hashVertical = Animator.StringToHash(vertical);
			hashCrouch = Animator.StringToHash(crouch);
		}
		private void OnStateStop(AbstractAbilityState obj)
		{
			if (obj as Crouch)
				_animator.SetBool(hashCrouch, false);
		}

		private void OnStateStart(AbstractAbilityState obj)
		{
			if (obj as Crouch)
				_animator.SetBool(hashCrouch, true);
		}
		private void Update()
		{
			
			speedAnimation = Mathf.Lerp(speedAnimation, _input.Magnituda, Time.deltaTime * speedChangeRate);
			if (speedAnimation < 0.1f) speedAnimation = 0f;
			_animator.SetFloat(hashMagnituda, speedAnimation);
		}
		
		private void FixedUpdate()
		{
		}

		public void ResetMovementParametrs()
		{
			_animator.SetFloat(hashMagnituda, 0f);
		}
		public void SetAnimationState(int hashName, int layerIndex, float transitionDuration = 0.1F)
		{
			if (_animator.HasState(layerIndex, hashName))
				_animator.CrossFadeInFixedTime(hashName, transitionDuration, layerIndex);
		}

		public void SetAnimationState(string stateName, int layerIndex, float transitionDuration = 0.1F)
		{
			if (_animator.HasState(layerIndex, Animator.StringToHash(stateName)))
				_animator.CrossFadeInFixedTime(stateName, transitionDuration, layerIndex);
		}

		public bool HasFinishedAnimation(string stateName, int layerIndex)
		{
			var stateInfo = _animator.GetCurrentAnimatorStateInfo(layerIndex);

			if (_animator.IsInTransition(layerIndex)) return false;

			if (stateInfo.IsName(stateName))
			{
				float normalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1);
				if (normalizeTime >= 0.95f) return true;
			}

			return false;
		}

		public bool HasFinishedAnimation(int layerIndex, float time)
		{
			return _animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= time && !_animator.IsInTransition(layerIndex);
		}

		public void StrafeUpdate()
		{
			
		}

		public void LocomotionUpdate()
		{
			throw new NotImplementedException();
		}
	}
}
