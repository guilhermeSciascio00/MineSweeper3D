using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Material _baseMaterial;
    [SerializeField] Material _mineMaterial;
    //This one will need to be an array ranging from 1 to 8 or (9)
    [SerializeField] Material _numberMaterial;
    public TileData Data { get; private set; }

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        UpdateVisuals();
    }

    public void InitializeData(TileData data)
    {
        Data = data;
    }

    //TODO Update the visuals based on the Tile Type
    //If it's empty, it should use the basic material
    //If it's a mine, for now it should be red.
    //If it's a number, for now it should be blue.
    //I'll create the proper assets later
    //but this step is vital to keep building the project, the colors above is just to test if the Type is being addressed correctly.

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
        }
    }
}
