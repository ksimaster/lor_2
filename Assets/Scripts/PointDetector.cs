using UnityEngine;
using UnityEngine.EventSystems;

public class PointDetector : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Нажали на тайл");
    }
}
