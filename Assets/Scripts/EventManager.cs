using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    
    public static event Action<Tile> OnTileJumped;
    public static event Action<Tile> OnFirstTileRevealed;

    public static void TileJumped(Tile tile)
    {
        OnTileJumped?.Invoke(tile);
    }

    //Testing
    public static void FirstTileRevealed(Tile tile)
    {
        OnFirstTileRevealed?.Invoke(tile);
    }
}
