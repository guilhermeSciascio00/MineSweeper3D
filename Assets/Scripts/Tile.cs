using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _numberCanvas;
    [SerializeField] TextMeshProUGUI _numberText;

    [Header("Materials")]
    [SerializeField] Material _unrevealedMaterial;
    [SerializeField] Material _revealedMaterial;
    [SerializeField] Material _mineMaterial;
    [SerializeField] GameObject _flagObj;
    [SerializeField] ParticleSystem _explosionParticles;

    [Header("DebugInfo")]
    [SerializeField] int _minesAround;
    [SerializeField] string _tileType;
    [SerializeField] bool isFlagged;

    public TileData Data { get; private set; }

    private List<Vector2Int> _tileNeighbors = new List<Vector2Int>();

    private Renderer _renderer;
    private GameBoard _board;

    private void Awake()
    {
        _board = GetComponentInParent<GameBoard>();
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        UpdateTileVisual();
        TurnCanvasOn();
        _tileType = Data.TiType.ToString();
        _flagObj.SetActive(false);
    }

    private void Update()
    {
        isFlagged = Data.IsFlagged;
    }

    public void InitializeData(TileData data)
    {
        Data = data;
    }

    //Visuals Revealed and Unrevealed.


    public void UpdateTileVisual()
    {
        if (!Data.IsRevealed)
        {
            _renderer.material = _unrevealedMaterial;
        }
        else
        {
            _renderer.material = _revealedMaterial;
            TurnCanvasOn();
        }
    }

    //Visuals Flag and UnFlag
    public void PutFlag()
    {
        _flagObj.SetActive(true);
    }

    public void RemoveFlag()
    {
        _flagObj.SetActive(false);
    }

    public void UpdateTileText()
    {
        _tileType = Data.TiType.ToString();
    }

    //CanvasNumberText
    private void TurnCanvasOn()
    {
        if (Data.TiType != TileData.TileType.Number) { return; }

        if (Data.IsRevealed)
        {
            _numberText.text = Data.MineNumbers.ToString();
            _numberCanvas.SetActive(true);
        }
    }

    public void PlayExplosionVFX()
    {
        if(Data.TiType == TileData.TileType.Mine)
        {
            if (!_explosionParticles.isPlaying)
            {
                _explosionParticles.Play();
            }
        }
    }

    //Method responsible for checking neighbor tiles
    public void CheckForNeighbors()
    {
        if(Data.TiType == TileData.TileType.Mine) { return; }

        int mineCount = 0;
        Vector2Int currentPosition = Data.TilePosition;

        //Loop Offset
        //RowOffset is the row checker, so it will start looking for mines in the bottom row first, then in the same work that we are and after it will check above us.
        for(int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            //Will check each column in that specif row
            for(int columnOffset = -1; columnOffset <= 1; columnOffset++)
            {
                //avoid checking our current position
                if (columnOffset == 0 && rowOffset == 0)
                {
                    continue;
                }

                //c stands for column and l for line
                //checkforX and Y, stores our position + offset
                //let's say we are in the bottom left-corner(0,0), if we want to check to the right, we are checking 1c, 0l(1 column to the right, and 0l means in the same line)
                int checkForX = currentPosition.x + columnOffset;
                int checkForY = currentPosition.y + rowOffset;
                if(_board.IsInsideBounds(checkForX, checkForY))
                {
                    Tile tile = _board.GetTile(checkForX, checkForY);
                    _tileNeighbors.Add(new Vector2Int(checkForX, checkForY));
                    if(tile.Data.TiType == TileData.TileType.Mine)
                    {
                        mineCount++;
                    }
                }
            }
        }

        if(mineCount > 0)
        {
            Data.TiType = TileData.TileType.Number;
            Data.MineNumbers = mineCount;
            _minesAround = mineCount;
            UpdateTileText();
        }
        else
        {
            Data.TiType = TileData.TileType.Empty;
        }
    }

    public List<Vector2Int> GetTileNeighbors()
    {
        return _tileNeighbors;
    }

    public Material GetMineMaterial() {  return _mineMaterial; }


}
