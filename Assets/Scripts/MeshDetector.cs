using UnityEngine;
using UnityEngine.EventSystems;

public class MeshDetector : MonoBehaviour, IPointerDownHandler
{
    public Material detectorMaterial;
    public Material defaultMaterial;
    
    private void Start()
    {
        addPhysicsRaycaster();
    }

    void addPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = Camera.main.GetComponent<PhysicsRaycaster>();// GameObject.FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);

         if(eventData.pointerId == -1)
         {
             eventData.pointerCurrentRaycast.gameObject.GetComponent<MeshRenderer>().materials[1].color = detectorMaterial.color;
             
         }

        if (eventData.pointerId == -2)
        {
            eventData.pointerCurrentRaycast.gameObject.GetComponent<MeshRenderer>().materials[1].color = defaultMaterial.color;

        }


    }
}