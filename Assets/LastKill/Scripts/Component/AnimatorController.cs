using System;
using UnityEngine;

namespace LastKill
{
	public class AnimatorController : MonoBehaviour, IAnimator
	{
		private Animator _animator;
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
		[SerializeField] private string weaponPose = "WeaponPose";

		[Header("Reload Parametrs")]
		[SerializeField] private string reloadPistol;
		[SerializeField] private string reloadRifle;
		[SerializeField] private string reloadSniper;

		[Header("Animator Layers")]
		[SerializeField] private string ArmsLayerID;

		//hash animation move
		private int hashMagnituda;
		private int hashHorizontal;
		private int hashVertical;
		private int hashCrouch;

		private int hashReload;
		private int hashNoAim;
		private int hashAim;
		private int hashDrawWeapon;
		private int hashWeaponID;
		private int hashWeaponPose;

		//hash reload
		private int hashReloadPistol;
		private int hashReloadRifle;
		private int hashReloadSniper;

		//hash layers
		private int hashLayerArmsID;

		private float speedAnimation;

		[SerializeField] private float speedChangeRate = 10f;

		public Animator Animator => _animator;
		public bool isDrawWeapon => throw new NotImplementedException();


		public bool NoAim { get => _animator.GetBool(hashNoAim); set => _animator.SetBool(hashNoAim, value); }
		public bool Aiming { get => _animator.GetBool(hashAim); set => _animator.SetBool(hashAim, value); }
		public int WeaponID { get => _animator.GetInteger(hashWeaponID); set => _animator.SetInteger(hashWeaponID, value); }
		public float WeaponPose { get => _animator.GetFloat(hashWeaponPose); set => _animator.SetFloat(hashWeaponPose, value); }
		public bool Reload
		{

			get => _animator.GetBool(hashReload);

			set
			{
				Aiming = false;
				_animator.SetLayerWeight(hashLayerArmsID, 0);
				_animator.SetBool(hashReload, value);
			}
		}

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_iWeapon = GetComponent<IWeapon>();
			_input = GetComponent<PlayerInput>();

			_input.EventSelectWeapon += OnSelectWeapon;
			_input.EventReload += OnReload;
			_input.EventFire += OnFire;
			_input.EventAim += OnAiming;


			_abilityState = GetComponent<AbilityState>();

			_abilityState.OnStateStart += OnStateStart;
			_abilityState.OnStateStop += OnStateStop;
		}
		private void Start()
		{
			hashLayerArmsID = _animator.GetLayerIndex(ArmsLayerID);
			AssignAnimationIDs();
		}
		private void OnFire(bool state)
		{
			if(!_input.Aim)
			SetLayersWeight(hashLayerArmsID, state == true ? 1 : 0);
		}
		private void OnAiming(bool state)
		{
			if (_input.CurrentWeapon != 0)
			{
				SetLayersWeight(hashLayerArmsID, state == true ? 1 : 0);
			}
			Aiming = state;
		}
		private void OnReload(int id)
		{

			Reload = true;
			switch (id)
			{
				case 1:
					SetAnimationState(hashReloadPistol, 1);
					break;
				case 2:
					SetAnimationState(hashReloadRifle, 1);
					break;
				case 3:
					SetAnimationState(hashReloadRifle, 1);
					break;
			}

		}
		private void OnSelectWeapon(int id)
		{

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
			hashWeaponPose = Animator.StringToHash(weaponPose);

			hashReloadPistol = Animator.StringToHash(reloadPistol);
			hashReloadRifle = Animator.StringToHash(reloadRifle);
			hashReloadSniper = Animator.StringToHash(reloadSniper);
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
		private void FixedUpdate()
		{
			
		}
		private void Update()
		{

			speedAnimation = Mathf.Lerp(speedAnimation, _input.Magnituda, Time.deltaTime * speedChangeRate);
			if (speedAnimation < 0.1f) speedAnimation = 0f;
			_animator.SetFloat(hashMagnituda, speedAnimation);
		}
		private void SetLayersWeight(int hashName, int weight)
		{
			_animator.SetLayerWeight(hashName, weight);
		}
		public void DisableAll()
		{
			Aiming = false;
			NoAim = false;
		}
		public void XYMove()
		{
			if (_input.Move != Vector2.zero)
			{
				_animator.SetFloat(hashHorizontal, _input.Move.x, 0.2f, Time.deltaTime);
				_animator.SetFloat(hashVertical, _input.Move.y, 0.2f, Time.deltaTime);
			}
			else
			{
				_animator.SetFloat(hashHorizontal, 0f, 0.2f, Time.deltaTime);
				_animator.SetFloat(hashVertical, 0f, 0.2f, Time.deltaTime);
			}
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
