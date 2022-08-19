using Assets.Scripts;
using Assets.Scripts.Actions;
using Assets.Scripts.Tiles;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject[] units;
    public GameObject forest;

    public Tilemap Tilemap;
    public GameState GameState;
    public GameLogic GameLogic;

    // Privates
    private GameObject castle;
    private GameObject garden;
    private GameObject tower;
    private GameObject militia;
    private GameObject swordmen;
    private GameObject spearman;
    private GameObject horseman;
    private GameObject balista;

    private TreasureManager treasureManager = new TreasureManager();
    private Dictionary<MapTile, Assets.Scripts.Tiles.Tile> tilesCache = new Dictionary<MapTile, Assets.Scripts.Tiles.Tile>();
    private readonly System.Random Random = new System.Random();

    private void Awake()
    {
        castle = units[0];
        garden = units[1];
        tower = units[2];
        militia = units[3];
        swordmen = units[4];
        spearman = units[5];
        horseman = units[6];
        balista = units[7];
        CreateTiles();
    }

    void Start()
    {
        var testTile1 = tilesCache[tilesCache.Keys.ToArray()[5]];
        var testTile2 = tilesCache[tilesCache.Keys.ToArray()[6]];
        var testTile3 = tilesCache[tilesCache.Keys.ToArray()[116]];

        ActionManager.AddListener<EndOfTurnActionData>((voidData) => Debug.Log("End of turn"));
        ActionManager.TriggerEvent(new EndOfTurnActionData());

        ActionManager.AddListener<MoveActionData>((data) => Debug.Log($"{data.UnitTile.MapTile}, {data.TargetTile.MapTile}"));
        ActionManager.TriggerEvent(new MoveActionData(testTile1, testTile2));

        ActionManager.AddListener<BuildActionData>((data) => {
            var mapTile = data.unitTile.MapTile;
            var position = Tilemap.CellToWorld(new Vector3Int(mapTile.X, mapTile.Y, 1));
            position.y = position.y + 1;
            var objectToBuild = data.unit switch
            {
                UnitType.CASTLE => castle,
                UnitType.GARDEN => garden,
                UnitType.TOWER => tower,
                UnitType.MILITIA => militia,
                UnitType.SWORDSMAN => swordmen,
                UnitType.SPEARMAN => spearman,
                UnitType.HORSEMAN => horseman,
                UnitType.BALISTA => balista,
                _ => null
            };

            // UI
            if (objectToBuild != null)
            {
                var unitObject = Instantiate(objectToBuild, position, new Quaternion());
                tilesCache[mapTile].UnitObject = unitObject;
            }

            // Logic
            if (objectToBuild != null)
            {
                var prevUnit = GameState.GetCellUnit(mapTile);
                var tileToBuild = GameState.GetTileState(mapTile);

                tileToBuild.SetUnitType(data.unit);
                if (prevUnit == UnitType.EMPTY)
                {
                    tileToBuild.Activate();
                }
                else if (tileToBuild.GetMoves() == 1)
                {
                    tileToBuild.SetMoves(GameConstants.MovableUnits[data.unit]);
                }
            }
        });
        
        ActionManager.TriggerEvent(new BuildActionData(testTile1, UnitType.CASTLE, "1"));
        ActionManager.TriggerEvent(new BuildActionData(testTile2, UnitType.BALISTA, "1"));
        ActionManager.TriggerEvent(new BuildActionData(testTile3, UnitType.BALISTA, "1"));
    }

    void Update()
    {

    }

    public void CreateTiles()
    {
        var size = new Vector2Int(32, 32);
        var seed = Time.realtimeSinceStartup;
        var objCache = new Dictionary<MapTile, GameObject>();
        var treasuresMap = new Dictionary<MapTile, TreasureTileType>();
        var availableTiles = new List<MapTile>();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                float sample = Mathf.PerlinNoise(seed * 999 + (float)x / 4, (float)y / 4);
                var mapTile = new MapTile(x, y);
                if (sample > 0.3)
                {
                    var treasure = CreateTreasure(x, y);
                    availableTiles.Add(mapTile);
                    if (treasure != TreasureTileType.GRASS)
                    {
                        treasuresMap.Add(mapTile, treasure);
                    }
                }
            }
        }

        Debug.Log($"TileMap size before filtering largest area = " + availableTiles.Count);
        availableTiles = Area.FindLargestArea(availableTiles);

        Debug.Log($"TileMap size after filtering largest area = " + availableTiles.Count);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var position = new Vector3Int(x, y, 0);
                var mapTile = new MapTile(x, y);

                if (availableTiles.Contains(mapTile))
                {
                    CreateMovableTile(mapTile, !treasuresMap.ContainsKey(mapTile) ? TreasureTileType.GRASS : treasuresMap[mapTile]);
                    objCache.Add(mapTile, Tilemap.GetInstantiatedObject(position));
                }
                else
                {
                    var prefab = new PrefabTile(water, mapTile);
                    Tilemap.SetTile(position, prefab);
                }
            }
        }

        GameState = new GameState(availableTiles.ToArray(), treasuresMap);



        foreach (var kvpair in objCache)
        {
            var mapTile = kvpair.Key;
            UpdateDict(mapTile, kvpair.Value);
        }

        var castleTile = tilesCache[tilesCache.Keys.ToArray()[5]];
        castleTile.SetUnitType(UnitType.CASTLE);
        var neighbors = GetNeighbors(castleTile);
        var areaId = Area.GetAreaId(0, 0);
        castleTile.SetAreaId(areaId);
        neighbors.ForEach(x => x.SetAreaId(areaId));

        GameLogic = new GameLogic(this, new Player[] { new Player(0,0,this)});
        GameLogic.RunNextTurn();
        Debug.Log("Prepared");
    }

    public List<Assets.Scripts.Tiles.Tile> GetNeighbors(Assets.Scripts.Tiles.Tile tile) =>
        GetNeighbors(tile.MapTile.X, tile.MapTile.Y);

    public List<Assets.Scripts.Tiles.Tile> GetNeighbors(int x, int y)
    {
        var result = new List<Assets.Scripts.Tiles.Tile>();
        var neighbors = MapTile.Neighborhoods(x, y);
        foreach (var neighbor in neighbors)
        {
            var tileGO = GetTileByXY(neighbor[0], neighbor[1]);
            if (tileGO != null)
            {
                result.Add(tileGO);
            }
        }

        return result;
    }

    private TreasureTileType CreateTreasure(int x, int y)
    {
        var isForestNear = treasureManager.CheckForest(x, y);
        if (Random.Next(0, VillageRate) == 0 && treasureManager.CheckVillage(x, y))
        {
            treasureManager.AddVillage(x, y);
            return TreasureTileType.VILLAGE;
        }
        else if (Random.Next(0, WindmillRate) == 0 && treasureManager.CheckWindmill(x, y))
        {
            treasureManager.AddWindmill(x, y);
            return TreasureTileType.WINDMILL;
        }
        else if (isForestNear && Random.Next(0, ForestRate) == 0
            || Random.Next(0, SingleForestRate) == 0)
        {
            treasureManager.AddSmallForest(x, y);
            return TreasureTileType.SMALL_FOREST;
        }

        return TreasureTileType.GRASS;
    }


    private void CreateMovableTile(MapTile mapTile, TreasureTileType treasure)
    {
        var position = new Vector3Int(mapTile.X, mapTile.Y, 0);

        if (treasure == TreasureTileType.VILLAGE)
        {
            var prefab = new PrefabTile(village, mapTile);
            Tilemap.SetTile(position, prefab);
        }
        else if (treasure == TreasureTileType.WINDMILL)
        {
            var prefab = new PrefabTile(windmill, mapTile);
            Tilemap.SetTile(position, prefab);
        }
        else if (treasure == TreasureTileType.SMALL_FOREST)
        {
            var prefab = new PrefabTile(forest, mapTile);
            Tilemap.SetTile(position, prefab);
        }
        else
        {
            var prefab = new PrefabTile(ground, mapTile);
            Tilemap.SetTile(position, prefab);
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
