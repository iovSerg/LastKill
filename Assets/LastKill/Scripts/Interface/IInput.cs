using UnityEngine;

public interface IInput
{
	public Vector2 Move { get; }

	public Vector2 Look { get; }
	public bool Sprint { get; }
	public bool Jump { get; }
	public bool Crouch { get; }
	public bool Roll { get; }
	public bool Fire { get; }
	public bool Crawl { get; }
	public bool Aim { get; }
	public bool Reload { get; }
	public float Magnituda { get; }
	public int CurrentWeapon { get; }
}
