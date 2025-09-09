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

    [Header("DebugInfo")]
    [SerializeField] int _minesAround;
    [SerializeField] string _tileType;

    //DebugOnly
    //[SerializeField] Material _mineMaterial;
    //[SerializeField] Material _numberMaterial;

    public TileData Data { get; private set; }

    private bool _isFirstTileRevealed = false;
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

        //Events
        EventManager.OnTileJumped += EventManager_OnTileJumped;
    }

    private void EventManager_OnTileJumped(Tile obj)
    {
        if(!obj.Data.IsRevealed)
        {
            obj.Data.IsRevealed = true;
            obj.UpdateTileVisual();
            if(!_isFirstTileRevealed) 
            {
                EventManager.FirstTileRevealed(obj);
                _isFirstTileRevealed = true;
            }
        }
    }

    public void InitializeData(TileData data)
    {
        Data = data;
    }

    //Debug Only
    //private void UpdateVisuals()
    //{
    //    switch (Data.TiType)
    //    {
    //        case TileData.TileType.Empty:
    //            _renderer.material = _unrevealedMaterial;
    //            break;
    //        case TileData.TileType.Mine:
    //            _renderer.material = _mineMaterial;
    //            break;
    //        case TileData.TileType.Number:
    //            _renderer.material = _numberMaterial;
    //            break;
    //    }
    //}

    //Visuals Revealed and Unrevealed.


    private void UpdateTileVisual()
    {
        if (!Data.IsRevealed)
        {
            _renderer.material = _unrevealedMaterial;
        }
        else
        {
            _renderer.material = _revealedMaterial;
        }
    }
    public void UpdateMineTileText()
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
        }
        else
        {
            Data.TiType = TileData.TileType.Empty;
        }

        //UpdateVisuals();
    }

    public List<Vector2Int> GetTileNeighbors()
    {
        return _tileNeighbors;
    }
}
