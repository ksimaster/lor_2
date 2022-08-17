using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Assets.Scripts.Actions
{
    public class ActionManager : MonoBehaviour
    {
        private Dictionary<string, UnityEventBase> actions;

        private static ActionManager instance;

        public static ActionManager Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindObjectOfType(typeof(ActionManager)) as ActionManager ;

                    if (!instance)
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                    else
                        instance.Init();
                }

                return instance;
            }
        }

        void Init()
        {
            if (actions == null)
            {
                actions = new Dictionary<string, UnityEventBase>();
            }
        }

        public static void AddListener<T>(UnityAction<T> listener)
        {
            var actionName = (typeof (T)).Name;
            if (Instance.actions.TryGetValue(actionName, out UnityEventBase evt))
            {
                (evt as UnityEvent<T>).AddListener(listener);
            }
            else
            {
                var typedEvent = new UnityEvent<T>();
                typedEvent.AddListener(listener);
                Instance.actions.Add(actionName, typedEvent);
            }
        }

        public static void RemoveListener<T>(UnityAction<T> listener)
        {
            var actionName = (typeof(T)).Name;
            if (instance == null) return;
            if (Instance.actions.TryGetValue(actionName, out UnityEventBase evt))
                (evt as UnityEvent<T>).RemoveListener(listener);
        }

        public static void TriggerEvent<T>(T data)
        {
            var actionName = (typeof(T)).Name;
            if (Instance.actions.TryGetValue(actionName, out UnityEventBase evt))
                (evt as UnityEvent<T>).Invoke(data);
        }
    }
}
