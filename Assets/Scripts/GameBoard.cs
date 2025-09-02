using UnityEngine;

public class GameBoard : MonoBehaviour
{
    
    [SerializeField] private GameObject _tileToSpawn;
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private Vector3 _tileOffset;

    private GameObject[,] _tiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject CreateOneTile()
    {
        GameObject tile = Instantiate(_tileToSpawn, transform.position, Quaternion.identity, this.transform);

        return tile;
    }

    private void CreateTiles()
    {
        _tiles = new GameObject[_boardSize.x, _boardSize.y];
        int tileXSize;
        int tileZSize;

        for (int i = 0; i < _boardSize.x; i++)
        {
            for (int j = 0;  j < _boardSize.y; j++)
            {
                _tiles[i, j] = CreateOneTile();

                GameObject tile = _tiles[i, j];
                tileXSize = (int)tile.transform.localScale.x;
                tileZSize = (int)tile.transform.localScale.z;

                tile.transform.position = new Vector3(i * (tileXSize + _tileOffset.x), 0f, j * (tileZSize + _tileOffset.z));

                tile.name = $"x: {tile.transform.position.x}, z: {tile.transform.position.z}";
            }
        }
    }
}
