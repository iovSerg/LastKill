using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class AnimatorController : MonoBehaviour,IAnimator
	{
		private	Animator _animator;
		private PlayerInput _input;

		[SerializeField] private string s_magnitudaID = "Magnituda";
		

		//hash animation
		private int hashMagnituda;

		private float speedAnimation;

		[SerializeField] private float speedChangeRate = 10f;

		public Animator Animator => _animator;

		public bool isAiming => throw new NotImplementedException();

		public bool isDrawWeapon => throw new NotImplementedException();

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_input = GetComponent<PlayerInput>();
		}
		private void Start()
		{
			AssignAnimationIDs();
		}

		private void AssignAnimationIDs()
		{
			hashMagnituda = Animator.StringToHash(s_magnitudaID);
		}

		private void Update()
		{
			
		}
		
		private void FixedUpdate()
		{
			speedAnimation = Mathf.Lerp(speedAnimation, _input.Magnituda, Time.deltaTime * speedChangeRate);
			if (speedAnimation < 0.1f) speedAnimation = 0f;
			_animator.SetFloat(hashMagnituda, speedAnimation);
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
			throw new NotImplementedException();
		}

		public bool HasFinishedAnimation(string stateName, int layerIndex)
		{
			throw new NotImplementedException();
		}

		public bool HasFinishedAnimation(int layerIndex)
		{
			throw new NotImplementedException();
		}

		public void StrafeUpdate()
		{
			throw new NotImplementedException();
		}

		public void LocomotionUpdate()
		{
			throw new NotImplementedException();
		}
	}
}
