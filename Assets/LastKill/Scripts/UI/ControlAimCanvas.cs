using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace LastKill
{
	public class ControlAimCanvas : MonoBehaviour
	{
		private int lastID = 0;
		private Vector2 screenCenterPoint;
		private Ray rayCenter;
		[SerializeField] private LayerMask aimColliderMask = new LayerMask();
		[SerializeField] private Transform targetTransform;

		PlayerInput _input;
		public List <AimCanvas> collectionAimCanvas;
		public AimCanvas currentAimCanvas;
		public bool aim = false;
		public UnityAction<float,bool> OnLensAimCamera { get; set; }


		private void Awake()
		{
			_input = GetComponentInParent<PlayerInput>();

			_input.OnSelectWeapon += OnSelectWeapon;
			_input.OnAiming += OnAiming;
			_input.OnFire += OnFire;
		}
		private void Start()
		{
			screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
		}
		private void Update()
		{
			rayCenter = Camera.main.ScreenPointToRay(screenCenterPoint);
			if (Physics.Raycast(rayCenter, out RaycastHit raycast, 999f, aimColliderMask))
			{
				targetTransform.position = raycast.point;
			}
		}

		private void OnFire(bool obj)
		{
			if(currentAimCanvas != null)
			{
				
			}
		}

		private void OnAiming(bool state)
		{
			if(currentAimCanvas != null)
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
			if(currentAimCanvas != null)
			{
				currentAimCanvas.SimpleCanvas(false);
				currentAimCanvas.ScopeCanvas(false);
			}
			lastID = 0;
			currentAimCanvas = null;
		}
	}
}
