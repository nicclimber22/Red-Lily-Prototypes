using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap land;
    [SerializeField] private Tilemap highlightZone;
    [SerializeField] private TileBase tile;

    private float ALPHA = 0.5f;

    private Vector3Int prevCell;

    void Update()
    {
        HighlightMouseHover();
    }

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
}
