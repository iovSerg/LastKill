using UnityEngine;

namespace LastKill
{
    public interface IDetection
    {
        public bool CanGetUp(float offset);
        public DescriptionClimb ClimbTargets();
        public Vector3 ClimbTarget();
        public float ForwardHeight { get; }

    }
}