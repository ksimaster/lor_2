using Assets.Scripts.Village;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{
    private const int VillageRate = 10;
    public GameObject ground;
    public GameObject water;
    public GameObject village;
    public Tilemap tilemap;
    private VillagesManager villageManager = new VillagesManager();
    private Dictionary<int, Dictionary<int, GameObject>> tilesCache = new Dictionary<int, Dictionary<int, GameObject>>();

    void Start()
    {
        SetTiles();
    }

    void Update()
    {

    }

    public void SetTiles()
    {
        var random = new System.Random();

        Vector2Int size = new Vector2Int(32, 32);
        float seed = Time.realtimeSinceStartup;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                float sample = Mathf.PerlinNoise(seed * 999 + (float)x / 4, (float)y / 4);
                var position = new Vector3Int(x, y, 0);
                if (sample > 0.3)
                {
                    if (random.Next(0, VillageRate) == 0 && villageManager.CheckVillage(x, y))
                    {
                        var prefab = new PrefabTile(village, x, y);
                        tilemap.SetTile(position, prefab);
                        villageManager.AddVillage(x, y);
                        UpdateDict(prefab);
                    }
                    else
                    {
                        var prefab = new PrefabTile(ground, x, y);
                        tilemap.SetTile(position, prefab);
                        UpdateDict(prefab);
                    }
                }
                else
                {
                    var prefab = new PrefabTile(water, x, y);
                    tilemap.SetTile(position, prefab);
                    UpdateDict(prefab);
                }

                // ((PrefabTile)tilemap.GetTile(position)).name = $"tile_{x}_{y}";
            }
        }
    }

    public List<GameObject> GetNeighbors(int x, int y)
    {
        var result = new List<GameObject>();
        var neighbors = MapTile.Neighborhoods(x, y);
        foreach(var neighbor in neighbors)
        {
            var tileGO = GetTileByXY(neighbor[0], neighbor[1]);
            if (tileGO != null)
            {
                result.Add(tileGO);
            }
        }

        return result;
    }
    private GameObject GetTileByXY(int x, int y)
    {
        if (!tilesCache.ContainsKey(x))
        {
            return null;
        }

        if (!tilesCache[x].ContainsKey(y))
        {
            return null;
        }

        return tilesCache[x][y];
    }

    private void UpdateDict(PrefabTile prefabTile)
    {
        int x = prefabTile.X;
        int y = prefabTile.Y;
        if (!tilesCache.ContainsKey(x))
        {
            tilesCache.Add(x, new Dictionary<int, GameObject>());
        }

        tilesCache[x].Add(y, prefabTile.Prefab);
    }
}
