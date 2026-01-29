using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class character : MonoBehaviour
{
    // first things first : make sure player is selectable
    private bool _isSelected = false;

    [SerializeField] private InputActionReference leftMouseClicked;
    [SerializeField] private Grid hexgrid;
    private Tilemap land;

    void Start()
    {
        land = hexgrid.transform.GetChild(1).gameObject.GetComponent<Tilemap>();

        // Locks player position to the center of the nearest cell
        Vector3Int playerPos = land.WorldToCell(transform.position);
        Vector3 closestPlayerGridPos = land.CellToLocal(playerPos);
        transform.position = closestPlayerGridPos;
    }

    void OnEnable()
    {
        leftMouseClicked.action.started += Select;
    }

    void OnDisable()
    {
        leftMouseClicked.action.started -= Select;
    }

    private void Select(InputAction.CallbackContext obj)
    {
        Vector2 mousePointerPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int tileMapPos = land.WorldToCell(mousePointerPos);
        Vector3Int playerPos = land.WorldToCell(this.transform.position);
        if (_isSelected == true)
        {
            if (land.GetTile(tileMapPos) == null) return;
            if (tileMapPos == playerPos)
            {
                _isSelected = false;
                return;
            }
            Vector2 closestPlayerGridPos = land.CellToLocal(tileMapPos);
            transform.position = closestPlayerGridPos;
            _isSelected = false;
            return;
        }
        if (tileMapPos == playerPos)
        {
            _isSelected = true;
            Debug.Log("This character is selected!");
        }

    }
}
