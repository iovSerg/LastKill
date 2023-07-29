using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LastKill
{
	public class WeaponController : MonoBehaviour, IWeapon
	{
		private Animator _animator;
		private PlayerInput _input;
		private IKController _iKController;
		private AnimatorController _animatorController;

		private WeaponData[] collectionWeapon;
		private WeaponData currentWeapon;

		private int weapon_id = 0;
		private float lastShot;
		private bool isWeapon = false;
		public bool isAim = false;


		public Transform raycastDestination;
		private Ray ray;
		private RaycastHit hitInfo;

		public Vector3 leftIK;

		public Transform handWeapon;
		public Transform spineWeapon;

		public Text bulletClip;
		public Text bulletTotal;

		public bool IsReload { get => _animator.GetBool("Reload"); }
		public bool IsWeapon { get => isWeapon; private set { isWeapon = value; } }


		private void Awake()
		{
			_iKController = GetComponent<IKController>();
			_input = GetComponent<PlayerInput>();
			_animator = GetComponent<Animator>();
			_animatorController = GetComponent<AnimatorController>();

			_input.EventSelectWeapon += OnSelectWeapon;
			_input.EventAim += OnAiming;
			_input.EventFire += OnFire;


			AddWeaponHolder();
			LoadResourcesWeapon();
			lastShot = 0;

		}

		private void OnAiming(bool state)
		{

		}
		private void OnFire(bool state)
		{

			
		}

		private void AddWeaponHolder()
		{
			Transform[] childs = GetComponentsInChildren<Transform>();
			foreach (Transform child in childs)
			{
				if (child.name.Contains("Right") && child.name.Contains("Hand"))
				{
					if (handWeapon == null)
					{
						GameObject weaponHand = new GameObject("HandHolder");
						weaponHand.transform.SetParent(child.gameObject.transform);
						handWeapon = weaponHand.transform;
						handWeapon.localRotation = Quaternion.identity;
						handWeapon.transform.localPosition = Vector3.zero;
						handWeapon.transform.localScale = Vector3.one;
					}
				}
				if (child.name.Contains("Spine"))
				{
					if (spineWeapon == null)
					{
						GameObject weaponSpine = new GameObject("SpineHolder");
						weaponSpine.transform.SetParent(child.gameObject.transform);
						spineWeapon = weaponSpine.transform;
						spineWeapon.localRotation = Quaternion.identity;
						spineWeapon.transform.localPosition = Vector3.zero;
						spineWeapon.transform.localScale = Vector3.one;
					}
				}
			}
		}
		private void LoadResourcesWeapon()
		{
			collectionWeapon = Resources.LoadAll<WeaponData>("Weapon");
			//Load Hand Holder
			foreach (WeaponData data in collectionWeapon)
			{
				data.Weapon = Instantiate(data.prefabWeapon, handWeapon);
				data.InstantiateTransform(data);
				data.Weapon.SetActive(false);
			}
		}
		private void OnSelectWeapon(int id)
		{

			if (id == 0)
			{
				DisableAll();
				return;
			}
			if (id != weapon_id)
			{
				if (currentWeapon != null)
				{
					currentWeapon.Weapon.SetActive(false);
				}
				foreach (WeaponData data in collectionWeapon)
				{
					if (data.Weapon_ID == id)
					{
						currentWeapon = data;
						currentWeapon.Weapon.SetActive(true);
					}

				}
				weapon_id = id;
				_animatorController.NoAim = true;
				_animatorController.WeaponID = currentWeapon.WeaponAnimatorID;
				_animatorController.WeaponPose = currentWeapon.WeaponPoseAnimator;
				_iKController.LeftHandIdle = currentWeapon.LeftHandIKIdle;
				_iKController.LeftHandAim = currentWeapon.LeftHandIKAiming;
				
			}
		}

		private void DisableAll()
		{
			_animatorController.WeaponID = 0;
			_animatorController.NoAim = false;
			weapon_id = 0;
			currentWeapon.Weapon.SetActive(false);
			currentWeapon = null;
		}

		private void Update()
		{
			if (weapon_id != 0)
			{
				if (_input.Fire)
				{
					Shoot();
				}
				if (_input.Reload) Reload();
				bulletClip.text = currentWeapon.ClipCountAmmo.ToString();
				bulletTotal.text = currentWeapon.CurrentCountAmmo.ToString();

				ray.origin = currentWeapon.muzzle.transform.position;
				ray.direction = raycastDestination.position - currentWeapon.muzzle.position;

			}
			else
			{
				bulletClip.text = string.Empty.ToString();
				bulletTotal.text = string.Empty.ToString();
			}
		}
		public void Shoot()
		{
			if (currentWeapon.ClipCountAmmo == 0)
			{
				if (!currentWeapon.audioSource.isPlaying)
					currentWeapon.audioSource.PlayOneShot(currentWeapon.emptyClip);
				return;
			}
			if (Time.time > lastShot + currentWeapon.FireRate)
			{
				foreach (ParticleSystem particle in currentWeapon.muzleFlash)
				{
					particle.Emit(1);
				}

				if(Physics.Raycast(ray , out hitInfo))
				{
					currentWeapon.HitEffect.transform.position = hitInfo.point;
					currentWeapon.HitEffect.transform.forward = hitInfo.normal;
					currentWeapon.HitEffect.Emit(1);
				}

				currentWeapon.audioSource.PlayOneShot(currentWeapon.shootClip);
				lastShot = Time.time;
			}
		}
		public void Reload()
		{
			StartCoroutine(TimeReload(2f));
			if (!currentWeapon.audioSource.isPlaying)
			{
				_input.EventReload?.Invoke(weapon_id);
				currentWeapon.audioSource.PlayOneShot(currentWeapon.reloadClip);
				if (currentWeapon.CurrentCountAmmo > currentWeapon.MaxAmmoClip)
				{
					currentWeapon.ClipCountAmmo = currentWeapon.MaxAmmoClip;
					currentWeapon.CurrentCountAmmo -= currentWeapon.MaxAmmoClip;
				}
			}

		}

		private IEnumerator TimeReload(float time)
		{

			while (_animator.GetBool("Reload"))
			{
				time -= Time.deltaTime;
				Debug.Log("Timer");
				yield return null;
			}
			Debug.Log("Stoped");
		}
	}
}
