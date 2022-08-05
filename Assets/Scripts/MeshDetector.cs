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
            var go = eventData.pointerCurrentRaycast.gameObject;
            eventData.pointerCurrentRaycast.gameObject.GetComponent<MeshRenderer>().materials[0].color = detectorMaterial.color;
            var tileMapPosition = go.GetComponentInParent<GridLayout>().WorldToCell(eventData.pointerCurrentRaycast.gameObject.transform.position);
            Debug.Log(tileMapPosition);
            var tileMap = go.GetComponentInParent<TileMap>();
            var neighbors = tileMap.GetNeighbors(tileMapPosition.x, tileMapPosition.y);
            neighbors.ForEach(x => x.GetComponent<MeshRenderer>().materials[0].color = detectorMaterial.color);
            //note x and y from tile
            //eventData.pointerCurrentRaycast.gameObject.transform.position.x
            PlayerPrefs.SetFloat("targetPointX", eventData.pointerCurrentRaycast.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("targetPointZ", eventData.pointerCurrentRaycast.gameObject.transform.position.z);
        }

        if (eventData.pointerId == -2)
        {
            eventData.pointerCurrentRaycast.gameObject.GetComponent<MeshRenderer>().materials[0].color = defaultMaterial.color;
        }
    }
}