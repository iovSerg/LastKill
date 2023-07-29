using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LastKill
{
	public class ControlAimCanvas : MonoBehaviour
	{
		private int lastID = 0;
		PlayerInput _input;
		public List<AimCanvas> collectionAimCanvas;
		public AimCanvas currentAimCanvas;
		public bool aim = false;
		public UnityAction<float, bool> OnLensAimCamera { get; set; }


		private void Awake()
		{
			_input = GameObject.FindAnyObjectByType<PlayerInput>();

			_input.EventSelectWeapon += OnSelectWeapon;
			_input.EventAim += OnAiming;
			_input.EventFire += OnFire;
		}
		private void Update()
		{
			
		}

		private void OnFire(bool obj)
		{
			if (currentAimCanvas != null)
			{

			}
		}

		private void OnAiming(bool state)
		{
			if (currentAimCanvas != null)
			{
				currentAimCanvas.SimpleCanvas(!state);
				currentAimCanvas.ScopeCanvas(state);
				OnLensAimCamera?.Invoke(currentAimCanvas.CameraLens, state);
			}
		}

		private void OnSelectWeapon(int id)
		{
			if (id == 0)
			{
				DisableAll();
			}
			if (id != lastID)
			{
				foreach (var aim in collectionAimCanvas)
				{
					if (aim.WeaponID == id)
					{
						DisableAll();
						currentAimCanvas = aim;
						currentAimCanvas.SimpleCanvas(true);
						lastID = id;
					}
				}
			}
			else DisableAll();
		}

		private void DisableAll()
		{
			if (currentAimCanvas != null)
			{
				currentAimCanvas.SimpleCanvas(false);
				currentAimCanvas.ScopeCanvas(false);
			}
			lastID = 0;
			currentAimCanvas = null;
		}
	}
}
