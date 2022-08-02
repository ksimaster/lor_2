using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClickDetector : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        

        if (eventData.pointerId == -1)
        {
            Debug.Log("������ �� " + eventData.pointerCurrentRaycast.gameObject.name + " ����� �������� ����");

        }

        if (eventData.pointerId == -2)
        {
            Debug.Log("������ �� " + eventData.pointerCurrentRaycast.gameObject.name + " ������ �������� ����");

        }
    }
}
