using Assets.Scripts.Tiles;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class SelectTargetAction : UnityEvent<SelectTargetActionData> { }

    public class SelectTargetActionData
    {
        public readonly float x;
        public readonly float z;

        public SelectTargetActionData(float x, float z)
        {
            this.x = x;
            this.z = z;
        }
    }
}
