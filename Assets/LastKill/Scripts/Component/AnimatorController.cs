using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class AnimatorController : MonoBehaviour
	{
		private	Animator _animator;
		private PlayerInput _input;

		[SerializeField] private string s_magnitudaID = "Magnituda";

		//hash animation
		private int hashMagnituda;

		private float speedAnimation;

		[SerializeField] private float speedChangeRate = 10f;
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
	}
}
