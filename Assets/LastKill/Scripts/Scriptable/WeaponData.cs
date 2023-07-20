using System;
using System.Collections;
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
        
        public GameObject prefab;
        public GameObject Weapon;
        public WeaponHolder holder;

        public Transform muzzle;
        public Transform LeftHandIK;

        public AudioSource audioSource;
        public ParticleSystem[] particleSystem;
        public AudioClip shootClip;
        public AudioClip emptyClip;
        public AudioClip reloadClip;

        public float FireRate;
        public int AmmoClipCount;
        public int AmmoTotalCount;

         public  int AmmoMaxCount;
        public  int AmmoMaxClip;

        public int Weapon_ID;
        public void InstantiateTransform(WeaponData data)
		{
            try
			{
                
                Weapon.transform.localPosition = data.position;
                Weapon.transform.localRotation = Quaternion.Euler(new Vector3(data.rotation.x, data.rotation.y, data.rotation.z));
                Weapon.transform.localScale = data.scale;
                particleSystem = data.Weapon.GetComponentsInChildren<ParticleSystem>();
                audioSource = data.Weapon.GetComponentInChildren<AudioSource>();
                LeftHandIK = GameObject.Find("leftHandIK").transform;
                muzzle = GameObject.Find("muzzle").transform;
            }
            catch(Exception ex)
			{
                Debug.Log(ex.Data);
			}
        }  
    }
}
