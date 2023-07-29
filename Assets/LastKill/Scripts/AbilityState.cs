using System;
using System.Collections;
using UnityEngine;

namespace LastKill
{
	public class AbilityState : MonoBehaviour
	{
		private AbstractAbilityState[] playerAbilities = null;
		private Animator _animator;
		private PlayerInput _input;
		private bool _died = false;

		public event Action OnUpdateState = null;
		public event Action<AbstractAbilityState> OnStateStop = null;
		public event Action<AbstractAbilityState> OnStateStart = null;

		public AbstractAbilityState CurrentState { get; private set; }
		public AbstractAbilityState LastState { get; private set; }

		private void Awake()
		{
			playerAbilities = GetComponents<AbstractAbilityState>();
			foreach (AbstractAbilityState state in playerAbilities) { }
			_animator = GetComponent<Animator>();
			_input = GetComponent<PlayerInput>();
			_input.EventDied += OnDied;
		}

		private void OnDied()
		{
			_died = true;
			StartCoroutine(OnAlive());
		}
		IEnumerator OnAlive()
		{
			yield return new WaitForSecondsRealtime(5);
			_animator.CrossFadeInFixedTime("Locomotion.Alive", 0.1f, 0);
			_died = false;
		}

		private void Update()
		{
			if (_died) return;

			CheckAbilitiesStates();
			if (CurrentState != null)
				CurrentState.UpdateState();

			OnUpdateState?.Invoke();
		}
		private void FixedUpdate()
		{
			if (CurrentState != null)
				CurrentState.FixedUpdateState();
		}
		private void CheckAbilitiesStates()
		{
			AbstractAbilityState nextState = CurrentState;

			foreach (AbstractAbilityState state in playerAbilities)
			{
				if (state == CurrentState) continue;

				if (state.ReadyToStart())
				{

					if (nextState == null || state.StatePriority > nextState.StatePriority)
					{
						nextState = state;
					}
				}
			}
			if (nextState != CurrentState)
			{
				//Stop
				if (CurrentState != null)
					CurrentState.StopState();

				//Next state start
				nextState.StartState();

				CurrentState = nextState;
				CurrentState.abilityStopped += StateHasStopped;
				OnStateStart?.Invoke(CurrentState);
			}
		}

		private void StateHasStopped(AbstractAbilityState state)
		{
			LastState = CurrentState;
			CurrentState = null;

			// Remove this function
			state.abilityStopped -= StateHasStopped;

			// call observer
			OnStateStop?.Invoke(LastState);
		}
	}
}
