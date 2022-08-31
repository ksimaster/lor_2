using Assets.Scripts.Tiles;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class SelectObjectAction : UnityEvent<SelectObjectActionData> { }

    public class SelectObjectActionData
    {
        public readonly GameObject _selectGameObject;

        public SelectObjectActionData(GameObject selectGameObject)
        {
            _selectGameObject = selectGameObject;
        }
    }
}
