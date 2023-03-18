using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RaycastWeapon : MonoBehaviour
{
    private void Update()
    {
       // Debug.DrawRay(transform.position,transform.forward, Color.green, 200f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;


        Gizmos.DrawRay(transform.position, transform.forward * 10f);
    }
}
