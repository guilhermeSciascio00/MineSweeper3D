using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    
    public static event Action<Tile> OnTileJumped;
    public static event Action<Tile> OnFirstTileRevealed;
    public static event Action OnGameOver;
    public static event Action OnGameWon;

    public static void TileJumped(Tile tile)
    {
        OnTileJumped?.Invoke(tile);
    }

    public static void FirstTileRevealed(Tile tile)
    {
        OnFirstTileRevealed?.Invoke(tile);
    }

    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public static void GameWon()
    {
        OnGameWon?.Invoke();
    }
}
