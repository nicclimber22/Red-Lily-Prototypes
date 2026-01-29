using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap land;
    [SerializeField] private Tilemap highlightZone;
    [SerializeField] private TileBase tile;
    // Update is called once per frame

    private Vector3Int prevCell;

    void Update()
    {
        HighlightMouseHover();
    }

    private void HighlightMouseHover()
    {
        Vector2 mousePointerPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int tileMapPos = highlightZone.WorldToCell(mousePointerPos);

        if (tileMapPos == prevCell) return;
        if (land.GetTile(tileMapPos) == null) highlightZone.color = Color.red;
        else highlightZone.color = Color.green;
        highlightZone.SetTile(prevCell, null);
        prevCell = tileMapPos;
        highlightZone.SetTile(tileMapPos, tile);
    }
}
