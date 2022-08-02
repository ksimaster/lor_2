using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClickDetector : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        

        if (eventData.pointerId == -1)
        {
            Debug.Log("Ќажали на баллисту левой клавишей мыши");

        }

        if (eventData.pointerId == -2)
        {
            Debug.Log("Ќажали на баллисту правой клавишей мыши");

        }
    }
}
