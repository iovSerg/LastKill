using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{

    public class WeaponController : MonoBehaviour
    {
		//[SerializeField] private string animBlandState = "Draw Weapon HighLeft";

		private IAnimator _animator;
		private IInput _input;
		private float lastShot;

		public bool isAim = false;

		public WeaponData[] weapons;
		int id_weapon = 0;

		IKController iKController;

		public Transform handWeapon;
		public Transform spineWeapon;

		private void Awake()
		{
			iKController = GetComponent<IKController>();
			_input = GetComponent<IInput>();
			_animator = GetComponent<IAnimator>();

			_input.OnSelectWeapon += SelectWeapon;
			AddWeaponHolder();
			LoadResourcesWeapon();
			lastShot = 0;
		}

		private void AddWeaponHolder()
		{
			weapons = Resources.LoadAll<WeaponData>("Weapon/Hand");
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
			//Load Hand Holder
			foreach (WeaponData data in weapons)
			{
				GameObject gameObject = GameObject.Instantiate(data.weapon, handWeapon);
				data.Instantiate(gameObject);
				gameObject.transform.localPosition = data.position;
				gameObject.transform.localRotation = Quaternion.Euler(new Vector3(data.rotation.x,data.rotation.y,data.rotation.z));
				gameObject.transform.localScale = data.scale;
				gameObject.SetActive(false);
			}
			
			
		}
		private void SelectWeapon()
		{
			Debug.Log(_input.CurrentWeapon);
			
		}

		private void Update()
		{
			if(_input.Fire)
			{
				Shoot();
			}
			if(_input.Reload)
			{
				Reload();
			}
		}
		public void Shoot()
		{
			if (weapons[id_weapon].bulletCount == 0)
			{
				if (!weapons[id_weapon].audioSource.isPlaying)
					weapons[id_weapon].audioSource.PlayOneShot(weapons[id_weapon].emptyClip);
				return;
			}
			if (Time.time > lastShot + weapons[id_weapon].fireRate)
			{

				foreach (ParticleSystem ps in weapons[id_weapon].particleSystem)
				{
					ps.Emit(1);
				}
				weapons[id_weapon].bulletCount--;
				weapons[id_weapon].audioSource.PlayOneShot(weapons[id_weapon].shootClip);
				lastShot = Time.time;
			}

		}
		public void Reload()
		{

		}
	}
}
