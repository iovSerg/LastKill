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
			
			
		}

		private void Update()
		{
			
		}
	}
}
