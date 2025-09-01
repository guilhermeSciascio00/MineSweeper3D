using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileData Data { get; private set; }

    private void Start()
    {
        
    }

    public void InitializeData(TileData data)
    {
        Data = data;
    }
}
