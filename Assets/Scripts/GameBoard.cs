using UnityEngine;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour
{
    
    [SerializeField] private Tile _tileToSpawn;
    [SerializeField] private Vector2Int _boardSize;
    //Mine position relative to the boardTile and not the Tile position.
    [SerializeField] private List<Vector2Int> _minePositions = new List<Vector2Int>();

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

                //Here we need to take the tileSize into consideration, otherwise they'll overlap each other.
                tile.transform.position = new Vector3(i * tileXSize, 0f, j * tileZSize);

                //Reference in the grid only
                tile.name = $"xTile: {i}, yTile: {j}";

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
        SetMinesRandomly();
    }

    private void SetMine(Vector2Int minePosition)
    {
        _tiles[minePosition.x, minePosition.y].Data.TiType = TileData.TileType.Mine;

        _minePositions.Add(minePosition);
    }

    private void SetMinesRandomly()
    {

        int maxMineAmount = 10;

        int randomX;
        int randomY;

        Vector2Int randomPos;

        while (_minePositions.Count < maxMineAmount)
        {
            //Gets a random position between the 0 and the gameBoard
            randomX = Random.Range(0, _boardSize.x);
            randomY = Random.Range(0, _boardSize.y);
            randomPos = new Vector2Int(randomX, randomY);

            //if it already exists in our _minePositions list, we skip
            if (_minePositions.Contains(randomPos))
            {
                continue;
            }
            //if not, we add the mine
            else
            {
                SetMine(randomPos);
            }
        }
    }
}
    