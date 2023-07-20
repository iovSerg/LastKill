using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LastKill
{
    public class WeaponController : MonoBehaviour,IWeapon
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


		public Vector3 leftIK;

		public Transform handWeapon;
		public Transform spineWeapon;

		public Text bulletClip;
		public Text bulletTotal;

		public bool IsReload { get => _animator.GetBool("Reload"); }
		public bool IsWeapon { get => isWeapon; private set {
				 isWeapon = value;
		} }


		private void Awake()
		{
			_iKController = GetComponent<IKController>();
			_input = GetComponent<PlayerInput>();
			_animator = GetComponent<Animator>();
			_animatorController = GetComponent<AnimatorController>();
			_input.OnSelectWeapon += OnSelectWeapon;

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
			collectionWeapon = Resources.LoadAll<WeaponData>("Weapon");
			//Load Hand Holder
			foreach (WeaponData data in collectionWeapon)
			{
			    data.Weapon =  Instantiate(data.prefab, handWeapon);
				data.InstantiateTransform(data);
				data.Weapon.SetActive(false);
			}

			_iKController.currentWeapon = collectionWeapon[1].Weapon;
			_iKController.TLeftHand = collectionWeapon[1].LeftHandIK;
		

		}
		private void OnSelectWeapon(int id)
		{

			if (id == 0)
			{
				DisableAll();
				return;
			}
			if(id != weapon_id)
			{
				if(currentWeapon != null)
				{
				    currentWeapon.Weapon.SetActive(false);
				}
				foreach(WeaponData data in collectionWeapon)
				{
					if(data.Weapon_ID == id)
					{
						currentWeapon = data;
						currentWeapon.Weapon.SetActive(true);
					}

				}
				weapon_id = id;
				_animatorController.NoAim = true;
				_animatorController.WeaponID = weapon_id;
			}



			
			//if (!isWeapon)
			//{
			//	foreach (WeaponData data in collectionWeapon)
			//	if (data.Weapon_ID == weapon_id)
			//	{
			//		data.Weapon.SetActive(true);
			//		last_weapon_id = weapon_id;
			//		isWeapon = true;
			//		_animatorController.NoAim = true;
			//		return;
			//	}
			//}

			//if (weapon_id == last_weapon_id && isWeapon)
			//{
			//	foreach (WeaponData data in collectionWeapon)
			//	if (data.Weapon_ID == weapon_id)
			//	{
			//		data.Weapon.SetActive(false);
			//		isWeapon = false;
			//		_animatorController.NoAim = false;
			//		weapon_id = last_weapon_id = 0;
			//		_animatorController.WeaponID = 0;
			//		return;
			//	}
			//}

			//if (weapon_id != last_weapon_id && isWeapon)
			//{
			//	foreach (WeaponData data in collectionWeapon)
			//	{
			//		if (data.Weapon_ID == last_weapon_id)
			//			data.Weapon.SetActive(false);
			//		if (data.Weapon_ID == weapon_id)
			//			data.Weapon.SetActive(true);
			//	}
			//	last_weapon_id = weapon_id;
			//}

			
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
			if(weapon_id != 0)
			{
				if (_input.Fire)
				{
					_input.OnFire.Invoke(true);
					Shoot();
				}
				if (_input.Reload) Reload();
				bulletClip.text = currentWeapon.AmmoClipCount.ToString();
				bulletTotal.text = currentWeapon.AmmoTotalCount.ToString();
				
			}
			else
			{
				bulletClip.text = string.Empty.ToString();
				bulletTotal.text = string.Empty.ToString();
			}
			
		}
		public void Shoot()
		{
			if (currentWeapon.AmmoClipCount == 0)
			{
				if (!currentWeapon.audioSource.isPlaying)
					currentWeapon.audioSource.PlayOneShot(currentWeapon.emptyClip);
				return;
			}
			if (Time.time > lastShot + currentWeapon.FireRate)
			{
				foreach (ParticleSystem particle in currentWeapon.particleSystem)
				{
					particle.Emit(1);
				}
				currentWeapon.AmmoClipCount--;
				currentWeapon.audioSource.PlayOneShot(currentWeapon.shootClip);
				lastShot = Time.time;
			}
		}
		public void Reload()
		{
			if (!currentWeapon.audioSource.isPlaying)
			{
				_input.OnReload?.Invoke(weapon_id);
				currentWeapon.audioSource.PlayOneShot(currentWeapon.reloadClip);
				if (currentWeapon.AmmoTotalCount > currentWeapon.AmmoMaxClip)
				{
					currentWeapon.AmmoClipCount = currentWeapon.AmmoMaxClip;
					currentWeapon.AmmoTotalCount -= currentWeapon.AmmoMaxClip;
				}
			}
		}
	}
}
