using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    [CreateAssetMenu(fileName = "Weapon data", menuName = "LastKill/Weapon data", order = 1)]
    public class WeaponData : ScriptableObject
    {
        public GameObject weapon;
        public WeaponHolder holder;

        public Vector3 leftIK;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public Vector3 muzzlePosition;
        public Vector3 hitBox;

        public Transform LeftHandIK;

        public AudioSource audioSource;
        public ParticleSystem[] particleSystem;
        public AudioClip shootClip;
        public AudioClip emptyClip;
        public AudioClip reloadClip;

        public int bulletCount;
        public int bulletClip;
        public int currentCount;

        public int weaponId;

        public void Instantiate(GameObject obj)
		{
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
        public void Shoot()
		{
           
            
		}

    }
}
