using Assets.Scripts.Actions;
using Assets.Scripts.Tiles;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeshDetector : MonoBehaviour, IPointerDownHandler
{
    public Material detectorMaterial;
    public Material defaultMaterial;
    public GameObject map;
    private GameObject [] tiles;
    
    private void Start()
    {
        addPhysicsRaycaster();

        tiles = new GameObject[map.transform.childCount];
        for (int i = 0; i < map.transform.childCount; i++)
        {
            tiles[i] = map.transform.GetChild(i).gameObject;
        }
        
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

            foreach(GameObject tile in tiles)
            {
                tile.transform.GetChild(2).gameObject.SetActive(false);
            }
            eventData.pointerCurrentRaycast.gameObject.transform.parent.GetChild(2).gameObject.SetActive(true);
            var tileMapPosition = go.GetComponentInParent<GridLayout>().WorldToCell(eventData.pointerCurrentRaycast.gameObject.transform.position);
            var tileMap = go.GetComponentInParent<TileMap>();
            var tileTarget = tileMap.GetTileByXY(tileMapPosition.x, tileMapPosition.y);
            ActionManager.TriggerEvent(new MoveActionData(tileTarget, tileTarget));


            //  neighbors.ForEach(x => x.GetComponent<MeshRenderer>().materials[0].color = detectorMaterial.color); // заменить на выделение объектов
            //note x and y from tile
            //eventData.pointerCurrentRaycast.gameObject.transform.position.x
            ActionManager.AddListener<SelectTargetActionData>((data) => {
                Debug.Log($"Координаты выбранной цели: {data.x}, {data.z}");
            });
            ActionManager.TriggerEvent(new SelectTargetActionData(eventData.pointerCurrentRaycast.gameObject.transform.position.x, eventData.pointerCurrentRaycast.gameObject.transform.position.z));
           // PlayerPrefs.SetFloat("targetPointX", eventData.pointerCurrentRaycast.gameObject.transform.position.x);
            //PlayerPrefs.SetFloat("targetPointZ", eventData.pointerCurrentRaycast.gameObject.transform.position.z);
        }

        if (eventData.pointerId == -2)
        {
            eventData.pointerCurrentRaycast.gameObject.GetComponent<MeshRenderer>().materials[0].color = defaultMaterial.color;
        }
    }
}