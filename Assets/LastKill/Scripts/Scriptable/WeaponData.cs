using System;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	[CreateAssetMenu(fileName = "Weapon data", menuName = "LastKill/Weapon data", order = 1)]
	public class WeaponData : ScriptableObject
	{
		[SerializeField] private Vector3 position;
		[SerializeField] private Vector3 rotation;
		[SerializeField] private Vector3 scale;
		public ParticleSystem prefabHitEffect;
		public GameObject prefabWeapon;

		public GameObject Weapon;

		public GameObject prefabDecalDamage;
		public List<GameObject> instanteDecalsDamage;

		public WeaponHolderState holder;

		public Transform muzzle;
		public Transform LeftHandIKIdle;
		public Transform LeftHandIKAiming;

		public AudioSource audioSource;
		public ParticleSystem[] muzleFlash;
		public ParticleSystem HitEffect;


		public AudioClip shootClip;
		public AudioClip emptyClip;
		public AudioClip reloadClip;

		public float FireRate;

		public int ClipCountAmmo;
		public int CurrentCountAmmo;

		public int MaxAmmoClip;
		public int MaxAmmoCount;

		public int Weapon_ID;
		public int WeaponAnimatorID;
		public float WeaponPoseAnimator;
		public void InstantiateTransform(WeaponData data)
		{
			try
			{
				Weapon.transform.localPosition = data.position;
				Weapon.transform.localRotation = Quaternion.Euler(new Vector3(data.rotation.x, data.rotation.y, data.rotation.z));
				Weapon.transform.localScale = data.scale;
				muzleFlash = data.Weapon.GetComponentsInChildren<ParticleSystem>();
				audioSource = data.Weapon.GetComponentInChildren<AudioSource>();
				LeftHandIKIdle = GameObject.Find("leftHandIKIdle").transform;
				LeftHandIKAiming = GameObject.Find("leftHandIKAiming").transform;
				muzzle = GameObject.Find("muzzle").transform;
				HitEffect = GameObject.Instantiate(prefabHitEffect);
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Data);
			}
		}
	}
}
