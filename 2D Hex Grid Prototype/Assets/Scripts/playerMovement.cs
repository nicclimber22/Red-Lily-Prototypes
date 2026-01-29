using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private Tilemap land;
    [SerializeField] private Tilemap highlightZone;
    [SerializeField] private Tilemap availableMovementZone;
    [SerializeField] private InputActionReference playerMov;
    [SerializeField] private TileBase highlightTile;
    [SerializeField] private TileBase moveTile;
    [SerializeField] private TileBase attackTile;

    private Vector3Int prevCell;

    private List<Vector3Int> movementArea;

    void Start()
    {
        // Locks player position to the center of the nearest cell
        Vector3Int playerPos = land.WorldToCell(transform.position);
        Vector3 closestPlayerGridPos = land.CellToLocal(playerPos);
        transform.position = closestPlayerGridPos;

        movementArea = new List<Vector3Int>();
    }

    void Update()
    {
        HighlightMouseHover();
    }

    /*
        Takes the grid position the point of the mouse pointer is at and places
        a tile on the Highlight grid (reffered to as highlightZone).

        This method does not overwrite any tiles on any other tilemap
    */
    private void HighlightMouseHover()
    {
        Vector3Int tileMapPos = land.WorldToCell(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        tileMapPos.z = 0; // Without this the position will be at -10 because the Camera is at -10
                          // Making the hightlighted tile invisible
        if (tileMapPos != prevCell)
        {
            highlightZone.SetTile(prevCell, null);
            prevCell = tileMapPos;
             highlightZone.SetTile(tileMapPos, highlightTile);
       }
    }

    // Input handling
    private void OnEnable()
    {
        playerMov.action.started += Select;
    }

    private void OnDisable()
    {
        playerMov.action.started -= Select;
    }

    /*
        Moves player to the center of selected cell
    */
    private void Select(InputAction.CallbackContext obj)
    {
        Vector3Int tileMapPos = land.WorldToCell(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        Vector3Int playerPos = land.WorldToCell(transform.position);
        tileMapPos.z = 0; // Without this the position will be at -10 because the Camera is at -10
        if (land.GetTile(tileMapPos) != null)
        {
            if (tileMapPos == playerPos)
            {
                CheckMovementArr();
                ShowPlayerRange(tileMapPos);
            }
            Vector2 closestPlayerGridPos = land.CellToLocal(tileMapPos);
            transform.position = closestPlayerGridPos;
        }
    }

    /*
        Surround player with tiles to show their effective movement / attack range
        Probably need info from player character to see what their range is
    */
    private void ShowPlayerRange(Vector3Int tileMapPos)
    {
        // God's shittiest implementation
            tileMapPos.x += 1;
            availableMovementZone.SetTile(tileMapPos, moveTile);
            movementArea.Add(tileMapPos);
            tileMapPos.x -= 2; 
            availableMovementZone.SetTile(tileMapPos, moveTile);
            movementArea.Add(tileMapPos);
            tileMapPos.x += 1;
            
            tileMapPos.y += 1;
            availableMovementZone.SetTile(tileMapPos, moveTile);
            movementArea.Add(tileMapPos);
            tileMapPos.y -= 2; 
            availableMovementZone.SetTile(tileMapPos, moveTile);
            movementArea.Add(tileMapPos);
            tileMapPos.y += 1;
        
        if (tileMapPos.y % 2 != 0) {
            tileMapPos.x += 1;
            tileMapPos.y -= 1;
            availableMovementZone.SetTile(tileMapPos, moveTile);
            movementArea.Add(tileMapPos);
            tileMapPos.y += 2;
            availableMovementZone.SetTile(tileMapPos, moveTile);
            movementArea.Add(tileMapPos);
            tileMapPos.y -= 1;
        }
        else
        {
            tileMapPos.x -= 1;
            tileMapPos.y += 1;
            availableMovementZone.SetTile(tileMapPos, moveTile);
            movementArea.Add(tileMapPos);
            tileMapPos.y -= 2;
            availableMovementZone.SetTile(tileMapPos, moveTile);
            movementArea.Add(tileMapPos);
            tileMapPos.y += 1;
            tileMapPos.x += 1;
        }
    }

    private void CheckMovementArr()
    {
        foreach (Vector3Int i in movementArea)
        {
            availableMovementZone.SetTile(i, null);
        }
    }
}
