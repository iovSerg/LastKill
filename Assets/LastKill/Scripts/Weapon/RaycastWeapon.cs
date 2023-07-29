using UnityEngine;

[ExecuteInEditMode]
public class RaycastWeapon : MonoBehaviour
{
	private void Update()
	{

	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;


		Gizmos.DrawRay(transform.position, transform.forward * 10f);
	}
}
