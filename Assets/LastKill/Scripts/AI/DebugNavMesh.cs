using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LastKill
{
	public class DebugNavMesh : MonoBehaviour
	{
		private NavMeshAgent _agent = null;

		public bool velocity = false;
		public bool desireVelocity = false;
		public bool path = false;

		private void Start()
		{
			_agent = GetComponent<NavMeshAgent>();
		}

		private void Update()
		{
			
		}
		private void OnDrawGizmos()
		{
			//if (velocity)
			//{
			//	Gizmos.color = Color.yellow;
			//	Gizmos.DrawLine(transform.position, transform.position + _agent.velocity);
			//}
			//if (desireVelocity)
			//{
			//	Gizmos.color = Color.red;
			//	Gizmos.DrawLine(transform.position, transform.position + _agent.desiredVelocity);
			//}

			//if(path)
			//{
			//	Gizmos.color = Color.green;
			//	NavMeshPath agentPath = _agent.path;
			//	Vector3 prevCorner = transform.position;
			//	foreach(var corner  in agentPath.corners)
			//	{
			//		Gizmos.DrawLine(prevCorner,corner);
			//		Gizmos.DrawSphere(corner, 0.1f);
			//		prevCorner = corner;
			//	}
			//}
		}
	}
}
