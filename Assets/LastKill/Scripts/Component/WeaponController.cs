using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LastKill
{
    public class WeaponController : MonoBehaviour,IWeapon
    {
		private IAnimator _animator;
		private IInput _input;
		private IKController _iKController;
		private WeaponData[] weapons;

		private int hashReload;
		private int weapon_id = 0;
		private int last_weapon_id = 0;
		private float lastShot;
		private bool isWeapon = false;
		public bool isAim = false;

		[SerializeField] private string reload = "Reload";
		[SerializeField] private Text weaponCount;
		//[SerializeField] private List<WeaponData> weapons;


		public Vector3 leftIK;

		public Transform handWeapon;
		public Transform spineWeapon;

		public bool IsReload { get => _animator.Animator.GetBool("Reload"); }
		public bool IsWeapon { get => isWeapon; }

		private void Awake()
		{
			_iKController = GetComponent<IKController>();
			_input = GetComponent<IInput>();
			_animator = GetComponent<IAnimator>();
			_input.OnSelectWeapon += OnSelectWeapon;

			hashReload = Animator.StringToHash(reload);

			AddWeaponHolder();
			LoadResourcesWeapon();
			lastShot = 0;
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
			weapons = Resources.LoadAll<WeaponData>("Weapon");
			//Load Hand Holder
			foreach (WeaponData data in weapons)
			{
			    data.Weapon =  Instantiate(data.prefab, handWeapon);
				data.InstantiateTransform(data);
				data.Weapon.SetActive(false);
			}
			_iKController.TLeftHand = weapons[1].LeftHandIK;
			
		}
		private void OnSelectWeapon()
		{
			weapon_id = _input.CurrentWeapon;

			foreach (WeaponData data in weapons)
			{
				if (!isWeapon)
				{
					if (data.Weapon_ID == weapon_id)
					{
						data.Weapon.SetActive(true);
						last_weapon_id = weapon_id;
						isWeapon = true;
						return;
					}
				}

				if (weapon_id == last_weapon_id && isWeapon)
				{
					if (data.Weapon_ID == weapon_id)
					{
						data.Weapon.SetActive(false);
						isWeapon = false;
						weapon_id = last_weapon_id = 0;
						return;
					}
				}

				if (weapon_id != last_weapon_id && isWeapon)
				{
					if (data.Weapon_ID == last_weapon_id)
						data.Weapon.SetActive(false);
					if (data.Weapon_ID == weapon_id)
						data.Weapon.SetActive(true);
				}

			}
		}

		private void Update()
		{
			if (weapon_id == 0)
			{
				weaponCount.text = string.Empty;
				return;
			}
			if(isWeapon)
			{
				if (_input.Fire)
				{
					Shoot();
				}
				if (_input.Reload)
				{
					Reload();
				}
				
			}
			weaponCount.text = weapons[weapon_id - 1].BulletCount.ToString();
		}
		public void Shoot()
		{
			foreach(WeaponData data in weapons)
			{
				if(data.Weapon_ID == weapon_id)
				{
					if(data.BulletCount == 0)
					{
						if (!data.audioSource.isPlaying)
							data.audioSource.PlayOneShot(data.emptyClip);
						return;
					}
					if(Time.time > lastShot + data.FireRate)
					{
						foreach(ParticleSystem particle in data.particleSystem)
						{
							particle.Emit(1);
						}
						data.BulletCount--;
						data.audioSource.PlayOneShot(data.shootClip);
						lastShot = Time.time;
					}
				}
			}
			

			//if (weapons[weapon_id - 1].BulletCount == 0)
			//{
			//	if (!weapons[weapon_id - 1].audioSource.isPlaying)
			//		weapons[weapon_id - 1].audioSource.PlayOneShot(weapons[weapon_id - 1].emptyClip);
			//	return;
			//}
			//if (Time.time > lastShot + weapons[weapon_id - 1].FireRate)
			//{

			//	foreach (ParticleSystem ps in weapons[weapon_id - 1].particleSystem)
			//	{
			//		ps.Emit(1);
			//	}
			//	weapons[weapon_id - 1].BulletCount--;
			//	weapons[weapon_id - 1].audioSource.PlayOneShot(weapons[weapon_id - 1].shootClip);
			//	lastShot = Time.time;
			//}

		}
		public void Reload()
		{
			foreach(WeaponData data in weapons)
			{
				if(data.Weapon_ID == weapon_id)
				{
					if (!data.audioSource.isPlaying)
					{
						data.audioSource.PlayOneShot(data.reloadClip);
						data.BulletCount = data.BulletMaxClip;
					}
				}
			}
		}
	}
}
