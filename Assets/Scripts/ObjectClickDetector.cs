using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClickDetector : MonoBehaviour, IPointerDownHandler
{
    public Material selectMaterial;
    //public Material targetMaterial;
    public void OnPointerDown(PointerEventData eventData)
    {
        

        if (eventData.pointerId == -1)
        {
            Debug.Log("Нажали на " + eventData.pointerCurrentRaycast.gameObject.name + " левой клавишей мыши");
            //note x and y from tile
            //eventData.pointerCurrentRaycast.gameObject.transform.position.x
            PlayerPrefs.SetFloat("objectPointX", eventData.pointerCurrentRaycast.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("objectPointZ", eventData.pointerCurrentRaycast.gameObject.transform.position.z);
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0].color = selectMaterial.color;
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (eventData.pointerId == -2)
        {
            Debug.Log("Нажали на " + eventData.pointerCurrentRaycast.gameObject.name + " правой клавишей мыши");

        }
    }
}
