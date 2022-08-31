using Assets.Scripts.Tiles;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class SelectTargetAction : UnityEvent<SelectTargetActionData> { }

    public class SelectTargetActionData
    {
        public readonly GameObject _selectGameObject;

        public SelectTargetActionData(GameObject selectGameObject)
        {
            _selectGameObject = selectGameObject;
        }
    }
}
