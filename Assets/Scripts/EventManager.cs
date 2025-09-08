using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    
    public static event Action<Tile> OnTileJumped;

    public static void TileJumped(Tile tile)
    {
        OnTileJumped?.Invoke(tile);
    }
}
