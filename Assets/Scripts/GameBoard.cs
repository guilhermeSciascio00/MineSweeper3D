using UnityEngine;

public class GameBoard : MonoBehaviour
{
    
    [SerializeField] private Tile _tileToSpawn;
    [SerializeField] private Vector2Int _boardSize;

    private Tile[,] _tiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initializing the tiles 2D array.
        _tiles = new Tile[_boardSize.x, _boardSize.y];
        CreateTiles();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Tile CreateOneTile()
    {
        Tile tile = Instantiate(_tileToSpawn, transform.position, Quaternion.identity, this.transform);

        return tile;
    }

    private void CreateTiles()
    {
        
        int tileXSize;
        int tileZSize;


        for (int i = 0; i < _boardSize.x; i++)
        {
            for (int j = 0;  j < _boardSize.y; j++)
            {
                _tiles[i, j] = CreateOneTile();

                Tile tile = _tiles[i, j];
                tileXSize = (int)tile.transform.localScale.x;
                tileZSize = (int)tile.transform.localScale.z;

                tile.transform.position = new Vector3(i * tileXSize, 0f, j * tileZSize);

                tile.name = $"x: {tile.transform.position.x}, z: {tile.transform.position.z}";

                //Initializing the data for each created tile
                TileData tileData = new TileData()
                {
                    TilePosition = new Vector3Int(i * tileXSize, 0, j * tileXSize),
                    TiType = TileData.TileType.Empty,
                    Number = 0,
                    HasExploded = false,
                    IsFlagged = false,
                    IsRevealed = false,
                };

                //Sets the newTileData
                tile.InitializeData(tileData);
            }
        }
    }
}
