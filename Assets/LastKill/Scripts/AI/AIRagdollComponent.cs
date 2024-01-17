using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	[RequireComponent(typeof(Rigidbody))]
	public class AIRagdollComponent : MonoBehaviour
	{
		private	Rigidbody rb;
		[SerializeField] private GameObject bodyPart;
		[SerializeField] private GameObject bodyPartRagdoll;
		[SerializeField] private MeshRenderer mesh;
		[SerializeField] private float _force;

		private void Start()
		{
			rb = GetComponent<Rigidbody>();
			mesh = GetComponent<MeshRenderer>();
		}
		[ContextMenu("Press")]
		public void Press()
		{
			bodyPart.SetActive(false);
			mesh.enabled = true;
			rb.isKinematic = false;
			rb.AddForce(transform.up * _force, ForceMode.Impulse);
		}
	}

}