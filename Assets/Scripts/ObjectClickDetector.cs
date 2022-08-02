using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClickDetector : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        

        if (eventData.pointerId == -1)
        {
            Debug.Log("Нажали на " + eventData.pointerCurrentRaycast.gameObject.name + " левой клавишей мыши");

        }

        if (eventData.pointerId == -2)
        {
            Debug.Log("Нажали на " + eventData.pointerCurrentRaycast.gameObject.name + " правой клавишей мыши");

        }
    }
}
