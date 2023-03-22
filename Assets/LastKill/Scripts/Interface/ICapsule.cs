
namespace LastKill
{
	public interface ICapsule
	{
        void SetCapsuleSize(float newHeight, float newRadius);
        void ResetCapsuleSize();
        float GetCapsuleHeight();
        float GetCapsuleRadius();
        float GetCapsuleCenter();
        void EnableCollision();
        void DisableCollision();
    }

}