using System;
using UnityEngine;
using UnityEngine.UI;

namespace LastKill
{
    public abstract class AbstractAbilityState : MonoBehaviour
    {
        [SerializeField] private int statePriority = 0;

        public bool IsStateRunning { get; private set; }

        public event Action<AbstractAbilityState> abilityStopped = null;

        public event Action<AbstractAbilityState> abilityStarted = null;
        public int StatePriority { get { return statePriority; } }
        public CameraState cameraState;

        // start time and stop time
        public float StartTime { get; private set; } = 0;
        public float StopTime { get; private set; } = 0;

        protected IMove _move;
        protected ICapsule _capsule;
        protected IWeapon _weapon;
        protected IAnimator _animator;
        protected IAudio _audio;
        protected ICamera _camera;
        protected IDebug _debug;
        protected IInput _input;
        protected IDetection _detection;
        protected virtual void Start()
        {
            _move = GetComponent<IMove>();
            _capsule = GetComponent<ICapsule>();
            _input = GetComponent<IInput>();
            _weapon = GetComponent<IWeapon>();
            _animator = GetComponent<IAnimator>();
            _camera = GetComponent<ICamera>();
            _audio = GetComponent<IAudio>();
            _debug = GetComponent<IDebug>();
            _detection = GetComponent<IDetection>();
        }

        public void StartState()
        {
            IsStateRunning = true;
            StartTime = Time.time;
            OnStartState();
            abilityStarted?.Invoke(this);
        }
        public void StopState()
        {
            if (Time.time - StartTime < 0.1f)
                return;

            IsStateRunning = false;
            StopTime = Time.time;
            OnStopState();
            abilityStopped?.Invoke(this);
            
        }

        public abstract bool ReadyToStart();

        public abstract void OnStartState();

        public abstract void UpdateState();
        public abstract void FixedUpdateState();

        public virtual void OnStopState() { }

    }
}
