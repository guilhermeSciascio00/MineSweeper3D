using UnityEngine;

public class FlagTile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("References")]
    [SerializeField] private GameInputManager _gameInputManager;
    [SerializeField] private GameBoard _gameBoardRef;
    [SerializeField] private UIMenuManager _menuManager;

    private GroundDetection _groundDetection;

    private int _flagsAmount;
    private Tile _tile;

    void Awake()
    {
        _flagsAmount = _gameBoardRef.GetMinesAmount();
        _groundDetection = GetComponent<GroundDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameInputManager.IsFlagButtonPressed() && _groundDetection.IsOnGround(out _tile))
        {
            if(_tile == null) { return; }
            FlagAndUnflagTile();
            _menuManager.UpdateFlagText();
        }
        
    }

    private void FlagAndUnflagTile()
    {
        if(_flagsAmount < 0 || !_gameBoardRef.HasGameStarted() || _tile.Data.IsRevealed) { return; }

        if(!_tile.Data.IsFlagged)
        {
            _flagsAmount--;
            _tile.Data.IsFlagged = true;
            _tile.SwitchToFlaggedMaterial();
        }
        else
        {
            _flagsAmount++;
            _tile.Data.IsFlagged = false;
            _tile.SwitchToUnflaggedMaterial();
        }

    }

    public int GetFlagsAmount() => _flagsAmount;

}
