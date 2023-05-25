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
        public ParticleSystem particleSystem;
        public AudioSource audioSource;
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
                GameObject leftIK = new GameObject("LeftIK");
                leftIK.transform.parent = obj.transform;
                leftIK.transform.localPosition = Vector3.zero;
                leftIK.transform.localPosition = this.leftIK;

                GameObject muzzlePosition = new GameObject("muzzel");
                muzzlePosition.transform.parent = obj.transform;
                muzzlePosition.transform.localPosition = Vector3.zero;
                muzzlePosition.transform.localPosition = this.muzzlePosition;

                Instantiate(particleSystem,Vector3.zero,Quaternion.identity, muzzlePosition.transform);
            }
            catch(Exception ex)
			{
                Debug.Log(ex.Data);
			}
        }

    }
}
