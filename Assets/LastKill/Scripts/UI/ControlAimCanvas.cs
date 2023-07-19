using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LastKill
{
	public class ControlAimCanvas : MonoBehaviour
	{
		PlayerInput _input;
		public List <AimCanvas> collectionAimCanvas;
		public bool aim = false;
		private void Awake()
		{
			_input = GetComponentInParent<PlayerInput>();

			_input.OnSelectWeapon += OnSelectWeapon;
			_input.OnAiming += OnAiming;
			_input.OnFire += OnFire;
		}

		private void OnFire(bool obj)
		{
			
		}

		private void OnAiming(bool state)
		{
			collectionAimCanvas[0].SimpleCanvas(!state);
			collectionAimCanvas[0].ScopeCanvas(state);
		}

		private void OnSelectWeapon(int id)
		{
			if (id != 0)
			{
				collectionAimCanvas[0].SimpleCanvas(true);
			}
			else
				collectionAimCanvas[0].SimpleCanvas(false);
		}
	}
}
