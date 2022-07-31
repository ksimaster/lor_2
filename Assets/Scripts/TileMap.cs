using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{
    public GameObject myPrefab;
    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        SetTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTiles() {
        Vector2Int size = new Vector2Int(32, 32);
        float seed = Time.realtimeSinceStartup;
         for (int x = 0; x < size.x; x++) {
             for (int y = 0; y < size.y; y++) {
                float sample = Mathf.PerlinNoise(seed *999 + (float)x/4, (float)y/4);
                if (sample > 0.3)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), new PrefabTile(myPrefab, x, y));
                }
             }
         }
     }
}
