using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class WeaponController : MonoBehaviour
    {
        //[SerializeField] private string animBlandState = "Draw Weapon HighLeft";

		private IAnimator animator;
		private PlayerInput input;
		public bool isAim = false;
		private void Awake()
		{
			input = GetComponent<PlayerInput>();
			input.OnSelectWeapon += SelectWeapon;
			animator = GetComponent<IAnimator>();
		}

		private void SelectWeapon()
		{
			
			
		}

		private void Update()
		{
			
		}
	}
}
