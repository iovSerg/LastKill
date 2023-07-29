using UnityEngine;

namespace LastKill
{

	public class IKController : MonoBehaviour
	{
		private Animator _animator;
		private PlayerInput _input;
		private ICamera _camera;


		[Header("---")]
		[SerializeField] private Transform targetLeftHand;
		[SerializeField] private Transform targetLeftHandIdle;
		[SerializeField] private Transform targetLeftHandAim;

		[SerializeField] private Transform targetRightHand;

		[SerializeField] private Transform targetRightFoot;
		[SerializeField] private Transform targetLeftFoot;

		[SerializeField] private Transform lookAtPosition;

		[SerializeField] private Transform bodySpine;

		public GameObject currentWeapon;
		[Space(2)]
		[Header("Weight IK")]
		[SerializeField] private float leftHandWeight = 0f;
		[SerializeField] private float rightHandWeight = 0f;

		[SerializeField] private float lookWeight;
		[SerializeField] private float bodyWeight;
		[SerializeField] private float headWeight;
		[SerializeField] private float eyesWeight;
		[SerializeField] private float clampWeight;

		public Transform LeftHandIdle { get => targetLeftHandIdle; set { targetLeftHandIdle = value; } }
		public Transform LeftHandAim { get => targetLeftHandAim; set { targetLeftHandAim = value; } }
		public Transform TRightHand { get => targetRightHand; set => targetRightHand = value; }
		public float WLeftHand { get => leftHandWeight; set => leftHandWeight = value; }
		public float WRightHand { get => rightHandWeight; set => rightHandWeight = value; }


		private void OnDrawGizmos()
		{
			Gizmos.color = Color.black;
			if (targetLeftHand != null && targetRightHand != null)
			{
				Gizmos.DrawSphere(targetLeftHand.position, 0.05f);
				Gizmos.DrawSphere(targetRightHand.position, 0.05f);
			}
		}
		private void Awake()
		{
			Debug.Log("Ik Controller");
			_input = GetComponent<PlayerInput>();
			_animator = GetComponent<Animator>();
			_camera = GetComponent<ICamera>();

			_input.EventAim += OnAiming;
			_input.EventFire += OnFire;
		}

		private void OnFire(bool state)
		{
			if(!_input.Aim)
			{
				lookWeight = state == false ? 0.0f : 0.4f;
				targetLeftHand = state == false ? targetLeftHandIdle : targetLeftHandAim;
			}
		}

		private void OnAiming(bool state)
		{
			lookWeight = state == false ? 0.0f : 0.4f;
			targetLeftHand = state == false ? targetLeftHandIdle : targetLeftHandAim;
		}

		private void Update()
		{
			leftHandWeight = _animator.GetFloat("LeftHand");
		}

		private void OnAnimatorIK(int layerIndex)
		{
			_animator.SetLookAtPosition(lookAtPosition.position);
			_animator.SetLookAtWeight(lookWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
			//LeftHand
			if (targetLeftHand != null)
			{
				_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
				_animator.SetIKPosition(AvatarIKGoal.LeftHand, targetLeftHand.position);

				_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
				_animator.SetIKRotation(AvatarIKGoal.LeftHand, targetLeftHand.rotation);
			}

			//RightHand
			if (targetRightHand != null)
			{
				_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
				_animator.SetIKPosition(AvatarIKGoal.RightHand, targetRightHand.position);

				_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
				_animator.SetIKRotation(AvatarIKGoal.RightHand, targetRightHand.rotation);
			}

			//RightFoot
			if (targetRightFoot != null)
			{

			}

			//LeftFoot
			if (targetLeftFoot != null)
			{

			}
		}



	}
}
