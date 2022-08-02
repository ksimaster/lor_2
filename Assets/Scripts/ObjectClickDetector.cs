using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClickDetector : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        

        if (eventData.pointerId == -1)
        {
            Debug.Log("Нажали на " + eventData.pointerCurrentRaycast.gameObject.name + " левой клавишей мыши");
            //note x and y from tile
            //eventData.pointerCurrentRaycast.gameObject.transform.position.x
            PlayerPrefs.SetFloat("objectPointX", eventData.pointerCurrentRaycast.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("objectPointZ", eventData.pointerCurrentRaycast.gameObject.transform.position.z);

        }

        if (eventData.pointerId == -2)
        {
            Debug.Log("Нажали на " + eventData.pointerCurrentRaycast.gameObject.name + " правой клавишей мыши");

        }
    }
}
