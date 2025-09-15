using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameBoard : MonoBehaviour
{

    [Header("Board Functions")]
    [SerializeField] private Tile _tileToSpawn;
    [SerializeField] private Vector2Int _boardSize;
    //Mine position relative to the boardTile and not the Tile position.
    [SerializeField] private List<Vector2Int> _minePositions = new List<Vector2Int>();

    private Tile[,] _tiles;

    private bool _firstTileRevealed = false;
    private bool _isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initializing the tiles 2D array.
        _tiles = new Tile[_boardSize.x, _boardSize.y];
        CreateTiles();

        EventManager.OnFirstTileRevealed += OnTileFirstRevealed;
        EventManager.OnGameOver += GameOverSequence;
        EventManager.OnTileJumped += TileJumped;
    }

    private void TileJumped(Tile obj)
    {
        if (!obj.Data.IsRevealed)
        {
            if (!_firstTileRevealed)
            {
                _firstTileRevealed = true;
                EventManager.FirstTileRevealed(obj);
            }

            if(obj.Data.TiType == TileData.TileType.Mine)
            {
                obj.GetComponent<Renderer>().material = obj.GetMineMaterial();

                obj.PlayExplosionVFX();
                obj.Data.IsRevealed = true;

                EventManager.GameOver();
            }

            FloodFillAlg(this, obj.Data.TilePosition);
        }
    }

    private void GameOverSequence()
    {
        _isGameOver = true;
        StartCoroutine(MineExplosionCoroutine());
    }

    IEnumerator MineExplosionCoroutine()
    {
        foreach(Vector2Int minePos in _minePositions)
        {
            if (_tiles[minePos.x, minePos.y].Data.IsRevealed) { yield return new WaitForSeconds(.1f); }

            _tiles[minePos.x, minePos.y].GetComponent<Renderer>().material = _tiles[minePos.x, minePos.y].GetMineMaterial();

            _tiles[minePos.x, minePos.y].PlayExplosionVFX();
            _tiles[minePos.x, minePos.y].Data.IsRevealed = true;
            yield return new WaitForSeconds(.2f);
        }
        
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

        //row
        for (int i = 0; i < _boardSize.x; i++)
        {
            //column
            for (int j = 0;  j < _boardSize.y; j++)
            {
                _tiles[i, j] = CreateOneTile();

                Tile tile = _tiles[i, j];
                tileXSize = (int)tile.transform.localScale.x;
                tileZSize = (int)tile.transform.localScale.z;

                //Here we need to take the tileSize into consideration, otherwise they'll overlap each other.
                tile.transform.position = new Vector3(i * tileXSize, 0f, j * tileZSize);

                //Initializing the data for each created tile
                TileData tileData = new TileData()
                {
                    TilePosition = new Vector2Int(i, j),
                    TiType = TileData.TileType.Empty,
                    MineNumbers = 0,
                    HasExploded = false,
                    IsFlagged = false,
                    IsRevealed = false,
                };

                //Sets the newTileData
                tile.InitializeData(tileData);

                //Reference in the grid only
                tile.name = $"xTile: {tile.Data.TilePosition.x}, yTile: {tile.Data.TilePosition.y}";
            }
            
        }
        //This methods should only be done after the player reveal the first tile!

        //SetMinesRandomly(new List<Vector2Int> { new Vector2Int(0,0)});
        
        //Check for neighbors.
        //for(int i = 0; i < _boardSize.x; i++)
        //{
        //    for (int j = 0; j < _boardSize.y; j++)
        //    {
        //        _tiles[i,j].CheckForNeighbors();
        //    }
        //}
    }

    private void OnTileFirstRevealed(Tile obj)
    {
        Vector2Int tilePos = obj.Data.TilePosition;
        _tiles[tilePos.x, tilePos.y].CheckForNeighbors();
        SetMinesRandomly(obj.GetTileNeighbors(), tilePos);
        CheckNeighborsInTheBoard();
    }

    private void SetMine(Vector2Int minePosition)
    {
        _tiles[minePosition.x, minePosition.y].Data.TiType = TileData.TileType.Mine;

        _tiles[minePosition.x, minePosition.y].UpdateTileText();

        _minePositions.Add(minePosition);
    }

    private void SetMinesRandomly(List<Vector2Int> tilesToIgnore, Vector2Int firstTilePos)
    {

        int maxMineAmount = 15;

        int randomX;
        int randomY;

        Vector2Int randomPos;

        while (_minePositions.Count < maxMineAmount)
        {
            //Gets a random position between the 0 and the gameBoard
            randomX = Random.Range(0, _boardSize.x);
            randomY = Random.Range(0, _boardSize.y);
            randomPos = new Vector2Int(randomX, randomY);

            //if it already exists in our _minePositions list or in the tiles that we asked tot ignore, we skip
            if (_minePositions.Contains(randomPos) || tilesToIgnore.Contains(randomPos) || randomPos == firstTilePos)
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

    private void CheckNeighborsInTheBoard()
    {
        for (int i = 0; i < _boardSize.x; i++)
        {
            for (int j = 0; j < _boardSize.y; j++)
            {
                _tiles[i, j].CheckForNeighbors();
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        return _tiles[x, y];
    }
    
    /// <summary>
    /// Returns true if the x,y coord values are inside the bounds, use this method when verifying neighbor tiles
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsInsideBounds(int x, int y)
    {
        return (x >= 0 && x < _boardSize.x) && (y >= 0 && y < _boardSize.y);
    }

    //Recursive method
    /// <summary>
    /// The first parameter here is the current game board, and the second one is the tile position.
    /// </summary>
    /// <param name="board"></param>
    /// <param name="startingPos"></param>
    private void FloodFillAlg(GameBoard board, Vector2Int startingPos)
    {

        if(_isGameOver) { return; }

        //Debug.Log("Testing flag 0");

        if (!board.IsInsideBounds(startingPos.x, startingPos.y) || board.GetTile(startingPos.x, startingPos.y).Data.IsRevealed)
        {
            return;
        }

        //Debug.Log("Testing flag1");
        if (board.GetTile(startingPos.x, startingPos.y).Data.TiType != TileData.TileType.Empty)
        {
            if (board.GetTile(startingPos.x, startingPos.y).Data.TiType == TileData.TileType.Number)
            {
                board.GetTile(startingPos.x, startingPos.y).Data.IsRevealed = true;
                board.GetTile(startingPos.x, startingPos.y).UpdateTileVisual();

            }

            return;
        }

        //Debug.Log("Testing flag2");
        //Empty Tile
        board.GetTile(startingPos.x, startingPos.y).Data.IsRevealed = true;
        board.GetTile(startingPos.x, startingPos.y).UpdateTileVisual();

        //Debug.Log("Testing flag3");
        Tile currentTile = board.GetTile(startingPos.x, startingPos.y);

        //Here it's important to check if the neighbor counts is lequal than zero, because if so, there won't be any neighbors around and we'll have to Check for it so the game doesn't throw us an error.
        if (currentTile.GetTileNeighbors().Count <= 0)
        {
            currentTile.CheckForNeighbors();
        }

        foreach (Vector2Int neighbor in currentTile.GetTileNeighbors())
        {
            FloodFillAlg(board, neighbor);
        }
    }
}
    