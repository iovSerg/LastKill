using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class CrosshairTarget : MonoBehaviour
	{
		private Camera _mainCamera;
		private Ray ray;
		RaycastHit hit;

		private void Start()
		{
			_mainCamera = Camera.main;
		}
		private void Update()
		{
			ray.origin = _mainCamera.transform.position;
			ray.direction = _mainCamera.transform.forward;
			Physics.Raycast(ray, out hit);
			transform.position = hit.point;
		}
	}
}
