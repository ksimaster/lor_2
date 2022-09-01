using Assets.Scripts.Tiles;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class SelectObjectAction : UnityEvent<SelectObjectActionData> { }

    public class SelectObjectActionData
    {
        public readonly float x;
        public readonly float z;
        public readonly int id;

        public SelectObjectActionData(float x, float z, int id)
        {
            this.x = x;
            this.z = z;
            this.id = id;
        }
    }
}
