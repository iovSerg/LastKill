using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    [CreateAssetMenu(fileName = "Weapon data", menuName = "LastKill/Weapon data", order = 1)]
    public class WeaponData : ScriptableObject
    {
        private float lastShot;

        public GameObject weapon;
        public WeaponHolder holder;

        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public Transform LeftHandIK;

        public AudioSource audioSource;
        public ParticleSystem[] particleSystem;
        public AudioClip shootClip;
        public AudioClip emptyClip;
        public AudioClip reloadClip;

        public float fireRate;
        public int bulletMaxCount;
        public int bulletMaxClip;
        public int bulletCount;

        public int weaponId;

        public void Instantiate(GameObject obj)
		{
            lastShot = 0;
            try
			{
                particleSystem = obj.gameObject.GetComponentsInChildren<ParticleSystem>();
                audioSource = obj.GetComponentInChildren<AudioSource>();
                LeftHandIK = GameObject.Find("leftHandIK").transform;
            }
            catch(Exception ex)
			{
                Debug.Log(ex.Data);
			}
        }  
    }
}
