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

		public Transform HandHolder;
		public Transform SpineHolder;

		private void Awake()
		{
			weapons = Resources.LoadAll<WeaponData>("Weapon/");

			_input = GetComponent<IInput>();
			_animator = GetComponent<IAnimator>();

			_input.OnSelectWeapon += SelectWeapon;

			Transform [] childs = GetComponentsInChildren<Transform>();
			foreach(Transform child in childs)
			{
				if (child.name.Contains("Right") && child.name.Contains("Hand"))
				{
					if(HandHolder == null)
					{
						GameObject weaponHand = new GameObject("HandHolder");
						weaponHand.transform.SetParent(child.gameObject.transform);

						HandHolder = weaponHand.transform;
						HandHolder.transform.localPosition = Vector3.zero;

					}
				}
				if (child.name.Contains("Spine"))
				{
					if(SpineHolder == null)
					{
						GameObject weaponSpine = new GameObject("SpineHolder");
						weaponSpine.transform.SetParent(child.gameObject.transform);

						SpineHolder = weaponSpine.transform;
						SpineHolder.transform.localPosition = Vector3.zero;
					}
				}
			}
			

		}
		private void Start()
		{
			//Load Hand Holder
			//foreach (WeaponData data in weapons)
			//{
			//	GameObject gameObject = GameObject.Instantiate(data.weapon,HandHolder);
			//	gameObject.transform.localPosition = data.position;
			//	gameObject.transform.rotation = Quaternion.Euler(data.rotation);
			//	gameObject.transform.localScale = data.scale;
			//	//gameObject.transform.SetParent(HandHolder.transform);
			//	//gameObject.transform.position = data.position;
			//	//gameObject.transform.rotation = data.rotation;
			//}

		}
		private void SelectWeapon()
		{
			
			
		}

		private void Update()
		{
			
		}
	}
}
