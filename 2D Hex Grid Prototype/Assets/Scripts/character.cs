using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class character : MonoBehaviour
{

    [SerializeField] private InputActionReference leftMouseClicked;
    [SerializeField] private Grid hexgrid;
    private bool _isSelected = false;
    private bool _isSneaking = false;
    private bool _movementEnabled = false;
    private float MAX_MOVE_DISTANCE = 1.5f;
    private Tilemap land;
    private CharacterStats stats;

    void Start()
    {
        stats = this.gameObject.GetComponent<CharacterStats>();
        MAX_MOVE_DISTANCE += stats.GetEndurance() / 4.0f; // I'm guessing the average agility will be 4?

        land = hexgrid.transform.GetChild(1).gameObject.GetComponent<Tilemap>();

        // Locks player position to the center of the nearest cell
        Vector3Int playerPos = land.WorldToCell(transform.position);
        Vector3 closestPlayerGridPos = land.CellToLocal(playerPos);
        transform.position = closestPlayerGridPos;
    }

    public void ToggleSelected()
    {
        _isSelected = !_isSelected;
    }

    public bool CheckIfSelected()
    {
        return _isSelected;
    }

    public void ToggleSneak()
    {
        if (_isSelected) _isSneaking = !_isSneaking;
    }

    public bool CheckIfSneaking()
    {
        return _isSneaking;
    }

    public bool checkIfMovementEnabled()
    {
        return _movementEnabled;
    }

    public void ToggleMovement()
    {
        if (_isSelected) _movementEnabled = !_movementEnabled;
    }

    public Vector3Int GetCharacterCellPos()
    {
        return land.WorldToCell(this.transform.position);
    }

    public float GetMaxMoveDist()
    {
        return MAX_MOVE_DISTANCE;
    }


    // The below code is for when the player clicks on a member of a unit
    void OnEnable()
    {
        leftMouseClicked.action.started += Select;
    }

    void OnDisable()
    {
        leftMouseClicked.action.started -= Select;
    }

    /*
        Locks onto a player if the tile they are standing on is clicked.
        If they are, allow them to move based on MAX_MOVE_DISTANCE.
        Click on the tile the character is standing on again to deselect / lock off of them.

        TODO add ability for player to choose other actions 
        (Attack, Use Item, etc.) when character is selected
    */
    private void Select(InputAction.CallbackContext obj)
    { 
        if (TurnManager.main.currentTurn != TurnManager.turn.PLAYERSTURN) return;

        Vector2 mousePointerPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int tileMapPos = land.WorldToCell(mousePointerPos);
        Vector3Int playerPos = land.WorldToCell(this.transform.position);

        if (_movementEnabled == true)
        {
            // Check if tile is walkable
            if (land.GetTile(tileMapPos) == null) return;
            
            // Deselect player
            if (tileMapPos == playerPos)
            {
                _isSelected = false;
                _movementEnabled = false;
                return;
            }

            if (Vector3Int.Distance(tileMapPos, playerPos) > MAX_MOVE_DISTANCE) return;
            
            Vector2 closestPlayerGridPos = land.CellToLocal(tileMapPos);
            transform.position = closestPlayerGridPos;
            _isSelected = false;
            _movementEnabled = false;
            return;
        }

        // Select player
        if (tileMapPos == playerPos)
        {
            _isSelected = true;
        }
    }
}
