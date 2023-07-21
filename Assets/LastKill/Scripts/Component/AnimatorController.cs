using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LastKill
{
	public class AnimatorController : MonoBehaviour,IAnimator
	{
		private	Animator _animator;
		private PlayerInput _input;
		private IWeapon _iWeapon;
		private AbilityState _abilityState;

		[Header("Animator Parameters")]
		[SerializeField] private string magnituda = "Magnituda";
		[SerializeField] private string horizontal = "Horizontal";
		[SerializeField] private string vertical = "Vertical";
		[SerializeField] private string crouch = "Crouch";

		[Header("Weapon Parametrs")]
		[SerializeField] private string reload = "Reload";
		[SerializeField] private string noAiminig = "isAiming";
		[SerializeField] private string aiming = "Aiming";
		[SerializeField] private string drawWeapon = "DrawWeapon";
		[SerializeField] private string weaponID = "WeaponID";

		[Header("Animator Layers")]
		[SerializeField] private string ArmsLayerID;

		//hash animation
		private int hashMagnituda;
		private int hashHorizontal;
		private int hashVertical;
		private int hashCrouch;

		private int hashReload;
		private int hashNoAim;
		private int hashAim;
		private int hashDrawWeapon;
		private int hashWeaponID;

		private int armsLayerID;
		
		private float speedAnimation;

		[SerializeField] private float speedChangeRate = 10f;

		public Animator Animator => _animator;
		public bool isDrawWeapon => throw new NotImplementedException();


		public bool NoAim { get => _animator.GetBool(hashNoAim); set => _animator.SetBool(hashNoAim,value); }
		public bool Aiming { get => _animator.GetBool(hashAim); set => _animator.SetBool(hashAim,value); }
		public float WeaponID { get => _animator.GetFloat(hashWeaponID); set => _animator.SetFloat(hashWeaponID,value); }



		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_iWeapon = GetComponent<IWeapon>();
			_input = GetComponent<PlayerInput>();

			_input.OnSelectWeapon += OnSelectWeapon;
			_input.OnReload += OnReload;
			_input.OnFire += OnFire;
			_input.OnAiming += OnAiming;


			_abilityState = GetComponent<AbilityState>();

			_abilityState.OnStateStart += OnStateStart;
			_abilityState.OnStateStop += OnStateStop;
		}
		private void Start()
		{
			AssignAnimationIDs();
			armsLayerID = _animator.GetLayerIndex(ArmsLayerID);
		}
		private void OnFire(bool state)
		{
			Aiming = state;
		}
		private void OnAiming(bool state)
		{
			Aiming = state;
		}
		private void OnReload(int id)
		{

		}
		private void OnSelectWeapon(int id)
		{
			if (id == 0)
			{
				_animator.SetLayerWeight(armsLayerID, 0);
			}
			else
				_animator.SetLayerWeight(armsLayerID, 1);
		}



		private void AssignAnimationIDs()
		{
			hashMagnituda = Animator.StringToHash(magnituda);
			hashHorizontal = Animator.StringToHash(horizontal);
			hashVertical = Animator.StringToHash(vertical);
			hashCrouch = Animator.StringToHash(crouch);

			hashAim = Animator.StringToHash(aiming);
			hashNoAim = Animator.StringToHash(noAiminig);
			hashReload = Animator.StringToHash(reload);
			hashDrawWeapon = Animator.StringToHash(drawWeapon);
			hashWeaponID = Animator.StringToHash(weaponID);
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
			if (speedAnimation < 0.05f) speedAnimation = 0f;
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
			
		}
	}
}
