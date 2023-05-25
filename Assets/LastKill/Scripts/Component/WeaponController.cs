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

		public bool isAim = false;

		public WeaponData[] weapons;

		public Transform handWeapon;
		public Transform spineWeapon;

		private void Awake()
		{
			_input = GetComponent<IInput>();
			_animator = GetComponent<IAnimator>();

			_input.OnSelectWeapon += SelectWeapon;
			AddWeaponHolder();
			LoadWeapon();

		}

		private void AddWeaponHolder()
		{
			weapons = Resources.LoadAll<WeaponData>("Weapon/");
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
						spineWeapon.transform.localPosition = Vector3.zero;
						spineWeapon.transform.localScale = Vector3.one;
					}
				}
			}
		}

		private void LoadWeapon()
		{
			//Load Hand Holder
			foreach (WeaponData data in weapons)
			{
				GameObject gameObject = GameObject.Instantiate(data.weapon, handWeapon);
				data.Instantiate(gameObject);
				gameObject.transform.localPosition = data.position;
				gameObject.transform.rotation = Quaternion.Euler(data.rotation);
				gameObject.transform.localScale = data.scale;
				gameObject.SetActive(false);
				//gameObject.transform.SetParent(HandHolder.transform);
				//gameObject.transform.position = data.position;
				//gameObject.transform.rotation = data.rotation;
			}
		}
		private void SelectWeapon()
		{
			
			
		}

		private void Update()
		{
			if(_input.Fire)
			{
				weapons[0].particleSystem.Play();
			}
		}
	}
}
