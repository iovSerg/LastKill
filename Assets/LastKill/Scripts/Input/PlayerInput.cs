using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LastKill
{
	public class PlayerInput : MonoBehaviour,IInput
	{
		private PlayerInputSystem _input = null;

		[SerializeField] private Vector2 move;
		[SerializeField] private Vector2 look;
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
		[SerializeField] private int lastWeapon;

		public Action OnDied;
		public Action OnSelectWeapon;
		public Action OnReload;

		//hold button or single click
		[SerializeField] public bool HoldButton;

		public Vector2 Move => move;
		public Vector2 Look => look;
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

		AbilityState _abilityState;

		private void Awake()
		{

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

			_input.Player.Fire.started += ctx => fire = ctx.ReadValueAsButton();
			_input.Player.Fire.performed += ctx => fire = ctx.ReadValueAsButton();
			_input.Player.Fire.canceled += ctx => fire = ctx.ReadValueAsButton();

			_input.Player.Roll.performed += ctx => roll = ctx.ReadValueAsButton();
			_input.Player.Roll.canceled += ctx => roll = ctx.ReadValueAsButton();

			_input.Player.Crawl.performed += ctx => crawl = ctx.ReadValueAsButton();
			_input.Player.Crawl.canceled += ctx => crawl = ctx.ReadValueAsButton();

			_input.Player.Exit.canceled += ctx => { Debug.Log("Quit"); Application.Quit(); };

			_input.Player.Reload.performed += ctx => { OnReload?.Invoke(); };

            _input.Player.Aim.performed += ctx =>{  aim = !aim; };

			_input.Player.Crouch.performed += OnCrouch;
			_input.Player.Crouch.canceled += OnCrouch;

			_input.Player.Sprint.performed += ctx => { sprint = !sprint; };

			_input.Player.Weapon.performed += SetWeapon;

		}

		private void AbilityState_OnStateStop(AbstractAbilityState obj)
		{
			if (obj as Crouch)
				Debug.Log("Crouch");
		}


		private void Update()
		{
			//when pressing a key
			if (crouch && timeIsPressed + 0.5f < Time.time && timeIsPressed != 0)
			{
				crawl = true;
				crouch = false;
			}
		}
		double timeIsPressed;
        private void OnCrouch(InputAction.CallbackContext obj)
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
			int.TryParse(obj.control.displayName,out currentWeapon);
			OnSelectWeapon?.Invoke();
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
