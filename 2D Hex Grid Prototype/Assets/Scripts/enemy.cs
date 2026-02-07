using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class enemy : MonoBehaviour
{
    [SerializeField] private Grid hexgrid;
    private Tilemap land;
    private readonly List<GameObject> detectedCharacters = new();

    [Header("Stats")]
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float damage = 1.0f;


    void Start()
    {
        land = hexgrid.transform.GetChild(1).gameObject.GetComponent<Tilemap>();

        LockToCell();
    }

    void LockToCell()
    {
        Vector3Int playerPos = land.WorldToCell(transform.position);
        Vector3 closestPlayerGridPos = land.CellToLocal(playerPos);
        transform.position = closestPlayerGridPos;
    }

    // Method to be changed to an actual pathfinding alogrithm
    // Also method doesn't check to see if new position is a valid tile
    private void MoveTowards(GameObject target)
    {
        float xDist = target.transform.position.x - this.transform.position.x;
        float yDist = target.transform.position.y - this.transform.position.y;
        this.transform.position = new Vector3(this.transform.position.x + (xDist < 0 ? -1 : 1) * (xDist != 0 ? xDist / xDist : 0) , this.transform.position.y + (yDist < 0 ? -1 : 1) * (yDist != 0 ? yDist / yDist : 0), this.transform.position.z);
        LockToCell();
    }

    public void MakeChoice()
    {
        GameObject closestPlayer = null;
        if (detectedCharacters.Count == 0) return;
        foreach (GameObject character in detectedCharacters)
        {
            if (closestPlayer == null) closestPlayer = character;
            if (Vector3.Distance(character.transform.position, this.transform.position) < Vector3.Distance(closestPlayer.transform.position, this.transform.position))
            {
                closestPlayer = character;
            }
        }
        if (closestPlayer != null) 
        {
            if (Vector3.Distance(closestPlayer.transform.position, this.transform.position) <= attackRange)
            {
                Debug.Log("You have been damaged for " + damage); // Actual damage implementation will be done... later!
            }
            else
            {
                MoveTowards(closestPlayer);
            }
        }
    }

    bool LookForPlayer(character player)
    {
        if (player.CheckIfSneaking() == true)
        {
            return false;
        }
        return true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        character player = collision.GetComponent<character>();
        if (player != null)
        {
            if (LookForPlayer(player) == true) {
                Debug.Log("Character found!");
                detectedCharacters.Add(collision.gameObject);
            }
        } 
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("character"))
        {
            Debug.Log("Character lost!");
            detectedCharacters.Remove(collision.gameObject);
        }
    }

}
