using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    /*
        Pretty sure this class shouldn't be a singleton because it changes each scene.
        Maybe it should be and I'm just an idiot idk.
    */
    [SerializeField] private Tilemap land;
    private BoundsInt landBounds;
    private TileBase[] landTiles;
    [SerializeField] private Tilemap highlightZone;
    [SerializeField] private TileBase tile;

    private float ALPHA = 0.5f;

    private Vector3Int prevCell;

    void Start()
    {
        land.CompressBounds();
        landBounds = land.cellBounds;
        landTiles = land.GetTilesBlock(landBounds);
    }

    void Update()
    {
        HighlightMouseHover();
    }

    /*
        Adds a visual representation to the tile the player is hovering their mouse over.
            A white tile means no player is selected,
            A green tile means that a player is selected and they can move to that tile,
            A red tile means that the player is selected BUT cannot move to that tile.
            If the tile is a different color something terrible has happened and we are all going to die.
    */
    private void HighlightMouseHover()
    {
        Vector2 mousePointerPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int tileMapPos = highlightZone.WorldToCell(mousePointerPos);

        chooseColor(tileMapPos);
        if (tileMapPos == prevCell) return;
        highlightZone.SetTile(prevCell, null);
        highlightZone.SetTile(tileMapPos, tile);
        prevCell = tileMapPos;
    }

    private void chooseColor(Vector3Int tileMapPos)
    {
        GameObject selectedCharacter = UnitManager.main.CheckIfAnySelected();
        if (selectedCharacter == null)
        {
            highlightZone.color = new Color(1.0f, 1.0f, 1.0f, ALPHA);
            return;
        }
        else if (land.GetTile(tileMapPos) == null || Vector3Int.Distance(tileMapPos, selectedCharacter.GetComponent<character>().GetCharacterCellPos()) > selectedCharacter.GetComponent<character>().GetMaxMoveDist()) // probably also check if mouse is too far away from player's movement range
        {
            highlightZone.color = new Color(1.0f, 0.0f, 0.0f, ALPHA);
            return;
        }
        else highlightZone.color = new Color(0.0f, 1.0f, 0.0f, ALPHA);
    }

    public TileBase[] GetLandTileBase()
    {
        return landTiles;
    }
}
