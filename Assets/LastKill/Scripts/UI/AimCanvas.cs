using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastKill
{

	public class AimCanvas : MonoBehaviour
	{
		[SerializeField] private GameObject simple, scope;
		[SerializeField] public float CameraLens;
		public int WeaponID;
		public void SimpleCanvas(bool state)
		{
			simple.SetActive(state);
		}
		public void ScopeCanvas(bool state)
		{
			scope.SetActive(state);
		}
	}
}
