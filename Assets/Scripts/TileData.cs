using UnityEngine;

public class TileData
{
    public enum TileType
    {
        Empty,
        Number,
        Mine
    }

    public TileType TiType { get; set; }
    //TilePosition in the grid not in the world space
    public Vector2Int TilePosition { get; set; }
    public int MineNumbers { get; set; }
    public bool IsFlagged { get; set; }
    public bool IsRevealed { get; set; }
    public bool HasExploded { get; set; }

}
