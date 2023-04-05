using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    [CreateAssetMenu(fileName = "Camera data", menuName = "LastKill/Camera data", order = 1)]
    public class CameraData : ScriptableObject
    {
        public Vector3 position;
        public string cameraName;
        public string tagName;
        public int priority;
    }
}
