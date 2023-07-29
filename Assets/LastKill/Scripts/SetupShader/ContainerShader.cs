using UnityEngine;

namespace LastKill
{
	public class ContainerShader : MonoBehaviour
	{
		[SerializeField] private Renderer render;

		private void Start()
		{
			render = GetComponent<Renderer>();
			render.material.SetFloat("_Opacity", Random.Range(0.5f, 1.5f));
			render.material.SetFloat("_Rotate", Random.Range(0, 4.7f));

		}
	}
}
