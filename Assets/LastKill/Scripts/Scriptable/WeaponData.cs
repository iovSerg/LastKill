using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    [CreateAssetMenu(fileName = "Weapon data", menuName = "LastKill/Weapon data", order = 1)]
    public class WeaponData : ScriptableObject
    {
        public GameObject weapon;

        //public Vector3 position { get => transform.position; set => transform.position = value; }
        //public Quaternion rotation {  get => transform.rotation ; set => transform.rotation = value; } 
        //public Vector3 scale { get => transform.localScale; set => transform.localScale = value; }

        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public List<ParticleSystem> particleSystem;   

        public int weaponId;

	}
}
