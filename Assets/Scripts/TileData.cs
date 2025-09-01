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
    public Vector3Int TilePosition { get; set; }
    public int Number { get; set; }
    public bool IsFlagged { get; set; }
    public bool IsRevealed { get; set; }
    public bool HasExploded { get; set; }

}
