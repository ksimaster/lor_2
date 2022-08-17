using Assets.Scripts;
using Assets.Scripts.Actions;
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
    public List<MapTile> availableTiles = new List<MapTile>();
    public GameState GameState;

    private BuildingsManager villageManager = new BuildingsManager();
    private Dictionary<MapTile, Assets.Scripts.Tiles.Tile> tilesCache = new Dictionary<MapTile, Assets.Scripts.Tiles.Tile>();
    private readonly System.Random Random = new System.Random();

    private void Awake()
    {
        CreateTiles();
    }

    void Start()
    {
        ActionManager.AddListener<EndOfTurnActionData>((voidData) => Debug.Log("End of turn"));
        ActionManager.TriggerEvent(new EndOfTurnActionData());

        ActionManager.AddListener<MoveActionData>((data) => Debug.Log($"{data.UnitTile.MapTile}, {data.TargetTile.MapTile}"));
        ActionManager.TriggerEvent(new MoveActionData(tilesCache[availableTiles[5]], tilesCache[availableTiles[6]]));

        ActionManager.AddListener<BuildActionData>((data) => Debug.Log(data));
        ActionManager.TriggerEvent(new BuildActionData(tilesCache[availableTiles[5]], UnitType.MILITIA, "1"));
    }

    void Update()
    {

    }

    public void CreateTiles()
    {
        var size = new Vector2Int(32, 32);
        var seed = Time.realtimeSinceStartup;
        var objCache = new Dictionary<MapTile, GameObject>();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var position = new Vector3Int(x, y, 0);
                float sample = Mathf.PerlinNoise(seed * 999 + (float)x / 4, (float)y / 4);
                if (sample > 0.3)
                {
                    CreateMovableTile(x, y);
                    var mapTile = new MapTile(x, y);
                    availableTiles.Add(mapTile);
                    objCache.Add(mapTile, tilemap.GetInstantiatedObject(position));
                }
                else
                {
                    var prefab = new PrefabTile(water, x, y);
                    tilemap.SetTile(position, prefab);
                }

            }
        }

        GameState = new GameState(availableTiles.ToArray());

        foreach(var kvpair in objCache)
        {
            var mapTile = kvpair.Key;
            UpdateDict(mapTile, kvpair.Value);
        }
        Debug.Log("Prepared");
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
                result.Add(tileGO.TileObject);
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
        }
        else if (Random.Next(0, WindmillRate) == 0 && villageManager.CheckWindmill(x, y))
        {
            var prefab = new PrefabTile(windmill, x, y);
            tilemap.SetTile(position, prefab);
            villageManager.AddWindmill(x, y);
        }
        else if (isForestNear && Random.Next(0, ForestRate) == 0
            || Random.Next(0, SingleForestRate) == 0)
        {
            var prefab = new PrefabTile(forest, x, y);
            tilemap.SetTile(position, prefab);
            villageManager.AddSmallForest(x, y);
        }
        else
        {
            var prefab = new PrefabTile(ground, x, y);
            tilemap.SetTile(position, prefab);
        }
    }

    public Assets.Scripts.Tiles.Tile GetTileByXY(int x, int y)
    {
        var tempMapTile = new MapTile(x, y);
        if (!tilesCache.ContainsKey(tempMapTile))
        {
            return null;
        }

        return tilesCache[tempMapTile];
    }

    private void UpdateDict(MapTile mapTile, GameObject objectInstance)
    {
        var tile = new Assets.Scripts.Tiles.Tile(mapTile, GameState, objectInstance);
        if (!tilesCache.ContainsKey(mapTile))
        {
            tilesCache.Add(mapTile, tile);
        }

        tilesCache[mapTile] = tile;
    }
}
