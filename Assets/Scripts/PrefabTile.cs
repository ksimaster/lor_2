using Assets.Scripts.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class PrefabTile : UnityEngine.Tilemaps.TileBase
{
    public readonly GameObject Prefab; //The gameobject to spawn
    public readonly MapTile MapTile;
    public readonly int Y;

    public PrefabTile(GameObject prefab, MapTile mapTile)
    {
        Prefab = prefab;
        MapTile = mapTile;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (Prefab) tileData.gameObject = Prefab;
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        go.transform.position += Vector3.up * 0.5f + Vector3.right * 0.5f;
        go.transform.rotation = Quaternion.Euler(0, 0, 0);

        return true;
    }

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
    {
        // Make sprite of tile invisiable
        tileAnimationData.animatedSprites = new Sprite[] { null };
        tileAnimationData.animationSpeed = 0;
        tileAnimationData.animationStartTime = 0;
        return true;
    }

}