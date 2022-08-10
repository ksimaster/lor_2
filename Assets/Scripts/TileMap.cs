using Assets.Scripts.Tiles;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{
    private const int VillageRate = 10;
    private const int WindmillRate = 20;
    private const int SingleForestRate = 40;
    private const int ForestRate = 10;
    public GameObject ground;
    public GameObject water;
    public GameObject village;
    public GameObject windmill;
    public GameObject castle;
    public GameObject forest;

    public Tilemap tilemap;
    private BuildingsManager villageManager = new BuildingsManager();
    private Dictionary<int, Dictionary<int, GameObject>> tilesCache = new Dictionary<int, Dictionary<int, GameObject>>();
    private readonly System.Random Random = new System.Random();

    private void Awake()
    {
        CreateTiles();
    }
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void CreateTiles()
    {
        Vector2Int size = new Vector2Int(32, 32);
        float seed = Time.realtimeSinceStartup;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                float sample = Mathf.PerlinNoise(seed * 999 + (float)x / 4, (float)y / 4);
                if (sample > 0.3)
                {
                    CreateMovableTile(x, y);
                }
                else
                {
                    var position = new Vector3Int(x, y, 0);
                    var prefab = new PrefabTile(water, x, y);
                    tilemap.SetTile(position, prefab);
                    var obj = tilemap.GetInstantiatedObject(position);
                    UpdateDict(obj, x, y);
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

    private void CreateMovableTile(int x, int y)
    {
        var position = new Vector3Int(x, y, 0);
        var isForestNear = villageManager.CheckForest(x, y);
        if (Random.Next(0, VillageRate) == 0 && villageManager.CheckVillage(x, y))
        {
            var prefab = new PrefabTile(village, x, y);
            tilemap.SetTile(position, prefab);
            villageManager.AddVillage(x, y);
            var obj = tilemap.GetInstantiatedObject(position);
            UpdateDict(obj, x, y);
        }
        else if (Random.Next(0, WindmillRate) == 0 && villageManager.CheckWindmill(x, y))
        {
            var prefab = new PrefabTile(windmill, x, y);
            tilemap.SetTile(position, prefab);
            villageManager.AddWindmill(x, y);
            var obj = tilemap.GetInstantiatedObject(position);
            UpdateDict(obj, x, y);
        }
        else if (isForestNear && Random.Next(0, ForestRate) == 0
            || Random.Next(0, SingleForestRate) == 0)
        {
            var prefab = new PrefabTile(forest, x, y);
            tilemap.SetTile(position, prefab);
            villageManager.AddSmallForest(x, y);
            var obj = tilemap.GetInstantiatedObject(position);
            UpdateDict(obj, x, y);
        }
        else
        {
            var prefab = new PrefabTile(ground, x, y);
            tilemap.SetTile(position, prefab);
            var obj = tilemap.GetInstantiatedObject(position);
            UpdateDict(obj, x, y);
        }
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

    private void UpdateDict(GameObject objectInstance, int x, int y)
    {
        if (!tilesCache.ContainsKey(x))
        {
            tilesCache.Add(x, new Dictionary<int, GameObject>());
        }

        tilesCache[x].Add(y, objectInstance);
    }
}
