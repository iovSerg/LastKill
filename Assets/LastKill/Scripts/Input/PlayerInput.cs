using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LastKill
{
	[RequireComponent(typeof(AbilityState))]
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(MovementController))]
	[RequireComponent(typeof(AnimatorController))]
	[RequireComponent(typeof(CameraController))]
	[RequireComponent(typeof(MovementController))]
	[RequireComponent(typeof(DetectionController))]
	[RequireComponent(typeof(AudioController))]
	[RequireComponent(typeof(IKController))]
	[RequireComponent(typeof(WeaponController))]

	public class PlayerInput : MonoBehaviour , IInput
	{
		private PlayerInputSystem _input = null;
		private Animator _animator;

		[SerializeField] private Vector2 move;
		[SerializeField] private Vector2 look;
		[SerializeField] private float scroll;
		[SerializeField] private bool sprint;
		[SerializeField] private bool jump;
		[SerializeField] private bool crouch;
		[SerializeField] private bool roll;
		[SerializeField] private bool fire;
		[SerializeField] private bool aim;
		[SerializeField] private bool strafe;
		[SerializeField] private bool crawl;
		[SerializeField] private bool reload;
		[SerializeField] private float magnituda;
		[SerializeField] private int currentWeapon;
		[SerializeField] private int lastWeapon = 0;

		//hold button or single click
		[SerializeField] public bool HoldButton;

		public Vector2 Move => move;
		public Vector2 Look => look;
		public float Scroll => scroll;
		public bool Sprint => sprint;
		public bool Jump => jump;
		public bool Crouch => crouch;
		public bool Roll => roll;
		public bool Fire => fire;
		public bool Crawl => crawl;
		public bool Aim => aim;
		public bool Reload => reload;
		public float Magnituda => magnituda;
		public int CurrentWeapon => currentWeapon;

		public Action EventDied { get; set; }
		public Action<bool> EventFire { get; set; }
		public Action<bool> EventAim { get; set; }
		public Action<int> EventSelectWeapon { get; set; }
		public Action<int> EventReload { get; set; }
		

		AbilityState _abilityState;

		private double timeIsPressed;


		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_abilityState = GetComponent<AbilityState>();
			_abilityState.OnStateStop += AbilityState_OnStateStop;

			if (_input == null)
				_input = new PlayerInputSystem();

			_input.Player.Move.performed += OnMove;
			_input.Player.Move.canceled += OnMove;

			_input.Player.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
			_input.Player.Look.canceled += ctx => look = ctx.ReadValue<Vector2>();

			_input.Player.Jump.performed += ctx => jump = ctx.ReadValueAsButton();
			_input.Player.Jump.canceled += ctx => jump = ctx.ReadValueAsButton();

			_input.Player.Fire.performed += OnFire;
			_input.Player.Fire.canceled += OnFire;

			_input.Player.Roll.performed += ctx => roll = ctx.ReadValueAsButton();
			_input.Player.Roll.canceled += ctx => roll = ctx.ReadValueAsButton();

			_input.Player.Crawl.performed += ctx => crawl = ctx.ReadValueAsButton();
			_input.Player.Crawl.canceled += ctx => crawl = ctx.ReadValueAsButton();

			_input.Player.Exit.canceled += ctx => { Debug.Log("Quit"); Application.Quit(); };

			_input.Player.Reload.performed += ctx => reload = ctx.ReadValueAsButton();
			_input.Player.Reload.canceled += ctx => reload = ctx.ReadValueAsButton();


			_input.Player.Aim.performed += ctx => { aim = !aim; EventAim?.Invoke(aim); };

			_input.Player.Crouch.performed += ClickCrouch;
			_input.Player.Crouch.canceled += ClickCrouch;

			_input.Player.Sprint.performed += ctx => { sprint = !sprint; };

			_input.Player.Weapon.performed += SetWeapon;

			_input.Player.Scroll.performed += ctx =>
			{

				scroll = ctx.ReadValue<float>();
			};
			//_input.Player.Scroll.canceled += ctx => { scroll = ctx.ReadValue<float>(); };

		}

		private void AbilityState_OnStateStop(AbstractAbilityState obj)
		{

		}


		private void Update()
		{
			//when pressing a key
			if (crouch && timeIsPressed + 0.5f < Time.time && timeIsPressed != 0)
			{
				crawl = true;
				crouch = false;
			}
			//_animator.SetFloat("Magnituda", magnituda);
		}

		private void OnFire(InputAction.CallbackContext obj)
		{
			fire = obj.ReadValueAsButton();
			if (currentWeapon != 0)
				EventFire?.Invoke(fire);
		}

		private void ClickCrouch(InputAction.CallbackContext obj)
		{
			//When pressing a key
			if (crawl && !obj.canceled)
			{
				crawl = false;
				return;
			}
			if (obj.performed)
			{
				crouch = !crouch;
				timeIsPressed = Time.time;
			}
			if (obj.canceled) timeIsPressed = 0f;

		}

		private void SetWeapon(InputAction.CallbackContext obj)
		{
			try
			{
				int.TryParse(obj.control.displayName, out currentWeapon);

				if (aim && currentWeapon != lastWeapon)
				{
					currentWeapon = lastWeapon;
					return;
				}

				if (currentWeapon != lastWeapon)
				{
					lastWeapon = currentWeapon;
					EventSelectWeapon?.Invoke(currentWeapon);
				}
				else
				{
					lastWeapon = currentWeapon = 0;
					EventSelectWeapon?.Invoke(currentWeapon);
					aim = false;
					EventAim?.Invoke(false);
				}

			}
			catch (Exception ex)
			{

				Debug.Log(ex.Data);
			}
		}

		private void OnMove(InputAction.CallbackContext obj)
		{
			move = obj.ReadValue<Vector2>();
			magnituda = Mathf.Clamp01(Mathf.Abs(move.x) + Mathf.Abs(move.y));
		}

		private void OnEnable()
		{
			_input.Enable();
		}
		private void OnDisable()
		{
			_input.Disable();
		}

	}
}
