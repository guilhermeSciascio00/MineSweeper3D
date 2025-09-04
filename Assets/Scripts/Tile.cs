using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _numberCanvas;
    [SerializeField] TextMeshProUGUI _numberText;

    [SerializeField] Material _baseMaterial;
    [SerializeField] Material _mineMaterial;
    //This one will need to be an array ranging from 1 to 8 or (9)
    [SerializeField] Material _numberMaterial;

    public TileData Data { get; private set; }

    private Renderer _renderer;
    private GameBoard _board;

    private void Awake()
    {
        _board = GetComponentInParent<GameBoard>();
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        UpdateVisuals();
        TurnCanvasOn();
    }


    public void InitializeData(TileData data)
    {
        Data = data;
    }

    //Debug Only
    private void UpdateVisuals()
    {
        
        switch (Data.TiType)
        {
            case TileData.TileType.Empty:
                _renderer.material = _baseMaterial;
                break;
            case TileData.TileType.Mine:
                _renderer.material = _mineMaterial;
                break;
            case TileData.TileType.Number:
                _renderer.material = _numberMaterial;
                break;
        }
    }

    //Visuals Revealed and Unrevealed.

    //CanvasNumberText
    private void TurnCanvasOn()
    {
        if (Data.TiType != TileData.TileType.Number) { return; }

        _numberText.text = Data.MineNumbers.ToString();
        _numberCanvas.SetActive(true);
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
                //let's say we are in the bottom left-corner(0,0), if we want to check to the right, we are checking 1c, 0l(1 column to the right, and 0l means in the samen line)
                int checkForX = currentPosition.x + columnOffset;
                int checkForY = currentPosition.y + rowOffset;
                if(_board.IsInsideBounds(checkForX, checkForY))
                {
                    Tile tile = _board.GetTile(checkForX, checkForY);
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
        }
        else
        {
            Data.TiType = TileData.TileType.Empty;
        }

        UpdateVisuals();
    }
}
